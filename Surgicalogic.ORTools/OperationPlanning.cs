using Google.OrTools.ConstraintSolver;
using Smartiks.Teydeb.Surgicalogic.ConsoleApp.InputModel;
using Smartiks.Teydeb.Surgicalogic.ConsoleApp.Model;
using Smartiks.Teydeb.Surgicalogic.ConsoleApp.OutputModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SurgicaLogic.ORTools
{
    public class OperationPlanning
    {

        /// <summary>
        /// Çözümlemeyi yapan ana metot.
        /// </summary>
        /// <param name="Operations">Planlanacak ameliyatlar.</param>
        /// <param name="Rooms">Ameliyathaneler</param>
        /// <param name="Settings.MaximumPeriod">Overtime dahil ameliyatlara ayrılacak maksimum süre.</param>
        /// <param name="Settings.RoomsPeriod">Ameliyathanelerin uygunluk süresini belirtir. Maksimum değer, mesai süresi olmalı.</param>
        /// <param name="Settings.StartingHour">Ameliyatların saat kaçta başlayacağı bilgisi</param>
        /// <param name="Settings.StartingMinute">Ameliyatların hangi dakikada başlayacağı bilgisi. Buçukta da başlayabilirse diye.</param>
        /// <param name="Settings.PeriodInMinutes">Ameliyat periyodlarının kaç dakika olduğu bilgisi</param>
        public static DailyPlan Solve(DailyPlanInputModel input)
        {
            Console.WriteLine("Uygulama başlıyor.");
            var result = new DailyPlan() { Rooms = new List<Room>() };

            foreach (var item in input.Rooms)
            {
                result.Rooms.Add(new Room { Id = item.Id, Name = item.Name, Operations = new List<Operation>() });
            }

            Solver solver = new Solver("SurgicaLogic");

            var roomPeriodIndex = Enumerable.Range(1, input.Rooms.Count * input.Settings.MaximumPeriod).ToList();

            //Buraya dummy bir status daha ekliyorum. Bunu, ameliyat süresinden daha ilerideki ihtimallere atayacağım.
            var dummyIndex = input.Rooms.Count * input.Settings.MaximumPeriod + 100;
            roomPeriodIndex.Add(dummyIndex);
            //Geçerli statüler. Maksimum süre dahil tüm statüleri içerir. Örnegin 2 ameliyathane olan bir senaryoda 1= 1.ameliyathanenin 1. periyodunu, 2= 2. ameliyathanenin 1.periyodunu, 3= 1.ameliyathanenin 2. periyodunu, 4= 2.ameliyathanenin 2. periyodunu vs. temsil eder.
            var validStatus = roomPeriodIndex.ToArray();

            //AllDifferent methodunda kullanmak üzere maksimum süreyi ienumerable olarak tutan değişkenler.
            IEnumerable<int> maximumLengthList = Enumerable.Range(0, input.Settings.MaximumPeriod);

            //Karar mekanizmasının davranışını tanımlıyorum. Belli durumlara göre bunu değiştirebiliyorum.
            int variableStart = Solver.CHOOSE_MIN_SLACK_RANK_FORWARD;

            //Arka arkaya ameliyatları aynı doktor yapıyorsa karar mekanizmasını bu şekilde değiştirmek daha doğru sonuç verdiği tespit edildi. 
            for (int i = 0; i < input.Operations.Count - 1; i++)
            {
                if (input.Operations[i].Doctor.Id == input.Operations[i + 1].Doctor.Id)
                {
                    variableStart = Solver.CHOOSE_MIN_SIZE_HIGHEST_MIN;
                }
            }

            //Ameliyatların hangi oda-zaman diliminde yapılacağına karar verilen değişken. Tüm ihtimaller bu değişken içerisine tanımlanıyor.
            IntVar[,] x =
                solver.MakeIntVarMatrix(input.Operations.Count, input.Settings.MaximumPeriod, validStatus, "x");

            //Yukarıdaki değişkeni çözümleyebilmek için tek boyutlu hali.
            IntVar[] x_flat = x.Flatten();

            //Hangi zaman dilimlerine atama yaptığımı anlayabilmek için ameliyat - süre ilişkisini iki boyutlu dizi olarak tutuyorum. İki ameliyatın aynı oda-zaman dilimine atanmasını bu şekilde engelliyorum.
            int[,] preview = new int[input.Operations.Count, input.Settings.MaximumPeriod];

            //Ameliyatların aynı odada devam edebilmesi ve önceki bir zamana atama yapmaması için, bir sonraki değerin bir önceki değerden en az oda sayısı kadar büyük olması olması kuralı.
            for (int i = input.Operations.Count - 1; i >= 0; i--)
            {
                for (int j = input.Operations[i].Period - 1; j >= 0; j--)
                {
                    solver.Add(x[i, j] != dummyIndex);

                    if (j > 0)
                    {
                        for (int k = 1; k < input.Rooms.Count; k++)
                        {
                            solver.Add(x[i, j] > x[i, j - 1] + k);
                        }
                    }
                }
            }

            for (int i = 0; i < input.Operations.Count; i++)
            {
                int doctorId = input.Operations[i].Doctor.Id;

                //Bir ameliyat bir odada veya bir zamanda yapılamaz bilgisi bu değişkende tutuluyor.
                int[] blockedTimes = GetBlockedTimes(input.Operations[i].UnavailableRooms, i, input.Rooms.Count, input.Settings.MaximumPeriod, validStatus);

                foreach (var item in blockedTimes)
                {
                    for (int m = 0; m < input.Settings.MaximumPeriod; m++)
                    {
                        //Burada bu ameliyat bu zamanda, bu odada yapılamaz kuralını ekliyoruz.
                        if (item != dummyIndex)
                        {
                            //Console.WriteLine("x[{0}, {1}] != {2}", i, m, item);
                            solver.Add(x[i, m] != item);
                        }
                    }
                }

                //Aynı doktora aynı anda birden fazla ameliyat planlanmaması için
                for (int ad = 0; ad < input.Operations.Count; ad++)
                {
                    //Eğer sonraki ameliyatı yapacak doktorlardan birisi, bu ameliyatı yapan doktor ise.
                    if (input.Operations[ad].Doctor.Id == doctorId && ad != i)
                    {
                        //Bir sonraki ameliyat süresi kadar dön.
                        for (int ml = 0; ml < input.Operations[ad].Period; ml++)
                        {
                            //Mevcut ameliyatın süresi kadar dön.
                            for (int t = 0; t < input.Operations[i].Period; t++)
                            {
                                //Ameliyat odası sayısı kadar dön.
                                for (int r = 1; r < input.Rooms.Count; r++)
                                {
                                    //Bir sonraki ameliyatın süreleri, mevcut ameliyat ile aynı süreye ve aynı indexe gelen değerlere eşit olamaz. 
                                    //Örneğin 2 ameliyathaneli bir hastanede aynı doktorun 1. ameliyatı 3 saat sürsün. Doktor bu ameliyata karşılık gelen a1,a3,a5 değerlerinin yanı sıra diğer odalardaki aynı zaman karşılık gelen a2,a4,a6 zamanlarında da bu ameliyatı yapamaz.
                                    solver.Add(x[i, t] % input.Rooms.Count <= r && x[i, t] % input.Rooms.Count != 0 ? x[ad, ml] != x[i, t] + (input.Rooms.Count - r) : x[0, 0] == x[0, 0]);
                                    //Console.WriteLine("x[{0}, {1}] % {2} <= {5} && x[{0}, {1}] % {2} != 0  ? x[{3}, {4}] != x[{0}, {1}] + ({2} - {5}) : x[0, 0] == x[0, 0]", i, t, input.RoomCount, ad, ml, r);
                                    //solver.Add(x[ad, ml] != x[i, t] + r);
                                    //Console.WriteLine("x[{0}, {1}] != x[{2}, {3}] + {4}", ad, ml, i, t, r);
                                }
                            }
                        }
                    }
                }

                bool ameliyatBasladi = false;
                bool forCompleted = false;
                for (int j = 0; j < input.Settings.MaximumPeriod; j++)
                {
                    //Bu for daha önce tamamlanmadıysa. En dışta ameliyatlar içerisinde döndüğüm için bir ameliyat için iki kez planlama yapılmasın diye bu kontrolü yapıyorum.
                    if (!forCompleted)
                    {
                        for (int k = 1; k < input.Operations[i].Period; k++)
                        {
                            //Eğer ameliyat için yeterli süre varsa, atama yapacağım yere daha önce herhangi bir ameliyat planlamadıysam.
                            if (j < input.Settings.MaximumPeriod - input.Operations[i].Period && preview[i, j + k] == 0)
                            {
                                //Ameliyatların aynı odada yapılmasını sağlamak için yeni atayacağımız değeri, bir önceki  değere ameliyathane sayısı kadar ekleyerek buluyoruz. Yani a1'de başladıysa, 2 ameliyathane varsa bir sonraki değer a3 olmalı.
                                //Console.WriteLine("x[{0}, {1} + {2}] == x[{0}, {1} + {2} - 1] + {3}", i, j, k, operationRoomCount);
                                solver.Add(x[i, j + k] == x[i, j + k - 1] + input.Rooms.Count);

                                preview[i, j + k] = 1;
                                preview[i, j + k - 1] = 1;
                                ameliyatBasladi = true;
                            }
                        }
                    }

                    //Eğer ameliyat başladıysa yukarıdaki for tamamlanmış demektir. 
                    if (ameliyatBasladi == true)
                    {
                        forCompleted = true;
                    }
                }
            }

            for (int i = 1; i < input.Operations.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    //Bundan önce yapılan tüm ameliyatların odaZaman değerleri, o an yapılan ameliyatın odaZaman değerlerinden farklı olsun.
                    var list = (from k in maximumLengthList.Take(input.Operations[j].Period)
                                select x[j, k]).ToList();
                    for (int t = 0; t < input.Operations[i].Period; t++)
                    {
                        list.Add(x[i, t]);
                        //Console.WriteLine("x[{0},{1}]", i, t);
                    }

                    solver.Add(list.ToArray().AllDifferent());
                }
            }

            //Burada da atama yapılmayan yerlere varsayilan bir deger atiyorum ki farkli ihtimallerde farkli senaryolar görünebilsin. Yoksa atama yapilmayan yerdeki ihtimalleri degistirip sonuçta ayni programi çikartiyordu.
            for (int i = 1; i < input.Operations.Count; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    //Bundan önce yapilan tüm ameliyatlarin bitisinden maksimum süreye kadar olan degerleri, hep ayni olsun diyorum ki kendi üretmesini istedigim ihtimaller farklilassin.
                    var list = (from k in maximumLengthList.Skip(input.Operations[j].Period)
                                select x[j, k]).ToList();

                    foreach (var item in list)
                    {
                        solver.Add(item == dummyIndex);
                    }
                }
            }

            // İhtimalleri bu kriterlere göre oluştur.
            DecisionBuilder db = solver.MakePhase(x_flat,
                                                  variableStart,
                                                  Solver.ASSIGN_MIN_VALUE);

            solver.NewSearch(db);

            int num_solutions = 0;

            //while (solver.NextSolution())
            //{
            //    num_solutions++;
            //    List<int> roomUsage = new List<int>();
            //    for (int i = 0; i < input.Operations.Count; i++)
            //    {
            //        Console.Write("#{0,-2}: ", i + 1);
            //        for (int j = 0; j < input.Operations[i].Period; j++)
            //        {
            //            int v = (int)x[i, j].Value();
            //            Console.Write(v + " ");
            //            roomUsage.Add(v);
            //        }

            //        Console.WriteLine();

            //    }
            //    Console.WriteLine();

            //    //Console.WriteLine("Usage statistics per room:\n");
            //    //for (int i = 0; i < operat; i++)
            //    //{
            //    //    Console.Write("Oda #{0,-2}: ", i + 1);
            //    //    var usage = roomUsage.Count(p => (p % ameliyathaneSayisi == i));
            //    //    Console.Write("Kullanılan Süre: {0}, Overtime: {1}", usage, usage > mesaiSuresi ? usage - mesaiSuresi : 0);
            //    //    Console.WriteLine();
            //    //}
            //    Console.WriteLine();

            //    // We just show 2 solutions
            //    if (num_solutions > 0)
            //    {
            //        break;
            //    }
            //}


            while (solver.NextSolution())
            {
                Console.WriteLine();
                num_solutions++;
                List<int> roomUsage = new List<int>();
                var operationTimes = new List<int>();
                for (int i = 0; i < input.Operations.Count; i++)
                {
                    Console.Write("Ameliyat #{0,-2}: ", input.Operations[i].Id);
                    for (int j = 0; j < input.Operations[i].Period; j++)
                    {
                        int v = (int)x[i, j].Value();
                        if (j == 0)
                        {
                            int valueIndex = Array.IndexOf(validStatus, v);
                            int room = v % input.Rooms.Count != 0 ? v % input.Rooms.Count : input.Rooms.Count;
                            var surgeryRoom = result.Rooms[room - 1];
                            int time = room == input.Rooms.Count ? (valueIndex + 1) / input.Rooms.Count : ((valueIndex + 1) / input.Rooms.Count) + 1;
                            var tomorrow = DateTime.Now.AddDays(1);
                            var dateTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, input.Settings.StartingHour, input.Settings.StartingMinute, 0);
                            dateTime = dateTime.AddMinutes((time - 1) * input.Settings.PeriodInMinutes);
                            Console.Write(surgeryRoom.Id + "-" + surgeryRoom.Name + " Ameliyathanesi, Saat: " + dateTime.ToShortTimeString());
                            //Console.Write(surgeryRoom.Id + ". Oda, Saat: " + dateTime.ToShortTimeString());
                            operationTimes.Add(dateTime.Hour);

                            surgeryRoom.Operations.Add(new Operation { Id = input.Operations[i].Id, Name = input.Operations[i].Name, Doctor = input.Operations[i].Doctor, StartDate = dateTime });
                        }
                        roomUsage.Add(v);
                    }

                    Console.WriteLine();

                }
                Console.WriteLine();

                Console.WriteLine("Usage statistics per room:\n");
                for (int i = 0; i < input.Rooms.Count; i++)
                {
                    Console.Write("Oda #{0,-2}: ", i + 1);
                    var usage = i + 1 == input.Rooms.Count ? roomUsage.Count(p => (p % input.Rooms.Count == 0)) : roomUsage.Count(p => (p % input.Rooms.Count == i + 1));
                    var usageTimes = i + 1 == input.Rooms.Count ? roomUsage.Where(p => (p % input.Rooms.Count == 0)) : roomUsage.Where(p => (p % input.Rooms.Count == i + 1));
                    int overTime = CalculateOverTime(usage, input.Settings.RoomsPeriod); //usageTimes.Count(t => t > operationRoomCount * roomPeriodLength[i]);
                    Console.Write("Uygunluk: {0}, Kullanılan Süre: {1}, Overtime: {2}", input.Settings.RoomsPeriod, usage, overTime);
                    Console.WriteLine();
                }
                Console.WriteLine();

                //We just show 1 solution
                if (num_solutions > 0)
                {
                    break;
                }
            }

            if (num_solutions == 0)
            {
                Console.WriteLine("Sonuç üretilemedi.");
            }

            //Console.WriteLine("\nSolutions: {0}", solver.Solutions());
            //Console.WriteLine("WallTime: {0}ms", solver.WallTime());
            //Console.WriteLine("Failures: {0}", solver.Failures());
            //Console.WriteLine("Branches: {0} ", solver.Branches());

            solver.EndSearch();

            return result;

        }
        private static int CalculateOverTime(int usage, int mesaiSuresi)
        {
            return usage > mesaiSuresi ? usage - mesaiSuresi : 0;
        }

        //Bir ameliyat bir odada veya bir zamanda yapılamaz bilgisi bu metodda hesaplanılıyor.
        private static int[] GetBlockedTimes(List<int> unavailableRooms, int ameliyatId, int operationRoomCount, int maximumLength, int[] validStatus)
        {
            var result = new List<int>();
            if (unavailableRooms == null)
            {
                return result.ToArray();
            }

            for (int i = 0; i < operationRoomCount; i++)
            {
                //Bu ameliyat, bu ameliyathanede yapılamazsa
                if (unavailableRooms.Any(t => t - 1 == i))
                {
                    for (int j = 0; j < maximumLength; j++)
                    {
                        result.Add(validStatus[(i + (j * operationRoomCount))]);
                    }
                }
            }

            return result.ToArray();
        }

        public static void Main(String[] args)
        {
            var rooms = new List<RoomInputModel>();

            rooms.Add(new RoomInputModel
            {
                Id = 1,
                Name = "Florence Nightingale"
            });

            rooms.Add(new RoomInputModel
            {
                Id = 2,
                Name = "Hipokrat"
            });

            rooms.Add(new RoomInputModel
            {
                Id = 3,
                Name = "William Harvey"
            });

            rooms.Add(new RoomInputModel
            {
                Id = 4,
                Name = "Lois Pasteur"
            });

            rooms.Add(new RoomInputModel
            {
                Id = 5,
                Name = "Gregor Mendel"
            });

            var surgeryPlan = new DailyPlanInputModel
            {
                Settings = new Settings
                {
                    RoomsPeriod = 3,
                    MaximumPeriod = 4,
                    StartingHour = 8,
                    StartingMinute = 00,
                    PeriodInMinutes = 60,
                },
                Operations = new List<OperationInputModel>(),
                Rooms = rooms
            };

            var operation = new OperationInputModel
            {
                Id = 1,
                Doctor = new Doctor { Id = 2 },
                Period = 1,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 2,
                Doctor = new Doctor { Id = 1 },
                Period = 1,
                UnavailableRooms = new List<int> { 1, 2, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 3,
                Doctor = new Doctor { Id = 1 },
                Period = 1,
                UnavailableRooms = new List<int> { 2, 3 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 4,
                Doctor = new Doctor { Id = 2 },
                Period = 1,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 5,
                Doctor = new Doctor { Id = 3 },
                Period = 1,
                UnavailableRooms = new List<int> { 1, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 6,
                Doctor = new Doctor { Id = 3 },
                Period = 1,
                UnavailableRooms = new List<int> { 1, 3, 4, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 7,
                Doctor = new Doctor { Id = 5 },
                Period = 1,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 8,
                Doctor = new Doctor { Id = 5 },
                Period = 1,
                UnavailableRooms = new List<int> { 1, 2, 3 }

            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 9,
                Doctor = new Doctor { Id = 3 },
                Period = 1,
                UnavailableRooms = new List<int> { 1, 2, 3, 5 }
            };

            surgeryPlan.Operations.Add(operation);

            operation = new OperationInputModel
            {
                Id = 10,
                Doctor = new Doctor { Id = 2 },
                Period = 1,
                UnavailableRooms = new List<int> { 3, 4 }
            };

            surgeryPlan.Operations.Add(operation);

            var result = Solve(surgeryPlan);
            Console.ReadKey();
        }
    }
}
