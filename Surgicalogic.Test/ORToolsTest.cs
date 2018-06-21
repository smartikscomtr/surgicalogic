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
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 2,
                RoomsPeriod = 10,
                MaximumPeriod = 10,
                StartingHour = 9,
                StartingMinute = 0,
                PeriodInMinutes = 15,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 4,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 2,
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });

            var actual = OperationPlanning.Solve(surgeryPlan);

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
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 2,
                RoomsPeriod = 10,
                MaximumPeriod = 10,
                StartingHour = 9,
                StartingMinute = 0,
                PeriodInMinutes = 15,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 4,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });

            var actual = OperationPlanning.Solve(surgeryPlan);

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
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 3,
                RoomsPeriod = 10,
                MaximumPeriod = 10,
                StartingHour = 9,
                StartingMinute = 0,
                PeriodInMinutes = 15,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 4,
                UnavailableRooms = new List<int> { 1, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });

            var actual = OperationPlanning.Solve(surgeryPlan);

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
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 3,
                RoomsPeriod = 24,
                MaximumPeriod = 24,
                StartingHour = 9,
                StartingMinute = 0,
                PeriodInMinutes = 15,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 2,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 1,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 1,
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 3,
                Period = 2,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 5,
                DoctorId = 2,
                Period = 6,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 6,
                DoctorId = 3,
                Period = 7,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 7,
                DoctorId = 1,
                Period = 5,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 8,
                DoctorId = 2,
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 9,
                DoctorId = 4,
                Period = 6,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 10,
                DoctorId = 4,
                Period = 10,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);
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

            var actual = OperationPlanning.Solve(surgeryPlan);

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
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 3,
                RoomsPeriod = 12,
                MaximumPeriod = 12,
                StartingHour = 9,
                StartingMinute = 0,
                PeriodInMinutes = 15,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 2,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 3,
                Period = 1,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 2,
                Period = 7,
                UnavailableRooms = new List<int> { 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 3,
                Period = 2,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 5,
                DoctorId = 2,
                Period = 2,
                UnavailableRooms = new List<int> { 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 6,
                DoctorId = 3,
                Period = 1,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 7,
                DoctorId = 1,
                Period = 3,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 8,
                DoctorId = 2,
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);
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

            var actual = OperationPlanning.Solve(surgeryPlan);

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
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 2,
                RoomsPeriod = 5,
                MaximumPeriod = 10,
                StartingHour = 9,
                StartingMinute = 0,
                PeriodInMinutes = 15,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 1,
                Period = 6,
                UnavailableRooms = new List<int> { 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 3,
                UnavailableRooms = new List<int> { 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 2,
                Period = 8,
                UnavailableRooms = new List<int> { 1 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var actual = OperationPlanning.Solve(surgeryPlan);

            if (actual.Count() > 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ChangedPeriod()
        {
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 4,
                RoomsPeriod = 9,
                MaximumPeriod = 9,
                StartingHour = 6,
                StartingMinute = 30,
                PeriodInMinutes = 60,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 2,
                Period = 2,
                UnavailableRooms = new List<int> { 1, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 1,
                Period = 1,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 2,
                Period = 4,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 5,
                DoctorId = 3,
                Period = 8,
                UnavailableRooms = new List<int> { 1, 4 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();
            var tomorrow = DateTime.Now.AddDays(1);
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 3, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 4, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            expected.Add(new OperationPlan { OperationId = 5, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });

            var actual = OperationPlanning.Solve(surgeryPlan);

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
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 5,
                RoomsPeriod = 18,
                MaximumPeriod = 24,
                StartingHour = 8,
                StartingMinute = 15,
                PeriodInMinutes = 60,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 1,
                DoctorId = 2,
                Period = 4,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 1,
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 1,
                Period = 5,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 2,
                Period = 7,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 5,
                DoctorId = 3,
                Period = 2,
                UnavailableRooms = new List<int> { 1, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 6,
                DoctorId = 3,
                Period = 4,
                UnavailableRooms = new List<int> { 1, 3, 4, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 7,
                DoctorId = 5,
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 8,
                DoctorId = 5,
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 3 }

            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 9,
                DoctorId = 3,
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 10,
                DoctorId = 2,
                Period = 8,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);
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

            var actual = OperationPlanning.Solve(surgeryPlan);

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
        public void SecondRoomResctrictionTest()
        {
            var surgeryPlan = new SurgeryPlan
            {
                RoomCount = 4,
                RoomsPeriod = 22,
                MaximumPeriod = 22,
                StartingHour = 7,
                StartingMinute = 0,
                PeriodInMinutes = 20,
                Operations = new List<Operation>()
            };

            #region Operations

            var operation = new Operation
            {
                Id = 9,
                DoctorId = 3,
                Period = 3,
                UnavailableRooms = new List<int> { 2, 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 16,
                DoctorId = 10,
                Period = 4,
                UnavailableRooms = new List<int> { 4, 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 1,
                DoctorId = 4,
                Period = 7,
                UnavailableRooms = new List<int> { 1, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 4,
                DoctorId = 7,
                Period = 6,
                UnavailableRooms = new List<int> { 4, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 11,
                DoctorId = 7,
                Period = 6,
                UnavailableRooms = new List<int> { 3, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 8,
                DoctorId = 8,
                Period = 5,
                UnavailableRooms = new List<int> { 1, 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 7,
                DoctorId = 10,
                Period = 7,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 20,
                DoctorId = 10,
                Period = 2,
                UnavailableRooms = new List<int> { 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 2,
                DoctorId = 3,
                Period = 4,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 16,
                DoctorId = 10,
                Period = 3,
                UnavailableRooms = new List<int> { 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 3,
                DoctorId = 3,
                Period = 9,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 100,
                DoctorId = 1,
                Period = 9,
                UnavailableRooms = new List<int> { 2, 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 16,
                DoctorId = 10,
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new Operation
            {
                Id = 37,
                DoctorId = 7,
                Period = 10,
                UnavailableRooms = new List<int> { 2, 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new List<OperationPlan>();

            var tomorrow = DateTime.Now.AddDays(1);

            expected.Add(new OperationPlan { OperationId = 9, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 20, 0) });
            expected.Add(new OperationPlan { OperationId = 16, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 20, 0) });
            expected.Add(new OperationPlan { OperationId = 1, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 8, 40, 0) });
            expected.Add(new OperationPlan { OperationId = 4, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 20, 0) });
            expected.Add(new OperationPlan { OperationId = 11, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 20, 0) });
            expected.Add(new OperationPlan { OperationId = 8, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 7, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 7, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 7, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 20, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 40, 0) });
            expected.Add(new OperationPlan { OperationId = 2, RoomId = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 16, RoomId = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 40, 0) });
            expected.Add(new OperationPlan { OperationId = 3, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 7, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 100, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 16, RoomId = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 13, 0, 0) });
            expected.Add(new OperationPlan { OperationId = 37, RoomId = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 7, 0, 0) });

            var actual = OperationPlanning.Solve(surgeryPlan);

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
