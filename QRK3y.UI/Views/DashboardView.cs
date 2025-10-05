using System;
using System.Collections.Generic;
using QRK3y.UI.Components;
using QRK3y.UI.Helpers;
using QRK3y.Core.Models;
using QRK3y.Data;

namespace QRK3y.UI.Views
{
    public class DashboardView
    {
        private User _currentUser;
        private string _userPassword;

        public DashboardView(User user, string password)
        {
            _currentUser = user;
            _userPassword = password;
        }

        public void Show()
        {
            bool running = true;

            while (running)
            {
                var selectedItem = ShowDashboardMenu();

                if (selectedItem == null)
                {
                    running = false;
                }
                else
                {
                    HandleMenuSelection(selectedItem);
                }
            }
        }

        private MenuItem ShowDashboardMenu()
        {
            Console.Clear();
            DrawHeader();

            var menuItems = new List<MenuItem>
            {
                new MenuItem("[ Neues Set anlegen ]", "create"),
                new MenuItem("[ Meine Sets anzeigen ]", "list"),
                new MenuItem("[ Profil ]", "profile"),
                new MenuItem("[ Speicherort oeffnen ]", "openstorage")
            };

            var menu = new AnimatedMenu("Dashboard", menuItems);
            return menu.Show();
        }

        private void DrawHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
    +===========================================+
    |           DASHBOARD                       |
    +===========================================+
            ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    Willkommen, {_currentUser.Username}!");
            Console.WriteLine($"    Gespeicherte Sets: {_currentUser.CredentialSets.Count}");
            Console.WriteLine($"    Letzter Login: {_currentUser.LastLogin:dd.MM.yyyy HH:mm}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"    (ESC druecken zum Abmelden)");
            Console.WriteLine();
            Console.ResetColor();
        }

        private void HandleMenuSelection(MenuItem item)
        {
            switch (item.ActionKey)
            {
                case "create":
                    CreateNewSet();
                    break;

                case "list":
                    ListSets();
                    break;

                case "profile":
                    ShowProfile();
                    break;

                case "openstorage":
                    OpenStorageLocation();
                    break;
            }
        }

        private void CreateNewSet()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    +===========================================+
    |         NEUES SET ANLEGEN                 |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            string setName = InputHelper.ReadLine("Set-Name");

            var typeMenu = new List<MenuItem>
            {
                new MenuItem("[ Username + Password ]", "userpass"),
                new MenuItem("[ Nur ein Wort ]", "word")
            };

            var menu = new AnimatedMenu("Waehle Set-Typ", typeMenu);
            var selectedType = menu.Show();

            if (selectedType == null) return;

            CredentialSet newSet = new CredentialSet(setName,
                selectedType.ActionKey == "userpass" ? CredentialType.UsernamePassword : CredentialType.SingleWord);

            Console.Clear();
            Console.WriteLine();

            if (selectedType.ActionKey == "userpass")
            {
                string username = InputHelper.ReadLine("Username");
                string password = InputHelper.ReadPassword("Password");

                // Temporaer unverschluesselt (spaeter mit EncryptionService)
                newSet.EncryptedUsername = username;
                newSet.EncryptedPassword = password;
            }
            else
            {
                string word = InputHelper.ReadLine("Wort");
                newSet.EncryptedWord = word;
            }

            _currentUser.CredentialSets.Add(newSet);

            Console.WriteLine();
            LoadingAnimation.Spinner("Speichere Set", 1000);

            try
            {
                // User mit neuem Set speichern
                StorageService.SaveUser(_currentUser, _currentUser.Username, _userPassword);
                LoadingAnimation.Success($"Set '{setName}' erfolgreich erstellt und gespeichert!");
            }
            catch (Exception ex)
            {
                LoadingAnimation.Error($"Speichern fehlgeschlagen: {ex.Message}");
            }

            Console.WriteLine();
            InputHelper.PressAnyKey();
        }

        private void ListSets()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    +===========================================+
    |         MEINE SETS                        |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            if (_currentUser.CredentialSets.Count == 0)
            {
                LoadingAnimation.Info("Noch keine Sets vorhanden.");
                Console.WriteLine();
                InputHelper.PressAnyKey();
                return;
            }

            // Sets als Menü anzeigen
            var menuItems = new List<MenuItem>();

            foreach (var set in _currentUser.CredentialSets)
            {
                string displayText = $"[ {set.SetName} ] - {(set.Type == CredentialType.UsernamePassword ? "User+Pwd" : "Wort")}";
                menuItems.Add(new MenuItem(displayText, set.SetId));
            }

            var menu = new AnimatedMenu("Waehle ein Set", menuItems);
            var selectedItem = menu.Show();

            if (selectedItem != null)
            {
                // Set finden
                var selectedSet = _currentUser.CredentialSets.Find(s => s.SetId == selectedItem.ActionKey);

                if (selectedSet != null)
                {
                    // Set-Details View oeffnen
                    SetDetailsView detailsView = new SetDetailsView(selectedSet);
                    bool wasDeleted = detailsView.Show();

                    // Speichern nach Bearbeitung oder Loeschung
                    try
                    {
                        if (wasDeleted)
                        {
                            // Set aus Liste entfernen
                            _currentUser.CredentialSets.Remove(selectedSet);
                            LoadingAnimation.Info("Aenderungen werden gespeichert...");
                        }

                        // User speichern (mit aktualisierten Sets)
                        StorageService.SaveUser(_currentUser, _currentUser.Username, _userPassword);

                        if (!wasDeleted)
                        {
                            LoadingAnimation.Success("Aenderungen gespeichert!");
                        }
                    }
                    catch (Exception ex)
                    {
                        LoadingAnimation.Error($"Speichern fehlgeschlagen: {ex.Message}");
                    }

                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void ShowProfile()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
    +===========================================+
    |              PROFIL                       |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine($"    Benutzername:  {_currentUser.Username}");
            Console.WriteLine($"    User ID:       {_currentUser.UserId}");
            Console.WriteLine($"    Erstellt am:   {_currentUser.CreatedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"    Letzter Login: {_currentUser.LastLogin:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"    Gesamt Sets:   {_currentUser.CredentialSets.Count}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"    Speicherort:   {StorageService.GetStoragePath()}");
            Console.ResetColor();

            Console.WriteLine();
            InputHelper.PressAnyKey();
        }

        private void OpenStorageLocation()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    +===========================================+
    |         SPEICHERORT                       |
    +===========================================+
            ");
            Console.ResetColor();
            Console.WriteLine();

            string path = StorageService.GetStoragePath();

            LoadingAnimation.Info($"Speicherort: {path}");
            Console.WriteLine();

            if (InputHelper.Confirm("Moechtest du den Ordner im Explorer oeffnen?"))
            {
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", path);
                    LoadingAnimation.Success("Ordner geoeffnet!");
                }
                catch (Exception ex)
                {
                    LoadingAnimation.Error($"Fehler: {ex.Message}");
                }
            }

            Console.WriteLine();
            InputHelper.PressAnyKey();
        }
    }
}