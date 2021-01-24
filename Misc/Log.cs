using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Misc
{
    class Log
    {
        public static void CreateLogMessage(TextWriter writer, string logMessage1, string logMessage2 = "-?/")
        {
            writer.Write("\r\nLog Entry : ");
            writer.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            writer.WriteLine($"{logMessage1}");
            if (logMessage2 != "-?/") writer.WriteLine($"{logMessage2}");
            writer.WriteLine("-------------------------------");
        }

    }
}
