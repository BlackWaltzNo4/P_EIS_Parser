using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Data
{
    class CheckEntries
    {

        public static bool LogHasEntries(string entry)
        {

            using (StreamReader reader = File.OpenText($"{Misc.Environment.dataStoreFolder}log.store"))
            {
                if (Store.LogContainsEntry(reader, entry)) return true; else return false;
            }

        }

    }
}
