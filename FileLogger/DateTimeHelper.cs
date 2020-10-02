using System;
using System.Collections.Generic;
using System.Text;

namespace FileLoggerProject
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
    }
}
