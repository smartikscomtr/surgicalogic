namespace Surgicalogic.Common.Settings
{
    public static class AppSettings
    {
        public static string TokenSecurityKey { get; set; }
        public static int TokenValidityPeriodInMinutes { get; set; }
        public static string Issuer { get; set; }
        public static string ApiBaseUrl { get; set; }
        public static string ApiPostUrl { get; set; }
        public static int PeriodInMinutes { get; set; }
    }
}