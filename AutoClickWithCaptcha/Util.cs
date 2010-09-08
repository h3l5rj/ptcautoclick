using System;
using System.IO;

namespace AutoClickWithCaptcha
{
    class Util
    {
        private static Boolean logToFile = true;

        public const string USERNAME = "tranvinhtruong";
        public const string PASSWORD = "tctlT1005";

        public static void writeLog(string logContent)
        {
            if (logToFile)
            {
                File.AppendAllText("AutoClickWithCaptcha.log", "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + logContent + "\r\n");
            }
            else
            {
                Console.WriteLine(logContent);
            }
        }
    }
}
