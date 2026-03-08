using CommunityToolkit.Mvvm.DependencyInjection;
using System.IO;
using System.Text;

namespace PomoTimer.Services
{

    public class LoggerService
    {
        public string logDirectoryName { get; } = "logs";
        public string logsPath { get; }
        private readonly TimerService _timerService;
        public LoggerService()
        {
            logsPath = @$"{Environment.CurrentDirectory}\{logDirectoryName}\";
            SafeCreateLogDirectory(logsPath);
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            string nowDateStr = _timerService.Now().ToString("yyyyMMdd");
        }
        public void Info(string message)
        {
            WriteLine("INFO", message);
        }
        public void Warn(string message)
        {
            WriteLine("WARN", message);
        }
        public void Error(string message)
        {
            WriteLine("ERROR", message);
        }
        public void fatal(string message)
        {
            WriteLine("FATAL", message);
        }
        public void Debug(string message)
        {
            WriteLine("DEBUG", message);
        }

        private void WriteLine(string levelName, string message)
        {
            DateTime now = DateTime.Now;
            string line = $"{now.ToString("yyyy/MM/dd HH:mm:ss.fff")} [{levelName}] {message}";
            using (StreamWriter _logWriter = new StreamWriter(@$"{logsPath}\log-{now.ToString("yyyyMMdd")}.txt", true, Encoding.UTF8))
            {
                _logWriter.WriteLine(line);
                _logWriter.Flush();
            }
            System.Diagnostics.Debug.WriteLine(line);
        }
        private bool SafeCreateLogDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return false;
            }
            Directory.CreateDirectory(path);
            return true;
        }
        public void DeletePastLogs()
        {
            // ログファイルを作成日の新しい順に並び替える
            var sortedFileNames = Directory.GetFiles(Environment.CurrentDirectory + @$"\logs\")
                        .Select(file => new { FilePath = file, CreationTime = new FileInfo(file).CreationTime })
                        .OrderByDescending(item => item.CreationTime)
                        .Select(item => item.FilePath);

            // 先頭から8(7+1)個目のログファイルから順に全て削除する
            int i = 1;
            foreach (string filename in sortedFileNames)
            {
                if (i > 7)
                {
                    File.Delete(filename);
                }
                i++;
            }
            Info("past log deleted.");
        }
    }
}
