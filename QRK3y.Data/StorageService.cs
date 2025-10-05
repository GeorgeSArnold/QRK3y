using Newtonsoft.Json;
using QRK3y.Security;
using System;
using System.IO;
using System.Security.Cryptography;

namespace QRK3y.Data
{
    public class StorageService
    {
        private static string AppDataFolder
        {
            get
            {
                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "QRK3y",
                    "users"
                );

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        // User speichern (verschluesselt)
        public static void SaveUser<T>(T user, string username, string password)
        {
            try
            {
                // User zu JSON
                string json = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented);

                // JSON verschluesseln
                string encryptedData = EncryptionService.Encrypt(json, password);

                // Dateiname: username.enc
                string fileName = SanitizeFilename(username) + ".enc";
                string filePath = Path.Combine(AppDataFolder, fileName);

                // Speichern
                File.WriteAllText(filePath, encryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Speichern: {ex.Message}");
            }
        }

        // User laden (entschluesselt)
        public static T? LoadUser<T>(string username, string password)
        {
            try
            {
                string fileName = SanitizeFilename(username) + ".enc";
                string filePath = Path.Combine(AppDataFolder, fileName);

                if (!File.Exists(filePath))
                {
                    return default(T);
                }

                // Datei lesen
                string encryptedData = File.ReadAllText(filePath);

                // Entschluesseln
                string json = EncryptionService.Decrypt(encryptedData, password);

                // JSON zu Objekt
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (CryptographicException)
            {
                throw new Exception("Falsches Passwort!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Laden: {ex.Message}");
            }
        }

        // Pruefen ob User existiert
        public static bool UserExists(string username)
        {
            string fileName = SanitizeFilename(username) + ".enc";
            string filePath = Path.Combine(AppDataFolder, fileName);
            return File.Exists(filePath);
        }

        // User loeschen
        public static void DeleteUser(string username)
        {
            string fileName = SanitizeFilename(username) + ".enc";
            string filePath = Path.Combine(AppDataFolder, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        // Dateinamen bereinigen (keine Sonderzeichen)
        private static string SanitizeFilename(string filename)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }
            return filename.ToLower();
        }

        // Speicherpfad anzeigen (fuer Debugging)
        public static string GetStoragePath()
        {
            return AppDataFolder;
        }
    }
}