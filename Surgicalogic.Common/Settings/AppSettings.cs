using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Common.Settings
{
    public static class AppSettings
    {
        public static string TokenSecurityKey { get; set; }
        public static int TokenValidityPeriodInMinutes { get; set; }
        public static string Issuer { get; set; }
    }
}
