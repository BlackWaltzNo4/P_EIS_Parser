using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.FileSystem
{
    class Delete
    {

        public static bool DeleteEmptyDirectory(string path)
        {

            if (String.IsNullOrEmpty(path))
                throw new ArgumentException(
                    "Starting directory is a null reference or an empty string",
                    "dir");

            try
            {
                var entries = Directory.EnumerateFileSystemEntries(path);

                if (!entries.Any())
                {
                    try
                    {
                        Directory.Delete(path);
                        return true;
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException) { }
                }
            }
            catch (UnauthorizedAccessException) { }

            return false;

        }

        public static void DeleteDirectory(string name)
        {
            string path = Path.Combine(Misc.Environment.tempFolder, name);

            if (String.IsNullOrEmpty(path))
                throw new ArgumentException(
                    "Starting directory is a null reference or an empty string",
                    "dir");
            try
            {
                string[] files = Directory.GetFiles(path);
                string[] directories = Directory.GetDirectories(path);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in directories)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(path, false);
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
        }

        public static void DeleteTempFile(string name)
        {

            File.Delete(Path.Combine(Misc.Environment.tempFolder, name));

        }

    }
}
