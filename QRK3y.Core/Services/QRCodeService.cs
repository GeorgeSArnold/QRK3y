using System;
using System.IO;
using QRCoder;

namespace QRK3y.Core.Services
{
    public class QRCodeService
    {
        // QR-Code als PNG-Bytes generieren
        public static byte[] GenerateQRCodePng(string text, int pixelsPerModule = 20)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(pixelsPerModule);
                }
            }
        }

        // QR-Code direkt als PNG-Datei speichern
        public static void SaveQRCodeToFile(string text, string filePath, int pixelsPerModule = 20)
        {
            byte[] qrCodeBytes = GenerateQRCodePng(text, pixelsPerModule);
            File.WriteAllBytes(filePath, qrCodeBytes);
        }

        // QR-Code als ASCII in Console anzeigen
        public static void DisplayQRCodeInConsole(string text)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
                using (AsciiQRCode qrCode = new AsciiQRCode(qrCodeData))
                {
                    string qrCodeAsAscii = qrCode.GetGraphic(1);
                    Console.WriteLine(qrCodeAsAscii);
                }
            }
        }

        // Komplettes Set als QR-Codes auf Desktop speichern
        public static string SaveCredentialSetQRCodes(string setName, string? username, string? password, string? word)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderName = $"QR_{SanitizeFolderName(setName)}";
            string folderPath = Path.Combine(desktopPath, folderName);

            // Ordner erstellen
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            int filesCreated = 0;

            // Username QR-Code
            if (!string.IsNullOrEmpty(username))
            {
                string userPath = Path.Combine(folderPath, $"{setName}_username.png");
                SaveQRCodeToFile(username, userPath);
                filesCreated++;
            }

            // Password QR-Code
            if (!string.IsNullOrEmpty(password))
            {
                string pwdPath = Path.Combine(folderPath, $"{setName}_password.png");
                SaveQRCodeToFile(password, pwdPath);
                filesCreated++;
            }

            // Word QR-Code
            if (!string.IsNullOrEmpty(word))
            {
                string wordPath = Path.Combine(folderPath, $"{setName}_word.png");
                SaveQRCodeToFile(word, wordPath);
                filesCreated++;
            }

            return filesCreated > 0 ? folderPath : string.Empty;
        }

        // Ordnernamen bereinigen
        private static string SanitizeFolderName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }
    }
}