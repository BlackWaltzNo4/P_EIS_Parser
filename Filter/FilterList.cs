using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Filter
{
    class FilterList
    {
        //FileSystem.Read textReader;

        private static string filter;

        public FilterList(string path)
        {            

            filter = FileSystem.Read.GetTextContent(path);

        }

        public string GetFilter()
        {

            return filter;

        }

    }
}
