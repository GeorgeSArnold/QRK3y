using System;
using System.Collections.Generic;
using System.Threading;

namespace QRK3y.UI.Components
{
    public class AnimatedMenu
    {
        private List<MenuItem> _items;
        private int _selectedIndex;
        private string _title;
        private ConsoleColor _selectedColor = ConsoleColor.Green;
        private ConsoleColor _normalColor = ConsoleColor.Gray;
        private ConsoleColor _titleColor = ConsoleColor.Cyan;

        public AnimatedMenu(string title, List<MenuItem> items)
        {
            _title = title;
            _items = items;
            _selectedIndex = 0;
        }

        public MenuItem Show()
        {
            Console.CursorVisible = false;
            Console.Clear();

            // Header nur einmal zeichnen
            DrawHeader();
            Console.WriteLine();

            ConsoleKey keyPressed;

            do
            {
                DrawMenuItems();
                DrawFooter();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                switch (keyPressed)
                {
                    case ConsoleKey.UpArrow:
                        MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        MoveDown();
                        break;
                    case ConsoleKey.Enter:
                        FlashSelection();
                        Console.CursorVisible = true;
                        return _items[_selectedIndex];
                    case ConsoleKey.Escape:
                        Console.CursorVisible = true;
                        return null;
                }
            } while (keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.Escape);

            Console.CursorVisible = true;
            return null;
        }

        private void DrawHeader()
        {
            Console.ForegroundColor = _titleColor;

            string border = new string('=', _title.Length + 4);

            Console.WriteLine($"    +{border}+");
            Console.WriteLine($"    |  {_title}  |");
            Console.WriteLine($"    +{border}+");

            Console.ResetColor();
        }

        private void DrawMenuItems()
        {
            // Cursor an den Anfang der Menü-Items setzen (Zeile 5)
            Console.SetCursorPosition(0, 5);

            for (int i = 0; i < _items.Count; i++)
            {
                Console.Write("    ");

                if (i == _selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = _selectedColor;
                    Console.Write(" > ");
                    Console.Write($" {_items[i].DisplayText} ");
                    Console.Write(" < ");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = _normalColor;
                    Console.Write("   ");
                    Console.Write($"   {_items[i].DisplayText}   ");
                    Console.ResetColor();
                }

                // Zeile mit Leerzeichen auffüllen um alte Zeichen zu überschreiben
                Console.Write(new string(' ', 50));
                Console.WriteLine();
            }
        }

        private void DrawFooter()
        {
            int footerLine = 5 + _items.Count + 1;
            Console.SetCursorPosition(0, footerLine);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    " + new string('-', 60));
            Console.WriteLine("    Pfeiltasten Navigation  |  Enter Auswaehlen  |  ESC Zurueck");
            Console.ResetColor();
        }

        private void MoveUp()
        {
            _selectedIndex = (_selectedIndex == 0) ? _items.Count - 1 : _selectedIndex - 1;
        }

        private void MoveDown()
        {
            _selectedIndex = (_selectedIndex == _items.Count - 1) ? 0 : _selectedIndex + 1;
        }

        private void FlashSelection()
        {
            for (int i = 0; i < 3; i++)
            {
                int itemLine = 5 + _selectedIndex;
                Console.SetCursorPosition(0, itemLine);

                Console.Write("    ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write(" > ");
                Console.Write($" {_items[_selectedIndex].DisplayText} ");
                Console.Write(" < ");
                Console.ResetColor();
                Console.Write(new string(' ', 50));

                Thread.Sleep(100);

                Console.SetCursorPosition(0, itemLine);
                Console.Write("    ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = _selectedColor;
                Console.Write(" > ");
                Console.Write($" {_items[_selectedIndex].DisplayText} ");
                Console.Write(" < ");
                Console.ResetColor();
                Console.Write(new string(' ', 50));

                Thread.Sleep(100);
            }
        }
    }
}