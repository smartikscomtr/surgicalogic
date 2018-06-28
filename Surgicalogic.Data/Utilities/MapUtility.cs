using AutoMapper;
using Surgicalogic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
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
            config.CreateMap<EquipmentType, EquipmentTypeModel>();
            config.CreateMap<WorkType, WorkTypeModel>();

            #endregion

            #region EntityModel to Entity

            config.CreateMap<BranchModel, Branch>();
            config.CreateMap<EquipmentModel, Equipment>();
            config.CreateMap<EquipmentTypeModel, EquipmentType>();
            config.CreateMap<WorkTypeModel, WorkType>();

            #endregion
        }
    }
}
