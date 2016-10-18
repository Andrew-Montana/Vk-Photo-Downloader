using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vk_Photo_Downloader
{
    class ChangeColor
    {
        public static void redForeground()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void greenForeground()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public static void whiteForeground()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

}
