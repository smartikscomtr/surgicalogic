using Microsoft.VisualStudio.TestTools.UnitTesting;
using Surgicalogic.ORTools.Model;
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
            int operationRoomCount = 2;
            int maximumPeriodLength = 10;
            int roomPeriodLength = 10;
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            #region Operations
            List<Operation> operations = new List<Operation>();

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 4,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 2,
                Period = 3,
            };

            operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });

            var actual = OperationPlanning.Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);

            if (actual.Count == 0)
            {
                Assert.Fail();
            }

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
            int operationRoomCount = 2;
            int maximumPeriodLength = 10;
            int roomPeriodLength = 10;
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            #region Operations
            List<Operation> operations = new List<Operation>();

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 4,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 3,
            };

            operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });

            var actual = OperationPlanning.Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);

            if (actual.Count == 0)
            {
                Assert.Fail();
            }

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
            int operationRoomCount = 3;
            int maximumPeriodLength = 10;
            int roomPeriodLength = 10;
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            #region Operations
            List<Operation> operations = new List<Operation>();

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 4,
                UnavailableRooms = new List<int> { 1, 3 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });

            var actual = OperationPlanning.Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);

            if (actual.Count == 0)
            {
                Assert.Fail();
            }

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
            int operationRoomCount = 3;
            int maximumPeriodLength = 24;
            int roomPeriodLength = 24;
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            #region Operations
            List<Operation> operations = new List<Operation>();

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 2,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 1,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 1,
                Period = 3,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 3,
                Period = 2,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 5,
                DoctorId = 2,
                Period = 6,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 6,
                DoctorId = 3,
                Period = 7,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 7,
                DoctorId = 1,
                Period = 5,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 8,
                DoctorId = 2,
                Period = 3,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 9,
                DoctorId = 4,
                Period = 6,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 10,
                DoctorId = 4,
                Period = 10,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            operations.Add(operation);
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

            var actual = OperationPlanning.Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);

            if (actual.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].OperationId != expected[i].OperationId || actual[i].RoomId != expected[i].RoomId || actual[i].StartDate != expected[i].StartDate)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void ConsecutiveOperationsTest()
        {
            #region ParameterValues
            int operationRoomCount = 3;
            int maximumPeriodLength = 12;
            int roomPeriodLength = 12;
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            #region Operations
            List<Operation> operations = new List<Operation>();

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 2,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 3,
                Period = 1,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 2,
                Period = 7,
                UnavailableRooms = new List<int> { 3 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 3,
                Period = 2,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 5,
                DoctorId = 2,
                Period = 2,
                UnavailableRooms = new List<int> { 2 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 6,
                DoctorId = 3,
                Period = 1,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 7,
                DoctorId = 1,
                Period = 3,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 8,
                DoctorId = 2,
                Period = 3,
            };

            operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 3, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 4, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 5, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 45, 0) });
            expected.Add(new OperationPlan { OperationId = 6, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 45, 0) });
            expected.Add(new OperationPlan { OperationId = 7, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 8, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 15, 0) });

            var actual = OperationPlanning.Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);

            if (actual.Count == 0)
            {
                Assert.Fail();
            }

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
            int operationRoomCount = 2;
            int maximumPeriodLength = 10;
            int roomPeriodLength = 5;
            int startingHour = 9;
            int startingMinute = 0;
            int timePeriod = 15;
            #endregion

            #region Operations
            List<Operation> operations = new List<Operation>();

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 6,
                UnavailableRooms = new List<int> { 2 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 3,
                UnavailableRooms = new List<int> { 1 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 2,
                Period = 8,
                UnavailableRooms = new List<int> { 1 }
            };

            operations.Add(operation);
            #endregion

            var actual = OperationPlanning.Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);

            if (actual.Count() > 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ChangedPeriod()
        {
            #region ParameterValues
            int operationRoomCount = 4;
            int maximumPeriodLength = 9;
            int roomPeriodLength = 9;
            int startingHour = 6;
            int startingMinute = 30;
            int timePeriod = 60;
            #endregion

            #region Operations
            List<Operation> operations = new List<Operation>();

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 2,
                Period = 2,
                UnavailableRooms = new List<int> { 1, 3 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 1,
                Period = 1,
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 2,
                Period = 4,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 5,
                DoctorId = 3,
                Period = 8,
                UnavailableRooms = new List<int> { 1, 4 }
            };

            operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 3, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 4, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 5, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });

            var actual = OperationPlanning.Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);

            if (actual.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].OperationId != expected[i].OperationId || actual[i].RoomId != expected[i].RoomId || actual[i].StartDate != expected[i].StartDate)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void SecondChangedPeriod()
        {
            #region ParameterValues
            int operationRoomCount = 5;
            int maximumPeriodLength = 24;
            int roomPeriodLength = 18;
            int startingHour = 8;
            int startingMinute = 15;
            int timePeriod = 60;
            #endregion

            #region Operations
            List<Operation> operations = new List<Operation>();

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 2,
                Period = 4,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 5 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 1,
                Period = 5,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 2,
                Period = 7,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 5,
                DoctorId = 3,
                Period = 2,
                UnavailableRooms = new List<int> { 1, 4 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 6,
                DoctorId = 3,
                Period = 4,
                UnavailableRooms = new List<int> { 1, 3, 4, 5 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 7,
                DoctorId = 5,
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 8,
                DoctorId = 5,
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 3 }

            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 9,
                DoctorId = 3,
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            operations.Add(operation);

            operation = new Operation
            {
                Id = 10,
                DoctorId = 2,
                Period = 8,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 5, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 23, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 3, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 15, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 4, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 5, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 6, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 7, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 15, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 8, RoomId = 5, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 9, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 21, 15, 0) });
            expected.Add(new OperationPlan { OperationId = 10, RoomId = 5, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 15, 15, 0) });

            var actual = OperationPlanning.Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);

            if (actual.Count == 0)
            {
                Assert.Fail();
            }

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
