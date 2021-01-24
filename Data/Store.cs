using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Data
{
    class Store
    {
        public static void StoreDownloadParameters(string logMessage, TextWriter writer)
        {
            writer.WriteLine(logMessage);
        }

        public static bool LogContainsEntry(StreamReader reader, string entry)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (String.Equals(entry, line, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

    }
}
