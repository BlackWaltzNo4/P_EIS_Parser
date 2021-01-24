using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Misc
{
    class ID
    {

        private static long num;

        private static string current;

        public ID()
        {
            num = 0;
        }

        public string GetUniqueID()
        {                     
            return current = $"{DateTime.Now.Ticks.ToString()}{num++:D12}_temp";
        }

        public string GetCurrentID()
        {
            return current;
        }

    }
}
