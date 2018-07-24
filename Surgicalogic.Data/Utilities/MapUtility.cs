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
                .ForMember(dest => dest.OperatingRoomEquipments, opt => { opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive && x.Equipment.IsActive && x.Equipment.IsPortable == false)); })
                .ForMember(dest => dest.OperatingRoomOperationTypes, opt => { opt.MapFrom(src => src.OperatingRoomOperationTypes.Where(x => x.IsActive && x.OperationType.IsActive)); });
            config.CreateMap<OperationType, OperationTypeModel>();
            config.CreateMap<OperatingRoomOperationType, OperatingRoomOperationTypeModel>();
            config.CreateMap<OperatingRoomEquipment, OperatingRoomEquipmentModel>();
            config.CreateMap<Personnel, PersonnelModel>();
            config.CreateMap<PersonnelBranch, PersonnelBranchModel>();
            config.CreateMap<PersonnelTitle, PersonnelTitleModel>();
            config.CreateMap<User, UserModel>();
            config.CreateMap<WorkType, WorkTypeModel>();
            #endregion

            #region EntityModel to Entity
            config.CreateMap<BranchModel, Branch>();
            config.CreateMap<EquipmentModel, Equipment>();
            config.CreateMap<EquipmentTypeModel, EquipmentType>();
            config.CreateMap<OperatingRoomModel, OperatingRoom>()
                .ForMember(src => src.OperatingRoomEquipments, opt => opt.Ignore())
                .ForMember(src => src.OperatingRoomOperationTypes, opt => opt.Ignore());
            config.CreateMap<OperationTypeModel, OperationType>();
            config.CreateMap<OperatingRoomEquipmentModel, OperatingRoomEquipment>();
            config.CreateMap<OperatingRoomOperationTypeModel, OperatingRoomOperationType>();
            config.CreateMap<PersonnelModel, Personnel>();
            config.CreateMap<PersonnelBranchModel, PersonnelBranch>();
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
            config.CreateMap<OperatingRoomOperationTypeModel, OperatingRoomOperationTypeOutputModel>();
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