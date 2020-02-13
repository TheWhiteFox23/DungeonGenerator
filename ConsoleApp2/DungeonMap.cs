using System;
using System.Collections.Generic;
using System.Linq;


namespace DungeonGenerator
{
    public class DungeonMap
    {
        int X;
        int Y;
        char[][] dMap;
        int [][] roomMap;
        char wall;
        int maxRoomSize;
        int minRoomSize;
        Dictionary<string, int> validStarts;
        Dictionary<int, Room2> rooms2;
        List<Room2> room2s;
        System.Diagnostics.Stopwatch watch;
        Random rn = new Random();

        public DungeonMap(int X, int Y, int maxRoomSize, int minRoomSize)
        {
            this.X = X;
            this.Y = Y;
            dMap = new char[Y][];
            roomMap = new int[Y][];
            this.maxRoomSize = maxRoomSize;
            this.minRoomSize = minRoomSize;
            wall = 'o';
            validStarts = new Dictionary<string, int>();
            room2s = new List<Room2>();
            watch = new System.Diagnostics.Stopwatch();
            rooms2 = new Dictionary<int, Room2>();

            onCreate();
        }
        private void onCreate()
        {

            TimeOfExecutionStart(watch);
            fillMap(wall);
            TimeOfExecutionEnd(watch, "Filling the map");
            findValid();
            TimeOfExecutionEnd(watch, "Finding Valid  ");
            generateRooms();
            TimeOfExecutionEnd(watch, "Generating rooms");
            mapRooms();
            TimeOfExecutionEnd(watch, "Maping Rooms   ");
            bufferRoomMap();
            mapBorders2();
            TimeOfExecutionEnd(watch, "Maping Borders  ");
            TimeOfExecutionEnd(watch, "Deleting Obsolent");
            conectRooms2();
            //bufferRoomMap();
            TimeOfExecutionEnd(watch, "Conecting Rooms");
            watch.Stop();
        }

        private void fillMap(char wall)
        {
            for (int i = 0; i < Y; i++)
            {
                char[] arr2 = new char[X];
                for (int j = 0; j < X; j++)
                {
                        arr2[j] = wall;
                }
                dMap[i] = arr2;
            }
        }

        private void generateRoom(string seed, int Xsize, int Ysize)
        {
            int Xindex = parseMap(seed)[0];
            int Yindex = parseMap(seed)[1];
            List<string> room = new List<string>();
            Dictionary<string, int> mapBorder = new Dictionary<string, int>();


            //replace all valid within the range
            for (int i = 0; i < Ysize; i++)
            {
                for (int j = 0; j < Xsize; j++)
                {
                    int iX = Xindex + j;
                    int iY = Yindex + i;

                    if (validStarts.ContainsKey(iY + "." + iX))
                    {
                        room.Add(iY + "." + iX);
                        dMap[Yindex + i][Xindex + j] = ' ';
                        validStarts.Remove(iY + "." + iX);
                    }
                    else
                    {
                        break;
                    }

                }
            }

            for (int i = Xindex - 1; i < Xindex + Xsize + 1; i++)
            {
                int Yi = Yindex - 1;
                if (validStarts.ContainsKey(Yi + "." + i))
                {
                    
                    dMap[Yi][i] = 'o';
                    validStarts.Remove(Yi + "." + i);
                }
            }

            for (int i = Xindex - 1; i < Xindex + Xsize + 1; i++)
            {
                int Yi = Yindex + Ysize;
                if (validStarts.ContainsKey(Yi + "." + i))
                {
                    
                    dMap[Yi][i] = 'o';
                    validStarts.Remove(Yi + "." + i);

                }
            }

            for (int i = Yindex - 1; i < Yindex + Ysize + 1; i++)
            {
                int Xi = Xindex - 1;
                if (validStarts.ContainsKey(i + "." + Xi))
                {
                    
                    dMap[i][Xi] = 'o';
                    validStarts.Remove(i + "." + Xi);
                }
            }

            for (int i = Yindex - 1; i < Yindex + Ysize + 1; i++)
            {
                int Xi = Xindex + Xsize;
                if (validStarts.ContainsKey(i + "." + Xi))
                {
                    
                    dMap[i][Xi] = 'o';
                    validStarts.Remove(i + "." + Xi);

                }
            }
        }

        private void generateRooms()
        {
            //Random rn = new Random();
            while (validStarts.Count != 0)
            {
                for (int i = 1; i < Y - 1; i++)
                {
                    for (int j = 1; j < X - 1; j++)
                    {
                        if (validStarts.ContainsKey(i + "." + j))
                        {
                            int roomSizeX = rn.Next(minRoomSize, maxRoomSize);
                            int roomSizeY = rn.Next(minRoomSize, maxRoomSize);
                            generateRoom(i + "." + j, roomSizeX, roomSizeY);

                        }

                    }
                }

            }
        }

        private void findValid()
        {
            for (int i = 1; i < Y - 1; i++)
            {
                for (int j = 1; j < X - 1; j++)
                {
                    try
                    {
                        if (dMap[i - 1][j - 1] == wall &&
                            dMap[i - 1][j] == wall &&
                            dMap[i - 1][j + 1] == wall &&
                            dMap[i][j - 1] == wall &&
                            dMap[i][j + 1] == wall &&
                            dMap[i + 1][j - 1] == wall &&
                            dMap[i + 1][j] == wall &&
                            dMap[i + 1][j + 1] == wall)
                        {
                            validStarts.Add(i + "." + j, 1);
                            //dMap[i][j] = 'x';
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Index");
                    }
                }
            }
        }

        public void print()
        {
            for (int i = 0; i < Y; i++)
            {
                Console.Write("{0}: \t", i);
                for (int j = 0; j < X; j++)
                {
                    Console.Write("{0} ", dMap[i][j]);
                }
                Console.WriteLine();
            }
        }

        private int[] parseMap(string mapOutput)
        {
            string X = "";
            string Y = "";
            bool dot = false;
            var arr = mapOutput.ToCharArray();
            for (int i = 0; i < mapOutput.Length; i++)
            {
                if (!dot && arr[i] != '.')
                {
                    Y = Y + arr[i];
                }
                else if (arr[i] == '.')
                {
                    dot = true;
                }
                else
                {
                    X = X + arr[i];
                }
            }
            int[] ret = { int.Parse(X), int.Parse(Y) };
            return ret;

        }
        
        private void conectRooms2()
        {
            /**Room Connect algorith destciption
             * 1, Select room - separate it from array
             * 2, Find all of the neghbourt of the room and randomly choose one
             * 3, selectst random border of the room - merge with current room and delete from list
             * 4, add current room to the list
             */
            //randomly choosing room
            Random random = new Random();
            int count = rooms2.Count();
;
            int rand = random.Next(2, count +1);
            Room2 megaRoom = rooms2[rand];


            while (rooms2.Count() != 1)
            {
                rooms2.Remove(megaRoom.getID());
                //Choose one random neighbour and conect
                Room2 neighboutr = new Room2(); //empty constructor (ID is zero and all tiles are empty);
                int roomToChange = 0;

                if(megaRoom.getSurrounding().Count != 0)
                {
                    roomToChange = megaRoom.getSurrounding().First();
                }
                else
                {
                    Console.WriteLine("Error ocured during room maping");
                    ImageBuffer buffer = new ImageBuffer(X, Y);
                    for (int i = 0; i < Y; i++)
                    {
                        for (int j = 0; j < X; j++)
                        {
                            if (dMap[i][j] == 'o')
                            {
                                buffer.PlotPixel(j, i, 0, 0, 0);
                            }
                            else
                            {
                                
                                buffer.PlotPixel(j, i, 255, 255, 255);
                            }
                        }
                    }
                    buffer.saveError();
                    break;
                }

                string border = megaRoom.getBorderMap()[roomToChange].Last();

                dMap[parseMap(border)[1]][parseMap(border)[0]] = ' ';

                //merging
                megaRoom.mergeWith2(rooms2[roomToChange], border);
                rooms2.Remove(roomToChange);
                rooms2.Add(megaRoom.getID(), megaRoom);
            }
        }

        private void mapRooms()
        {
            //TODO - !!!!!!!room maping can be more effective - iterate throu raw map - in case current tile will be 1 - flood fill and save room


            /*new map room method - indexing similar then writing the indexes into room list
             *1. Create copy of the dMap byt in integers - walls are 0, floor has ID number of the room
             *2. Generate floor tiles based on id - if no negbourth tiles has id - create new ID else set id to the one of 
             * the neghbourt tile
             *3.create the rooms based on the id - borders can be simultinuosly maped with the rooms
             */
            fillRoomMap2();

            int ID = 2;
            //storing all tiles and indexes of the room
            Dictionary<string, int> allTiles = new Dictionary<string, int>(); 
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    if (dMap[i][j] != 'o') allTiles.Add(i + "." + j, 0);
                    
                }
            }

            while (allTiles.Count()>0)
            {
                //randomly choose tile and perform flood fill
                var watch2 = new System.Diagnostics.Stopwatch();
                string toParse = allTiles.Keys.First();
                int indexX = parseMap(toParse)[0];
                int indexY = parseMap(toParse)[1];

                List<string> toRemove = floodFill(ID, indexX, indexY, 1, roomMap);

                rooms2.Add(ID, new Room2(toRemove, ID));

                ID++;

                foreach (var r in toRemove)
                {
                    allTiles.Remove(r);
                }

            }

        }

        public void fillRoomMap2()
        {
            for (int i = 0; i < Y; i++)
            {
                roomMap[i] = new int[X];
                for (int j = 0; j < X; j++)
                {
                    roomMap[i][j] = 0;
                }
            }
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    if (dMap[i][j] == 'o')
                    {
                        roomMap[i][j] = 0;
                    }
                    else
                    {
                        roomMap[i][j] = 1;
                    }
                }
            }
        }

        private List<string> floodFill(int ID, int X, int Y, int target, int[][] roomMap)
        {
            List<string> floodFiled = new List<string>();
            List<string> borders = new List<string>();
            if(roomMap[Y][X] == ID)
            {
                Console.WriteLine("ID is already correct");
                return floodFiled;
            }else if(roomMap[Y][X] != target)
            {
                Console.WriteLine("ID do not response to target");
                return floodFiled;
            }

            Queue<int[]> queue = new Queue<int[]>();
            queue.Enqueue(new int[] { X, Y });
            while (queue.Count() > 0)
            {
                floodFiled.Add(Y + "." + X);
                int indexX = queue.Peek()[0];
                int indexY = queue.Dequeue()[1];
                int[] W = { indexX, indexY };
                int[] E = { indexX, indexY };
                
                while (roomMap[W[1]][W[0]] == target)
                {
                    W[0]--; 
                }

                while (roomMap[E[1]][E[0]] == target)
                {
                    E[0]++;
                }
                for (int i = W[0]+1; i<E[0]; i++)
                {
                    roomMap[W[1]][i] = ID;
                    floodFiled.Add(W[1] + "." + i);
                    if(roomMap[W[1] + 1][i] == target)
                    {
                        queue.Enqueue(new int[] { i, W[1] + 1 });
                    }
                    if (roomMap[W[1] - 1][i] == target)
                    {
                        queue.Enqueue(new int[] { i, W[1] - 1 });
                    }
                }
            }

            return floodFiled;
        }

        private void bufferRoomMap()
        {
            ImageBuffer buffer = new ImageBuffer(X, Y);
            Dictionary<int, int[]> colors = new Dictionary<int, int[]>();
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    if (roomMap[i][j] == 0)
                    {
                        buffer.PlotPixel(j, i, 0, 0, 0);
                    }
                    else
                    {
                        if (colors.ContainsKey(roomMap[i][j]))
                        {
                            byte red = (Byte)colors[roomMap[i][j]][0];
                            byte green = (Byte)colors[roomMap[i][j]][1];
                            byte blue = (Byte)colors[roomMap[i][j]][2];

                            buffer.PlotPixel(j, i, red, green, blue);
                        }
                        else
                        {
                            byte red = (Byte)rn.Next(0, 255);
                            byte green = (Byte)rn.Next(0, 255);
                            byte blue = (Byte)rn.Next(0, 255);
                            byte b = (Byte)rn.Next(0, 255);
                            int[] col = { red, green, blue  };
                            colors.Add(roomMap[i][j], col);
                            buffer.PlotPixel(j, i, red, green, blue);
                        }

                    }
                }
            }
            buffer.saveColor();
        }

        public char[][] getMap()
        {
            return dMap;
        }

        private void TimeOfExecutionStart(System.Diagnostics.Stopwatch watch)
        {
            watch.Reset();
            watch.Start();

        }

        private void TimeOfExecutionEnd(System.Diagnostics.Stopwatch watch, string message)
        {
            Console.WriteLine("\t {0} \t time of execution {1}", message, watch.Elapsed);
            watch.Reset();
            watch.Start();

        }

        private void mapBorders2()
        {
            int[][] indexes =
            {
                //indexes for X1
                new int[] {0,1},
                //indexes for Y1
                new int[] {1,0},
                //indexes for X2
                new int[] {0,-1},
                //indexes for Y2
                new int[] {-1,0}
            };
            for(int i = 1; i<Y-1; i++)
            {
                for(int j = 1; j< X-1; j++)
                {
                    if(roomMap[i][j] == 0)
                    {
                        for(int t = 0; t<indexes[0].Length; t++)
                        {
                            int indexX1 = j + indexes[0][t];
                            int indexY1 = i + indexes[1][t];
                            int indexX2 = j + indexes[2][t];
                            int indexY2 = i + indexes[3][t];
                            if (indexX1 >= X || indexX1 <= 0 || indexX2 >= X || indexX2 <= 0 || indexY1 >= Y || indexY1 <= 0 || indexY2 >= Y || indexY2 <= 0) continue;
                            if(roomMap[indexY1][indexX1]>0 && roomMap[indexY2][indexX2]>0 && roomMap[indexY1][indexX1] != roomMap[indexY2][indexX2])
                            {
                                if(!rooms2[roomMap[indexY1][indexX1]].getBorder2().ContainsKey(i + "." + j))rooms2[roomMap[indexY1][indexX1]].addBorder2(i + "." + j, roomMap[indexY2][indexX2]);
                                if(!rooms2[roomMap[indexY2][indexX2]].getBorder2().ContainsKey(i + "." + j))rooms2[roomMap[indexY2][indexX2]].addBorder2(i + "." + j, roomMap[indexY1][indexX1]);
                                break;
                            }
                        }
                        
                    }
                }
            }
        }

        //UNUSED

        private void printRoomMap()
        {
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    Console.Write(roomMap[i][j] + " ");

                }
                Console.WriteLine();
            }

        }

        private void printRooms()
        {
            foreach(var r in rooms2.Values.ToList())
            {
                Console.WriteLine("Room with ID: {0} has following statistics", r.getID());
                //Console.Write("\t");
                foreach(var n in r.getBorderMap())
                {
                    Console.Write("\t");
                    Console.Write("Neighbourt ID: {0} has following borders : ", n.Key);
                    foreach(var l in n.Value)
                    {
                        Console.Write("{0} ", l);
                    }
                    Console.WriteLine();
                }
                
            }
        }

        private void writeRoomMap()
        {
            string text = "";
            foreach (var r in rooms2.Values.ToList())
            {
                text +=  ("Room with ID: " + r.getID()+" has following statistics \n");
                //Console.Write("\t");
                foreach (var n in r.getBorderMap())
                {
                    text += ("\t");
                    text += ("Neighbourt ID: " + n.Key + " has following borders : ");
                    foreach (var l in n.Value)
                    {
                        text +=  (l + " ");
                    }
                    text += ("\n");
                }

            }
            System.IO.File.WriteAllText(@"C:\Users\pavelkaf\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\RoomBorders.txt", text);
        }  
        private void writeMap()
        {
            string text = "";
            for (int i = 0; i < Y; i++)
            {
                text += (i + ": \t");
                for (int j = 0; j < X; j++)
                {
                    text += (dMap[i][j] + " ");
                }
                text += "\n";
            }
            System.IO.File.WriteAllText(@"C:\Users\pavelkaf\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\RoomMap.txt", text);
        }

        private void setMap()
        {
            char[][] testMap = new char[30][];
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\pavelkaf\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\TestMap.txt");
            //test if reading
            /*foreach(var l in lines)
            {
                Console.WriteLine(l);

            }*/
            int IndexY =0;
            
            foreach(var l in lines)
            {
                int IndexX = 0;
                //Console.WriteLine("l.lengt is {0}", l.Length);
                for(int i =0; i<l.Length; i++) 
                {
                    if(i%2 == 0)
                    {
                        try
                        {
                            dMap[IndexY][IndexX] = l.ElementAt(i);
                        }
                        catch
                        {

                        }
                        
                        IndexX++;
                    }
                    
                    

                }
                IndexY++;
            }

        }
        //private void deleteObsolent()
        //{
        //    foreach (var rm in room2s)
        //    {

        //        rm.cleanRoom(dMap);

        //    }
        //}



    }
}