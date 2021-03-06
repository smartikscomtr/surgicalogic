﻿using System;
using System.Globalization;
using System.Threading;

namespace Surgicalogic.Common.Settings
{
    public static class AppSettings
    {
        public static string TokenSecurityKey { get; set; }
        public static int TokenValidityPeriodInMinutes { get; set; }
        public static string Issuer { get; set; }
        public static string ApiBaseUrl { get; set; }
        public static string ApiPostUrl { get; set; }
        public static string AdminRole { get; set; }
        public static string MemberRole { get; set; }
        public static int DoctorId { get; set; }
        public static string DoctorPicture { get; set; }
        public static string ImagesFolder { get; set; }
        public static string WebSiteUrl { get; set; }

        public static void SetSiteLanguage(string code)
        {
            var culture = code == "tr" ? "tr-TR" : "en-US";
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        }
    }
}