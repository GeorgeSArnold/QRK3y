using System;
using System.Threading;

namespace QRK3y.UI.Components
{
    public static class LoadingAnimation
    {
        public static void Spinner(string message, int durationMs = 2000)
        {
            char[] frames = { '|', '/', '-', '\\' };
            int counter = 0;
            int elapsed = 0;
            int interval = 100;

            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Cyan;

            while (elapsed < durationMs)
            {
                Console.Write($"\r    [ {frames[counter % frames.Length]} ]  {message}     ");
                counter++;
                Thread.Sleep(interval);
                elapsed += interval;
            }

            Console.Write("\r" + new string(' ', message.Length + 30) + "\r");
            Console.ResetColor();
            Console.CursorVisible = true;
        }

        public static void Dots(string message, int durationMs = 2000)
        {
            int dots = 0;
            int elapsed = 0;
            int interval = 400;

            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;

            while (elapsed < durationMs)
            {
                string dotString = new string('.', (dots % 4));
                Console.Write($"\r    {message}{dotString}       ");
                dots++;
                Thread.Sleep(interval);
                elapsed += interval;
            }

            Console.Write("\r" + new string(' ', message.Length + 30) + "\r");
            Console.ResetColor();
            Console.CursorVisible = true;
        }

        public static void ProgressBar(string message, int durationMs = 2000)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;

            int barWidth = 40;
            int steps = 20;
            int stepDuration = durationMs / steps;

            for (int i = 0; i <= steps; i++)
            {
                int filled = (int)((double)i / steps * barWidth);
                int empty = barWidth - filled;

                string bar = "[" + new string('#', filled) + new string('.', empty) + "]";
                int percentage = (int)((double)i / steps * 100);

                Console.Write($"\r    {message}  {bar}  {percentage}%     ");
                Thread.Sleep(stepDuration);
            }

            Console.WriteLine();
            Console.ResetColor();
            Console.CursorVisible = true;
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"    [ OK ]  {message}");
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"    [ FEHLER ]  {message}");
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    [ INFO ]  {message}");
            Console.ResetColor();
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"    [ WARNUNG ]  {message}");
            Console.ResetColor();
        }
    }
}