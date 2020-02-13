using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    class Program
    {
        public static string time = "";
        public static string input = "";
        public static int passes = 0;
        
        static void Main(string[] args)
        {
            int X = 1000;
            int Y = 1000;
            passes = X * Y;
            //DungeonMap map = new DungeonMap(X, Y, 15, 1);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            //TODO Clean Up - wreime more efective conection and maping algorith
            //TODO : What is the most time demanding part of the algorithm ?????
            //var currentTime =  
            Console.WriteLine("Starting the map generation \t time of execution:{0} ", watch.Elapsed);
            watch.Restart();
            //watch.Start();
            /*for (int i = 1; i< 200; i++)
            {

                    Console.WriteLine(i);
                    passes = X * Y;
                    map = new DungeonMap(X, Y, 15, 1);
                //passes = X * Y;
                //map = new DungeonMap(X, Y, 15, 1);
            }*/

            System.IO.File.WriteAllText(@"C:\Users\pavelkaf\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\time.txt", time);
            System.IO.File.WriteAllText(@"C:\Users\pavelkaf\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\input.txt", input);
            DungeonMap map = new DungeonMap(X, Y, 15,1);
            Console.WriteLine("Ending the map generation \t time of execution:{0} ", watch.Elapsed);
            watch.Restart();
            //watch.Reset();
            //watch.Start();
            ImageBuffer buffer = new ImageBuffer(X, Y);
            Console.WriteLine("Image Buffer Created \t time of execution:{0} ", watch.Elapsed);
            watch.Restart();
            //watch.Reset();
            //watch.Start();
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
            //watch.Reset();
            //watch.Start();
            buffer.save();
            Console.WriteLine("Image saved \t time of execution:{0} ", watch.Elapsed);
            //map.print();
            Console.ReadKey();
        }
    }

}
