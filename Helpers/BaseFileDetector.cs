using NLog;
using System;

namespace Helpers
{
    public class BaseFileDetector
    {
        public Logger Logger { get; set; }

        public BaseFileDetector()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public DateTime FileDate(
            string dir, 
            string fileNameWithNoExtension)
        {
            var len = fileNameWithNoExtension.Length;
            if (DateIsNotEmbeddedIntheFileName(len))
            {
                //  the date is not embedded in the log file name
                var fi = new System.IO.FileInfo(dir + fileNameWithNoExtension);
                var dateOfFile = fi.LastWriteTime.ToString();
                Logger.Trace("  Update Date of {0} is {1}", fi.Name, dateOfFile);
                var fileDate = new DateTime(1, 1, 1);
                if (DateTime.TryParse(dateOfFile, out fileDate))
                    return fileDate;
                else
                {
                    Logger.Error("filedate {0} did not parse", dateOfFile);
                    return new DateTime(1, 1, 1);
                }
            }
            return DateTime.Parse(
                fileNameWithNoExtension.Substring(
                    len - 10, 
                    10));
        }

        private static bool DateIsNotEmbeddedIntheFileName(int len)
        {
            return len < 21;
        }

        public bool FileMatches(
            string dir, 
            string fileNameWithNoExtension, 
            string logType, 
            DateTime logDate)
        {
            var isMatch = false;
            if (fileNameWithNoExtension.Length < 10)
            {
                Trace(
                    $"  file {fileNameWithNoExtension} length is too small");
                return false;
            }
            if (fileNameWithNoExtension.StartsWith(logType))
            {
                var fileDate = FileDate(
                    dir, 
                    fileNameWithNoExtension);
                if (fileDate.Date > logDate.Date)
                {
                    if (fileDate.Date == DateTime.Now.Date)
                        Trace(
                            $" file {fileNameWithNoExtension} may still be in progress");
                    else
                        isMatch = true;
                }
                else
                    Trace(
                        $"file {fileNameWithNoExtension} is less than the last logdate of {logDate.Date:d}");
            }
            else
                Trace($"file {fileNameWithNoExtension} does not start with {logType}");
            return isMatch;
        }

		private void Trace(
            string msg)
		{
			Logger.Trace($"      {msg}");
		}
	}
}