using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    class Program
    {
        //public static string time = "";
        //public static string input = "";
        //public static int passes = 0;
        
        static void Main(string[] args)
        {
            int X = 50;
            int Y = 30;
            //passes = X * Y;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("Starting the map generation \t time of execution:{0} ", watch.Elapsed);
            watch.Restart();

            //System.IO.File.WriteAllText(@"C:\Users\pavelkaf\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\time.txt", time);
            //System.IO.File.WriteAllText(@"C:\Users\pavelkaf\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\input.txt", input);
            DungeonMap map = new DungeonMap(X, Y, 15, 1);
            Console.WriteLine("Ending the map generation \t time of execution:{0} ", watch.Elapsed);
            watch.Restart();

            bufferImage(X, Y, watch, map);

            map.print();
            Console.ReadKey();
        }

        private static void bufferImage(int X, int Y, Stopwatch watch, DungeonMap map)
        {
            ImageBuffer buffer = new ImageBuffer(X, Y);
            Console.WriteLine("Image Buffer Created \t time of execution:{0} ", watch.Elapsed);
            watch.Restart();

            char[][] dmap = map.getMap();
            //write array into png
            for (int i = 0; i < Y - 1; i++)
            {
                for (int j = 0; j < X - 1; j++)
                {
                    if (dmap[i][j] == 'o')
                    {
                        buffer.PlotPixel(j, i, 0, 0, 0);
                    }
                    else
                    {
                        buffer.PlotPixel(j, i, 255, 255, 255);
                    }
                }
            }
            Console.WriteLine("Image created base on the map \t time of execution:{0} ", watch.Elapsed);
            watch.Restart();

            buffer.save();
            Console.WriteLine("Image saved \t time of execution:{0} ", watch.Elapsed);
        }
    }

}
