using System.ComponentModel.DataAnnotations;

namespace Surgicalogic.Data.Entities
{
    public class SettingDataType : Base.Entity
    {
        [StringLength(100)]
        public string Name { get; set; }
    }
}
