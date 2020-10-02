using Moq;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;

namespace FileLoggerProject
{
    public class FileLogger
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        //The IDateTimeHelper is used for testing purposes to ensure proper functionality without damaging the Log method used by the logging system
        public IDateTimeHelper _dateTimeHelper;
        

        public FileLogger(){ }

        /// <summary>
        /// This constructor of a FileLogger takes in an old date to use for testing purposes in the form of a IDateTimeHelper which can be manipulated to have a different date
        /// </summary>
        /// <param name="dateTimeHelper"></param>
        public FileLogger(IDateTimeHelper dateTimeHelper)
        {
            _dateTimeHelper = dateTimeHelper;
        }

        /// <summary>
        /// The Log Method logs a message with the exact date and time of the message. If it is a weekend there are special naming conventions
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            //Change this folderName to match the location of the project
            string folderName = @"C:\Users\j.whaley\source\repos\FileLoggerKata\FileLogger";
            string fileName = GetFileName();
            string pathString = System.IO.Path.Combine(folderName, fileName);
            string outputMessage = GetMessageWithTime(message);

            // AppendAllText opens a file, appends the specified string to the file then closes the file. 
            // If the file doesn't exist it creates the file first then writes the message
            File.AppendAllText(pathString, outputMessage);

        }

        /// <summary>
        /// This method is used for testing to ensure proper functionality for old logs and weekends.
        /// </summary>
        /// <param name="message"></param>
        public void LogOldMessage(string message)
        {
            string folderName = @"C:\Users\j.whaley\source\repos\FileLoggerKata\FileLogger";
            string fileName = GetOldFileName();
            string pathString = System.IO.Path.Combine(folderName, fileName);
            string outputMessage = GetOldMessageWithTime(message);
            // AppendAllText opens a file, appends the specified string to the file then closes the file. 
            // If the file doesn't exist it creates the file first then writes the message
            File.AppendAllText(pathString, outputMessage);
        }

        /// <summary>
        /// Gets the file name for a current log also does weekend checks
        /// </summary>
        /// <returns></returns>
        private string GetFileName()
        {
            string folderName = @"C:\Users\j.whaley\source\repos\FileLoggerKata\FileLogger";
            string date = DateTime.Now.ToString("yyyyMMdd");
            DateTime today = DateTime.Now;
            return GetFileName(date, today, folderName);
        }

        private string GetMessageWithTime(string message)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + message + " ";
        }


        // These methods are used to create old logs for testing purposes
        private string GetOldFileName()
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            string folderName = @"C:\Users\j.whaley\source\repos\FileLoggerKata\FileLogger";
            string date = _dateTimeHelper.GetDateTimeNow().ToString("yyyyMMdd");
            DateTime today = _dateTimeHelper.GetDateTimeNow();
            return GetFileName(date, today, folderName);
        }

        //Helper method to determine if the file is on the weekend, if there is already a weekend.txt file, and if a new weekend.txt file needs to be made
        private string GetFileName(string date, DateTime today,string folderName)
        {
            if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
            {
                string filePath = System.IO.Path.Combine(folderName, "weekend.txt");
                if (File.Exists(filePath))
                {
                    string logText = File.ReadAllText(filePath);
                    string logtoread = logText.Substring(0, 10);
                    DateTime datetime = new DateTime();
                    DateTime.TryParse(logtoread, out datetime);
                    //Checks to see if the datetime is during the current weekend
                    if (datetime.Date == today.Date || datetime.AddDays(1).Date == today.Date)
                    {
                        //if it is then write the log to the current weekend
                        return "weekend.txt";
                    }
                    if (datetime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        //Makes sure that the new file name will start on the saturday
                        datetime = datetime.AddDays(-1);
                    }
                    string newFilePath = System.IO.Path.Combine(folderName, "weekend-" + datetime.ToString("yyyyMMdd") + ".txt");
                    System.IO.File.Move(filePath, newFilePath);
                }
                return "weekend.txt";
            }
            return "log" + date + ".txt";
        }


        /// <summary>
        /// This method is used in testing to write an old message using dateTimeHelper fake time functionality 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string GetOldMessageWithTime(string message)
        {
            return _dateTimeHelper.GetDateTimeNow().ToString("yyyy-MM-dd HH:mm:ss") + " " + message + " ";
        }

    }
}
