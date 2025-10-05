using System;
using QRK3y.UI.Components;
using QRK3y.UI.Helpers;
using QRK3y.Core.Models;
using QRK3y.Security;
using QRK3y.Data;

namespace QRK3y.UI.Views
{
    public class LoginView
    {
        public User Show()
        {
            Console.Clear();
            DrawHeader();

            LoadingAnimation.Info("Melde dich an...");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    (ESC druecken zum Abbrechen)");
            Console.ResetColor();
            Console.WriteLine();

            // Username eingeben
            string username = InputHelper.ReadLine("Benutzername");
            if (username == null)
            {
                LoadingAnimation.Info("Login abgebrochen.");
                InputHelper.PressAnyKey();
                return null;
            }

            // Pruefen ob User existiert
            if (!StorageService.UserExists(username))
            {
                LoadingAnimation.Error($"Benutzer '{username}' nicht gefunden!");
                LoadingAnimation.Info("Bitte registriere dich zuerst.");
                Console.WriteLine();
                InputHelper.PressAnyKey();
                return null;
            }

            // Passwort eingeben
            string password = InputHelper.ReadPassword("Passwort");
            if (password == null)
            {
                LoadingAnimation.Info("Login abgebrochen.");
                InputHelper.PressAnyKey();
                return null;
            }

            Console.WriteLine();
            LoadingAnimation.Spinner("Lade Benutzerdaten", 1000);

            try
            {
                // User laden (entschluesseln mit Passwort)
                User user = StorageService.LoadUser<User>(username, password);

                if (user == null)
                {
                    LoadingAnimation.Error("Login fehlgeschlagen!");
                    Console.WriteLine();
                    InputHelper.PressAnyKey();
                    return null;
                }

                // Passwort verifizieren
                if (EncryptionService.VerifyPassword(password, user.PasswordHash))
                {
                    LoadingAnimation.Success("Anmeldung erfolgreich!");
                    user.LastLogin = DateTime.Now;

                    // Updated User speichern
                    StorageService.SaveUser(user, username, password);

                    Console.WriteLine();
                    InputHelper.PressAnyKey();
                    return user;
                }
                else
                {
                    LoadingAnimation.Error("Passwort falsch!");
                    Console.WriteLine();
                    InputHelper.PressAnyKey();
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoadingAnimation.Error($"Fehler: {ex.Message}");
                Console.WriteLine();
                InputHelper.PressAnyKey();
                return null;
            }
        }

        private void DrawHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    +===========================================+
    |              LOGIN                        |
    +===========================================+
            ");
            Console.ResetColor();
        }
    }
}