using System;
using System.Globalization;

namespace Useful.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime StringToDate(string date, string format = "MM/dd/yyyy")
        {
            var idx = 0;
            string[] formats = { "MM/dd/yyyy", "MM/d/yyyy", "M/d/yyyy" };
            while (true)
            {
                if (DateTime.TryParseExact(date.Split(' ')[0], format, new CultureInfo("pt-BR"), DateTimeStyles.None, out var result))
                    return result;

                if (idx == formats.Length)
                    return DateTime.MinValue;

                format = formats[idx++];
            }
        }

        public static DateTime GetFirstDayOfTheMonth(DateTime? date = null)
        {
            if (date == null)
                date = DateTime.Now;

            return new DateTime(date.Value.Year, date.Value.Month, 1);
        }

        public static DateTime GetLastDayOfTheMonth(DateTime? date = null) => GetFirstDayOfTheMonth(date).AddMonths(1).AddMilliseconds(-1);
    }
}
