﻿using Surgicalogic.Model.Enum;
using System;

namespace Surgicalogic.Model.CommonModel
{
    [Serializable]
    public class Info
    {
        public bool Succeeded { get; set; } = true;
        public MessageType Message { get; set; }
        public InfoType InfoType { get; set; }
    }
}