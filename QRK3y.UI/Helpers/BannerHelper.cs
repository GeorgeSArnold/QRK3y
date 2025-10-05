using System;

namespace QRK3y.UI.Helpers
{
    public static class BannerHelper
    {
        private static readonly string[] Banners = new[]
        {
            // Banner 1 - Original
            @"
                                                                   
  ,ad8888ba,    88888888ba   88         ad888888b,              
 d8'    '8b   88      '8b  88        d8'     '88              
d8'        `8b  88      ,8P  88                a8P              
88          88  88aaaaaa8P'  88   ,d8       aad8'  8b       d8  
88          88  88''''''88'    88 ,a8'        ''Y8,  `8b     d8'  
Y8,    '88,,8P  88    `8b    8888[             '8b  `8b   d8'   
 Y8a.    Y88P   88     `8b   88`'Yba,  Y8,     a88   `8b,d8'    
  `'Y8888Y'Y8a  88      `8b  88   `Y8a  'Y888888P'     Y88'     
                                                       d8'      
                                                      d8' 
            ",

            // Banner 2 - Minimalist
            @"
                                            
 @@@@@@   @@@@@@@  @@@  @@@ @@@@@@  @@@ @@@ 
@@!  @@@  @@!  @@@ @@!  !@@     @@! @@! !@@ 
@!@  !@!  @!@!!@!  @!@@!@!   @!!!:   !@!@!  
!!:!!:!:  !!: :!!  !!: :!!      !!:   !!:   
 : :. :::  :   : :  :   ::: ::: ::    .:    
                                            
            ",

            // Banner 3 - Block Style
            @"
  ooooooo  oooooooooo  oooo          ooooooo               
o888   888o 888    888  888  ooooo o88    888o oooo   oooo 
888     888 888oooo88   888o888        88888o   888   888  
888o  8o888 888  88o    8888 88o   88o    o888   888 888   
  88ooo88  o888o  88o8 o888o o888o   88ooo88       8888    
       88o8                                     o8o888     
            ",

            // Banner 4 - Slant
            @"
  e88 88e   888 88e  888    ,8,""88b,           
 d888 888b  888 888D 888 ee  "" ,88P' Y8b Y888P 
C8888 8888D 888 88""  888 P     C8K    Y8b Y8P  
 Y888 888P  888 b,   888 b   e `88b,   Y8b Y   
  ""88 88""   888 88b, 888 8b ""8"",88P'    888    
      b                                 888    
      8b,                               888    
            ",

            // Banner 5 - Digital
            @"
  .oooooo.      ooooooooo.   oooo          .oooo.               
 d8P'  `Y8b     `888   `Y88. `888        .dP""Y88b              
888      888     888   .d88'  888  oooo        ]8P' oooo    ooo 
888      888     888ooo88P'   888 .8P'       <88b.   `88.  .8'  
888      888     888`88b.     888888.         `88b.   `88..8'   
`88b    d88b     888  `88b.   888 `88b.  o.   .88P     `888'    
 `Y8bood8P'Ybd' o888o  o888o o888o o888o `8bd88P'       .8'     
                                                    .o..P'      
                                                    `Y8P'       
                                                                
            "
        };

        private static readonly Random Random = new Random();

        public static void ShowBanner(string message = null)
        {
            Console.Clear();

            // Zufälliges Banner auswählen
            string selectedBanner = Banners[Random.Next(Banners.Length)];

            // Banner anzeigen
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(selectedBanner);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("                                        by_gSa+ 10-2025");
            Console.ResetColor();

            // Optionale Nachricht
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"    {message}");
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}