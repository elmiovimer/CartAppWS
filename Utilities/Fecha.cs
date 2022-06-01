using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Utilities
{
    public class Fecha
    {
        public static DateTime toDateTimeUTC(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
        }

        public static DateTime toDateTimeUTC(Object date)
        {
            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(date);
            }
            catch
            {
                dt = DateTime.MinValue;
            }
            
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Utc);
        }

        public static String ToSQL(DateTime date)
        {
            return date.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
