using Google.OrTools.ConstraintSolver;
using Surgicalogic.ORTools.Model;
using SurgicaLogic.ORTools.Model;
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
        /// <param name="operatios">Planlanacak ameliyatlar.</param>
        /// <param name="roomCount">Ameliyathane sayısı</param>
        /// <param name="maximumPeriodLength">Overtime dahil ameliyatlara ayrılacak maksimum süre.</param>
        /// <param name="roomPeriodLength">Ameliyathanelerin uygunluk süresini belirtir. Maksimum değer, mesai süresi olmalı.</param>
        /// <param name="startingHour">Ameliyatların saat kaçta başlayacağı bilgisi</param>
        /// <param name="startingMinute">Ameliyatların hangi dakikada başlayacağı bilgisi. Buçukta da başlayabilirse diye.</param>
        /// <param name="timePeriod">Ameliyat periyodlarının kaç dakika olduğu bilgisi</param>

        public static List<OperationPlan> Solve(List<Operation> operations, int roomCount, int maximumPeriodLength, int roomPeriodLength, int startingHour, int startingMinute, int timePeriod)
        {
            Console.WriteLine("Uygulama başlıyor.");
            var result = new List<OperationPlan>();

            Solver solver = new Solver("SurgicaLogic");

            var roomPeriodIndex = Enumerable.Range(1, roomCount * maximumPeriodLength).ToList();

            //Buraya dummy bir status daha ekliyorum. Bunu, ameliyat süresinden daha ilerideki ihtimallere atayacağım.
            var dummyIndex = roomCount * maximumPeriodLength + 100;
            roomPeriodIndex.Add(dummyIndex);
            //Geçerli statüler. Maksimum süre dahil tüm statüleri içerir. Örnegin 2 ameliyathane olan bir senaryoda 1= 1.ameliyathanenin 1. periyodunu, 2= 2. ameliyathanenin 1.periyodunu, 3= 1.ameliyathanenin 2. periyodunu, 4= 2.ameliyathanenin 2. periyodunu vs. temsil eder.
            var validStatus = roomPeriodIndex.ToArray();

            //AllDifferent methodunda kullanmak üzere maksimum süreyi ienumerable olarak tutan değişkenler.
            IEnumerable<int> maximumLengthList = Enumerable.Range(0, maximumPeriodLength);

            //Karar mekanizmasının davranışını tanımlıyorum. Belli durumlara göre bunu değiştirebiliyorum.
            int variableStart = Solver.CHOOSE_MIN_SLACK_RANK_FORWARD;

            //Arka arkaya ameliyatları aynı doktor yapıyorsa karar mekanizmasını bu şekilde değiştirmek daha doğru sonuç verdiği tespit edildi. 
            for (int i = 0; i < operations.Count - 1; i++)
            {
                if (operations[i].DoctorId == operations[i + 1].DoctorId)
                {
                    variableStart = Solver.CHOOSE_MIN_SIZE_HIGHEST_MIN;
                }
            }

            //Ameliyatların hangi oda-zaman diliminde yapılacağına karar verilen değişken. Tüm ihtimaller bu değişken içerisine tanımlanıyor.
            IntVar[,] x =
                solver.MakeIntVarMatrix(operations.Count, maximumPeriodLength, validStatus, "x");

            //Yukarıdaki değişkeni çözümleyebilmek için tek boyutlu hali.
            IntVar[] x_flat = x.Flatten();

            //Hangi zaman dilimlerine atama yaptığımı anlayabilmek için ameliyat - süre ilişkisini iki boyutlu dizi olarak tutuyorum. İki ameliyatın aynı oda-zaman dilimine atanmasını bu şekilde engelliyorum.
            int[,] preview = new int[operations.Count, maximumPeriodLength];

            //Ameliyatların aynı odada devam edebilmesi ve önceki bir zamana atama yapmaması için, bir sonraki değerin bir önceki değerden en az oda sayısı kadar büyük olması olması kuralı.
            for (int i = operations.Count - 1; i >= 0; i--)
            {
                for (int j = operations[i].Period - 1; j > 0; j--)
                {
                    for (int k = 1; k < roomCount; k++)
                    {
                        solver.Add(x[i, j] != dummyIndex);
                        solver.Add(x[i, j] > x[i, j - 1] + k);
                    }
                }
            }

            for (int i = 0; i < operations.Count; i++)
            {
                int doctorId = operations[i].DoctorId;

                //Bir ameliyat bir odada veya bir zamanda yapılamaz bilgisi bu değişkende tutuluyor.
                int[] blockedTimes = GetBlockedTimes(operations[i].UnavailableRooms, i, roomCount, maximumPeriodLength, validStatus);

                foreach (var item in blockedTimes)
                {
                    for (int m = 0; m < maximumPeriodLength; m++)
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
                for (int ad = 0; ad < operations.Count; ad++)
                {
                    //Eğer sonraki ameliyatı yapacak doktorlardan birisi, bu ameliyatı yapan doktor ise.
                    if (operations[ad].DoctorId == doctorId && ad != i)
                    {
                        //Bir sonraki ameliyat süresi kadar dön.
                        for (int ml = 0; ml < operations[ad].Period; ml++)
                        {
                            //Mevcut ameliyatın süresi kadar dön.
                            for (int t = 0; t < operations[i].Period; t++)
                            {
                                //Ameliyat odası sayısı kadar dön.
                                for (int r = 1; r < roomCount; r++)
                                {
                                    //Console.WriteLine("x[{0}, {1}] % {2} <= {5} && x[{0}, {1}] % {2} != 0  ? x[{3}, {4}] != x[{0}, {1}] + ({2} - {5}) : x[0, 0] == x[0, 0]", i, t, operationRoomCount, ad, ml, r);
                                    solver.Add(x[i, t] % roomCount <= r && x[i, t] % roomCount != 0 ? x[ad, ml] != x[i, t] + (roomCount - r) : x[0, 0] == x[0, 0]);
                                    //Bir sonraki ameliyatın süreleri, mevcut ameliyat ile aynı süreye ve aynı indexe gelen değerlere eşit olamaz. 
                                    //Örneğin 2 ameliyathaneli bir hastanede aynı doktorun 1. ameliyatı 3 saat sürsün. Doktor bu ameliyata karşılık gelen a1,a3,a5 değerlerinin yanı sıra diğer odalardaki aynı zaman karşılık gelen a2,a4,a6 zamanlarında da bu ameliyatı yapamaz.
                                    //Console.WriteLine("x[{0}, {1}] != x[{2}, {3}] + {4}", ad, ml, i, t, r);
                                    //solver.Add(x[ad, ml] != x[i, t] + r);
                                }
                            }
                        }
                    }
                }

                bool ameliyatBasladi = false;
                bool forCompleted = false;
                for (int j = 0; j < maximumPeriodLength; j++)
                {
                    //Bu for daha önce tamamlanmadıysa. En dışta ameliyatlar içerisinde döndüğüm için bir ameliyat için iki kez planlama yapılmasın diye bu kontrolü yapıyorum.
                    if (!forCompleted)
                    {
                        for (int k = 1; k < operations[i].Period; k++)
                        {
                            //Eğer ameliyat için yeterli süre varsa, atama yapacağım yere daha önce herhangi bir ameliyat planlamadıysam.
                            if (j < maximumPeriodLength - operations[i].Period && preview[i, j + k] == 0)
                            {
                                //Ameliyatların aynı odada yapılmasını sağlamak için yeni atayacağımız değeri, bir önceki  değere ameliyathane sayısı kadar ekleyerek buluyoruz. Yani a1'de başladıysa, 2 ameliyathane varsa bir sonraki değer a3 olmalı.
                                //Console.WriteLine("x[{0}, {1} + {2}] == x[{0}, {1} + {2} - 1] + {3}", i, j, k, operationRoomCount);
                                solver.Add(x[i, j + k] == x[i, j + k - 1] + roomCount);

                                //solver.Add(x[i, j + k - 1] % operationRoomCount == x[i, j + k] % operationRoomCount);

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

            for (int i = 1; i < operations.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    //Bundan önce yapılan tüm ameliyatların odaZaman değerleri, o an yapılan ameliyatın odaZaman değerlerinden farklı olsun.
                    var list = (from k in maximumLengthList.Take(operations[j].Period)
                                select x[j, k]).ToList();
                    for (int t = 0; t < operations[i].Period; t++)
                    {
                        list.Add(x[i, t]);
                        //Console.WriteLine("x[{0},{1}]", i, t);
                    }
                    solver.Add(list.ToArray().AllDifferent());
                }
            }

            //Burada da atama yapılmayan yerlere varsayilan bir deger atiyorum ki farkli ihtimallerde farkli senaryolar görünebilsin. Yoksa atama yapilmayan yerdeki ihtimalleri degistirip sonuçta ayni programi çikartiyordu.
            for (int i = 1; i < operations.Count; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    //Bundan önce yapilan tüm ameliyatlarin bitisinden maksimum süreye kadar olan degerleri, hep ayni olsun diyorum ki kendi üretmesini istedigim ihtimaller farklilassin.
                    var list = (from k in maximumLengthList.Skip(operations[j].Period)
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

            while (solver.NextSolution())
            {
                Console.WriteLine();
                num_solutions++;
                List<int> roomUsage = new List<int>();
                for (int i = 0; i < operations.Count; i++)
                {
                    Console.Write("Ameliyat #{0,-2}: ", operations[i].Id);
                    for (int j = 0; j < operations[i].Period; j++)
                    {
                        int v = (int)x[i, j].Value();
                        if (j == 0)
                        {
                            int valueIndex = Array.IndexOf(validStatus, v);
                            int room = v % roomCount != 0 ? v % roomCount : roomCount;
                            int time = room == roomCount ? (valueIndex + 1) / roomCount : ((valueIndex + 1) / roomCount) + 1;
                            var tomorrow = DateTime.Now.AddDays(1);
                            var dateTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, startingHour, startingMinute, 0);
                            dateTime = dateTime.AddMinutes((time - 1) * timePeriod);
                            Console.Write(room + ". Oda, Saat: " + dateTime.ToShortTimeString());

                            result.Add(new OperationPlan
                            {
                                OperationId = operations[i].Id,
                                RoomId = room,
                                StartDate = dateTime
                            });
                        }
                        roomUsage.Add(v);
                    }

                    Console.WriteLine();

                }
                Console.WriteLine();

                Console.WriteLine("Usage statistics per room:\n");
                for (int i = 0; i < roomCount; i++)
                {
                    Console.Write("Oda #{0,-2}: ", i + 1);
                    var usage = i + 1 == roomCount ? roomUsage.Count(p => (p % roomCount == 0)) : roomUsage.Count(p => (p % roomCount == i + 1));
                    var usageTimes = i + 1 == roomCount ? roomUsage.Where(p => (p % roomCount == 0)) : roomUsage.Where(p => (p % roomCount == i + 1));
                    int overTime = CalculateOverTime(usage, roomPeriodLength); //usageTimes.Count(t => t > operationRoomCount * roomPeriodLength[i]);
                    Console.Write("Uygunluk: {0}, Kullanılan Süre: {1}, Overtime: {2}", roomPeriodLength, usage, overTime);
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
            #region ParameterValues
            int operationRoomCount = 5;
            int maximumPeriodLength = 24;
            int roomPeriodLength = 18;
            int startingHour = 8;
            int startingMinute = 15;
            int timePeriod = 60;
            #endregion

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

            Solve(operations, operationRoomCount, maximumPeriodLength, roomPeriodLength, startingHour, startingMinute, timePeriod);
            Console.ReadKey();
        }
    }
}
