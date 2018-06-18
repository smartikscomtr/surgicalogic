using Google.OrTools.ConstraintSolver;
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
        /// <param name="operationCount">Planlanacak ameliyat sayısı.</param>
        /// <param name="operationRoomCount">Ameliyathane sayısı</param>
        /// <param name="operationTimes">Ameliyatların süresini sırasıyla içerir. (temizlik, hazırlık vs. dahil)</param>
        /// <param name="maximumPeriodLength">Overtime dahil ameliyatlara ayrılacak maksimum süre.</param>
        /// <param name="operationRoomTimes">Ameliyathanelerin uygunluk süresini belirtir. Maksimum değer, mesai süresi olmalı.</param>
        /// <param name="operationRoomRestriction">Hangi ameliyat hangi odada yapılabilir kısıtı. Örneğin 1. ameliyat 1. odada yapılır, 2. odada yapılamaz, 3. ameliyat 1. odada yapılamaz 2. odada yapılabilir.</param>
        /// <param name="operationDoctor">Ameliyatı hangi doktorun yapacağı bilgisini dizi olarak tutan değişken.</param>
        /// <param name="startingHour">Ameliyatların saat kaçta başlayacağı bilgisi</param>
        /// <param name="startingMinute">Ameliyatların hangi dakikada başlayacağı bilgisi. Buçukta da başlayabilirse diye.</param>
        /// <param name="timePeriod">Ameliyat periyodlarının kaç dakika olduğu bilgisi</param>
        public static List<OperationPlan> Solve(int operationCount, int operationRoomCount, int[] operationTimes, int maximumPeriodLength, int[] operationRoomTimes, int[,] operationRoomRestriction, int[] operationDoctor, int startingHour, int startingMinute, int timePeriod)
        {
            Console.WriteLine("Uygulama başlıyor.");
            var result = new List<OperationPlan>();

            Solver solver = new Solver("SurgicaLogic");

            var roomPeriodIndex = Enumerable.Range(1, operationRoomCount * maximumPeriodLength).ToList();

            //Buraya dummy bir status daha ekliyorum. Bunu, ameliyat süresinden daha ilerideki ihtimallere atayacağım.
            var dummyIndex = operationRoomCount * maximumPeriodLength + 100;
            roomPeriodIndex.Add(dummyIndex);
            //Geçerli statüler. Maksimum süre dahil tüm statüleri içerir. Örnegin 2 ameliyathane olan bir senaryoda 1= 1.ameliyathanenin 1. periyodunu, 2= 2. ameliyathanenin 1.periyodunu, 3= 1.ameliyathanenin 2. periyodunu, 4= 2.ameliyathanenin 2. periyodunu vs. temsil eder.
            var validStatus = roomPeriodIndex.ToArray();

            //AllDifferent methodunda kullanmak üzere maksimum süreyi ienumerable olarak tutan değişkenler.
            IEnumerable<int> maximumLengthList = Enumerable.Range(0, maximumPeriodLength);

            //Karar mekanizmasının davranışını tanımlıyorum. Belli durumlara göre bunu değiştirebiliyorum.
            int variableStart = Solver.CHOOSE_MIN_SIZE;

            #region Ameliyathanelerin uygunluklarını burada ayarlıyorum.
            //Odaların sahip olduğu en uzun süreyi alıyorum.
            int optimalRoomAvailability = operationRoomTimes.Max();

            for (int i = 0; i < operationRoomCount; i++)
            {
                //Eğer bu ameliyathane en uzun süreye sahip ameliyathaneden daha kısa süre kullanılacaksa, en uzun süreye kadar olan değerlerini en uzun süreye sahip odanın değerlerinden daha yüksek yapıyoruz ki önce onlara ameliyat ataması yapılsın.
                //Örneğin 2 ameliyathane var birinci 10, ikinci 3 saat kullanılabilir. İkinci ameliyathanenin 4. saat ve sonraki değerlerini birinci ameliyathanenin 10. saat değerinden daha büyük yapıyorum.
                if (operationRoomTimes[i] < optimalRoomAvailability)
                {
                    //Daha doğru sonuç verdiği için karar değişkenininin davranışını değiştiriyorum.
                    variableStart = Solver.CHOOSE_MIN_SLACK_RANK_FORWARD;
                    //Ameliyathane uygunluk süreleri birbirinden farklıysa validStatus değerlerini değiştiriyorum. 
                    for (int j = operationRoomTimes[i]; j < (validStatus.Length - 1) / operationRoomCount; j++)
                    {
                        validStatus[(j * operationRoomCount) + i] += (optimalRoomAvailability - operationRoomTimes[i]) * operationRoomCount;
                    }
                }
            }

            #endregion

            //Ameliyatların hangi oda-zaman diliminde yapılacağına karar verilen değişken. Tüm ihtimaller bu değişken içerisine tanımlanıyor.
            IntVar[,] x =
                solver.MakeIntVarMatrix(operationCount, maximumPeriodLength, validStatus, "x");

            //Yukarıdaki değişkeni çözümleyebilmek için tek boyutlu hali.
            IntVar[] x_flat = x.Flatten();

            //Hangi zaman dilimlerine atama yaptığımı anlayabilmek için ameliyat - süre ilişkisini iki boyutlu dizi olarak tutuyorum. İki ameliyatın aynı oda-zaman dilimine atanmasını bu şekilde engelliyorum.
            int[,] preview = new int[operationCount, maximumPeriodLength];

            //Ameliyatların aynı odada devam edebilmesi ve önceki bir zamana atama yapmaması için, bir sonraki değerin bir önceki değerden en az oda sayısı kadar büyük olması olması kuralı.
            for (int i = operationCount - 1; i >= 0; i--)
            {
                for (int j = operationTimes[i] - 1; j > 0; j--)
                {
                    for (int k = 1; k < operationRoomCount; k++)
                    {
                        solver.Add(x[i, j] != dummyIndex);
                        solver.Add(x[i, j] > x[i, j - 1] + k);
                    }
                }
            }

            for (int i = 0; i < operationCount; i++)
            {
                int doctorId = operationDoctor[i];

                //Bir ameliyat bir odada veya bir zamanda yapılamaz bilgisi bu değişkende tutuluyor.
                int[] blockedTimes = GetBlockedTimes(operationRoomRestriction, i, operationRoomCount, maximumPeriodLength, validStatus);

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
                for (int ad = 0; ad < operationTimes.Length; ad++)
                {
                    //Eğer sonraki ameliyatı yapacak doktorlardan birisi, bu ameliyatı yapan doktor ise.
                    if (operationDoctor[ad] == doctorId && ad != i)
                    {
                        //Bir sonraki ameliyat süresi kadar dön.
                        for (int ml = 0; ml < operationTimes[ad]; ml++)
                        {
                            //Mevcut ameliyatın süresi kadar dön.
                            for (int t = 0; t < operationTimes[i]; t++)
                            {
                                //Ameliyat odası sayısı kadar dön.
                                for (int r = 1; r < operationRoomCount; r++)
                                {
                                    //Console.WriteLine("x[{0}, {1}] % {2} <= {5} && x[{0}, {1}] % {2} != 0  ? x[{3}, {4}] != x[{0}, {1}] + ({2} - {5}) : x[0, 0] == x[0, 0]", i, t, operationRoomCount, ad, ml, r);
                                    solver.Add(x[i, t] % operationRoomCount <= r && x[i, t] % operationRoomCount != 0 ? x[ad, ml] != x[i, t] + (operationRoomCount - r) : x[0, 0] == x[0, 0]);
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
                        for (int k = 1; k < operationTimes[i]; k++)
                        {
                            //Eğer ameliyat için yeterli süre varsa, atama yapacağım yere daha önce herhangi bir ameliyat planlamadıysam.
                            if (j < maximumPeriodLength - operationTimes[i] && preview[i, j + k] == 0)
                            {
                                //Ameliyatların aynı odada yapılmasını sağlamak için yeni atayacağımız değeri, bir önceki  değere ameliyathane sayısı kadar ekleyerek buluyoruz. Yani a1'de başladıysa, 2 ameliyathane varsa bir sonraki değer a3 olmalı.
                                //Console.WriteLine("x[{0}, {1} + {2}] == x[{0}, {1} + {2} - 1] + {3}", i, j, k, operationRoomCount);
                                solver.Add(x[i, j + k] == x[i, j + k - 1] + operationRoomCount);

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

            for (int i = 1; i < operationCount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    //Bundan önce yapılan tüm ameliyatların odaZaman değerleri, o an yapılan ameliyatın odaZaman değerlerinden farklı olsun.
                    var list = (from k in maximumLengthList.Take(operationTimes[j])
                                select x[j, k]).ToList();
                    for (int t = 0; t < operationTimes[i]; t++)
                    {
                        list.Add(x[i, t]);
                        //Console.WriteLine("x[{0},{1}]", i, t);
                    }
                    solver.Add(list.ToArray().AllDifferent());
                }
            }

            //Burada da atama yapilmayan yerlere va/rsayilan bir deger atiyorum ki farkli ihtimallerde farkli senaryolar görünebilsin. Yoksa atama yapilmayan yerdeki ihtimalleri degistirip sonuçta ayni programi çikartiyordu.
            for (int i = 1; i < operationCount; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    //Bundan önce yapilan tüm ameliyatlarin bitisinden maksimum süreye kadar olan degerleri, hep ayni olsun diyorum ki kendi üretmesini istedigim ihtimaller farklilassin.
                    var list = (from k in maximumLengthList.Skip(operationTimes[j])
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
            //    for (int i = 0; i < operationCount; i++)
            //    {
            //        Console.Write("ameliyat #{0,-2}: ", i + 1);
            //        for (int j = 0; j < operationTimes[i]; j++)
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
                for (int i = 0; i < operationCount; i++)
                {
                    Console.Write("Ameliyat #{0,-2}: ", i + 1);
                    for (int j = 0; j < operationTimes[i]; j++)
                    {
                        int v = (int)x[i, j].Value();
                        if (j == 0)
                        {
                            int valueIndex = Array.IndexOf(validStatus, v);
                            int room = v % operationRoomCount != 0 ? v % operationRoomCount : operationRoomCount;
                            int time = room == operationRoomCount ? (valueIndex + 1) / operationRoomCount : ((valueIndex + 1) / operationRoomCount) + 1;
                            var tomorrow = DateTime.Now.AddDays(1);
                            var dateTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, startingHour, startingMinute, 0);
                            dateTime = dateTime.AddMinutes((time - 1) * timePeriod);
                            Console.Write(room + ". Oda, Saat: " + dateTime.ToShortTimeString());

                            result.Add(new OperationPlan
                            {
                                OperationId = i + 1,
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
                for (int i = 0; i < operationRoomCount; i++)
                {
                    Console.Write("Oda #{0,-2}: ", i + 1);
                    var usage = i + 1 == operationRoomCount ? roomUsage.Count(p => (p % operationRoomCount == 0)) : roomUsage.Count(p => (p % operationRoomCount == i + 1));
                    int overTime = CalculateOverTime(usage, operationRoomTimes[i]);
                    Console.Write("Uygunluk: {0}, Kullanılan Süre: {1}, Overtime: {2}", operationRoomTimes[i], usage, overTime);
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
        private static int[] GetBlockedTimes(int[,] operationRoomRestriction, int ameliyatId, int operationRoomCount, int maximumLength, int[] validStatus)
        {
            List<int> blockedTimes = new List<int>();
            for (int i = 0; i < operationRoomCount; i++)
            {
                //Bu ameliyat, bu ameliyathanede yapılamazsa
                if (operationRoomRestriction[ameliyatId, i] == 0)
                {
                    for (int j = 0; j < maximumLength; j++)
                    {
                        blockedTimes.Add(validStatus[(i + (j * operationRoomCount))]);
                    }
                }
            }

            return blockedTimes.ToArray();
        }

        public static void Main(String[] args)
        {
            int ameliyatSayisi = 0;
            int ameliyathaneSayisi = 0;
            int maksimumSure = 0;
            List<int> ameliyatSureleri = new List<int>();
            List<int> ameliyathaneSureleri = new List<int>();
            List<int> ameliyatDoktor = new List<int>();
            bool odaKisiti = true;
            int startingHour = 0;
            int startingMinute = -1;
            int timePeriod = 0;

            Console.Write("Lütfen ameliyat sayısını giriniz: ");
            while (ameliyatSayisi == 0)
            {
                if (!int.TryParse(Console.ReadLine(), out ameliyatSayisi))
                {
                    Console.Write("Lütfen geçerli bir sayı giriniz: ");

                }
            }

            Console.Write("Lütfen ameliyathane sayısını giriniz: ");
            while (ameliyathaneSayisi == 0)
            {
                if (!int.TryParse(Console.ReadLine(), out ameliyathaneSayisi))
                {
                    Console.Write("Lütfen geçerli bir sayı giriniz: ");

                }
            }

            Console.Write("Ameliyat sürelerini kaç dakikalık periyod olarak belirleyeceksiniz: ");
            while (timePeriod == 0)
            {
                if (!int.TryParse(Console.ReadLine(), out timePeriod))
                {
                    Console.Write("Lütfen geçerli bir sayı giriniz: ");

                }
            }

            for (int i = 0; i < ameliyatSayisi; i++)
            {
                int ameliyatSuresi = 0;
                Console.Write("Lütfen {0}. ameliyatın kaç periyod süreceğini giriniz: ", i + 1);
                while (ameliyatSuresi == 0)
                {
                    if (!int.TryParse(Console.ReadLine(), out ameliyatSuresi))
                    {
                        Console.Write("Lütfen geçerli bir sayı giriniz: ");
                    }
                    else
                    {
                        ameliyatSureleri.Add(ameliyatSuresi);
                    }
                }

                int doktorId = 0;
                Console.Write("Lütfen {0}. ameliyatı yapacak doktorun ID'sini giriniz: ", i + 1);
                while (doktorId == 0)
                {
                    if (!int.TryParse(Console.ReadLine(), out doktorId))
                    {
                        Console.Write("Lütfen geçerli bir ID giriniz: ");
                    }
                    else
                    {
                        ameliyatDoktor.Add(doktorId);
                    }
                }
            }

            Console.Write("Lütfen overtime dahil ameliyat süresinin kaç periyod olacağını giriniz: ");
            while (maksimumSure == 0)
            {
                if (!int.TryParse(Console.ReadLine(), out maksimumSure))
                {
                    Console.Write("Lütfen geçerli bir sayı giriniz: ");
                }
                else if (ameliyatSureleri.Any(x => x > maksimumSure))
                {
                    maksimumSure = 0;
                    Console.Write("Lütfen en uzun ameliyat süresine eşit veya büyük bir değer giriniz: ");
                }
            }

            for (int i = 0; i < ameliyathaneSayisi; i++)
            {
                int ameliyathaneSuresi = 0;
                Console.Write("Lütfen {0}. ameliyathanenin kaç periyod uygun olacağını giriniz: ", i + 1);
                while (ameliyathaneSuresi == 0)
                {
                    if (!int.TryParse(Console.ReadLine(), out ameliyathaneSuresi))
                    {
                        Console.Write("Lütfen geçerli bir sayı giriniz: ");
                    }
                    else
                    {
                        ameliyathaneSureleri.Add(ameliyathaneSuresi);
                    }
                }
            }

            int[,] ameliyatOdaKisiti = new int[ameliyatSayisi, ameliyathaneSayisi];

            for (int i = 0; i < ameliyatSayisi; i++)
            {
                for (int j = 0; j < ameliyathaneSayisi; j++)
                {
                    ameliyatOdaKisiti[i, j] = 1;
                }
            }

            while (odaKisiti)
            {
                Console.Write("Oda - Ameliyat kısıtlaması yapmak istiyor musunuz? (E/H): ");
                string val = Console.ReadLine();
                if (val.ToUpper() == "H")
                {
                    odaKisiti = false;
                }
                else if (val.ToUpper() == "E")
                {
                    int ameliyatNo = 0;
                    Console.Write("Lütfen ameliyat numarasını giriniz: ");
                    while (ameliyatNo == 0)
                    {
                        if (!int.TryParse(Console.ReadLine(), out ameliyatNo))
                        {
                            Console.Write("Lütfen geçerli bir sayı giriniz: ");
                        }
                    }

                    int odaNo = 0;
                    Console.Write("Lütfen {0} numaralı ameliyatın yapılamayacağı oda numarasını giriniz: ", ameliyatNo);
                    while (odaNo == 0)
                    {
                        if (!int.TryParse(Console.ReadLine(), out odaNo))
                        {
                            Console.Write("Lütfen geçerli bir sayı giriniz: ");
                        }
                    }

                    ameliyatOdaKisiti[ameliyatNo - 1, odaNo - 1] = 0;
                    Console.WriteLine("Tanımlama başarılı bir şekilde yapılmıştır.");
                }
                else
                {
                    Console.WriteLine("Bunu hayır olarak kabul ediyorum.");
                    odaKisiti = false;
                }
            }

            Console.Write("Lütfen ameliyatların başlangıç saatini giriniz: ");
            while (startingHour == 0)
            {
                if (!int.TryParse(Console.ReadLine(), out startingHour))
                {
                    Console.Write("Lütfen geçerli bir sayı giriniz: ");

                }
            }

            Console.Write("Lütfen ameliyatların başlangıç dakikasını giriniz: ");
            while (startingMinute < 0)
            {
                if (!int.TryParse(Console.ReadLine(), out startingMinute))
                {
                    Console.Write("Lütfen geçerli bir sayı giriniz: ");

                }
            }

            var result = Solve(ameliyatSayisi, ameliyathaneSayisi, ameliyatSureleri.ToArray(), maksimumSure, ameliyathaneSureleri.ToArray(), ameliyatOdaKisiti, ameliyatDoktor.ToArray(), startingHour, startingMinute, timePeriod);
            Console.ReadKey();
        }
    }
}
