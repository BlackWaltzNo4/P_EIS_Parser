using FluentFTP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPApp_Server.FileSystem;
using FTPApp_Server.XML.PurchasePlanPosition;
using System.Globalization;

namespace FTPApp_Server
{
    class Main
    {

        FtpClient client;

        //FTPConnection.FilesListing listing;

        Misc.ID id;

        Misc.Tracking newEntriesCounter;

        Stopwatch stopwatch = new Stopwatch();
        Stopwatch stopwatchLocal = new Stopwatch();
        TimeSpan timeSpan;

        public enum Mode { CurrentMonth, All}

        public Main()
        {

            client = new FTPConnection.Connection().getClient();

            //listing = new FTPConnection.FilesListing(String.Format("/fcs_regions/{0}/plangraphs2020/currMonth/", "Adygeja_Resp"));

            id = new Misc.ID();

            newEntriesCounter = new Misc.Tracking();

        }

        //Пока что оставил в параметрах регион, потому что не обрабатываю 223 ФЗ, затем это нужно будет заменить на путь, и написать метод, 
        //возвращающий его; см. также – (enum {44fz, 223fz})
        public void GetNewEntriesList(string region, Mode mode)
        {
            newEntriesCounter.ResetCount();

            stopwatch.Start();

            int positions = 0;
            int positionsFiltered = 0;
            //nt positionDoubled = 0;
            //int positionsModified = 0;
            int dataReadingErrors = 0;
            //int SQLReadingErrors = 0;

            string _path;

            if (mode == Mode.CurrentMonth) _path = "/fcs_regions/{0}/plangraphs2020/currMonth/";
            else _path = "/fcs_regions/{0}/plangraphs2020/";

            foreach (FtpListItem item in FTPConnection.FilesListing.GetListing(client, String.Format(_path, region)))
            {
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    //Проверка на наличие уже скаченного ранее архива
                    if (!Data.CheckEntries.LogHasEntries(Path.GetFileNameWithoutExtension(item.FullName)))
                    {
                        stopwatchLocal.Start();

                        int t = 0;  //filtered
                        int p = 0;  //position
                        int u = 0;  //unique
                        int e = 0;  //errors
                        int d = 0;  //doubles
                        int m = 0;  //non-doubles positions requieres rewrite

                        //Скачивание, распаковка и проверка на наличие вложений
                        if (!Temporary.SetupTempoparyFolder(out DateTime dateTime, ref newEntriesCounter, item, ref id, ref client, out string fileName))
                        {
                            try
                            {
                                foreach (string path in Files.GetListOfFiles($"{Misc.Environment.tempFolder}{id.GetCurrentID()}", Extension.xml))
                                {
                                    XML.Document temp_Document = XML.Data.GetDocumentData(path, region, out (ushort positions, ushort dataReadingErrors) counter);

                                    int total = 0;
                                    int unique = 0;
                                    int doubles = 0;
                                    int nondoubles = 0;

                                    foreach (Position position in temp_Document.positionsList)
                                    {
                                        ///NOW CHECK ONLY 202003761000019001000016       POSITION ---- ATTENTION!!!
                                        if (!Filter.BlackBox.GetPreliminaryFilterResult(position.OKPDCode.GetText()) && (position.FZ.GetText() == "44FZ"))// && (position.positionNumber.GetText().Trim(' ') == "202003761000019001000016      ".Trim(' ')))
                                        {
                                            total++;

                                            Console.Title = $"{total} / {path}";

                                            //Проверка на наличие такой же позиции в базе
                                            //
                                            //Возможное узкое место, вероятно, на время парсинга имеет смысл хранить файл, считывающий список всех ИКЗ и сравнивать с ним,
                                            //а не прогонять каждый раз базу. Есть мнение, что операции с файлами слишком затратны, поэтому нужно хранить массив в памяти
                                            //
                                            //NB! ПЕРЕДЕЛАТЬ
                                            //

                                            switch (SQL.Entry.GetNumberOfEntries(position, position.positionNumber.GetText(), dateTime, out string databaseName))
                                            {
                                                //In case of thing is last modified
                                                case SQL.Entry.CheckResult.Cancelled:
                                                    {
                                                        nondoubles++;
                                                        SQL.Record.UpdatetPositionInDatabase(position, dateTime, databaseName, position.positionNumber.GetText(), SQL.Entry.CheckResult.Cancelled);
                                                        break;
                                                    }
                                                case SQL.Entry.CheckResult.M_KBK:
                                                    {
                                                        nondoubles++;
                                                        SQL.Record.UpdatetPositionInDatabase(position, dateTime, databaseName, position.positionNumber.GetText(), SQL.Entry.CheckResult.M_KBK);
                                                        break;
                                                    }
                                                case SQL.Entry.CheckResult.M_KVR:
                                                    {
                                                        nondoubles++;
                                                        SQL.Record.UpdatetPositionInDatabase(position, dateTime, databaseName, position.positionNumber.GetText(), SQL.Entry.CheckResult.M_KVR);
                                                        break;
                                                    }
                                                case SQL.Entry.CheckResult.M_OKPD:
                                                    {
                                                        nondoubles++;
                                                        SQL.Record.UpdatetPositionInDatabase(position, dateTime, databaseName, position.positionNumber.GetText(), SQL.Entry.CheckResult.M_OKPD);
                                                        break;
                                                    }
                                                case SQL.Entry.CheckResult.M_publishYear:
                                                    {
                                                        nondoubles++;
                                                        SQL.Record.UpdatetPositionInDatabase(position, dateTime, databaseName, position.positionNumber.GetText(), SQL.Entry.CheckResult.M_publishYear);
                                                        break;
                                                    }
                                                case SQL.Entry.CheckResult.M_purchase:
                                                    {
                                                        nondoubles++;
                                                        SQL.Record.UpdatetPositionInDatabase(position, dateTime, databaseName, position.positionNumber.GetText(), SQL.Entry.CheckResult.M_purchase);
                                                        break;
                                                    }
                                                case SQL.Entry.CheckResult.M_tot:
                                                    {
                                                        nondoubles++;
                                                        SQL.Record.UpdatetPositionInDatabase(position, dateTime, databaseName, position.positionNumber.GetText(), SQL.Entry.CheckResult.M_tot);
                                                        break;
                                                    }

                                                //In case of thing is not changed
                                                case SQL.Entry.CheckResult.NonChanged:
                                                    {
                                                        doubles++;
                                                        //SQL.Record.UpdatetPositionInDatabase(position, dateTime, databaseName, position.positionNumber.GetText());
                                                        break;
                                                    }

                                                //In case of thing is unique
                                                case SQL.Entry.CheckResult.Unique:
                                                    {
                                                        unique++;
                                                        SQL.Record.InsertPositionsToDatabase(position, DateTimeFormatInfo.InvariantInfo.GetMonthName(dateTime.Month), dateTime);
                                                        //SQL.Record.InsertPositionsToDatabase(position, "January", dateTime);
                                                        break;
                                                    }
                                            }
                                        }
                                    }

                                    positions += counter.positions;
                                    positionsFiltered += unique;
                                    dataReadingErrors += counter.dataReadingErrors;

                                    t += total;
                                    p += counter.positions;
                                    u += unique;
                                    e += counter.dataReadingErrors;
                                    d += doubles;
                                    m += nondoubles;

                                    Console.WriteLine($"[{Path.GetFileName(path),10:#}][region: {region}][month: {DateTimeFormatInfo.InvariantInfo.GetMonthName(dateTime.Month)}][pos (ex SP): {counter.positions,3:##0}][data reading errors: {counter.dataReadingErrors,3:##0}][total: {(unique + nondoubles + doubles),3:##0}\\{total,3:##0}][modified: {nondoubles,3:##0}][doubled: {doubles,3:##0}][stored: {unique,3:##0}]");
                                    
                                }

                                Delete.DeleteDirectory(id.GetCurrentID());

                                //Запись в лог об успешно обработанном файле
                                using (StreamWriter w = File.AppendText($"{Misc.Environment.dataStoreFolder}log.store"))
                                {
                                    Data.Store.StoreDownloadParameters(fileName, w);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }

                        using (StreamWriter w = File.AppendText($"{Misc.Environment.logFolder}log.LOG"))
                        {
                            if (p > 0) Misc.Log.CreateLogMessage(w, $"{item.Name}", $"Positions: {p}. Errors: {e}. Stored: {u}.");
                            else Misc.Log.CreateLogMessage(w, $"{item.Name}");
                        }

                        stopwatchLocal.Stop();
                        timeSpan = stopwatchLocal.Elapsed;
                        stopwatchLocal.Reset();

                        Console.WriteLine($"> {id.GetCurrentID()} {item.Name.Substring(0, 5)}... Modified:[{item.Modified.ToString("dd-MM-yyyy HH:mm:ss")}][elapsed time: {timeSpan.ToString(@"mm\:ss\:ffff")}]");
                    }
                }
            }

            stopwatch.Stop();
            //
            timeSpan = stopwatch.Elapsed;

            Console.WriteLine($"{region} has complete. Time elapsed: {timeSpan.ToString(@"hh\:mm\:ss")}. New archives: {newEntriesCounter.GetCount()}. Positions total: {positions}. Positions stored: {positionsFiltered}");

            //Console.ReadKey();
        }

        //public void GetNewEntriesList_FORTEST(string region)
        //{
        //    newEntriesCounter.ResetCount();

        //    stopwatch.Start();

        //    int positions = 0;
        //    int positionsFiltered = 0;
        //    int errors = 0;

        //    foreach (FtpListItem item in FTPConnection.FilesListing.GetListing(client, String.Format($"/fcs_regions/{region}/plangraphs2020/")))
        //    {
        //        if (item.Type == FtpFileSystemObjectType.File)
        //        {
        //            //Проверка на наличие уже скаченного ранее файла
        //            if (!Data.CheckEntries.LogHasEntries(Path.GetFileNameWithoutExtension(item.FullName)) && (item.Modified.Year == DateTime.Now.Year))
        //            {
        //                stopwatchLocal.Start();

        //                int p = 0;
        //                int f = 0;
        //                int e = 0;

        //                //Скачивание, распаковка и проверка на наличие вложений
        //                if (!Temporary.SetupTempoparyFolder(out DateTime dateTime, ref newEntriesCounter, item, ref id, ref client))
        //                {
        //                    foreach (string path in Files.GetListOfFiles($"{Misc.Environment.tempFolder}{id.GetCurrentID()}", Extension.xml))
        //                    {

        //                        XML.Document temp_Document = XML.Data.GetDocumentData(path, region, out (ushort positions, ushort errors) counter);

        //                        int positionsStoredAfterPreFilter = 0;

        //                        foreach (Position position in temp_Document.positionsList)
        //                        {
        //                            if (!Filter.BlackBox.GetPreliminaryFilterResult(position.OKPDCode.GetText()) && (position.FZ.GetText() == "44FZ"))
        //                            {
        //                                positionsStoredAfterPreFilter++;
        //                                SQL.Record.InsertPositionsToDatabase(position, dateTime);
        //                            }
        //                        }

        //                        positions += counter.positions;
        //                        positionsFiltered += positionsStoredAfterPreFilter;
        //                        errors += counter.errors;

        //                        p += counter.positions;
        //                        f += positionsStoredAfterPreFilter;
        //                        e += counter.errors;

        //                        Console.WriteLine($"[{Path.GetFileName(path),50:#}][pos (exclude SP): {counter.positions,4:###0}][err: {counter.errors,4:###0}][stored: {positionsStoredAfterPreFilter}]");
        //                    }

        //                    Delete.DeleteDirectory(id.GetCurrentID());
        //                }

        //                using (StreamWriter w = File.AppendText($"{Misc.Environment.logFolder}log.LOG"))
        //                {
        //                    if (p > 0) Misc.Log.CreateLogMessage($"{item.Name} has been stored with {e} errors. Positions: {p}. Stored: {f}.", w);
        //                    else Misc.Log.CreateLogMessage($"{item.Name} has been stored", w);
        //                }

        //                stopwatchLocal.Stop();
        //                timeSpan = stopwatchLocal.Elapsed;
        //                stopwatchLocal.Reset();

        //                Console.WriteLine($"> {id.GetCurrentID()} {item.Name.Substring(0, 5)}... Modified:[{item.Modified.ToString("dd-mm-yyyy HH:mm:ss")}][elapsed time: {timeSpan.ToString(@"mm\:ss\:ffff")}]");
        //            }
        //        }
        //    }

        //    stopwatch.Stop();
        //    //
        //    timeSpan = stopwatch.Elapsed;

        //    Console.WriteLine($"{region} TESTDOWNLOAD has complete. Time elapsed: {timeSpan.ToString(@"hh\:mm\:ss")}. New archives: {newEntriesCounter.GetCount()}. Positions total: {positions}. Positions stored: {positionsFiltered}");

        //    //Console.ReadKey();
        //}

    }
}
