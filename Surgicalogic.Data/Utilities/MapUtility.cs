using AutoMapper;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;
using System.Linq;

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
            config.CreateMap<OperatingRoom, OperatingRoomModel>()
                .ForMember(dest => dest.OperatingRoomEquipments, opt => { opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive == true)); });                
            config.CreateMap<OperationType, OperationTypeModel>();
            config.CreateMap<Personnel, PersonnelModel>();
            config.CreateMap<PersonnelTitle, PersonnelTitleModel>();
            config.CreateMap<WorkType, WorkTypeModel>();
            #endregion

            #region EntityModel to Entity
            config.CreateMap<BranchModel, Branch>();
            config.CreateMap<EquipmentModel, Equipment>();
            config.CreateMap<EquipmentTypeModel, EquipmentType>();
            config.CreateMap<OperatingRoomModel, OperatingRoom>();
            config.CreateMap<OperationTypeModel, OperationType>();
            config.CreateMap<PersonnelModel, Personnel>();
            config.CreateMap<PersonnelTitleModel, PersonnelTitle>();
            config.CreateMap<WorkTypeModel, WorkType>();
            #endregion

            #region EntityModel to EntityOutputModel
            config.CreateMap<EquipmentModel, EquipmentOutputModel>()
                .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Name));
            config.CreateMap<EquipmentTypeModel, EquipmentTypeOutputModel>();
            config.CreateMap<OperatingRoomModel, OperatingRoomOutputModel>();
            #endregion
        }
    }
}