using System;
using System.Collections.Generic;
using System.IO;

namespace logparser
{
    interface ILogParser
    {
        void readLogFiles();
        void formatLine(String line);
        void writeLineToCSV(String line);
    };
    class LogParser : ILogParser
    {
        public string inputFilePath;
        public string outputFilePath;
        public List<string> logLevel;
        public LogParser(String inputFilePath, String outputFilePath, List<string> logLevel)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.logLevel = logLevel;
        }
        public void readLogFiles()
        {
            String line;
            using (FileStream fileStream = new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
        public void formatLine(String line)
        {
            String[] data = line.Split(":.");
            if (data.Length > 1)
            {
                String[] Information = data[0].Trim().Split(' ');

                foreach (var logLevel in this.logLevel)
                {
                    if (logLevel == Information[2])
                    {
                        String text = data[1].Trim();
                        var date = new DateTime(2020, Int32.Parse(Information[0].Split('/')[0]), Int32.Parse(Information[0].Split('/')[1]));
                        var time = DateTime.ParseExact(Information[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        line = Information[2] + ',' + time.ToLongTimeString() + ',' + String.Format("{0:dd MMMM yyyy}", date) + ',' + '"' + text + '"';
                        writeLineToCSV(line);
                    }
                }
            }
        }
        public void writeLineToCSV(String line)
        {
            int count = 1;
            if (!File.Exists(this.outputFilePath))
            {
                string header = "No,Level,Date,Time,Text\n";
                File.WriteAllText(this.outputFilePath, header);
            }
            else
            {
                string lLine = null;
                using (var s = File.Open(this.outputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
            using (FileStream fileStreamW = new FileStream(this.outputFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
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