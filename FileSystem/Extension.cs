using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.FileSystem
{
    public class Extension
    {

        private Extension(string value) { Value = value; }

        public string Value { get; set; }

        public static Extension xml { get { return new Extension(".xml"); } }
        public static Extension sig { get { return new Extension(".sig"); } }
        public static Extension zip { get { return new Extension(".zip"); } }

    }
}
