using AutoMapper;
using Surgicalogic.Common.Settings;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.Enum;
using Surgicalogic.Model.ExportModel;
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
            config.CreateMap<AppointmentCalendar, AppointmentCalendarModel>();
            config.CreateMap<Branch, BranchModel>();
            config.CreateMap<DoctorCalendar, DoctorCalendarModel>();
            config.CreateMap<Equipment, EquipmentModel>()
                .ForMember(dest => dest.OperatingRoomEquipments, opt => { opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive && x.Equipment.IsActive && x.OperatingRoom.IsActive)); });
            config.CreateMap<EquipmentType, EquipmentTypeModel>();
            config.CreateMap<Feedback, FeedbackModel>();
            config.CreateMap<Operation, OperationModel>();
            config.CreateMap<Operation, OperationForOperationPlanModel>();
            config.CreateMap<OperationPersonnel, OperationPersonnelModel>();
            config.CreateMap<OperationPlan, OperationPlanModel>();
            config.CreateMap<OperationPlan, OperationPlanHistoryModel>();
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
            config.CreateMap<Patient, PatientModel>();
            config.CreateMap<Personnel, PersonnelModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
            config.CreateMap<PersonnelBranch, PersonnelBranchModel>();
            config.CreateMap<PersonnelCategory, PersonnelCategoryModel>();
            config.CreateMap<Setting, SettingModel>();
            config.CreateMap<SettingDataType, SettingDataTypeModel>();
            config.CreateMap<User, UserModel>();
            config.CreateMap<WorkType, WorkTypeModel>();
            #endregion

            #region EntityModel to Entity
            config.CreateMap<AppointmentCalendarModel, AppointmentCalendar>();
            config.CreateMap<BranchModel, Branch>();
            config.CreateMap<DoctorCalendarModel, DoctorCalendar>();
            config.CreateMap<EquipmentModel, Equipment>();
            config.CreateMap<EquipmentTypeModel, EquipmentType>();
            config.CreateMap<FeedbackModel, Feedback>();
            config.CreateMap<OperationModel, Operation>()
                .ForMember(src => src.OperationType, opt => opt.Ignore())
                .ForMember(src => src.OperationPersonels, opt => opt.Ignore())
                .ForMember(src => src.OperationBlockedOperatingRooms, opt => opt.Ignore());
            config.CreateMap<OperationPersonnelModel, OperationPersonnel>();
            config.CreateMap<OperationPlanModel, OperationPlan>()
                .ForMember(src => src.Operation, opt => opt.Ignore())
                .ForMember(src => src.OperatingRoom, opt => opt.Ignore());
            config.CreateMap<OperatingRoomModel, OperatingRoom>()
                .ForMember(src => src.OperatingRoomEquipments, opt => opt.Ignore())
                .ForMember(src => src.OperatingRoomOperationTypes, opt => opt.Ignore());
            config.CreateMap<OperationBlockedOperatingRoomModel, OperationBlockedOperatingRoom>();
            config.CreateMap<OperatingRoomCalendarModel, OperatingRoomCalendar>();
            config.CreateMap<OperationTypeModel, OperationType>();
            config.CreateMap<OperationTypeEquipmentModel, OperationTypeEquipment>();
            config.CreateMap<OperatingRoomEquipmentModel, OperatingRoomEquipment>();
            config.CreateMap<OperatingRoomOperationTypeModel, OperatingRoomOperationType>();
            config.CreateMap<PatientModel, Patient>();
            config.CreateMap<PersonnelModel, Personnel>()
                .ForMember(src => src.PersonnelBranches, opt => opt.Ignore());
            config.CreateMap<PersonnelBranchModel, PersonnelBranch>();
            config.CreateMap<PersonnelCategoryModel, PersonnelCategory>();
            config.CreateMap<SettingModel, Setting>()
                .ForMember(src => src.Key, opt => opt.Ignore());
            config.CreateMap<SettingDataTypeModel, SettingDataType>();
            config.CreateMap<UserModel, User>();
            config.CreateMap<WorkTypeModel, WorkType>();
            #endregion

            #region EntityModel to EntityOutputModel
            config.CreateMap<AppointmentCalendarModel, AppointmentCalendarOutputModel>();
            config.CreateMap<BranchModel, BranchOutputModel>();
            config.CreateMap<DoctorCalendarModel, DoctorCalendarOutputModel>();
            config.CreateMap<EquipmentModel, EquipmentOutputModel>()
                .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Name))
                .ForMember(dest => dest.OperatingRoomIds, opt => opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive).Select(x => x.OperatingRoomId)))
                .ForMember(dest => dest.OperatingRoomName, opt => opt.MapFrom(src => string.Join(", ", src.OperatingRoomEquipments.Where(x => x.IsActive).Select(x => x.OperatingRoom.Name))));
            config.CreateMap<EquipmentTypeModel, EquipmentTypeOutputModel>();
            config.CreateMap<FeedbackModel, FeedbackOutputModel>();
            config.CreateMap<OperationModel, Model.OutputModel.OperationOutputModel>()
                 .ForMember(dest => dest.OperationTypeName, opt => opt.MapFrom(src => src.OperationType.Name))
                 .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.OperationType.BranchId))
                 .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.OperationType.Branch.Name))
                 .ForMember(dest => dest.PersonnelIds, opt => opt.MapFrom(src => src.OperationPersonels.Where(x => x.IsActive).Select(t => t.PersonnelId).ToArray()))
                 .ForMember(dest => dest.DoctorIds, opt => opt.MapFrom(src => src.OperationPersonels.Where(x => x.IsActive && x.Personnel.PersonnelCategoryId == AppSettings.DoctorId).Select(t => t.PersonnelId).ToArray()))
                 .ForMember(dest => dest.DoctorNames, opt => opt.MapFrom(src => string.Join(", ", src.OperationPersonels.Where(x => x.IsActive).Select(x => x.Personnel.FullName))))
                 .ForMember(dest => dest.BlockedOperatingRoomIds, opt => opt.MapFrom(src => src.OperationBlockedOperatingRooms.Where(x => x.IsActive).Select(t => t.OperatingRoomId).ToArray()))
                 .ForMember(dest => dest.OperatingRoomNames, opt => opt.MapFrom(src => string.Join(", ", src.OperationBlockedOperatingRooms.Where(x => x.IsActive).Select(x => x.OperatingRoom.Name))))
                 .ForMember(dest => dest.OperationTime, opt => opt.MapFrom(src => (src.OperationTime / 60 < 10 ? ("0" + src.OperationTime / 60) : (src.OperationTime / 60 + "")) + ":" + (src.OperationTime % 60 < 10 ? ("0" + src.OperationTime % 60) : (src.OperationTime % 60 + ""))))
                 .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
                 //.ForMember(dest => dest.Period, opt => opt.MapFrom(src => src.OperationTime % AppSettings.PeriodInMinutes == 0 ? src.OperationTime / AppSettings.PeriodInMinutes : src.OperationTime / AppSettings.PeriodInMinutes + 1));
            config.CreateMap<OperationPlanModel, OperationPlanOutputModel>()
                 .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.OperationId))
                 .ForMember(dest => dest.group, opt => opt.MapFrom(src => src.OperatingRoomId))
                 .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.Operation.Name))
                 .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.OperatingRoom.Name))
                 .ForMember(dest => dest.operationPlanId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.start, opt => opt.MapFrom(src => src.OperationDate.ToString("yyyy-MM-dd HH:mm:ss")))
                 .ForMember(dest => dest.end, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime).ToString("yyyy-MM-dd HH:mm:ss")));
            config.CreateMap<OperatingRoomModel, OperatingRoomOutputModel>()
                 .ForMember(dest => dest.EquipmentIds, opt => opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive).Select(x => x.EquipmentId)))
                 .ForMember(dest => dest.OperationTypeIds, opt => opt.MapFrom(src => src.OperatingRoomOperationTypes.Where(x => x.IsActive).Select(x => x.OperationTypeId)));
            config.CreateMap<OperatingRoomModel, OperatingRoomForTimelineModel>()
                 .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.Name));
            config.CreateMap<Planning.Model.InputModel.RoomInputModel, OperatingRoomForTimelineModel>()
                 .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.className, opt => opt.MapFrom(src => src.ClassName));
            config.CreateMap<OperatingRoomCalendarModel, OperatingRoomCalendarOutputModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("yyyy-MM-dd")));
            config.CreateMap<OperatingRoomOperationTypeModel, OperatingRoomOperationTypeOutputModel>();
            config.CreateMap<OperationTypeModel, OperationTypeOutputModel>()
                .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.OperationTypeEquipment.Where(x => x.IsActive).Select(x => x.EquipmentId)))
                .ForMember(dest => dest.OperatingRoomIds, opt => opt.MapFrom(src => src.OperatingRoomOperationTypes.Where(x => x.IsActive).Select(x => x.OperatingRoomId)))
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => string.Join(", ", src.OperationTypeEquipment.Where(x => x.IsActive).Select(x => x.Equipment.Name))))
                .ForMember(dest => dest.OperatingRoomName, opt => opt.MapFrom(src => string.Join(", ", src.OperatingRoomOperationTypes.Where(x => x.IsActive).Select(x => x.OperatingRoom.Name))));
            config.CreateMap<PatientModel, PatientOutputModel>();
            config.CreateMap<PersonnelModel, PersonnelOutputModel>()
                .ForMember(dest => dest.BranchNames, opt => opt.MapFrom(src => string.Join(", ", src.PersonnelBranches.Where(x => x.IsActive && x.Branch.IsActive).Select(x => x.Branch.Name))))
                .ForMember(dest => dest.PersonnelCategoryName, opt => opt.MapFrom(src => src.PersonnelCategory.Name))
                .ForMember(dest => dest.WorkTypeName, opt => opt.MapFrom(src => src.WorkType.Name))
                .ForMember(dest => dest.BranchIds, opt => opt.MapFrom(src => src.PersonnelBranches.Where(x => x.IsActive).Select(x => x.BranchId)));
            config.CreateMap<PersonnelCategoryModel, PersonnelCategoryOutputModel>();
            config.CreateMap<SettingModel, SettingOutputModel>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.SettingDataTypeId == (int)SettingDataTypeNames.Int ? src.IntValue.ToString() : src.SettingDataTypeId == (int)SettingDataTypeNames.String ? src.StringValue : src.TimeValue));
            config.CreateMap<SettingDataTypeModel, SettingDataTypeOutputModel>();
            config.CreateMap<UserModel, UserOutputModel>()
                .ForMember(src => src.IsAdmin, opt => opt.Ignore());
            config.CreateMap<WorkTypeModel, WorkTypeOutputModel>();
            #endregion

            #region Entity to EntityExportModel
            config.CreateMap<AppointmentCalendar, AppointmentCalendarExportModel>();
            config.CreateMap<Branch, BranchExportModel>();
            config.CreateMap<DoctorCalendar, DoctorCalendarExportModel>();
            config.CreateMap<Equipment, EquipmentExportModel>()
                .ForMember(dest => dest.IsPortable, opt => opt.MapFrom(src => src.IsPortable ? "Evet" : "Hayır"));
            config.CreateMap<EquipmentType, EquipmentTypeExportModel>();
            config.CreateMap<Feedback, FeedbackExportModel>();
            config.CreateMap<OperatingRoomCalendar, OperatingRoomCalendarExportModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("dd/MM/yyyy")));
            config.CreateMap<OperatingRoom, OperatingRoomExportModel>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable ? "Evet" : "Hayır"));
            config.CreateMap<Personnel, PersonnelExportModel>();
            config.CreateMap<PersonnelCategory, PersonnelCategoryExportModel>();
            config.CreateMap<WorkType, WorkTypeExportModel>();
            config.CreateMap<OperationPlan, OperationPlanExportModel>()
                .ForMember(dest => dest.OperationName, opt => opt.MapFrom(src => src.Operation.Name))
                .ForMember(dest => dest.OperatingRoomName, opt => opt.MapFrom(src => src.OperatingRoom.Name))
                .ForMember(dest => dest.OperationDate, opt => opt.MapFrom(src => src.OperationDate.ToString("dd/MM/yyyy")));
            config.CreateMap<Operation, OperationExportModel>()
                .ForMember(dest => dest.OperationTime, opt => opt.MapFrom(src => (src.OperationTime / 60 < 10 ? ("0" + src.OperationTime / 60) : (src.OperationTime / 60 + "")) + ":" + (src.OperationTime % 60 < 10 ? ("0" + src.OperationTime % 60) : (src.OperationTime % 60 + ""))));
            config.CreateMap<Patient, PatientExportModel>();
            config.CreateMap<User, UserExportModel>();
            #endregion

            #region Entity To InputOutputModel
            config.CreateMap<Operation, OperationInputModel>();
                //.ForMember(dest => dest.Period, opt => opt.MapFrom(src => src.OperationTime % AppSettings.PeriodInMinutes == 0 ? src.OperationTime / AppSettings.PeriodInMinutes : src.OperationTime / AppSettings.PeriodInMinutes + 1));
            config.CreateMap<OperationType, OperationTypeForOperationOutputModel>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name + " - " + src.Branch.Name));
            config.CreateMap<OperationPlan, OperationPlanHistoryOutputModel>()
                .ForMember(dest => dest.RealizedStartDate, opt => opt.MapFrom(src => src.OperationDate))
                .ForMember(dest => dest.RealizedEndDate, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime)))
                .ForMember(dest => dest.OperationStartDate, opt => opt.MapFrom(src => src.OperationDate))
                .ForMember(dest => dest.OperationEndDate, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime)))
                .ForMember(dest => dest.OperationName, opt => opt.MapFrom(src => src.Operation.Name))
                .ForMember(dest => dest.OperatingRoomName, opt => opt.MapFrom(src => src.OperatingRoom.Name));
            config.CreateMap<Personnel, PersonnelOutputModel>()
                .ForMember(dest => dest.BranchNames, opt => opt.MapFrom(src => string.Join(", ", src.PersonnelBranches.Where(x => x.IsActive && x.Branch.IsActive).Select(x => x.Branch.Name))))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.PersonnelCategoryName, opt => opt.MapFrom(src => src.PersonnelCategory.Name + " " + src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.BranchIds, opt => opt.MapFrom(src => src.PersonnelBranches.Where(x => x.IsActive).Select(x => x.BranchId)))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => AppSettings.DoctorPicture + src.PictureUrl));
            config.CreateMap<OperatingRoom, OperatingRoomOutputModel>()
                 .ForMember(dest => dest.EquipmentIds, opt => opt.MapFrom(src => src.OperatingRoomEquipments.Where(x => x.IsActive).Select(x => x.EquipmentId)))
                 .ForMember(dest => dest.OperationTypeIds, opt => opt.MapFrom(src => src.OperatingRoomOperationTypes.Where(x => x.IsActive).Select(x => x.OperationTypeId)));
            config.CreateMap<OperationPlan, OperationPlanOutputModel>()
                 .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.OperationId))
                 .ForMember(dest => dest.group, opt => opt.MapFrom(src => src.OperatingRoomId))
                 .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.Operation.Name))
                 .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.OperatingRoom.Name))
                 .ForMember(dest => dest.operationPlanId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.start, opt => opt.MapFrom(src => src.OperationDate.ToString("yyyy-MM-dd HH:mm:ss")))
                 .ForMember(dest => dest.end, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime).ToString("yyyy-MM-dd HH:mm:ss")));
            config.CreateMap<OperationPlanModel, OperationPlanHistoryOutputModel>()
                .ForMember(dest => dest.RealizedStartDate, opt => opt.MapFrom(src => src.OperationDate))
                .ForMember(dest => dest.RealizedEndDate, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime)))
                .ForMember(dest => dest.OperationStartDate, opt => opt.MapFrom(src => src.OperationDate))
                .ForMember(dest => dest.OperationEndDate, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime)))
                .ForMember(dest => dest.OperationName, opt => opt.MapFrom(src => src.Operation.Name))
                .ForMember(dest => dest.OperatingRoomName, opt => opt.MapFrom(src => src.OperatingRoom.Name));
            config.CreateMap<OperationPlanHistoryModel, OperationPlanHistoryOutputModel>()
               .ForMember(dest => dest.RealizedStartDate, opt => opt.MapFrom(src => src.OperationDate))
               .ForMember(dest => dest.RealizedEndDate, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime)))
               .ForMember(dest => dest.OperationStartDate, opt => opt.MapFrom(src => src.OperationDate))
               .ForMember(dest => dest.OperationEndDate, opt => opt.MapFrom(src => src.OperationDate.AddMinutes(src.Operation.OperationTime)))
               .ForMember(dest => dest.OperationName, opt => opt.MapFrom(src => src.Operation.Name))
               .ForMember(dest => dest.OperatingRoomName, opt => opt.MapFrom(src => src.OperatingRoom.Name));

            #endregion
        }
    }
}