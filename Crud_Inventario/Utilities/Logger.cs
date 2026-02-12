using System;
using System.IO;
using System.Web;

namespace Crud_Inventario.Utilities
{
    public static class Logger
    {
        private static readonly string LogDirectory;
        static Logger()
        {
            LogDirectory = HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/Logs.txt") :
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Logs.txt");

            if (Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        public static void Log(string message)
        {
            try
            {
                File.AppendAllText(LogDirectory, $"[{DateTime.Now:HH:mm:ss}] {message}-{Environment.NewLine}");
            }
            catch { }
        }

        public static void LogError(string message, Exception ex = null)
        {
            string logMessage = ex != null ? $"ERROR: {message} - {ex.Message}" : $"ERROR: {message}";
            Log(logMessage);
        }
    }
}