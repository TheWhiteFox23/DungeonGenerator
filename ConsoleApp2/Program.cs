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
            DungeonMap map = new DungeonMap(50, 30, 15,1);
            ImageBuffer buffer = new ImageBuffer();
            buffer.save();
            map.print();
            Console.ReadKey();
        }
    }

}
