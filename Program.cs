//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.04.2025 10:43:36</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//
// <remarks>
// https://stackoverflow.com/questions/22604941/how-can-i-unzip-a-file-to-a-net-memory-stream
// </remarks>
//-----------------------------------------------------------------------

namespace Console.ReadStreamFromZip
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;

    public class Program
    {
        private static string exePath = string.Empty;

        private static void Main(string[] args)
        {
            /* Pfad der EXE-Datei*/
            exePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            do
            {
                Console.Clear();
                Console.WriteLine("1. Create Zip File");
                Console.WriteLine("2. Menüpunkt 2");
                Console.WriteLine("X. Beenden");

                Console.WriteLine("Wählen Sie einen Menüpunkt oder 'x' für beenden");
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.X)
                {
                    Environment.Exit(0);
                }
                else
                {
                    if (key == ConsoleKey.D1)
                    {
                        MenuPoint1();
                    }
                    else if (key == ConsoleKey.D2)
                    {
                        MenuPoint2();
                    }
                }
            }
            while (true);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            IEnumerable<string> files = new DirectoryInfo(exePath).GetFiles("*.txt").Select(s => s.FullName);

            string zipName = Path.Combine(exePath, "ZipContext.zip");
            ZipHelper.CreateZipFile(zipName, files);

            Console.ReadKey();
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            string zipName = Path.Combine(exePath, "ZipContext.zip");
            using (var file = File.OpenRead(zipName))
            {
                using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
                {
                    foreach (var entry in zip.Entries)
                    {
                        using (StreamReader sr = new StreamReader(entry.Open()))
                        {
                            Console.WriteLine(sr.ReadToEnd());
                        }
                    }
                }
            }

           Console.ReadKey();
        }
    }

    public static class ZipHelper
    {
        /// <summary>
        /// Create a ZIP file of the files provided.
        /// </summary>
        /// <param name="fileName">The full path and name to store the ZIP file at.</param>
        /// <param name="files">The list of files to be added.</param>
        public static void CreateZipFile(string fileName, IEnumerable<string> files)
        {
            // Create and open a new ZIP file
            var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                // Add the entry for each file
                zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
            }
            // Dispose of the object when we are done
            zip.Dispose();
        }
    }
}
