using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPApp_Server.Misc;

namespace FTPApp_Server
{
    class Program
    {

        static void Main(string[] args)
        {

            Main main = new Main();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //string d = DateTime.Now.ToString();

            //RegionList.Region44FZ.Length
            for (int i = 0; i < RegionList.Region44FZ.Length; i++)
            {
                main.GetNewEntriesList(RegionList.Region44FZ[i], FTPApp_Server.Main.Mode.CurrentMonth);

                //main.GetNewEntriesList_FORTEST(RegionList.Region44FZ[i]);
            }
            Console.WriteLine("Data receiving has complete. Waiting for next execution.");
            Console.WriteLine("");
            Console.WriteLine("");
            //Console.ReadLine();

            do
            {
                if (stopwatch.Elapsed.TotalMinutes >= 360)
                {
                    for (int i = 0; i < RegionList.Region44FZ.Length; i++)
                    {
                        main.GetNewEntriesList(RegionList.Region44FZ[i], FTPApp_Server.Main.Mode.CurrentMonth);

                        //main.GetNewEntriesList_FORTEST(RegionList.Region44FZ[i]);
                    }
                    stopwatch.Reset();
                    Console.WriteLine("Periodic data receiving has complete. Waiting for next execution.");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
            }
            while (true);

        }
    }
}
