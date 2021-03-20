using Helpers.Interfaces;
using System;
using System.Collections.Generic;

namespace Helpers
{
    public class LogFileDetector : BaseFileDetector, IDetectLogFiles
    {
        public List<string> DetectLogFileIn(
            string dir, 
            string logType, 
            DateTime logDate)
        {
            var fileList = new List<string>();
            var filesInDir = System.IO.Directory.GetFiles(dir);
            Trace($"      found {filesInDir.Length} files");
            foreach (var file in filesInDir)
            {
                var filepart = FilePartFile(
                    dir, 
                    file);
                if (FileMatches(
                        dir,
                        filepart,
                        logType,
                        logDate))
                {
                    fileList.Add(file);
                }
                else
                    Trace($"      filepart:{filepart} does not match");
            }
            return fileList;
        }

		private void Trace(string msg)
		{
            Logger.Trace(msg);
		}

		public string FilePartFile(string dir, string file)
        {
            var len = file.Length - 4 - dir.Length;
            if (len < 10) return file;
            var filePart = file.Substring(dir.Length, len);
            return filePart;
        }
    }
}