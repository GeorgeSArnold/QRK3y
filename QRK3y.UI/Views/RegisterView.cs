using System;
using QRK3y.UI.Components;
using QRK3y.UI.Helpers;
using QRK3y.Core.Models;
using QRK3y.Security;
using QRK3y.Data;

namespace QRK3y.UI.Views
{
    public class RegisterView
    {
        public User Show()
        {
            Console.Clear();
            DrawHeader();

            LoadingAnimation.Info("Erstelle neues Benutzerkonto...");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    (ESC druecken zum Abbrechen)");
            Console.ResetColor();
            Console.WriteLine();

            // Username eingeben
            string username = InputHelper.ReadLine("Benutzername");
            if (username == null)
            {
                LoadingAnimation.Info("Registrierung abgebrochen.");
                InputHelper.PressAnyKey();
                return null;
            }

            // Pruefen ob User schon existiert
            if (StorageService.UserExists(username))
            {
                LoadingAnimation.Error($"Benutzer '{username}' existiert bereits!");
                InputHelper.PressAnyKey();
                return null;
            }

            // Passwort eingeben
            string password = InputHelper.ReadPassword("Passwort");
            if (password == null)
            {
                LoadingAnimation.Info("Registrierung abgebrochen.");
                InputHelper.PressAnyKey();
                return null;
            }

            string passwordConfirm = InputHelper.ReadPassword("Passwort bestaetigen");
            if (passwordConfirm == null)
            {
                LoadingAnimation.Info("Registrierung abgebrochen.");
                InputHelper.PressAnyKey();
                return null;
            }

            // Passwort-Validierung
            if (password != passwordConfirm)
            {
                LoadingAnimation.Error("Passwoerter stimmen nicht ueberein!");
                InputHelper.PressAnyKey();
                return Show();
            }

            Console.WriteLine();
            LoadingAnimation.Spinner("Erstelle Benutzer", 1000);

            // User erstellen (mit gehashtem Passwort)
            string passwordHash = EncryptionService.HashPassword(password);
            User newUser = new User(username, passwordHash);

            // User speichern (verschluesselt)
            try
            {
                LoadingAnimation.Spinner("Speichere Benutzerdaten", 1000);
                StorageService.SaveUser(newUser, username, password);

                LoadingAnimation.Success($"Benutzer '{username}' erfolgreich erstellt!");
                LoadingAnimation.Info($"Gespeichert in: {StorageService.GetStoragePath()}");
            }
            catch (Exception ex)
            {
                LoadingAnimation.Error($"Speichern fehlgeschlagen: {ex.Message}");
                InputHelper.PressAnyKey();
                return null;
            }

            Console.WriteLine();
            InputHelper.PressAnyKey();

            return newUser;
        }

        private void DrawHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
    +===========================================+
    |         REGISTRIERUNG                     |
    +===========================================+
            ");
            Console.ResetColor();
        }
    }
}