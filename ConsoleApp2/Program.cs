using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //TODO : What is the most time demanding part of the algorithm ?????
            //var currentTime =  
            Console.WriteLine("Starting the map generation \t time of execution:{0} ", watch.Elapsed);
            watch.Reset();
            watch.Start();
            DungeonMap map = new DungeonMap(500, 300, 15,1);
            Console.WriteLine("Ending the map generation \t time of execution:{0} ", watch.Elapsed);
            watch.Reset();
            watch.Start();
            ImageBuffer buffer = new ImageBuffer(500, 300);
            Console.WriteLine("Image Buffer Created \t time of execution:{0} ", watch.Elapsed);
            watch.Reset();
            watch.Start();
            char[][] dmap = map.getMap();
            //write array into png
            for(int i = 0; i< 300; i++)
            {
                for(int j = 0; j< 500; j++)
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
            watch.Reset();
            watch.Start();
            buffer.save();
            Console.WriteLine("Image saved \t time of execution:{0} ", watch.Elapsed);
            //map.print();
            Console.ReadKey();
        }
    }

}
