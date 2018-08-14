using AutoMapper;
using Surgicalogic.Common.Settings;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
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
            config.CreateMap<OperationPersonnel, OperationPersonnelModel>();
            config.CreateMap<OperationPlan, OperationPlanModel>();
            config.CreateMap<OperatingRoom, OperatingRoomModel>()
                 .ForMember(dest => dest.OperatingRoomEquipments, opt => { opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive && x.Equipment.IsActive && x.OperatingRoom.IsActive && x.Equipment.IsPortable == false)); })
                 .ForMember(dest => dest.OperatingRoomOperationTypes, opt => { opt.MapFrom(src => src.OperatingRoomOperationTypes.Where(x => x.IsActive && x.OperationType.IsActive && x.OperatingRoom.IsActive)); });
            config.CreateMap<OperatingRoom, OperatingRoomForOperationPlanModel>();
            config.CreateMap<OperationBlockedOperatingRoom, OperationBlockedOperatingRoomModel>();
            config.CreateMap<OperatingRoomCalendar, OperatingRoomCalendarModel>();
            config.CreateMap<OperationType, OperationTypeModel>();
            config.CreateMap<OperationTypeEquipment, OperationTypeEquipmentModel>();
            config.CreateMap<OperatingRoomOperationType, OperatingRoomOperationTypeModel>();
            config.CreateMap<OperatingRoomEquipment, OperatingRoomEquipmentModel>();
            config.CreateMap<Personnel, PersonnelModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
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
            config.CreateMap<OperationPersonnelModel, OperationPersonnel>();
            config.CreateMap<OperationPlanModel, OperationPlan>();
            config.CreateMap<OperatingRoomModel, OperatingRoom>()
                .ForMember(src => src.OperatingRoomEquipments, opt => opt.Ignore())
                .ForMember(src => src.OperatingRoomOperationTypes, opt => opt.Ignore());
            config.CreateMap<OperationBlockedOperatingRoomModel, OperationBlockedOperatingRoom>();
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
                 .ForMember(dest => dest.OperationTypeName, opt => opt.MapFrom(src => src.OperationType.Name))
                 .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.OperationType.BranchId))
                 .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.OperationType.Branch.Name))
                 .ForMember(dest => dest.DoctorIds, opt => opt.MapFrom(src => src.OperationPersonels.Where(x => x.IsActive).Select(t => t.PersonnelId).ToArray()))
                 .ForMember(dest => dest.DoctorNames, opt => opt.MapFrom(src => string.Join(", ", src.OperationPersonels.Select(x => x.Personnel.FullName))))
                 .ForMember(dest => dest.OperatingRoomIds, opt => opt.MapFrom(src => src.OperationBlockedOperatingRooms.Where(x => x.IsActive).Select(t => t.OperatingRoomId).ToArray()))
                 .ForMember(dest => dest.OperatingRoomNames, opt => opt.MapFrom(src => string.Join(", ", src.OperationBlockedOperatingRooms.Select(x => x.OperatingRoom.Name))))
                 .ForMember(dest => dest.OperationTime, opt => opt.MapFrom(src => (src.OperationTime / 60 < 10 ? ("0" + src.OperationTime / 60) : (src.OperationTime / 60 + "")) + ":" + (src.OperationTime % 60 < 10 ? ("0" + src.OperationTime % 60) : (src.OperationTime % 60 + ""))))
                 .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")))
                 .ForMember(dest => dest.Period, opt => opt.MapFrom(src => src.OperationTime % AppSettings.PeriodInMinutes == 0 ? src.OperationTime / AppSettings.PeriodInMinutes : src.OperationTime / AppSettings.PeriodInMinutes + 1));
            config.CreateMap<OperationPlanModel, OperationPlanOutputModel>()
                 .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.OperationId))
                 .ForMember(dest => dest.group, opt => opt.MapFrom(src => src.OperatingRoomId))
                 .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.Operation.Name))
                 .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.OperatingRoom.Name))
                 .ForMember(dest => dest.start, opt => opt.MapFrom(src => src.OperationDate.ToString("yyyy-MM-dd HH:mm:ss")))
                 .ForMember(dest => dest.end, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime).ToString("yyyy-MM-dd HH:mm:ss")));
            config.CreateMap<OperatingRoomModel, OperatingRoomOutputModel>();
            config.CreateMap<OperatingRoomModel, OperatingRoomForTimelineModel>()
                 .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.Name));
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
            config.CreateMap<UserModel, UserOutputModel>()
                .ForMember(src => src.IsAdmin, opt => opt.Ignore());
            config.CreateMap<WorkTypeModel, WorkTypeOutputModel>();
            #endregion

            #region CustomMapping
            config.CreateMap<Operation, OperationInputModel>()
                .ForMember(dest => dest.Period, opt => opt.MapFrom(src => src.OperationTime % AppSettings.PeriodInMinutes == 0 ? src.OperationTime / AppSettings.PeriodInMinutes : src.OperationTime / AppSettings.PeriodInMinutes + 1));
            config.CreateMap<Personnel, PersonnelOutputModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
            config.CreateMap<OperatingRoom, OperatingRoomOutputModel>();
            #endregion
        }
    }
}