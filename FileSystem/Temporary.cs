using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.FileSystem
{
    class Temporary
    {

        public static bool SetupTempoparyFolder(out DateTime modified, ref Misc.Tracking tracker, FtpListItem item, ref Misc.ID assignedID, ref FtpClient client, out string fileName)
        {
            try 
            {
                FTPConnection.Download.GetFile(ref client, item, out DateTime dateTime, out string name, ref assignedID);

                fileName = name;
                modified = dateTime;

                Unpack.GetFiles(assignedID);

                tracker.IncreaseCount();

                //Clear .SIG files
                foreach (string path in Files.GetListOfFiles($"{Misc.Environment.tempFolder}{assignedID.GetCurrentID()}", Extension.sig))
                {
                    Delete.DeleteTempFile(path);
                }

                bool isEmpty = Delete.DeleteEmptyDirectory($"{Misc.Environment.tempFolder}{assignedID.GetCurrentID()}");
                Delete.DeleteTempFile(Path.Combine(Misc.Environment.tempFolder, $"{assignedID.GetCurrentID()}{Extension.zip.Value}"));

                return isEmpty;
            }
            catch
            {
                fileName = String.Empty;
                modified = DateTime.Today;
                return false;
            }
        }
    }
}
