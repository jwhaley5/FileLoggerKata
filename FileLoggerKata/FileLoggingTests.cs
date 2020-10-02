using FileLoggerProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;

namespace FileLoggerKata
{
    [TestClass]
    public class FileLoggingTests
    {
        // Change this folderName to correspond to the correct location of The Project
        string folderName = @"C:\Users\j.whaley\source\repos\FileLoggerKata\FileLogger\";
        
        [TestMethod]
        public void CreateFile()
        {
            FileLogger logger = new FileLogger();
            logger.Log("Test");
            string date = DateTime.Today.ToString("yyyyMMdd");
            string fileName = "log" + date + ".txt";
            string FilePath = folderName + fileName;
            bool fileExists = System.IO.File.Exists(FilePath);
            Assert.AreEqual(true, fileExists);
        }

        [TestMethod]
        public void CheckMessage()
        {
            DateTime date = DateTime.Now;
            FileLogger logger = new FileLogger();
            logger.Log("Test");
            string fileName = "log" + date.ToString("yyyyMMdd") + ".txt";
            string filePath = folderName + fileName;

            string message = DateTime.Now.ToString("M/d/yyyy HH:mm:ss") + " Test ";

            if (System.IO.File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                Assert.IsTrue(text.Contains("Test"));
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AddToExistingFile()
        {
            DateTime date = DateTime.Now;
            FileLogger logger = new FileLogger();
            logger.Log("Test");
            logger.Log("Test 2");
            string fileName = "log" + date.ToString("yyyyMMdd") + ".txt";
            string filePath = folderName + fileName;

            if (System.IO.File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                Assert.IsTrue(text.Contains("Test 2"));
            }
            else
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void UsedLoggerOnNewDay()
        {
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2020, 9, 29);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow()).Returns(fakeDate);
            FileLogger logger = new FileLogger(mockDateTimeHelper.Object);
            //Should create a log.txt file for the 29th
            logger.LogOldMessage("oldMessage");
            //Should create a log.txt file for the current day and place info inside
            logger.Log("New Message on the next day");

            DateTime date = DateTime.Now;
            string fileName = "log" + date.ToString("yyyyMMdd") + ".txt";
            string filePath = folderName + fileName;

            // If the logger created a new file then the test should pass
            if (System.IO.File.Exists(filePath))
            {
                Assert.IsTrue(1 == 1);
            }
            else
            {
                Assert.Fail();
            }


        }

        [TestMethod]
        public void VerifyWeekendFileCreation()
        {
            DeleteAllWeekendEntries();

            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            var fakeDate = new DateTime(2020, 9, 19);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow()).Returns(fakeDate);
            FileLogger logger = new FileLogger(mockDateTimeHelper.Object);
            logger.LogOldMessage("Logged on Saturday 9/19");

            string fileName = "weekend.txt";
            string filePath = folderName + fileName;
            if(System.IO.File.Exists(filePath))
            {
                Assert.IsTrue(1 == 1);
            }
            else
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void VerifySaturdaySundaySameWeekendEntry()
        {
            DeleteAllWeekendEntries();

            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            //Saturday
            var fakeDate = new DateTime(2020, 9, 19);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow()).Returns(fakeDate);
            FileLogger logger = new FileLogger(mockDateTimeHelper.Object);
            logger.LogOldMessage("Logged on Saturday 9/19");

            //Sunday
            fakeDate = new DateTime(2020, 9, 20);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow()).Returns(fakeDate);
            logger = new FileLogger(mockDateTimeHelper.Object);
            logger.LogOldMessage("Logged on Sunday 9/20");

            string fileName = "weekend.txt";
            string filePath = folderName + fileName;
            string weekendBeforeFileName = "weekend-20200919.txt";
            string weekendBeforeFilePath = folderName + weekendBeforeFileName;
            if (System.IO.File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                Assert.IsTrue(text.Contains("Sunday 9/20") && text.Contains("Saturday 9/19"));
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void VerifyNewWeekendFileCreated()
        {
            DeleteAllWeekendEntries();
            var mockDateTimeHelper = new Mock<IDateTimeHelper>();
            //Saturday 9/19
            var fakeDate = new DateTime(2020, 9, 19);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow()).Returns(fakeDate);
            FileLogger logger = new FileLogger(mockDateTimeHelper.Object);
            logger.LogOldMessage("Logged on Saturday 9/19");

            //Sunday 9/20
            fakeDate = new DateTime(2020, 9, 20);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow()).Returns(fakeDate);
            logger = new FileLogger(mockDateTimeHelper.Object);
            logger.LogOldMessage("Logged on Sunday 9/20");

            //Saturday 9/26
            fakeDate = new DateTime(2020, 9, 26);
            mockDateTimeHelper.Setup(o => o.GetDateTimeNow()).Returns(fakeDate);
            logger = new FileLogger(mockDateTimeHelper.Object);
            logger.LogOldMessage("Logged on Saturday 9/26");

            string weekendBeforeFileName = "weekend-20200919.txt";
            string weekendBeforeFilePath = folderName + weekendBeforeFileName;
            string fileName = "weekend.txt";
            string filePath = folderName + fileName;
            if (System.IO.File.Exists(filePath) && System.IO.File.Exists(weekendBeforeFilePath)) 
            {
                Assert.IsTrue(1 == 1);
                string newText = File.ReadAllText(filePath);
                string oldText = File.ReadAllText(weekendBeforeFilePath);
                Assert.IsTrue(newText.Contains("Saturday 9/26") && oldText.Contains("Saturday 9/19"));
            }
            else
            {
                Assert.Fail();
            }
        }

        public void DeleteAllWeekendEntries()
        {
            string weekendBeforeFileName = "weekend-20200919.txt";
            string weekendBeforeFilePath = folderName + weekendBeforeFileName;
            if (System.IO.File.Exists(weekendBeforeFilePath))
            {
                File.Delete(weekendBeforeFilePath);
            }
            string fileName = "weekend.txt";
            string filePath = folderName + fileName;
            if (System.IO.File.Exists(filePath))
            {
                File.Delete(filePath);
            }


        }

    }
}
