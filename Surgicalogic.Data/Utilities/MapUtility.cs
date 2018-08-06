using AutoMapper;
using Surgicalogic.Common.Settings;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Planning.Model.InputModel;
using Surgicalogic.Planning.Model.OutputModel;
using System.Linq;

namespace Surgicalogic.Data.Utilities
{
    public static class MapUtility
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            #region Entity to EntityModel
            config.CreateMap<Branch, BranchModel>();
            config.CreateMap<Equipment, EquipmentModel>()
                .ForMember(dest => dest.OperatingRoomEquipments, opt => { opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive && x.Equipment.IsActive && x.OperatingRoom.IsActive)); });
            config.CreateMap<EquipmentType, EquipmentTypeModel>();
            config.CreateMap<Operation, OperationModel>();
            config.CreateMap<OperationPlan, OperationPlanModel>();
            config.CreateMap<OperatingRoom, OperatingRoomModel>()
                .ForMember(dest => dest.OperatingRoomEquipments, opt => { opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive && x.Equipment.IsActive && x.OperatingRoom.IsActive && x.Equipment.IsPortable == false)); })
                .ForMember(dest => dest.OperatingRoomOperationTypes, opt => { opt.MapFrom(src => src.OperatingRoomOperationTypes.Where(x => x.IsActive && x.OperationType.IsActive && x.OperatingRoom.IsActive)); });
            config.CreateMap<OperatingRoomCalendar, OperatingRoomCalendarModel>();
            config.CreateMap<OperationType, OperationTypeModel>();
            config.CreateMap<OperationTypeEquipment, OperationTypeEquipmentModel>();
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
            config.CreateMap<OperationModel, Operation>();
            config.CreateMap<OperationPlanModel, OperationPlan>();
            config.CreateMap<OperatingRoomModel, OperatingRoom>()
                .ForMember(src => src.OperatingRoomEquipments, opt => opt.Ignore())
                .ForMember(src => src.OperatingRoomOperationTypes, opt => opt.Ignore());
            config.CreateMap<OperatingRoomCalendarModel, OperatingRoomCalendar>();
            config.CreateMap<OperationTypeModel, OperationType>();
            config.CreateMap<OperationTypeEquipmentModel, OperationTypeEquipment>();
            config.CreateMap<OperatingRoomEquipmentModel, OperatingRoomEquipment>();
            config.CreateMap<OperatingRoomOperationTypeModel, OperatingRoomOperationType>();
            config.CreateMap<PersonnelModel, Personnel>()
                .ForMember(src => src.PersonnelBranches, opt => opt.Ignore());
            config.CreateMap<PersonnelBranchModel, PersonnelBranch>();
            config.CreateMap<PersonnelTitleModel, PersonnelTitle>();
            config.CreateMap<UserModel, User>();
            config.CreateMap<WorkTypeModel, WorkType>();
            #endregion

            #region EntityModel to EntityOutputModel
            config.CreateMap<BranchModel, BranchOutputModel>();
            config.CreateMap<EquipmentModel, EquipmentOutputModel>()
                .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Name))
                .ForMember(dest => dest.OperatingRoomIds, opt => opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive).Select(x => x.OperatingRoomId)));
            config.CreateMap<EquipmentTypeModel, EquipmentTypeOutputModel>();
            config.CreateMap<OperationModel, Model.OutputModel.OperationOutputModel>()
                 .ForMember(dest => dest.OperationTypeName, opt => opt.MapFrom(src => src.OperationType.Name));
            config.CreateMap<OperationPlanModel, OperationPlanOutputModel>()
                 .ForMember(dest => dest.OperatingRoomName, opt => opt.MapFrom(src => src.OperatingRoom.Name))
                 .ForMember(dest => dest.OperationName, opt => opt.MapFrom(src => src.Operation.Name));
            config.CreateMap<OperatingRoomModel, OperatingRoomOutputModel>();
            config.CreateMap<OperatingRoomCalendarModel, OperatingRoomCalendarOutputModel>();
            config.CreateMap<OperatingRoomOperationTypeModel, OperatingRoomOperationTypeOutputModel>();
            config.CreateMap<OperationTypeModel, OperationTypeOutputModel>()
                .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.OperationTypeEquipment.Where(x => x.IsActive).Select(x => x.EquipmentId)))
                .ForMember(dest => dest.OperatingRoomIds, opt => opt.MapFrom(src => src.OperatingRoomOperationTypes.Where(x => x.IsActive).Select(x => x.OperatingRoomId)));
            config.CreateMap<PersonnelModel, PersonnelOutputModel>()
                .ForMember(dest => dest.BranchNames, opt => opt.MapFrom(src => string.Join(", ", src.PersonnelBranches.Where(x => x.IsActive && x.Branch.IsActive).Select(x => x.Branch.Name))))
                .ForMember(dest => dest.PersonnelTitleName, opt => opt.MapFrom(src => src.PersonnelTitle.Name))
                .ForMember(dest => dest.WorkTypeName, opt => opt.MapFrom(src => src.WorkType.Name))
                .ForMember(dest => dest.BranchIds, opt => opt.MapFrom(src => src.PersonnelBranches.Where(x => x.IsActive).Select(x => x.BranchId)));
            config.CreateMap<PersonnelTitleModel, PersonnelTitleOutputModel>();
            config.CreateMap<WorkTypeModel, WorkTypeOutputModel>();
            #endregion

            #region CustomMapping
            config.CreateMap<Operation, OperationInputModel>()
                .ForMember(dest => dest.Period, opt => opt.MapFrom(src => src.OperationTime / AppSettings.PeriodInMinutes))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Id));
            #endregion
        }
    }
}