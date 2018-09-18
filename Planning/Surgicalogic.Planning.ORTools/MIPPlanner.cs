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
            var result = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };

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
                for (int j = 0; j < roomsCount; j++)
                {
                    for (int k = 0; k < totalPeriod; k++)
                    {
                        production[i, j, k] = solver.MakeIntVar(0, 1, string.Format("{0}-{1}-{2}", i, j, k));
                    }
                }
            }

            //Overlap olmaması için
            for (int o1 = 0; o1 < operationsCount; o1++)
            {
                for (int o2 = 0; o2 < operationsCount; o2++)
                {
                    if (o2 != o1)
                    {
                        for (int r = 0; r < roomsCount; r++)
                        {
                            var longer = operationTimes[o1] > operationTimes[o2] ? operationTimes[o1] : operationTimes[o2];

                            for (int p = 0; p <= totalPeriod - longer; p++)
                            {
                                var timeList = Enumerable.Range(p, longer);

                                var c1 = (from p1 in timeList
                                          select production[o1, r, p1])
                                          .ToArray().Sum();

                                var c2 = (from p2 in timeList
                                          select production[o2, r, p2])
                                          .ToArray().Sum();

                                solver.Add(c1 + c2 <= 1);
                            }
                        }
                    }
                }
            }

            //Aynı doktora birden fazla ameliyat programlanmaması için
            for (int o1 = 0; o1 < operationsCount; o1++)
            {
                for (int o2 = 0; o2 < operationsCount; o2++)
                {
                    if (o2 != o1 && input.Operations[o1].DoctorIds.Intersect(input.Operations[o2].DoctorIds).Count() > 0)
                    {
                        var longer = operationTimes[o1] > operationTimes[o2] ? operationTimes[o1] : operationTimes[o2];

                        for (int p = 0; p <= totalPeriod - longer; p++)
                        {
                            var timeList = Enumerable.Range(p, longer);

                            var c1 = (from p1 in timeList
                                      from r in rooms
                                      select production[o1, r, p1])
                                      .ToArray().Sum();

                            var c2 = (from p2 in timeList
                                      from r in rooms
                                      select production[o2, r, p2])
                                      .ToArray().Sum();

                            solver.Add(c1 + c2 <= 1);
                        }
                    }
                }
            }

            //Uygun olmayan odalarda ameliyat yapılamasın.
            for (int i = 0; i < operationsCount; i++)
            {
                for (int j = 0; j < roomsCount; j++)
                {
                    if (input.Operations[i].UnavailableRooms.Any(x => x == input.Rooms[j].Id))
                    {
                        solver.Add((from p in periods
                                    select production[i, j, p])
                                    .ToArray().Sum() == 0);
                    }
                }
            }

            //Her ameliyat bir kez yapılsın. 
            for (int i = 0; i < operationsCount; i++)
            {
                solver.Add((from rt in rooms
                            from p in periods
                            select production[i, rt, p])
                            .ToArray().Sum() == 1);
            }

            //aynı odada aynı anda 2 ameliyat olmasın
            for (int j = 0; j < roomsCount; j++)
            {
                for (int k = 0; k < totalPeriod; k++)
                {
                    solver.Add((from p in operations
                                select production[p, j, k])
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
                                from tt in tList
                                select production[i, r, tt])
                                .ToArray().Sum() == 0);
                }
            }

            solver.Minimize((from rt in rooms
                             from ot in overtimes
                             from o in operations
                             select production[o, rt, ot])
                             .ToArray().Sum());

            if (solver.Solve() != Solver.OPTIMAL)
            {
                return result;
            }

            for (int i = 0; i < operationsCount; i++)
            {
                for (int j = 0; j < roomsCount; j++)
                {
                    for (int k = 0; k < totalPeriod; k++)
                    {
                        if (production[i, j, k].SolutionValue() == 1)
                        {
                            var surgeryRoom = result.Rooms[j];
                            var tomorrow = DateTime.Now.AddDays(1);
                            var dateTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, input.Settings.StartingHour, input.Settings.StartingMinute, 0);
                            dateTime = dateTime.AddMinutes(k * input.Settings.PeriodInMinutes);
                            surgeryRoom.Operations.Add(new OperationOutputModel { Id = input.Operations[i].Id, Name = input.Operations[i].Name, DoctorIds = input.Operations[i].DoctorIds, Period = input.Operations[i].Period, StartDate = dateTime });
                        }
                    }
                }
            }

            return result;
        }
    }
}