using System;
using System.Linq;

namespace TypescriptCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1) { Console.WriteLine("Please supply the target directory to clean."); }

            var targetDir = args.First().Trim();
            var logDetailedErrors = false;

            if (args.Length > 1)
            {
                Boolean.TryParse(args[1], out logDetailedErrors);
            }

            cleanRecursive(targetDir, logDetailedErrors);
        }

        private static void cleanRecursive(String targetDirectory, bool logDetailedErrors)
        {
            if (String.IsNullOrWhiteSpace(targetDirectory)) { return; }

            if (!targetDirectory.EndsWith("\\")) { targetDirectory += "\\"; }

            try
            {
                var currentDir = new System.IO.DirectoryInfo(targetDirectory);
                if (!currentDir.Exists) { return; }

                cleanTypeScriptOutput(currentDir, logDetailedErrors);
                foreach (var subfolder in currentDir.EnumerateDirectories())
                {
                    cleanRecursive(subfolder.FullName, logDetailedErrors);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening directory for cleaning: " + targetDirectory);
                if (logDetailedErrors)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }


        private static void cleanTypeScriptOutput(System.IO.DirectoryInfo targetDirectory, bool logDetailedErrors)
        {
            foreach (var mapFile in targetDirectory.EnumerateFiles("*.js.map"))
            {
                if (!mapFile.IsReadOnly)
                {
                    try
                    {
                        var baseName = mapFile.FullName.Replace(".js.map", String.Empty);
                        var associatedScript = new System.IO.FileInfo(baseName + ".js");

                        mapFile.Delete();
                        Console.WriteLine("Cleaned file: " + mapFile.FullName);

                        if (associatedScript.Exists)
                        {
                            associatedScript.Delete();
                            Console.WriteLine("Cleaned file: " + associatedScript.FullName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error cleaning file: " + mapFile.FullName);
                        if (logDetailedErrors)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}
