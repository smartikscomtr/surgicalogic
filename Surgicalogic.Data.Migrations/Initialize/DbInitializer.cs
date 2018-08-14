using Smartiks.Framework.IO;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Surgicalogic.Data.Migrations.Initialize
{
    public static class DbInitializer
    {
        public const string MetaSheetName = "Meta";

        static Random _random = new Random();

        public static void Seed(DataContext context)
        {
            try
            {
                if (context.WorkTypes.Any() ||
                context.EquipmentTypes.Any() ||
                context.Equipments.Any() ||
                context.Branches.Any() ||
                context.OperatingRooms.Any() ||
                context.Personnels.Any() ||
                context.PersonnelTitles.Any())
                {
                    return; // DB has been seeded
                }

                #region Branches
                var branch = new Entities.Branch();

                string[] branches = new string[] {
                                    "Acil Tıp",
                                    "Anesteziyoloji ve Reanimasyon",
                                    "Beyin ve Sinir Cerrahisi",
                                    "Cerrahi Onkoloji",
                                    "Çocuk Cerrahisi",
                                    "Çocuk Endokrinolojisi",
                                    "Çocuk Enfeksiyon Hastalıkları",
                                    "Çocuk Gastroentrolojisi",
                                    "Çocuk Hematolojisi Ve Onkolojisi",
                                    "Çocuk Kardiyolojisi",
                                    "Çocuk Nörolojisi",
                                    "Çocuk Sağlığı ve Hastalıkları",
                                    "Çocuk Ve Ergen Ruh Sağlığı Ve Hastalıkları",
                                    "Deri Ve Zührevi Hastalıkları",
                                    "Endokrinoloji ve Metabolizma Hastalıkları",
                                    "Enfeksiyon Hastalıkları",
                                    "Fiziksel Tıp ve Rehabilitasyon",
                                    "Gastroenteroloji",
                                    "Gastroentroloji Cerrahisi",
                                    "Göğüs Cerrahisi",
                                    "Göğüs Hastalıkları",
                                    "Göğüs Hastalıkları Ve Tüberküloz",
                                    "Göz Hastalıkları",
                                    "Halk Sağlığı",
                                    "Hematoloji",
                                    "Histoloji Ve Embriyoloji",
                                    "İç Hastalıkları",
                                    "Kadın Hastalıkları ve Doğum",
                                    "Kalp Damar Hastalıkları",
                                    "Kalp ve Damar Cerrahisi",
                                    "Kardiyoloji",
                                    "Kulak Burun Boğaz Hastalıkları",
                                    "Mikrobiyoloji",
                                    "Nefroloji",
                                    "Neonatoloji",
                                    "Nöroloji",
                                    "Nükleer Tıp",
                                    "Ortopedi ve Travmatoloji",
                                    "Perinatoloji",
                                    "Plastik, Rekonstrüktif ve Estetik Cerrahi",
                                    "Pratisyen Hekim",
                                    "Radyasyon Onkolojisi",
                                    "Radyoloji",
                                    "Romatoloji",
                                    "Ruh Sağlığı Ve Hastalıkları",
                                    "Spor Hekimliği",
                                    "Tıbbi Biyokimya",
                                    "Tıbbi Genetik",
                                    "Tıbbi Mikrobiyoloji",
                                    "Tıbbi Onkoloji",
                                    "Tıbbi Patoloji",
                                    "Üroloji",
                                    "Genel Cerrahi"
                };

                foreach (var item in branches)
                {
                    branch = context.Branches.Add(new Entities.Branch
                    {
                        Name = item,
                        Description = item,
                        CreatedDate = DateTime.Now
                    }).Entity;
                }
                #endregion

                #region PersonnelTitles
                string[] personnelTitles = new string[] {
                    "Çocuk Gelişimcisi",
                    "Diyetisyen",
                    "Ebe",
                    "Eczacı",
                    "Fizyoterapist",
                    "Güvenlik",
                    "Hastane Müdür Yardımcısı",
                    "Hemşire",
                    "Hizmetli",
                    "İmam",
                    "Kimyager",
                    "Memur",
                    "Şoför",
                    "Tekniker",
                    "Teknisyen",
                    "Terzi",
                    "Doktor"
                };

                var title = new Entities.PersonnelTitle();

                foreach (var item in personnelTitles)
                {
                    title = context.PersonnelTitles.Add(new Entities.PersonnelTitle
                    {
                        Name = item,
                        Description = item,
                        CreatedDate = DateTime.Now
                    }).Entity;
                }
                #endregion

                #region WorkTypes
                var workType = new Entities.WorkType();

                string[] workTypes = new string[] { "Kadrolu Personel", "Sözleşmeli Personel" };

                foreach (var item in workTypes)
                {
                    workType = context.WorkTypes.Add(new Entities.WorkType
                    {
                        Name = item,
                        Description = item,
                        CreatedDate = DateTime.Now
                    }).Entity;
                }
                #endregion

                #region EquipmentType
                string[] equipmentTypes = new string[] {
                    "Aspirasyon Cihazları",
                    "Santrifüj Cihazları",
                    "EKG Cihazları",
                    "Sarf Malzemeler",
                    "Defibrilatörler",
                    "Cerrahi Aletler",
                    "Solunum Cihazları & Ürünleri",
                    "Spirometre & Peak Flow Metreler",
                    "Hastane Demirbaş Malzemeleri",
                    "Yatan Hasta Ürünleri",
                    "İlk Yardım Ürünleri",
                    "Fizik Tedavi Ürünleri",
                    "Diagnostik (Tanı) Ürünleri",
                    "Ortopedik Ürünler",
                    "Tansiyon Aletleri",
                    "Steteskoplar",
                    "Anne & Bebek Sağlığı",
                    "Boy ve Kilo Ölçerler",
                    "Diş Hekimliği Malzemeleri",
                    "Eğitim Maketleri",
                    "Diyabet Ürünleri",
                    "Dijital Derece & Ateş Ölçerler",
                    "Fetal Monitör ve Dopplerler",
                    "Alkolmetre Cihazları",
                    "Sterilizatörler",
                    "OSGB Malzemeleri",
                    "Laboratuvar Cihazları & Ürünleri",
                };

                var equipmentType = new Entities.EquipmentType();

                foreach (var item in equipmentTypes)
                {
                    equipmentType = context.EquipmentTypes.Add(new Entities.EquipmentType
                    {
                        Name = item,
                        Description = item,
                        CreatedDate = DateTime.Now
                    }).Entity;
                }
                #endregion

                #region SaveChanges
                //Saving here because needed above records IDs below.
                context.SaveChanges();
                #endregion

                #region Equipments
                string[] equipments = new string[] {
                    "Hegar-olsen makaslı portequei 14 cm",
                    "Penset 13 cm eğri",
                    "Mosquito pensi 13 cm düz",
                    "Mathieu portequei 14 cm",
                    "Penset 18 cm dişsiz",
                    "Mosquito pensi 13 cm eğri",
                    "Mathieu portequei 20 cm",
                    "Kıymık penseti 11.5 cm",
                    "Hemostatik pensi 14 cm düz",
                    "Mayo-hegar portequei 13 cm tungsten",
                    "Penset 13 cm dişsiz",
                    "Cerrahi makas 14 cm K/K düz",
                    "Bistüri sapı no:3",
                    "Penset 13 cm dişli",
                    "Cerrahi makas 14 cm K/ S düz",
                    "Bistüri sapı no:4",
                    "Ekartör Senn - miller 16 cm",
                    "C.makas 14 cm - Bir ucu düğmeli",
                    "Ekartör Gelpi 18 cm",
                    "Bağırsak pensi 23 cm düz",
                    "Sütür makası 10.5 cm",
                    "Ekartör Weitlaner 14 cm",
                    "Serviyet pensi 9 cm",
                    "Cerrahi makas 14 cm K/ S eğri",
                    "Volkman küret 17 cm",
                    "Allis pensi 16 cm",
                    "Cerrahi makas 16 cm K/ S eğri",
                    "Oluklu sonda 14.5 cm",
                    "Bandaj makası 18 cm",
                    "Cerrahi makas 14 cm K/ K eğri",
                    "Stile 14.5 cm",
                    "Kemik pensi 14 cm",
                    "Babcock´s göz pensi",
                    "Stile 16 cm",
                    "KH Tırnak kesme pensi 11 cm"
                };
                int codeIndex = 1;
                foreach (var item in equipments)
                {
                    context.Equipments.Add(new Entities.Equipment
                    {
                        EquipmentTypeId = equipmentType.Id,
                        Name = item,
                        Code = "A00" + codeIndex,
                        Description = item,
                        CreatedDate = DateTime.Now
                    });
                    codeIndex++;
                }
                #endregion

                #region Personnels
                var PersonnelsList = new List<Personnel>()
                {
                  new Personnel
                  {
                    PersonnelCode = "TB01",
                    FirstName = "Tuba",
                    LastName = "Bayraktutar",
                    PersonnelTitleId = title.Id,
                    BranchId = branch.Id,
                    WorkTypeId = workType.Id,
                    CreatedDate = DateTime.Now
                  },

                  new Personnel
                  {
                    PersonnelCode = "GK01",
                    FirstName = "Gürkan",
                    LastName = "Kesebir",
                    PersonnelTitleId = title.Id,
                    BranchId = branch.Id,
                    WorkTypeId = workType.Id,
                    CreatedDate = DateTime.Now
                  },

                   new Personnel
                   {
                    PersonnelCode = "HC01",
                    FirstName = "Hasan",
                    LastName = "Çolak",
                    PersonnelTitleId = title.Id,
                    BranchId = branch.Id,
                    WorkTypeId = workType.Id,
                    CreatedDate = DateTime.Now
                   }
                };

                context.Personnels.AddRange(PersonnelsList);
                #endregion

                #region OperatingRooms
                for (int i = 0; i <= 10; i++)
                {
                    context.OperatingRooms.Add(new Entities.OperatingRoom
                    {
                        Name = GetLetter(),
                        Height = _random.Next(190, 300),
                        Width = _random.Next(200, 600),
                        Length = _random.Next(200, 500),
                        CreatedDate = DateTime.Now
                    });
                }
                #endregion

                #region OperationTypes
                for (int i = 1; i <= 8; i++)
                {

                        context.OperationTypes.Add(new Entities.OperationType
                        {
                            Name = "Tip " + i,
                            Description = "Tip " + i,
                            BranchId = i,
                            CreatedDate = DateTime.Now,
                            CreatedBy = 1,
                            IsActive = true
                        });
                }
                #endregion

                #region SaveChanges
                context.SaveChanges();
                #endregion

                #region OperatingRoomEquipments

                for (int i = 1; i <= 8; i++)
                {
                    for (int k = 0; k < _random.Next(1, 5); k++)
                    {
                        context.OperatingRoomEquipments.Add(new Entities.OperatingRoomEquipment
                        {
                            OperatingRoomId = i,
                            EquipmentId = _random.Next(1, 30),
                            CreatedDate = DateTime.Now
                        });
                    }

                }

                for (int i = 1; i <= 8; i++)
                {
                    context.Operations.Add(new Entities.Operation
                    {
                        Name = "Operasyon " + i,
                        Description = "Operasyon " + i,
                        OperationTypeId = i,
                        OperationTime = _random.Next(5, 25) * 5,
                        Date = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day),
                        CreatedDate = DateTime.Now,
                        CreatedBy = 1,
                        IsActive = true
                    });
                }

                #endregion

                #region SaveChanges
                context.SaveChanges();
                #endregion

            }
            catch (Exception)
            { }

        }

        public static void InitializeDataFromExcel(DataContext context, string excelFilePath)
        {
            if (context.WorkTypes.Any() ||
                context.EquipmentTypes.Any() ||
                context.Equipments.Any() ||
                context.Branches.Any() ||
                context.OperatingRooms.Any() ||
                context.Personnels.Any() ||
                context.PersonnelTitles.Any())
            {
                return; // DB has been seeded
            }

            ExcelDocumentService excelDocumentService = new ExcelDocumentService();

            var path = Path.Combine(Environment.CurrentDirectory, excelFilePath);

            FileStream fs = new FileStream(path, FileMode.Open);

            var worksheetNames = excelDocumentService.GetWorksheetNames(fs);
            var metaType = Type.GetType($"Surgicalogic.Model.CommonModel.InitialWorkSheetMetaModel, Surgicalogic.Model");
            var workSheetObject = excelDocumentService.Read(fs, MetaSheetName, metaType, System.Globalization.CultureInfo.CurrentCulture);
            var workSheetData = ConvertWorkSheetObjectToModel(workSheetObject);

            foreach (var item in worksheetNames)
            {
                if (item != MetaSheetName)
                {
                    var entityName = workSheetData.FirstOrDefault(x => x.Worksheet == item)?.Entity;
                    var type = Type.GetType($"Surgicalogic.Data.Entities.{entityName}, Surgicalogic.Data");

                    if (type == null)
                    {
                        return;
                    }

                    var excelData = excelDocumentService.Read(fs, item, type, System.Globalization.CultureInfo.CurrentCulture);

                    context.AddRange(excelData);
                    context.SaveChanges();
                }
            }
        }

        private static List<InitialWorkSheetMetaModel> ConvertWorkSheetObjectToModel(IReadOnlyCollection<object> workSheetObject)
        {
            var result = new List<InitialWorkSheetMetaModel>();

            foreach (var item in workSheetObject)
            {
                result.Add((InitialWorkSheetMetaModel)item);
            }

            return result;
        }

        public static string GetLetter()
        {
            int num = _random.Next(0, 26);
            char let = (char)('a' + num);

            return let + (_random.Next(1, 10)).ToString();
        }
    }
}