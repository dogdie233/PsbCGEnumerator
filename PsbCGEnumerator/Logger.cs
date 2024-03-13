namespace PsbCGEnumerator
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public static class Logger
    {
        public static void Log(LogLevel level, string content)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}][{level}] {content}");
        }
    }

    public static class LoggerExtension
    {
        public static void D(this string content) => Logger.Log(LogLevel.Debug, content);
        public static void I(this string content) => Logger.Log(LogLevel.Info, content);
        public static void W(this string content) => Logger.Log(LogLevel.Warning, content);
        public static void E(this string content, bool exit = true)
        {
            Logger.Log(LogLevel.Error, content);
            if (exit)
                Environment.Exit(1);
        }
        public static void Log(this Exception e, string? desc = null, bool exit = true)
            => ((desc != null ? desc + Environment.NewLine : "") + e.ToString()).E(exit);
    }
}
