using System;
using System.IO;

namespace logparser
{
    class logParser
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: logParser --log-dir <dir> --log-level <level> --csv <out>");
                Console.WriteLine("--log-dir    Directory to parse recursively for .log files");
                Console.WriteLine("--csv        Output file-path (absolute/relative)");
                Console.WriteLine("--log-level  <info/war/debug>");
                return 1;
            }
            string[] filePaths = Directory.GetFiles(@"C:/Users/abhijeetm/logparser/", "*.log");
            foreach (var path in filePaths)
            {
                readLogFiles(path);
            };
            return 0;
        }

        static void readLogFiles(String path)
        {
            String line;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        line = streamReader.ReadLine();
                        formatLine(line);
                    }
                }
            }
        }

        public static void formatLine(String line)
        {
            String[] data = line.Split(":.");
            if (data.Length > 1)
            {
                String[] Information = data[0].Trim().Split(' ');
                String text = data[1].Trim();
                var date = new DateTime(2020, Int32.Parse(Information[0].Split('/')[0]), Int32.Parse(Information[0].Split('/')[1]));
                var time = DateTime.ParseExact(Information[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                line = Information[2] + ',' + time.ToLongTimeString() + ',' + String.Format("{0:dd MMMM yyyy}", date) + ',' + '"' + text + '"';
                // Console.WriteLine(line);
                writeLineToCSV(line);
            }
        }
        public static void writeLineToCSV(String line)
        {
            int count = 1;
            if (!File.Exists("C:/Users/abhijeetm/logparser/log.csv"))
            {
                string header = "No,Level,Date,Time,Text\n";
                File.WriteAllText("C:/Users/abhijeetm/logparser/log.csv", header);
            }
            else
            {
                string lLine = null;
                using (var s = File.Open("C:/Users/abhijeetm/logparser/log.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var sr = new StreamReader(s))
                    {
                        string nextLine;
                        while ((nextLine = sr.ReadLine()) != null) lLine = nextLine;
                        count = Int32.Parse(lLine.Split(',')[0]);
                        count++;
                    }
                }
            }
            using (FileStream fileStreamW = new FileStream("C:/Users/abhijeetm/logparser/log.csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStreamW))
                {
                    string writeLine = count.ToString() + ',' + line;
                    streamWriter.WriteLine(writeLine);
                }
                fileStreamW.Close();
            }
        }
    }
}