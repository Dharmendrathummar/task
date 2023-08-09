using System;
using System.Collections.Generic;
using System.Text;


namespace task.data.Essentials
{
    public static class DataDateTime
    {
        public static string MobileFormat = "yyyy-MM-dd'T'HH:mm:ss.fff";
        public static DateTime Now
        {
            get
            {
                return System.DateTime.Now.ToUniversalTime();
            }
        }
        public static DateTime IndiaTimeNow
        {
            get
            {
                return ToIndianTime(System.DateTime.Now.ToUniversalTime());
            }
        }
        public static DateTime Today
        {
            get
            {
                return System.DateTime.Today.ToUniversalTime();
            }
        }

        public static long EpochTime
        {
            get
            {
                return (DateTime.UtcNow.Ticks - 621355968000000000) / 10000;
            }
        }
        public static Int64 ToUTCTimeStamp(DateTime date)
        {
            return (Int64)(date).Subtract(new DateTime(1970, 1, 1)).TotalSeconds * 1000;
        }

        public static DateTime FromUTCTimeStamp(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
        }


        public static long ToEpochTime(DateTime date)
        {
            try
            {
                return (date.Ticks - 621355968000000000) / 10000;
            }
            catch
            { return date.Ticks / 10000; }
        }

        public static DateTime ToIndianTime(DateTime date)
        {
            try
            {
                return date.AddMinutes(330);
            }
            catch
            { return date; }
        }

        public static DateTime ToLocalZone(DateTime UTC, int Minutes = 0)
        {
            try
            {
                DateTimeOffset utcoffset = new DateTimeOffset(UTC);
                TimeSpan span = new TimeSpan(0, Minutes, 0);
                DateTimeOffset offset = utcoffset.ToOffset(span);
                return DataDateTime.Now;
            }
            catch
            { return DataDateTime.Now; }
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
        public static string ToMobileTimeString(DateTime date, bool isUtc = true)
        {
            if (isUtc)
                return date.ToString(MobileFormat) + "Z";
            else
                return date.ToString(MobileFormat);
        }
    }
}
