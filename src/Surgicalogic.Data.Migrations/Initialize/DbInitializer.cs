using Microsoft.EntityFrameworkCore;
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
                context.PersonnelCategories.Any())
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

                #region PersonnelCategories
                string[] PersonnelCategories = new string[] {
                    "Anestezi Uzmanı",
                    "Hemşire",
                    "Doktor"
                };

                var category = new Entities.PersonnelCategory();

                foreach (var item in PersonnelCategories)
                {
                    category = context.PersonnelCategories.Add(new Entities.PersonnelCategory
                    {
                        Name = item,
                        Description = item,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    }).Entity;
                }
                #endregion

                #region PersonnelTitles
                string[] PersonnelTitles = new string[] {
                    "Uzman",
                    "Doçent",
                    "Profesör"
                };

                var title = new Entities.PersonnelTitle();

                foreach (var item in PersonnelTitles)
                {
                    title = context.PersonnelTitles.Add(new Entities.PersonnelTitle
                    {
                        Name = item,
                        CreatedDate = DateTime.Now,
                        IsActive = true
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
                    PersonnelCategoryId = category.Id,
                    PersonnelTitleId = title.Id,
                    WorkTypeId = workType.Id,
                    PictureUrl = "tuba.jpg",
                    CreatedDate = DateTime.Now
                  },

                  new Personnel
                  {
                    PersonnelCode = "GK01",
                    FirstName = "Gürkan",
                    LastName = "Kesebir",
                    PersonnelCategoryId = category.Id,
                    PersonnelTitleId = title.Id,
                    WorkTypeId = workType.Id,
                    PictureUrl = "gurkan.jpg",
                    CreatedDate = DateTime.Now
                  },

                   new Personnel
                   {
                    PersonnelCode = "BK01",
                    FirstName = "Berk",
                    LastName = "Kuğu",
                    PersonnelCategoryId = category.Id,
                    PersonnelTitleId = title.Id,
                    WorkTypeId = workType.Id,
                    PictureUrl = "berk.jpg",
                    CreatedDate = DateTime.Now
                   },

                   new Personnel
                   {
                    PersonnelCode = "DD01",
                    FirstName = "Deniz",
                    LastName = "Dara",
                    PersonnelCategoryId = category.Id,
                    PersonnelTitleId = title.Id,
                    WorkTypeId = workType.Id,
                    PictureUrl = "deniz.jpg",
                    CreatedDate = DateTime.Now
                   },

                   new Personnel
                   {
                    PersonnelCode = "KS01",
                    FirstName = "Kenan",
                    LastName = "Su",
                    PersonnelCategoryId = category.Id,
                    PersonnelTitleId = title.Id,
                    WorkTypeId = workType.Id,
                    PictureUrl = "kenan.jpg",
                    CreatedDate = DateTime.Now
                   },

                   new Personnel
                   {
                    PersonnelCode = "MB01",
                    FirstName = "Mehmet",
                    LastName = "Bal",
                    PersonnelCategoryId = category.Id,
                    PersonnelTitleId = title.Id,
                    WorkTypeId = workType.Id,
                    PictureUrl = "mehmet.jpg",
                    CreatedDate = DateTime.Now
                   },

                   new Personnel
                   {
                    PersonnelCode = "SU01",
                    FirstName = "Selim",
                    LastName = "Ural",
                    PersonnelCategoryId = category.Id,
                    PersonnelTitleId = title.Id,
                    WorkTypeId = workType.Id,
                    PictureUrl = "selim.jpg",
                    CreatedDate = DateTime.Now
                   },

                   new Personnel
                   {
                    PersonnelCode = "UK01",
                    FirstName = "Uğur",
                    LastName = "Kara",
                    PersonnelCategoryId = category.Id,
                    PersonnelTitleId = title.Id,
                    WorkTypeId = workType.Id,
                    PictureUrl = "ugur.jpg",
                    CreatedDate = DateTime.Now
                   }
                };

                context.Personnels.AddRange(PersonnelsList);
                #endregion

                #region OperatingRooms
                for (int i = 0; i <= 4; i++)
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

                #region SettingDataTypes
                context.SettingDataTypes.Add(new Entities.SettingDataType
                {
                    Name = "Metin",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.SaveChanges();

                context.SettingDataTypes.Add(new Entities.SettingDataType
                {
                    Name = "Sayı",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.SaveChanges();

                context.SettingDataTypes.Add(new Entities.SettingDataType
                {
                    Name = "Zaman",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });
                #endregion

                #region OperatingRoomEquipments

                //for (int i = 1; i <= 8; i++)
                //{
                //    for (int k = 0; k < _random.Next(1, 5); k++)
                //    {
                //        context.OperatingRoomEquipments.Add(new Entities.OperatingRoomEquipment
                //        {
                //            OperatingRoomId = i,
                //            EquipmentId = _random.Next(1, 30),
                //            CreatedDate = DateTime.Now
                //        });
                //    }

                //}

                #endregion

                #region Patients

                for (int i = 1; i <= 8; i++)
                {
                    context.Patients.Add(new Entities.Patient
                    {
                        FirstName = "İsim " + i,
                        LastName = "Soyisim " + i,
                        CreatedDate = DateTime.Now,
                        CreatedBy = 1,
                        IsActive = true
                    });
                }

                #endregion

                #region SaveChanges
                //Saving here because needed above records IDs below.
                context.SaveChanges();
                #endregion

                #region Operations

                for (int i = 1; i <= 8; i++)
                {
                    context.Operations.Add(new Entities.Operation
                    {
                        Name = "Operasyon " + i,
                        Description = "Operasyon " + i,
                        OperationTypeId = i,
                        EventNumber = _random.Next(1000, 2000).ToString(),
                        OperationTime = _random.Next(5, 25) * 5,
                        Date = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day),
                        CreatedDate = DateTime.Now,
                        CreatedBy = 1,
                        PatientId = i,
                        IsActive = true
                    });
                }

                #endregion

                #region OperatingRoomOperationTypes

                for (int i = 1; i <= 5; i++)
                {
                    for (int k = 1; k <= 8; k++)
                    {
                        context.OperatingRoomOperationTypes.Add(new Entities.OperatingRoomOperationType
                        {
                            OperationTypeId = k,
                            OperatingRoomId = i,
                            CreatedDate = DateTime.Now,
                            CreatedBy = 1,
                            IsActive = true
                        });
                    }
                }
                #endregion

                #region SettingDataTypes

                context.SettingDataTypes.Add(new SettingDataType
                {
                    Name = "String",
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                });

                context.SaveChanges();

                context.SettingDataTypes.Add(new SettingDataType
                {
                    Name = "Int",
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                });

                context.SaveChanges();

                context.SettingDataTypes.Add(new SettingDataType
                {
                    Name = "Time",
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                });

                context.SaveChanges();

                context.SettingDataTypes.Add(new SettingDataType
                {
                    Name = "Double",
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                });

                context.SaveChanges();

                #endregion

                #region Settings
                context.Settings.Add(new Entities.Setting
                {
                    Key = "OperationPeriodInMinutes",
                    Name = "Ameliyat Periyod Süresi",
                    IntValue = 15,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "OperationWorkingHourStart",
                    Name = "Ameliyat Mesai Başlangıcı",
                    TimeValue = "08:00",
                    SettingDataTypeId = 3,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "OperationWorkingHourEnd",
                    Name = "Ameliyat Mesai Bitişi",
                    TimeValue = "17:00",
                    SettingDataTypeId = 3,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "ClinicPeriodInMinutes",
                    Name = "Klinik Periyod Süresi",
                    IntValue = 15,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "ClinicWorkingHourStart",
                    Name = "Klinik Mesai Başlangıcı",
                    TimeValue = "08:00",
                    SettingDataTypeId = 3,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "ClinicWorkingHourEnd",
                    Name = "Klinik Mesai Bitişi",
                    TimeValue = "17:00",
                    SettingDataTypeId = 3,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "ClinicPersonPerPeriod",
                    Name = "Klinik Periyod Başı Hasta Sayısı",
                    IntValue = 1,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "ClinicAppointmentDays",
                    Name = "Klinik Randevu Verilecek Gün Sayısı",
                    IntValue = 15,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "SumPatientNumber",
                    Name = "Toplam Hasta Sayısı",
                    IntValue = 12,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "ForAverageTimeEveryPatient",
                    Name = "Her Hasta İçin Ortalama Süre(dk)",
                    IntValue = 14,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "ForStandardDeviationAverageTime",
                    Name = "Ortalama Süre İçin Standart Sapma(dk)",
                    DoubleValue = 6.75,
                    SettingDataTypeId = 7,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "PatientsFailureRate",
                    Name = "Hastaların Gelmeme Oranı(%)",
                    IntValue = 12,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "WithoutAppointmentPatientRate",
                    Name = "Randevusuz Gelen Hasta Oranı(%)",
                    IntValue = 28,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "DoctorTimePatientTimeRate",
                    Name = "Doktor Zamanı/Hasta Zamanı",
                    IntValue = 10,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                context.Settings.Add(new Entities.Setting
                {
                    Key = "RoundingIntervalValue",
                    Name = "Yuvarlama Aralık Değeri",
                    IntValue = 10,
                    SettingDataTypeId = 2,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    IsActive = true
                });

                #endregion

                #region SaveChanges
                context.SaveChanges();
                #endregion

                #region InsertRole
                context.Database.ExecuteSqlCommand("insert into [dbo].[AspNetRoles](Name,NormalizedName) values('Admin', 'ADMIN')");
                context.Database.ExecuteSqlCommand("insert into [dbo].[AspNetRoles](Name,NormalizedName) values('Member', 'MEMBER')");
                #endregion

            }
            catch (Exception ec)
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
                context.PersonnelCategories.Any())
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