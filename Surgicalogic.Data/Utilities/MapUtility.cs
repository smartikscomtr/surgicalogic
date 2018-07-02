using AutoMapper;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Data.Utilities
{
    public static class MapUtility
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            #region Entity to EntityModel

            config.CreateMap<Branch, BranchModel>();
            config.CreateMap<Equipment, EquipmentModel>();
            config.CreateMap<EquipmentType, EquipmentTypeModel>();
            config.CreateMap<Personnel, PersonnelModel>();
            config.CreateMap<PersonnelTitle, PersonnelTitleModel>();
            config.CreateMap<WorkType, WorkTypeModel>();

            #endregion

            #region EntityModel to Entity

            config.CreateMap<BranchModel, Branch>();
            config.CreateMap<EquipmentModel, Equipment>();
            config.CreateMap<EquipmentTypeModel, EquipmentType>();
            config.CreateMap<PersonnelModel, Personnel>();
            config.CreateMap<PersonnelTitleModel, PersonnelTitle>();
            config.CreateMap<WorkTypeModel, WorkType>();

            #endregion
        }
    }
}
