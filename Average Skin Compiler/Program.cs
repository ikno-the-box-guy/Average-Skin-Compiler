using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Average_Skin_Compiler
{
    internal class Program
    {
        static string skins;
        static void Main()
        {
            Console.WriteLine("Please enter the skin folder directory!");
            skins = Console.ReadLine();

            //Check if skins folder exists
            if (!Directory.Exists(skins))
            {
                Console.WriteLine("Directory does not exist. The app will close as soon as you press a key.");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Every other skin, every other skin offset by 1 or don't alternate? \n1 is: '0', 2 is '1', 3 is '-1'.");
            int settings = int.Parse(Console.ReadLine());

            Console.WriteLine("Directory found! I expect every image to be 64x64 to end in '.png' and there to be a number in every file name counting upwards with each image. \nPress any key to proceed.");
            Console.ReadKey(true);

            Console.WriteLine("Started compiling.");

            int amount = Directory.GetFiles(skins).Length;

            Bitmap result = new Bitmap(64, 64);
            float progress = 0;
            for(int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    int[] rgba = new int[4];
                    foreach (string file in Directory.GetFiles(skins))
                    {
                        if(settings != -1)
                            if(settings == 1)
                                if (int.Parse(Regex.Match(file, @"\d+").Value) % 2 == 0) continue;
                            else
                                if (int.Parse(Regex.Match(file, @"\d+").Value) % 2 != 0) continue;

                        Bitmap bmp = new Bitmap(file);

                        Color col = bmp.GetPixel(x, y);

                        if (col.A <= 1) continue;

                        rgba[0] += col.R;
                        rgba[1] += col.G;
                        rgba[2] += col.B;
                        rgba[3] += col.A;
                    }

                    rgba[0] /= amount;
                    rgba[1] /= amount;
                    rgba[2] /= amount;
                    rgba[3] /= amount;

                    Color c;
                    if(rgba[3] < 100)
                        c = Color.FromArgb(0, rgba[0], rgba[1], rgba[2]);
                    else
                        c = Color.FromArgb(255, rgba[0], rgba[1], rgba[2]);

                    result.SetPixel(x, y, c);
                }
                progress += (100f / 64f);
                Console.Clear();
                Console.WriteLine(progress + "%");
            }
            if (File.Exists(skins + "\\result.png"))
                File.Delete(skins + "\\result.png");
            result.Save(skins + "\\result.png");

            Console.WriteLine("Finished compiling.");
            Console.ReadKey(true);
        }
    }
}
