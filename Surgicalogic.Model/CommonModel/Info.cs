using System;
using System.Collections.Generic;
using System.Text;
using Surgicalogic.Model.Enum;

namespace Surgicalogic.Model.CommonModel
{
    [Serializable]
    public class Info
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; }
        public InfoType InfoType { get; set; }
    }
}
