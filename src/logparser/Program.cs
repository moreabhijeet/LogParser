using System;
using System.Collections.Generic;
using System.IO;

namespace logparser
{
    class Program
    {
        static int Main(string[] args)
        {
            List<string> inputDirectories = new List<string>();
            List<string> logLevel = new List<string>();
            String outputDirectory = "";

            if (args.Length == 0 || args[0] == "--help")
            {
                Console.WriteLine("Usage: logParser --log-dir <dir> --log-level <level> --csv <out>");
                Console.WriteLine("--log-dir    Directory to parse recursively for .log files");
                Console.WriteLine("--csv        Output file-path (absolute/relative)");
                Console.WriteLine("--log-level  <info/war/debug>");
                return 1;
            }
            else
            {
                int count = 0;
                foreach (var arg in args)
                {
                    if (arg == "--log-dir")
                    {
                        if (!Directory.Exists(args[count + 1]))
                        {
                            Console.WriteLine("Directory does not exist");
                            return -1;
                        }
                        inputDirectories.Add(args[count + 1]);
                    }
                    else if (arg == "--csv")
                    {
                        outputDirectory = args[count + 1];
                    }
                    else if (arg == "--log-level")
                    {
                        logLevel.Add(args[count + 1]);
                    }
                    count++;
                }
            }
            List<string> filePaths = new List<string>();
            foreach (var inputPath in inputDirectories)
            {
                string[] filePathsDir = Directory.GetFiles(@inputPath, "*.log");
                foreach (var filePathDir in filePathsDir)
                {
                    filePaths.Add(filePathDir);
                }
            }
            foreach (var path in filePaths)
            {
                LogParser logparse = new LogParser(path, outputDirectory, logLevel);
                logparse.readLogFiles();
            };
            return 0;
        }
    }
}
