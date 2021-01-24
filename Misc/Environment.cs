using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Misc
{
    class Environment
    {

        public static string tempFolder = $"{System.Environment.CurrentDirectory}/Temp_Data/";

        public static string dataStoreFolder = $"{System.Environment.CurrentDirectory}/Store/";

        public static string logFolder = $"{System.Environment.CurrentDirectory}/Log/";

        public static string filtersFolder = $"{System.Environment.CurrentDirectory}/Filters/";

    }
}
