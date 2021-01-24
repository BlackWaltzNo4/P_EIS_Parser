using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.FileSystem
{
    class Unpack
    {

        public static void GetFiles(Misc.ID id)
        {

            Directory.GetFiles(Misc.Environment.tempFolder, $"{id.GetCurrentID()}.zip", SearchOption.TopDirectoryOnly).ToList().ForEach(zipFilePath =>
            {
                var extractPathForCurrentZip = Path.Combine(Misc.Environment.tempFolder, Path.GetFileNameWithoutExtension(id.GetCurrentID()));
                if (!Directory.Exists(extractPathForCurrentZip))
                {
                    Directory.CreateDirectory(extractPathForCurrentZip);
                }
                try
                {
                    ZipFile.ExtractToDirectory(zipFilePath, extractPathForCurrentZip);

                    //Console.WriteLine(zipFilePath);
                }
                catch
                {
                    //Console.WriteLine("Unzipping error occupied");
                }
            });

        }

    }
}
