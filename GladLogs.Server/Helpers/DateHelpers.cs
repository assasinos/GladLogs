using System.Globalization;

namespace GladLogs.Server.Helpers
{
    public static class DateHelpers
    {
/*        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            return ci.Calendar.GetWeekOfYear(date, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
        }*/

        public static int GetWeekNumber(this DateTime date)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            return ci.Calendar.GetWeekOfYear(date, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
        }
        public static DateTime GetStartOfWeek(DateTime date)
        {
            int daysToSubtract = (int)date.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (daysToSubtract < 0)
            {
                daysToSubtract += 7;
            }
            return date.Subtract(TimeSpan.FromDays(daysToSubtract));
        }
    }
}
