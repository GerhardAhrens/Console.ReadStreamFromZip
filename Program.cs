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
// Konsolen Applikation für Beispiele mit ZIP Archiv
// </summary>
//-----------------------------------------------------------------------

namespace Console.ReadStreamFromZip
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public class Program
    {
        private static string exePath = string.Empty;

        private static void Main(string[] args)
        {
            /* Pfad der EXE-Datei*/
            exePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            CreateDemoFiles();

            do
            {
                Console.Clear();
                Console.WriteLine("1. Erstellen Zip File");
                Console.WriteLine("2. Lesen Zip File");
                Console.WriteLine("3. Update Zip File");
                Console.WriteLine("4. Löschen einer Datei im Zip File");
                Console.WriteLine("5. Hinzufügen einer Datei zum Zip File");
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
                    else if (key == ConsoleKey.D3)
                    {
                        MenuPoint3();
                    }
                    else if (key == ConsoleKey.D4)
                    {
                        MenuPoint4();
                    }
                    else if (key == ConsoleKey.D5)
                    {
                        MenuPoint5();
                    }
                }
            }
            while (true);
        }

        private static void CreateDemoFiles()
        {
            for (int i = 1; i < 4; i++)
            {
                string demoDatei = Path.Combine(exePath, $"Datei_{i}.txt");
                if (File.Exists(demoDatei) == false)
                {
                    File.WriteAllText(demoDatei, $"Inhalt von Datei {i}");
                }
            }
        }

        private static void MenuPoint1()
        {
            Console.Clear();
            Console.WriteLine("Neues ZIP Archiv erstellen");

            string zipName = Path.Combine(exePath, "ZipContext.zip");
            IEnumerable<string> files = new DirectoryInfo(exePath).GetFiles("*.txt").Select(s => s.FullName);

            if (File.Exists(zipName))
            {
                File.Delete(zipName);
            }

            using (ZipArchive archive = ZipFile.Open(zipName, ZipArchiveMode.Create))
            {
                archive.Comment = "Kommentar zur Archiv";
                foreach (var fPath in files)
                {
                    archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                }
            }

            Console.WriteLine($"ZIP Archiv {Path.GetFileName(zipName)} erstellt.");
            Console.WriteLine("Mit einer beliebigen Taste zum Menü");
            Console.ReadKey();
        }

        private static void MenuPoint2()
        {
            Console.Clear();
            Console.WriteLine("ZIP Archiv lesen");

            string zipName = Path.Combine(exePath, "ZipContext.zip");
            using (ZipArchive archive = ZipFile.Open(zipName, ZipArchiveMode.Read))
            {
                archive.Comment = "Test Archiv";
                foreach (var entry in archive.Entries)
                {
                    using (StreamReader sr = new StreamReader(entry.Open()))
                    {
                        string archivFile = entry.Name;
                        Console.WriteLine(sr.ReadToEnd());
                        Console.WriteLine($"Datei {archivFile}.");
                    }
                }
            }

            Console.WriteLine("Mit einer beliebigen Taste zum Menü");
            Console.ReadKey();
        }

        private static void MenuPoint3()
        {
            Console.Clear();
            Console.WriteLine("ZIP Archiv updaten");

            string zipName = Path.Combine(exePath, "ZipContext.zip");
            using (ZipArchive archive = ZipFile.Open(zipName, ZipArchiveMode.Update))
            {
                archive.Comment = "Update ZIP Archiv";

                StringBuilder document = null;
                var entry = archive.GetEntry("Datei_1.txt");
                using (StreamReader reader = new StreamReader(entry.Open()))
                {
                    if (document == null)
                    {
                        document = new StringBuilder();
                        document.AppendLine(reader.ReadToEnd());
                    }
                }

                entry.Delete();
                entry = archive.CreateEntry("Datei_1.txt");
                entry.Comment = $"Update {entry.Name}";
                document.AppendLine("Inhalt von Datei 2");
                document.AppendLine("Inhalt von Datei 3");
                document.AppendLine("Inhalt von Datei 4");
                document.AppendLine("Inhalt von Datei 5");

                using (StreamWriter writer = new StreamWriter(entry.Open()))
                {
                    writer.Write(document);
                }
            }

            Console.WriteLine("Mit einer beliebigen Taste zum Menü");
            Console.ReadKey();
        }

        private static void MenuPoint4()
        {
            Console.Clear();
            Console.WriteLine("ZIP Archiv, Datei entfernen");

            string zipName = Path.Combine(exePath, "ZipContext.zip");
            using (ZipArchive archive = ZipFile.Open(zipName, ZipArchiveMode.Update))
            {
                archive.Comment = "Update ZIP Archiv";
                var entry = archive.GetEntry("Datei_1.txt");
                entry.Delete();
            }

            Console.WriteLine("Mit einer beliebigen Taste zum Menü");
            Console.ReadKey();
        }

        private static void MenuPoint5()
        {
            Console.Clear();
            Console.WriteLine("ZIP Archiv, Datei hinzufügen");

            string zipName = Path.Combine(exePath, "ZipContext.zip");
            string datei = Path.Combine(exePath, "Datei_3.txt");
            using (ZipArchive archive = ZipFile.Open(zipName, ZipArchiveMode.Update))
            {
                archive.Comment = "Hinzufügen einer Datei zum ZIP Archiv";
                archive.CreateEntryFromFile(datei, Path.GetFileName(datei));
            }

            Console.WriteLine("Mit einer beliebigen Taste zum Menü");
            Console.ReadKey();
        }
    }
}
