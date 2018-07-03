using Smartiks.Framework.IO;
using Surgicalogic.Data.DbContexts;
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
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated(); //if db is not exist ,it will create database .but ,do nothing .

            if (context.WorkType.Any() || 
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
            var path = Path.Combine(Environment.CurrentDirectory, "App_Data", "Migration", "InitialData.xlsx");
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

                    foreach (var model in excelData)
                    {
                        context.Add(model);
                    }

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
    }
}
