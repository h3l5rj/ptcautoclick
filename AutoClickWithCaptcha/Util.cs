using System;
using System.IO;

namespace AutoClickWithCaptcha
{
    class Util
    {
        private static Boolean logToFile = true;

        public const string USERNAME = "tranvinhtruong";
        public const string PASSWORD = "tctlT1005";

        public static void writeLog(string siteName, string logContent)
        {
            if (logToFile)
            {
                if (!Directory.Exists(siteName))
                {
                    Directory.CreateDirectory(siteName);
                }
                File.AppendAllText(siteName + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log", "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + logContent + "\r\n");
            }
            else
            {
                Console.WriteLine(logContent);
            }
        }
    }
}
