namespace OrderManagement.Application.Mappings.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly TimeZoneInfo SaoPauloTimeZone =
            OperatingSystem.IsWindows()
                ? TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        public static DateTime ToSaoPaulo(this DateTime utcDate)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, SaoPauloTimeZone);
        }

        public static DateTime? ToSaoPaulo(this DateTime? utcDate)
        {
            return utcDate.HasValue
                ? TimeZoneInfo.ConvertTimeFromUtc(utcDate.Value, SaoPauloTimeZone)
                : null;
        }
    }
}
