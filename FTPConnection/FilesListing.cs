using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.FTPConnection
{
    class FilesListing
    {

        //string ftpPath;

        //public FilesListing(string ftpPath)
        //{

        //    this.ftpPath = ftpPath;

        //}

        public static FtpListItem[] GetListing(FtpClient client, string ftpPath)
        {           

            try 
            {
                return client.GetListing(ftpPath);
            }
            catch
            {
                return null;
            }

        }

    }
}
