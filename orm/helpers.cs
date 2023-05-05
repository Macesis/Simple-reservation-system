using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace orm
{
    public static class helpers
    {
        public static int date_to_unix(DateTime date)
        {
            return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
        public static DateTime unix_to_date(int unix)
        {
            DateTime out_date = new DateTime(1970, 1, 1);
            out_date = out_date.AddSeconds(unix);
            return out_date;
        }
    }
}
