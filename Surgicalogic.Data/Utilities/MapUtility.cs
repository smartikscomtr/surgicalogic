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

            config.CreateMap<Equipment, EquipmentModel>();
            config.CreateMap<EquipmentType, EquipmentTypeModel>();
            config.CreateMap<Branch, BranchModel>();          

            #endregion

            #region EntityModel to Entity
            config.CreateMap<EquipmentModel, Equipment>();
            config.CreateMap<EquipmentTypeModel, EquipmentType>();

            #endregion
        }
    }
}
