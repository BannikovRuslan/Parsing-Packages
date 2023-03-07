
namespace ParsingPackages.Logging
{
    public class Logger
    {
        public string infoFile { get; set; } = "info.log";
        public string warningFile { get; set; } = "warnings.log";
        public string errorFile { get; set; } = "errors.log";

        public void infoLogger(string message, bool toConsole, bool toFile)
        {
            if (toConsole)
            {
                Console.Write($"[{DateTime.Now}] Information: ");
                Console.WriteLine(message); 
            }

            if (toFile) 
            {
                writeToFile(message, infoFile);
            } 
        }

        public void warningLogger(string message, bool toConsole, bool toFile)
        {
            if (toConsole)
            {
                Console.WriteLine($"[{DateTime.Now}] Warning: ");
                Console.WriteLine(message + "\n");
            }

            if (toFile)
            {
                writeToFile(message, warningFile);
            }
        }

        public void errorLogger(string message, bool toConsole, bool toFile)
        {
            if (toConsole)
            {
                Console.WriteLine($"\n[{DateTime.Now}] Error: ");
                Console.WriteLine(message + "\n\n");
            }

            if (toFile)
            {
                writeToFile(message, warningFile);
            }
        }

        public void writeToFile(string message, string fileName)
        {
            using (StreamWriter file = File.AppendText(fileName))
            {
                file.WriteLine($"[{DateTime.Now}]:");
                file.WriteLine(message + "\n");
            }
        }
    }
}
