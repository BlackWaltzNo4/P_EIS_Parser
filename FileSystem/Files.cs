using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.FileSystem
{
    class Files
    {

        public static string[] GetListOfFiles(string path, Extension extension)
        {

            return Directory.GetFiles(path, $"*{extension.Value}", SearchOption.TopDirectoryOnly);

        }

    }

}
