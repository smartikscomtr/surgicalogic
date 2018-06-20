using AutoMapper;
using Surgicalogic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Surgicalogic.Model.EntityModel;

namespace Surgicalogic.Data.Utils
{
    public static class MapUtil
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            #region Entity to EntityModel

            config.CreateMap<Equipment, EquipmentModel>();
            config.CreateMap<EquipmentType, EquipmentTypeModel>();
            config.CreateMap<BranchType, BranchTypeModel>();

            #endregion

            #region EntityModel to Entity
            config.CreateMap<EquipmentModel, Equipment>();
            config.CreateMap<EquipmentTypeModel, EquipmentType>();

            #endregion
        }
    }
}
