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
                .ForMember(dest => dest.OperatingRoomEquipments, opt => { opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive && x.Equipment.IsActive && x.Equipment.IsPortable == false)); });
            config.CreateMap<OperationType, OperationTypeModel>();
            config.CreateMap<Personnel, PersonnelModel>();
            config.CreateMap<PersonnelTitle, PersonnelTitleModel>();
            config.CreateMap<User, UserModel>();
            config.CreateMap<WorkType, WorkTypeModel>();
            #endregion

            #region EntityModel to Entity
            config.CreateMap<BranchModel, Branch>();
            config.CreateMap<EquipmentModel, Equipment>();
            config.CreateMap<EquipmentTypeModel, EquipmentType>();
            config.CreateMap<OperatingRoomModel, OperatingRoom>()
                .ForMember(src => src.OperatingRoomEquipments, opt => opt.Ignore());
            config.CreateMap<OperationTypeModel, OperationType>();
            config.CreateMap<PersonnelModel, Personnel>();
            config.CreateMap<PersonnelTitleModel, PersonnelTitle>();
            config.CreateMap<UserModel, User>();
            config.CreateMap<WorkTypeModel, WorkType>();
            #endregion

            #region EntityModel to EntityOutputModel
            config.CreateMap<BranchModel, BranchOutputModel>();
            config.CreateMap<EquipmentModel, EquipmentOutputModel>()
                .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Name));
            config.CreateMap<EquipmentTypeModel, EquipmentTypeOutputModel>();
            config.CreateMap<OperatingRoomModel, OperatingRoomOutputModel>();
            config.CreateMap<PersonnelModel, PersonnelOutputModel>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.PersonnelTitleName, opt => opt.MapFrom(src => src.PersonnelTitle.Name))
                .ForMember(dest => dest.WorkTypeName, opt => opt.MapFrom(src => src.WorkType.Name));
            config.CreateMap<PersonnelTitleModel, PersonnelTitleOutputModel>();
            config.CreateMap<WorkTypeModel, WorkTypeOutputModel>();
            #endregion
        }
    }
}