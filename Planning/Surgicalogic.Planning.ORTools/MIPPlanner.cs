using Google.OrTools.LinearSolver;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Surgicalogic.Planning.ORTools
{
    public class MIPPlanner
    {
        public static DailyPlanOutputModel Solve(DailyPlanInputModel input)
        {
            var result = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>(), HasSolution = true };

            foreach (var item in input.Rooms)
            {
                result.Rooms.Add(new RoomOutputModel { Id = item.Id, Name = item.Name, Operations = new List<OperationOutputModel>() });
            }

            var solver = new Solver("SurgicaLogic", Solver.CBC_MIXED_INTEGER_PROGRAMMING);

            var operationsCount = input.Operations.Count;
            var roomsCount = input.Rooms.Count;
            var totalPeriod = input.Settings.MaximumPeriod;
            var overtime = input.Settings.RoomsPeriod;

            var operations = Enumerable.Range(0, operationsCount);
            var rooms = Enumerable.Range(0, roomsCount);
            var periods = Enumerable.Range(0, totalPeriod);
            var overtimes = Enumerable.Range(overtime - 1, totalPeriod - overtime);

            var operationNames = input.Operations.Select(x => x.Name).ToArray();
            var operationTimes = input.Operations.Select(x => x.Period).ToArray();
            var roomNames = input.Rooms.Select(x => x.Name).ToArray();

            //3 boyutlu diziyi tanımladık.
            Variable[,,] production = new Variable[operationsCount, roomsCount, totalPeriod];
            for (int i = 0; i < operationsCount; i++)
            {
                for (int r = 0; r < roomsCount; r++)
                {
                    for (int t = 0; t < totalPeriod; t++)
                    {
                        production[i, r, t] = solver.MakeIntVar(0, 1, string.Format("{0}-{1}-{2}", i, r, t));
                    }
                }
            }

            //Overlap olmaması için
            for (int i1 = 0; i1 < operationsCount; i1++)
            {
                for (int i2 = 0; i2 < operationsCount; i2++)
                {
                    if (i2 != i1)
                    {
                        for (int r = 0; r < roomsCount; r++)
                        {
                            var longer = operationTimes[i1] > operationTimes[i2] ? operationTimes[i1] : operationTimes[i2];

                            for (int t = 0; t <= totalPeriod - longer; t++)
                            {
                                var timeList = Enumerable.Range(t, longer);

                                var c1 = (from t1 in timeList
                                          select production[i1, r, t1])
                                          .ToArray().Sum();

                                var c2 = (from t2 in timeList
                                          select production[i2, r, t2])
                                          .ToArray().Sum();

                                solver.Add(c1 + c2 <= 1);
                            }
                        }
                    }
                }
            }

            //Aynı doktora birden fazla ameliyat programlanmaması için
            for (int i1 = 0; i1 < operationsCount; i1++)
            {
                for (int i2 = 0; i2 < operationsCount; i2++)
                {
                    if (i2 != i1 && input.Operations[i1].DoctorIds.Intersect(input.Operations[i2].DoctorIds).Count() > 0)
                    {
                        var longer = operationTimes[i1] > operationTimes[i2] ? operationTimes[i1] : operationTimes[i2];

                        for (int t = 0; t <= totalPeriod - longer; t++)
                        {
                            var timeList = Enumerable.Range(t, longer);

                            var c1 = (from t1 in timeList
                                      from r in rooms
                                      select production[i1, r, t1])
                                      .ToArray().Sum();

                            var c2 = (from t2 in timeList
                                      from r in rooms
                                      select production[i2, r, t2])
                                      .ToArray().Sum();

                            solver.Add(c1 + c2 <= 1);
                        }
                    }
                }
            }

            //Uygun olmayan odalarda ameliyat yapılamasın.
            for (int i = 0; i < operationsCount; i++)
            {
                for (int r = 0; r < roomsCount; r++)
                {
                    if (input.Operations[i].UnavailableRooms.Any(x => x == input.Rooms[r].Id))
                    {
                        solver.Add((from t in periods
                                    select production[i, r, t])
                                    .ToArray().Sum() == 0);
                    }
                }
            }

            //Her ameliyat bir kez yapılsın. 
            for (int i = 0; i < operationsCount; i++)
            {
                solver.Add((from r in rooms
                            from t in periods
                            select production[i, r, t])
                            .ToArray().Sum() == 1);
            }

            //aynı odada aynı anda 2 ameliyat olmasın
            for (int r = 0; r < roomsCount; r++)
            {
                for (int t = 0; t < totalPeriod; t++)
                {
                    solver.Add((from i in operations
                                select production[i, r, t])
                                .ToArray().Sum() <= 1);
                }
            }

            //Ameliyat overtime dahil süreden daha uzun sürmesin diye bu kontrolü yapıyorum.
            for (int i = 0; i < operationsCount; i++)
            {
                int operationTime = operationTimes[i];
                if (operationTime > 1)
                {
                    var tList = Enumerable.Range(totalPeriod - operationTime + 1, operationTime - 1);

                    solver.Add((from r in rooms
                                from t in tList
                                select production[i, r, t])
                                .ToArray().Sum() == 0);
                }
            }

            //Overtime'ı minimize ediyoruz.
            solver.Minimize((from i in operations
                             from r in rooms
                             from t in overtimes
                             select production[i, r, t])
                             .ToArray().Sum());

            if (solver.Solve() != Solver.OPTIMAL)
            {
                result.HasSolution = false;
                return result;
            }

            for (int i = 0; i < operationsCount; i++)
            {
                for (int r = 0; r < roomsCount; r++)
                {
                    for (int t = 0; t < totalPeriod; t++)
                    {
                        if (production[i, r, t].SolutionValue() == 1)
                        {
                            var surgeryRoom = result.Rooms[r];
                            var tomorrow = DateTime.Now.AddDays(1);
                            var dateTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, input.Settings.StartingHour, input.Settings.StartingMinute, 0);
                            dateTime = dateTime.AddMinutes(t * input.Settings.PeriodInMinutes);
                            surgeryRoom.Operations.Add(new OperationOutputModel { Id = input.Operations[i].Id, Name = input.Operations[i].Name, DoctorIds = input.Operations[i].DoctorIds, Period = input.Operations[i].Period, StartDate = dateTime });
                        }
                    }
                }
            }

            return result;
        }
    }
}