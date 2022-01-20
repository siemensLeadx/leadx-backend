using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime SystemDateTimeNow(this DateTime dateTime)
        {
            return DateTime.UtcNow;
        }


        public static long ToUnixTimeStamp(this DateTime dateTime)
        {
            return (long)(dateTime - DateTime.UnixEpoch).TotalSeconds;
        }

        public static DateTime FromUnixTimeStamp(this long unixEpoch)
        {
            return DateTime.UnixEpoch.AddSeconds(unixEpoch);
        }
    }
}
