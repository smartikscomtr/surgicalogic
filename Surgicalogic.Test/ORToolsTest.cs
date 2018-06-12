using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurgicaLogic.ORTools;
using SurgicaLogic.ORTools.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Surgicalogic.Test
{
    [TestClass]
    public class ORToolsTest
    {
        [TestMethod]
        public void TwoOperationsTwoRoomTwoDoctorTest()
        {
            #region ParameterValues
            int operationCount = 2;
            int operationRoomCount = 2;
            int[] operationTimes = new int[] { 4, 3 };
            int maximumPeriodLength = 10;
            int[] operationRoomTimes = new int[] { 10, 10 };
            int[,] operationRoomRestriction = new int[,] { { 1, 1 }, { 1, 1 } };
            int[] operationDoctor = new int[] { 1, 2 };
            int startingHour = 9;
            int startingMinute = 0; 
            int timePeriod = 15;
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });

            var actual = OperationPlanning.Solve(operationCount, operationRoomCount, operationTimes, maximumPeriodLength, operationRoomTimes, operationRoomRestriction, operationDoctor, startingHour, startingMinute, timePeriod);

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].OperationId != expected[i].OperationId || actual[i].RoomId != expected[i].RoomId || actual[i].StartDate != expected[i].StartDate)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void TwoOperationsOneDoctorTwoRoomTest()
        {
            #region ParameterValues
            int operationCount = 2;
            int operationRoomCount = 2;
            int[] operationTimes = new int[] { 4, 3 };
            int maximumPeriodLength = 10;
            int[] operationRoomTimes = new int[] { 10, 10 };
            int[,] operationRoomRestriction = new int[,] { { 1, 1 }, { 1, 1 } };
            int[] operationDoctor = new int[] { 1, 1 };
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });

            var actual = OperationPlanning.Solve(operationCount, operationRoomCount, operationTimes, maximumPeriodLength, operationRoomTimes, operationRoomRestriction, operationDoctor, startingHour, startingMinute, timePeriod);

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].OperationId != expected[i].OperationId || actual[i].RoomId != expected[i].RoomId || actual[i].StartDate != expected[i].StartDate)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void RoomRestrictionTest()
        {
            #region ParameterValues
            int operationCount = 2;
            int operationRoomCount = 3;
            int[] operationTimes = new int[] { 4, 3 };
            int maximumPeriodLength = 10;
            int[] operationRoomTimes = new int[] { 10, 10, 10 };
            int[,] operationRoomRestriction = new int[,] { { 0, 1, 0 }, { 0, 0, 1 } };
            int[] operationDoctor = new int[] { 1, 1 };
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });

            var actual = OperationPlanning.Solve(operationCount, operationRoomCount, operationTimes, maximumPeriodLength, operationRoomTimes, operationRoomRestriction, operationDoctor, startingHour, startingMinute, timePeriod);

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].OperationId != expected[i].OperationId || actual[i].RoomId != expected[i].RoomId || actual[i].StartDate != expected[i].StartDate)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void TooManyOperationsTest()
        {
            #region ParameterValues
            int operationCount = 10;
            int operationRoomCount = 3;
            int[] operationTimes = new int[] { 2, 1, 3, 2, 6, 7, 5, 3, 6, 10 };
            int maximumPeriodLength = 24;
            int[] operationRoomTimes = new int[] { 24, 24, 24 };
            int[,] operationRoomRestriction = new int[,] { { 0, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 0, 0 } };
            int[] operationDoctor = new int[] { 1, 1, 1, 3, 2, 3, 1, 2, 4, 4 };
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 45, 0) });
            expected.Add(new OperationPlan { OperationId = 3, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 4, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 45, 0) });
            expected.Add(new OperationPlan { OperationId = 5, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 6, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 7, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 45, 0) });
            expected.Add(new OperationPlan { OperationId = 8, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 9, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 10, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 0, 0) });

            var actual = OperationPlanning.Solve(operationCount, operationRoomCount, operationTimes, maximumPeriodLength, operationRoomTimes, operationRoomRestriction, operationDoctor, startingHour, startingMinute, timePeriod);

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].OperationId != expected[i].OperationId || actual[i].RoomId != expected[i].RoomId || actual[i].StartDate != expected[i].StartDate)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void NotEnoughTimeTest()
        {
            #region ParameterValues
            int operationCount = 3;
            int operationRoomCount = 2;
            int[] operationTimes = new int[] { 6, 3, 8 };
            int maximumPeriodLength = 10;
            int[] operationRoomTimes = new int[] { 5, 10 };
            int[,] operationRoomRestriction = new int[,] { { 0, 1 }, { 0, 1 }, { 1, 0 } };
            int[] operationDoctor = new int[] { 1, 1, 2 };
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            var actual = OperationPlanning.Solve(operationCount, operationRoomCount, operationTimes, maximumPeriodLength, operationRoomTimes, operationRoomRestriction, operationDoctor, startingHour, startingMinute, timePeriod);

            if (actual.Count() > 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ChangedPeriod()
        {
            #region ParameterValues
            int operationCount = 5;
            int operationRoomCount = 4;
            int[] operationTimes = new int[] { 2, 3, 1, 4, 8 };
            int maximumPeriodLength = 9;
            int[] operationRoomTimes = new int[] { 9, 9, 8, 5 };
            int[,] operationRoomRestriction = new int[,] { { 0, 1, 0, 1 }, { 0, 0, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 0, 0 }, { 0, 1, 1, 0 } };
            int[] operationDoctor = new int[] { 2, 1, 1, 2, 3 };
            int startingHour = 6;
            int startingMinute = 30;
            int timePeriod = 60;
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 3, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 4, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 5, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });

            var actual = OperationPlanning.Solve(operationCount, operationRoomCount, operationTimes, maximumPeriodLength, operationRoomTimes, operationRoomRestriction, operationDoctor, startingHour, startingMinute, timePeriod);

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].OperationId != expected[i].OperationId || actual[i].RoomId != expected[i].RoomId || actual[i].StartDate != expected[i].StartDate)
                {
                    Assert.Fail();
                }
            }
        }
    }
}
