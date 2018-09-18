using Microsoft.VisualStudio.TestTools.UnitTesting;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;
using Surgicalogic.Planning.ORTools;
using System;
using System.Collections.Generic;

namespace Surgicalogic.Planning.Test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TwoOperationsTwoRoomTwoDoctorTest()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 10,
                    MaximumPeriod = 10,
                    StartingHour = 9,
                    StartingMinute = 0,
                    PeriodInMinutes = 15,
                },
                Rooms = MockService.Rooms(2),
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds = new int[] { 1 },
                Period = 4,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 2 },
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);
            var operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 2, Operations = operations });

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void TwoOperationsOneDoctorTwoRoomTest()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(3),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 10,
                    MaximumPeriod = 10,
                    StartingHour = 9,
                    StartingMinute = 0,
                    PeriodInMinutes = 15,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds =new int[] { 1 },
                Period = 4,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 1 },
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);
            var operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void RoomRestrictionTest()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(3),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 10,
                    MaximumPeriod = 10,
                    StartingHour = 9,
                    StartingMinute = 0,
                    PeriodInMinutes = 15,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds =new int[] { 1 },
                Period = 4,
                UnavailableRooms = new List<int> { 1, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 1 },
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);
            var operations = new List<OperationOutputModel>();
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 2, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 3, Operations = operations });

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void TooManyOperationsTest()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(3),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 24,
                    MaximumPeriod = 24,
                    StartingHour = 9,
                    StartingMinute = 0,
                    PeriodInMinutes = 15,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds =new int[] { 1 },
                Period = 2,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 1 },
                Period = 1,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 3,
                DoctorIds =new int[] { 1 },
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 4,
                DoctorIds =new int[] { 3 },
                Period = 2,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 5,
                DoctorIds =new int[] { 2 },
                Period = 6,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 6,
                DoctorIds =new int[] { 3 },
                Period = 7,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 7,
                DoctorIds =new int[] { 1 },
                Period = 5,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 8,
                DoctorIds =new int[] { 2 },
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 9,
                DoctorIds =new int[] { 4 },
                Period = 6,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 10,
                DoctorIds =new int[] { 4 },
                Period = 10,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);
            var operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 9, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 30, 0) });
            operations.Add(new OperationOutputModel { Id = 10, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 45, 0) });
            operations.Add(new OperationOutputModel { Id = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 0, 0) });
            operations.Add(new OperationOutputModel { Id = 6, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            operations.Add(new OperationOutputModel { Id = 7, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 45, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 2, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0) });
            operations.Add(new OperationOutputModel { Id = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 45, 0) });
            operations.Add(new OperationOutputModel { Id = 5, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 30, 0) });
            operations.Add(new OperationOutputModel { Id = 8, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 0, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 3, Operations = operations });

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void ConsecutiveOperationsTest()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(3),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 12,
                    MaximumPeriod = 12,
                    StartingHour = 9,
                    StartingMinute = 0,
                    PeriodInMinutes = 15,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds =new int[] { 1 },
                Period = 2,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 3 },
                Period = 1,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 3,
                DoctorIds =new int[] { 2 },
                Period = 7,
                UnavailableRooms = new List<int> { 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 4,
                DoctorIds =new int[] { 3 },
                Period = 2,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 5,
                DoctorIds =new int[] { 2 },
                Period = 2,
                UnavailableRooms = new List<int> { 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 6,
                DoctorIds =new int[] { 3 },
                Period = 1,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 7,
                DoctorIds =new int[] { 1 },
                Period = 3,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 8,
                DoctorIds =new int[] { 2 },
                Period = 3,
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);
            var operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 5, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 45, 0) });
            operations.Add(new OperationOutputModel { Id = 6, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 45, 0) });
            operations.Add(new OperationOutputModel { Id = 7, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 00, 0) });
            operations.Add(new OperationOutputModel { Id = 8, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 15, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 0, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 2, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 45, 0) });
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 30, 0) });
            operations.Add(new OperationOutputModel { Id = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 00, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 3, Operations = operations });

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void NotEnoughTimeTest()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(2),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 5,
                    MaximumPeriod = 10,
                    StartingHour = 9,
                    StartingMinute = 0,
                    PeriodInMinutes = 15,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds =new int[] { 1 },
                Period = 6,
                UnavailableRooms = new List<int> { 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 1 },
                Period = 3,
                UnavailableRooms = new List<int> { 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 2 },
                Period = 8,
                UnavailableRooms = new List<int> { 1 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var actual = CPPlanner.Solve(surgeryPlan);

            foreach (var item in actual.Rooms)
            {
                if (item.Operations.Count > 0)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void ChangedPeriod()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(4),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 9,
                    MaximumPeriod = 9,
                    StartingHour = 6,
                    StartingMinute = 30,
                    PeriodInMinutes = 60,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds =new int[] { 2 },
                Period = 2,
                UnavailableRooms = new List<int> { 1, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 1 },
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 3,
                DoctorIds =new int[] { 1 },
                Period = 1,
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 4,
                DoctorIds =new int[] { 2 },
                Period = 4,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 5,
                DoctorIds =new int[] { 3 },
                Period = 8,
                UnavailableRooms = new List<int> { 1, 4 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);
            var operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 5, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 2, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 06, 30, 0) });
            operations.Add(new OperationOutputModel { Id = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 09, 30, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 3, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 30, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 4, Operations = operations });

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void SecondChangedPeriod()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(5),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 18,
                    MaximumPeriod = 24,
                    StartingHour = 8,
                    StartingMinute = 15,
                    PeriodInMinutes = 60,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds =new int[] { 2 },
                Period = 4,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 1 },
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 3,
                DoctorIds =new int[] { 1 },
                Period = 5,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 4,
                DoctorIds =new int[] { 2 },
                Period = 7,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 5,
                DoctorIds =new int[] { 3 },
                Period = 2,
                UnavailableRooms = new List<int> { 1, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 6,
                DoctorIds =new int[] { 3 },
                Period = 4,
                UnavailableRooms = new List<int> { 1, 3, 4, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 7,
                DoctorIds =new int[] { 5 },
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 8,
                DoctorIds =new int[] { 5 },
                Period = 6,
                UnavailableRooms = new List<int> { 1, 2, 3 }

            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 9,
                DoctorIds =new int[] { 3 },
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 10,
                DoctorIds =new int[] { 2 },
                Period = 8,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);
            var operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 23, 15, 0) });
            operations.Add(new OperationOutputModel { Id = 10, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 15, 15, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 5, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 15, 0) });
            operations.Add(new OperationOutputModel { Id = 6, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 8, 15, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 2, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 8, 15, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 3, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 15, 0) });
            operations.Add(new OperationOutputModel { Id = 7, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 15, 15, 0) });
            operations.Add(new OperationOutputModel { Id = 9, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 21, 15, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 4, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 14, 15, 0) });
            operations.Add(new OperationOutputModel { Id = 8, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 8, 15, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 5, Operations = operations });

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }

        }

        [TestMethod]
        public void SecondRoomResctrictionTest()
        {
            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(4),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 22,
                    MaximumPeriod = 22,
                    StartingHour = 7,
                    StartingMinute = 0,
                    PeriodInMinutes = 20,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 9,
                DoctorIds =new int[] { 3 },
                Period = 3,
                UnavailableRooms = new List<int> { 2, 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 36,
                DoctorIds =new int[] { 10 },
                Period = 4,
                UnavailableRooms = new List<int> { 4, 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds =new int[] { 4 },
                Period = 7,
                UnavailableRooms = new List<int> { 1, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 4,
                DoctorIds =new int[] { 7 },
                Period = 6,
                UnavailableRooms = new List<int> { 4, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 11,
                DoctorIds =new int[] { 7 },
                Period = 6,
                UnavailableRooms = new List<int> { 3, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 8,
                DoctorIds =new int[] { 8 },
                Period = 5,
                UnavailableRooms = new List<int> { 1, 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 7,
                DoctorIds =new int[] { 10 },
                Period = 7,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 20,
                DoctorIds =new int[] { 10 },
                Period = 2,
                UnavailableRooms = new List<int> { 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds =new int[] { 3 },
                Period = 4,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 26,
                DoctorIds =new int[] { 10 },
                Period = 3,
                UnavailableRooms = new List<int> { 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 3,
                DoctorIds =new int[] { 3 },
                Period = 9,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 100,
                DoctorIds =new int[] { 1 },
                Period = 9,
                UnavailableRooms = new List<int> { 2, 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 16,
                DoctorIds =new int[] { 10 },
                Period = 3,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 37,
                DoctorIds =new int[] { 7 },
                Period = 10,
                UnavailableRooms = new List<int> { 2, 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);
            var operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 20, 0) });
            operations.Add(new OperationOutputModel { Id = 11, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 20, 0) });
            operations.Add(new OperationOutputModel { Id = 37, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 07, 00, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 36, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 20, 0) });
            operations.Add(new OperationOutputModel { Id = 7, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 7, 00, 0) });
            operations.Add(new OperationOutputModel { Id = 20, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 40, 0) });
            operations.Add(new OperationOutputModel { Id = 26, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 40, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 2, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 07, 00, 0) });
            operations.Add(new OperationOutputModel { Id = 100, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 00, 0) });
            operations.Add(new OperationOutputModel { Id = 16, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 13, 00, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 3, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 9, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 20, 0) });
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 40, 0) });
            operations.Add(new OperationOutputModel { Id = 8, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 07, 00, 0) });
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 00, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 4, Operations = operations });

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void Operations()
        {

            var surgeryPlan = new DailyPlanInputModel
            {
                Rooms = MockService.Rooms(3),
                Settings = new SettingsInputModel
                {
                    RoomsPeriod = 1,
                    MaximumPeriod = 32,
                    StartingHour = 8,
                    StartingMinute = 10,
                    PeriodInMinutes = 40,
                },
                Operations = new List<OperationInputModel>()
            };

            #region Operations

            var operation = new OperationInputModel
            {
                Id = 1,
                DoctorIds = new int[] { 1 },
                Period = 3,
                UnavailableRooms = new List<int> { 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                DoctorIds = new int[] { 1 },
                Period = 4,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 3,
                DoctorIds = new int[] { 2 },
                Period = 5,
                UnavailableRooms = new List<int> { 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 4,
                DoctorIds = new int[] { 2 },
                Period = 5,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 5,
                DoctorIds = new int[] { 2 },
                Period = 4,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 6,
                DoctorIds = new int[] { 1 },
                Period = 5,
                UnavailableRooms = new List<int> { 1, 1 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 7,
                DoctorIds = new int[] { 2 },
                Period = 5,
                UnavailableRooms = new List<int> { 2, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 8,
                DoctorIds = new int[] { 2 },
                Period = 4,
                UnavailableRooms = new List<int> { 1, 2 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 9,
                DoctorIds = new int[] { 2 },
                Period = 4,
                UnavailableRooms = new List<int> { 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 10,
                DoctorIds = new int[] { 2 },
                Period = 4,
                UnavailableRooms = new List<int> { 3 }
            };

            surgeryPlan.Operations.Add(operation);
            #endregion

            var expected = new DailyPlanOutputModel() { Rooms = new List<RoomOutputModel>() };
            var tomorrow = DateTime.Now.AddDays(1);

            var operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 1, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 50, 0) });
            operations.Add(new OperationOutputModel { Id = 2, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 10, 0) });
            operations.Add(new OperationOutputModel { Id = 3, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 16, 50, 0) });
            operations.Add(new OperationOutputModel { Id = 7, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 20, 10, 0) });
            operations.Add(new OperationOutputModel { Id = 9, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 23, 30, 0) });
            operations.Add(new OperationOutputModel { Id = 10, StartDate = new DateTime(tomorrow.AddDays(1).Year, tomorrow.AddDays(1).Month, tomorrow.AddDays(1).Day, 02, 10, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 1, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 6, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 12, 50, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 2, Operations = operations });
            operations = new List<OperationOutputModel>();
            operations.Add(new OperationOutputModel { Id = 4, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 10, 0) });
            operations.Add(new OperationOutputModel { Id = 5, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 11, 30, 0) });
            operations.Add(new OperationOutputModel { Id = 8, StartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 14, 10, 0) });
            expected.Rooms.Add(new RoomOutputModel { Id = 3, Operations = operations });
            operations = new List<OperationOutputModel>();

            var actual = CPPlanner.Solve(surgeryPlan);

            if (actual.Rooms.Count == 0)
            {
                Assert.Fail();
            }

            for (int i = 0; i < actual.Rooms.Count; i++)
            {
                for (int j = 0; j < actual.Rooms[i].Operations.Count; j++)
                {
                    if (actual.Rooms[i].Id != expected.Rooms[i].Id || actual.Rooms[i].Operations[j].Id != expected.Rooms[i].Operations[j].Id || actual.Rooms[i].Operations[j].StartDate != expected.Rooms[i].Operations[j].StartDate)
                    {
                        Assert.Fail();
                    }
                }
            }
        }
    }
}