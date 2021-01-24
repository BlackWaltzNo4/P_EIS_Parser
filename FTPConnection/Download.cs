using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.FTPConnection
{
    class Download
    {

        public static void GetFile(ref FtpClient client, FtpListItem item, out DateTime dateTime, out string name, ref Misc.ID id)
        {

            FtpClient current = new Connection().getClient();

            current.DownloadFile($"{Misc.Environment.tempFolder}{id.GetUniqueID()}{Path.GetExtension(item.FullName)}", item.FullName, FtpLocalExists.Append, FtpVerify.None, null);

            dateTime = item.Modified;

            name = Path.GetFileNameWithoutExtension(item.FullName);

            current.Dispose();

        }

    }
}
