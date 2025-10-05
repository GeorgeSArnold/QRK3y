using System;
using System.Text;

namespace QRK3y.UI.Helpers
{
    public static class InputHelper
    {
        public static string ReadLine(string prompt, bool allowEmpty = false)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"    {prompt}: ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;

            // ESC-Handling
            string input = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return null; // ESC gedrueckt
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            } while (true);

            Console.WriteLine();
            Console.ResetColor();

            if (!allowEmpty && string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    [ FEHLER ]  Eingabe darf nicht leer sein!");
                Console.ResetColor();
                return ReadLine(prompt, allowEmpty);
            }

            return input?.Trim();
        }

        public static string ReadPassword(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"    {prompt}: ");
            Console.ResetColor();

            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return null; // ESC gedrueckt
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();

            if (password.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    [ FEHLER ]  Passwort darf nicht leer sein!");
                Console.ResetColor();
                return ReadPassword(prompt);
            }

            return password.ToString();
        }

        public static bool Confirm(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"    {message} (j/n): ");
            Console.ResetColor();

            while (true)
            {
                var key = Console.ReadKey(true);
                Console.WriteLine(key.KeyChar);

                if (key.Key == ConsoleKey.J || key.KeyChar == 'j')
                    return true;
                if (key.Key == ConsoleKey.N || key.KeyChar == 'n')
                    return false;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    Bitte 'j' fuer Ja oder 'n' fuer Nein eingeben.");
                Console.ResetColor();
                Console.Write($"    {message} (j/n): ");
            }
        }

        public static void PressAnyKey(string message = "Druecke eine beliebige Taste um fortzufahren...")
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n    {message}");
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}