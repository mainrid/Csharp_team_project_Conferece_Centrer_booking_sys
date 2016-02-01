using System;

namespace Keskus.BLL
{
    /// <remarks>
    /// Class Filter handles logging of app events.
    /// Application logic events are written to /log.txt file
    /// </remarks>
     public class Logger
    {
        private string _messageToLog;
        private string _logFileName;
        private string _currentEcecutionDirPath;
        private string _logFilePath;
        public Logger(string classNameToLog)
        {
            _logFileName = "log.txt";
            _messageToLog = classNameToLog;
            _currentEcecutionDirPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _logFilePath = string.Format("{0}\\{1}", _currentEcecutionDirPath, _logFileName);
        }


        public Logger(bool isUserLog, string changeToLog)
        {
            _logFileName = "userlog.txt";
            _messageToLog = changeToLog;
            _currentEcecutionDirPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _logFilePath = string.Format("{0}\\{1}", _currentEcecutionDirPath, _logFileName);
        }

        /// <remarks>
        /// http://stackoverflow.com/questions/5057567/how-to-do-logging-in-c
        /// </remarks>
        /// <param name="logString"></param>
        public void Trace(String logString)
        {
            DateTime logTimeStamp = DateTime.Now;

            logString = string.Format("{0} | {1} - {2}", logTimeStamp, _messageToLog, logString);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(_logFilePath, true))
            {
                // Write the string to a file.append mode is enabled so that the log
                // lines get appended to log.txt than wiping content and writing the log
                file.WriteLine(logString);
            }
        }
    }
}
