using QRK3y.UI.Components;
using QRK3y.UI.Views;
using QRK3y.UI.Helpers;
using QRK3y.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRK3y.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            ShowWelcome();

            bool running = true;
            while (running)
            {
                var selectedItem = ShowMainMenu();

                if (selectedItem == null)
                {
                    running = false;
                }
                else
                {
                    HandleMenuSelection(selectedItem);
                }
            }

            ShowGoodbye();
        }

        static void ShowWelcome()
        {
            BannerHelper.ShowBanner();
            LoadingAnimation.Spinner("Initialisiere System", 1500);
            LoadingAnimation.Success("System bereit!");
            System.Threading.Thread.Sleep(1000);
        }

        static void ShowGoodbye()
        {
            BannerHelper.ShowBanner("Vielen Dank fuer die Nutzung von QRK3y!");
            LoadingAnimation.Spinner("Beende Anwendung", 1000);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    Druecke eine beliebige Taste zum Beenden...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        static MenuItem ShowMainMenu()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem("[ Anmelden ]", "login"),
                new MenuItem("[ Registrieren ]", "register"),
                new MenuItem("[ Info ]", "info"),
                new MenuItem("[ Beenden ]", "exit")
            };

            var menu = new AnimatedMenu("QRK3y Hauptmenue", menuItems);
            return menu.Show();
        }

        static void HandleMenuSelection(MenuItem item)
        {
            switch (item.ActionKey)
            {
                case "login":
                    LoginView loginView = new LoginView();
                    User loggedInUser = loginView.Show();

                    if (loggedInUser != null)
                    {
                        string password = GetPasswordForSession(loggedInUser.Username);
                        if (!string.IsNullOrEmpty(password))
                        {
                            DashboardView dashboard = new DashboardView(loggedInUser, password);
                            dashboard.Show();
                        }
                    }
                    break;

                case "register":
                    RegisterView registerView = new RegisterView();
                    User newUser = registerView.Show();

                    if (newUser != null)
                    {
                        LoadingAnimation.Info("Du kannst dich jetzt anmelden!");
                        System.Threading.Thread.Sleep(2000);
                    }
                    break;

                case "info":
                    ShowInfo();
                    break;

                case "exit":
                    BannerHelper.ShowBanner();
                    Environment.Exit(0);
                    break;
            }
        }

        static string GetPasswordForSession(string username)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    [ Sicherheitsabfrage ]");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("    Passwort bestaetigen: ");
            Console.ResetColor();

            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
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
            return password.ToString();
        }

        static void ShowInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    +===========================================+
    |              QRK3y  v1.0                  |
    +===========================================+
            ");
            Console.ResetColor();

            LoadingAnimation.Info("Verschluesselter Password Manager");
            LoadingAnimation.Info("QR-Code Generator fuer Credentials");
            LoadingAnimation.Info("Entwickelt mit C#");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    Funktionen:");
            Console.WriteLine("    - Sichere Passwortspeicherung");
            Console.WriteLine("    - QR-Code Generierung");
            Console.WriteLine("    - Verschluesselte Datenspeicherung");
            Console.WriteLine("    - Set-basierte Credential-Verwaltung");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    Druecke eine beliebige Taste um fortzufahren...");
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}