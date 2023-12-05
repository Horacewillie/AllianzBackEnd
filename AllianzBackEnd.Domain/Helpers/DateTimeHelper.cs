using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Helpers
{
    public class DateTimeHelper
    {
        public static class NigerianTime
        {
            public static DateTime Now => DateTime.UtcNow.AddHours(1);
            public static DateTime Date => Now.Date;
        }
    }
}
