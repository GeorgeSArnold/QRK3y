using System;
using System.Collections.Generic;
using QRK3y.UI.Components;
using QRK3y.UI.Helpers;
using QRK3y.Core.Models;
using QRK3y.Core.Services;

namespace QRK3y.UI.Views
{
    public class SetDetailsView
    {
        private CredentialSet _set;

        public SetDetailsView(CredentialSet set)
        {
            _set = set;
        }

        public bool Show()
        {
            bool running = true;
            bool setDeleted = false;

            while (running)
            {
                var selectedItem = ShowSetMenu();

                if (selectedItem == null)
                {
                    running = false;
                }
                else
                {
                    if (selectedItem.ActionKey == "delete")
                    {
                        if (DeleteSet())
                        {
                            setDeleted = true;
                            running = false;
                        }
                    }
                    else
                    {
                        HandleMenuSelection(selectedItem);
                    }
                }
            }

            return setDeleted;
        }

        private MenuItem? ShowSetMenu()
        {
            Console.Clear();
            DrawHeader();

            var menuItems = new List<MenuItem>();

            menuItems.Add(new MenuItem("[ Daten anzeigen ]", "show"));
            menuItems.Add(new MenuItem("[ Set bearbeiten ]", "edit"));

            if (_set.Type == CredentialType.UsernamePassword)
            {
                if (!string.IsNullOrEmpty(_set.EncryptedUsername))
                {
                    menuItems.Add(new MenuItem("[ QR-Code Username anzeigen ]", "qr_username"));
                }
                if (!string.IsNullOrEmpty(_set.EncryptedPassword))
                {
                    menuItems.Add(new MenuItem("[ QR-Code Password anzeigen ]", "qr_password"));
                }
                menuItems.Add(new MenuItem("[ Alle QR-Codes speichern ]", "save"));
            }
            else
            {
                if (!string.IsNullOrEmpty(_set.EncryptedWord))
                {
                    menuItems.Add(new MenuItem("[ QR-Code Wort anzeigen ]", "qr_word"));
                }
                menuItems.Add(new MenuItem("[ QR-Code speichern ]", "save"));
            }

            menuItems.Add(new MenuItem("[ Set loeschen ]", "delete"));

            var menu = new AnimatedMenu($"Set: {_set.SetName}", menuItems);
            return menu.Show();
        }

        private void DrawHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    +===========================================+
    |         SET DETAILS                       |
    +===========================================+
            ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"    Set: {_set.SetName}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"    Typ: {(_set.Type == CredentialType.UsernamePassword ? "Username + Password" : "Einzelnes Wort")}");
            Console.WriteLine($"    Erstellt: {_set.CreatedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine();
            Console.ResetColor();
        }

        private void HandleMenuSelection(MenuItem item)
        {
            switch (item.ActionKey)
            {
                case "show":
                    ShowCredentials();
                    break;

                case "edit":
                    EditSet();
                    break;

                case "qr_username":
                    DisplayUsernameQRCode();
                    break;

                case "qr_password":
                    DisplayPasswordQRCode();
                    break;

                case "qr_word":
                    DisplayWordQRCode();
                    break;

                case "save":
                    SaveQRCodes();
                    break;
            }
        }

        private void ShowCredentials()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    +===========================================+
    |         CREDENTIALS                       |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"    Set: {_set.SetName}");
            Console.WriteLine();
            Console.ResetColor();

            if (_set.Type == CredentialType.UsernamePassword)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"    Username: {_set.EncryptedUsername ?? "[leer]"}");
                Console.WriteLine($"    Password: {_set.EncryptedPassword ?? "[leer]"}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"    Wort: {_set.EncryptedWord ?? "[leer]"}");
                Console.ResetColor();
            }

            Console.WriteLine();
            InputHelper.PressAnyKey();
        }

        private void EditSet()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    +===========================================+
    |         SET BEARBEITEN                    |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"    Aktuelles Set: {_set.SetName}");
            Console.WriteLine();
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    Aktueller Name: {_set.SetName}");
            Console.ResetColor();

            string newName = InputHelper.ReadLine("Neuer Set-Name (Enter = behalten)", allowEmpty: true);
            if (!string.IsNullOrWhiteSpace(newName))
            {
                _set.SetName = newName;
                LoadingAnimation.Success("Set-Name aktualisiert!");
            }

            Console.WriteLine();

            if (_set.Type == CredentialType.UsernamePassword)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"    Aktueller Username: {_set.EncryptedUsername}");
                Console.ResetColor();

                string newUsername = InputHelper.ReadLine("Neuer Username (Enter = behalten)", allowEmpty: true);
                if (!string.IsNullOrWhiteSpace(newUsername))
                {
                    _set.EncryptedUsername = newUsername;
                    LoadingAnimation.Success("Username aktualisiert!");
                }

                Console.WriteLine();

                if (InputHelper.Confirm("Moechtest du das Passwort aendern?"))
                {
                    string newPassword = InputHelper.ReadPassword("Neues Passwort");
                    _set.EncryptedPassword = newPassword;
                    LoadingAnimation.Success("Passwort aktualisiert!");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"    Aktuelles Wort: {_set.EncryptedWord}");
                Console.ResetColor();

                string newWord = InputHelper.ReadLine("Neues Wort (Enter = behalten)", allowEmpty: true);
                if (!string.IsNullOrWhiteSpace(newWord))
                {
                    _set.EncryptedWord = newWord;
                    LoadingAnimation.Success("Wort aktualisiert!");
                }
            }

            _set.ModifiedAt = DateTime.Now;

            Console.WriteLine();
            LoadingAnimation.Spinner("Speichere Aenderungen", 1000);
            LoadingAnimation.Success("Set erfolgreich bearbeitet!");

            Console.WriteLine();
            InputHelper.PressAnyKey();
        }

        private bool DeleteSet()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
    +===========================================+
    |         SET LOESCHEN                      |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"    Set: {_set.SetName}");
            Console.WriteLine();
            Console.ResetColor();

            LoadingAnimation.Warning("Achtung! Dieser Vorgang kann nicht rueckgaengig gemacht werden!");
            Console.WriteLine();

            if (InputHelper.Confirm("Moechtest du dieses Set wirklich loeschen?"))
            {
                LoadingAnimation.Spinner("Loesche Set", 1000);
                LoadingAnimation.Success($"Set '{_set.SetName}' wurde geloescht!");
                Console.WriteLine();
                InputHelper.PressAnyKey();
                return true;
            }
            else
            {
                LoadingAnimation.Info("Loeschvorgang abgebrochen.");
                Console.WriteLine();
                InputHelper.PressAnyKey();
                return false;
            }
        }

        private void DisplayUsernameQRCode()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    +===========================================+
    |         USERNAME QR-CODE                  |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            LoadingAnimation.Info($"Username: {_set.EncryptedUsername}");
            Console.WriteLine();

            QRCodeService.DisplayQRCodeInConsole(_set.EncryptedUsername ?? "");
            Console.WriteLine();
            InputHelper.PressAnyKey();
        }

        private void DisplayPasswordQRCode()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    +===========================================+
    |         PASSWORD QR-CODE                  |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            LoadingAnimation.Info("Password: **********");
            Console.WriteLine();

            QRCodeService.DisplayQRCodeInConsole(_set.EncryptedPassword ?? "");
            Console.WriteLine();
            InputHelper.PressAnyKey();
        }

        private void DisplayWordQRCode()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
    +===========================================+
    |         WORT QR-CODE                      |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            LoadingAnimation.Info($"Wort: {_set.EncryptedWord}");
            Console.WriteLine();

            QRCodeService.DisplayQRCodeInConsole(_set.EncryptedWord ?? "");
            Console.WriteLine();
            InputHelper.PressAnyKey();
        }

        private void SaveQRCodes()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
    +===========================================+
    |         QR-CODES SPEICHERN                |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            LoadingAnimation.Info($"Erstelle QR-Codes fuer Set '{_set.SetName}'...");
            Console.WriteLine();

            try
            {
                LoadingAnimation.Spinner("Generiere QR-Codes", 1000);

                string folderPath = QRCodeService.SaveCredentialSetQRCodes(
                    _set.SetName,
                    _set.EncryptedUsername,
                    _set.EncryptedPassword,
                    _set.EncryptedWord
                );

                if (!string.IsNullOrEmpty(folderPath))
                {
                    LoadingAnimation.Success("QR-Codes erfolgreich gespeichert!");
                    Console.WriteLine();
                    LoadingAnimation.Info($"Speicherort: {folderPath}");
                    Console.WriteLine();

                    if (InputHelper.Confirm("Moechtest du den Ordner oeffnen?"))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", folderPath);
                    }
                }
                else
                {
                    LoadingAnimation.Error("Keine Daten zum Speichern vorhanden!");
                }
            }
            catch (Exception ex)
            {
                LoadingAnimation.Error($"Fehler: {ex.Message}");
            }

            Console.WriteLine();
            InputHelper.PressAnyKey();
        }
    }
}