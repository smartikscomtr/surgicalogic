﻿using AutoMapper;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Data.Utilities
{
    public static class MapUtility
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            #region Entity to EntityModel
            config.CreateMap<Branch, BranchModel>();
            config.CreateMap<Equipment, EquipmentModel>();
                //.ForMember(dest => dest.EquipmentTypeModel, opt => opt.MapFrom(src => src.EquipmentType));
            config.CreateMap<EquipmentType, EquipmentTypeModel>();
            config.CreateMap<OperatingRoom, OperatingRoomModel>();
                //.ForMember(dest => dest.OperatingRoomEquipmentModels, opt => opt.MapFrom(src => src.OperatingRoomEquipments));
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
                //.ForMember(dest => dest.OperatingRoomEquipmentOutputModels, opt => opt.MapFrom(src => src.OperatingRoomEquipmentModels));
            //config.CreateMap<OperatingRoomModel, OperatingRoomOutputModel>()
            //    .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.EquipmentModel.Name));
            #endregion
        }
    }
}