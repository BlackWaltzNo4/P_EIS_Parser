using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.FTPConnection
{
    class Connection
    {

        private FtpClient client;        

        public Connection()
        {

            client = new FtpClient(@"ftp://ftp.zakupki.gov.ru/");
            client.Credentials = new NetworkCredential("free", "free");

        }

        public FtpClient getClient()
        {

            //Console.WriteLine("Start connection");

            try
            {
                client.Connect();
            }
            catch
            {
                Console.WriteLine("Error occupied");
                return null;
            }

            //Console.WriteLine("Success");

            return client;

        }

    }
}
