using System;
using System.Collections.Generic;
using System.Text;

namespace FileLoggerProject
{
    public class FileTimeHandler
    {
        private IDateTimeHelper _dateTimeHelper;
        public FileTimeHandler(IDateTimeHelper dateTimeHelper)
        {
            _dateTimeHelper = dateTimeHelper;
        }
    }
}
