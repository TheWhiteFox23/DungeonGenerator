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
        char empty;
        int maxRoomSize;
        int minRoomSize;
        Dictionary<string, int> validStarts;
        Dictionary<int, Room2> rooms2;
        List<Room2> room2s;
        System.Diagnostics.Stopwatch watch;

        public DungeonMap(int X, int Y, int maxRoomSize, int minRoomSize)
        {
            this.X = X;
            this.Y = Y;
            dMap = new char[Y][];
            roomMap = new int[Y][];
            this.maxRoomSize = maxRoomSize;
            this.minRoomSize = minRoomSize;
            wall = 'o';
            empty = ' ';
            validStarts = new Dictionary<string, int>();
            room2s = new List<Room2>();
            watch = new System.Diagnostics.Stopwatch();
            rooms2 = new Dictionary<int, Room2>();

            onCreate();
        }
        private void onCreate()
        {
            //setMap();
            TimeOfExecutionStart(watch);
            fillMap(wall);
            //setMap();
            TimeOfExecutionEnd(watch, "Filling the map");
            //TODO - findValid and showValid methods are practicly obsolent can be replaced with in the fillmap method
            findValid();
            TimeOfExecutionEnd(watch, "Finding Valid  ");
            showValid();
            //TimeOfExecutionEnd(watch, "Showing Valid  ");
            generateRooms();
            TimeOfExecutionEnd(watch, "Generating rooms");
            mapRooms();
            TimeOfExecutionEnd(watch, "Maping Rooms   ");
            //TODO : is room maping working correctly ?
            mapBorders2();
            //printRooms();
            //writeRoomMap();
            //writeMap();
            //setMap();
            //print();
            TimeOfExecutionEnd(watch, "Maping Borders  ");
            //floodFillTest();
            //printMapRoom();
            //deleteObsolent();
            //TimeOfExecutionEnd(watch, "Deleting Obsolent");
            //fillRoomMap();
            //TimeOfExecutionEnd(watch, "Filling Room Map");
            //mapBorders();
            //TimeOfExecutionEnd(watch, "Maping Borders");
            //room2s = rooms2.Values.ToList();
            //conectRooms();
            //TODO : cobectRooms method needs fix !!!
            conectRooms2();
            //writeMap();
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

        public void fillRoomMap()
        {
            //initialize
            for(int i = 0; i< Y; i++)
            {
                roomMap[i] = new int[X];
                for(int j = 0; j< X; j++)
                {
                    roomMap[i][j] = 0;
                }
            }
            foreach(Room2 r in room2s)
            {
                foreach(var t in r.getTiles())
                {
                    roomMap[parseMap(t)[1]][parseMap(t)[0]] = r.getID();
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
            for (int i = 0; i< Y; i++)
            {
                for(int j = 0; j< X; j++)
                {
                    if(dMap[i][j] == 'o')
                    {
                        roomMap[i][j] = 0;
                    }else
                    {
                        roomMap[i][j] = 1;
                    }
                }
            }
        }

        private void generateRoom(string seed, int Xsize, int Ysize)
        {
            Random rn = new Random();
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

        private void deleteObsolent()
        {
            foreach(var rm in room2s)
            {

                rm.cleanRoom(dMap);

            }
        }

        private void mapBorders()
        {
            foreach (Room2 rm in room2s)
            {
                rm.mapBorder(roomMap);
            }
        }

        private void generateRooms()
        {
            Random rn = new Random();
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

        private void showValid()
        {
            var ar = validStarts.Keys.ToList();
            foreach (var st in ar)
            {
                int Xindex = parseMap(st)[0];
                int Yindex = parseMap(st)[1];
                dMap[Yindex][Xindex] = 'x';
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

        private void conectRooms()
        {
            //randomly choosing room
            Random random = new Random();

            //purpose of the megaroom is to celect all conected room
            Room2 megaRoom = room2s.ElementAt(random.Next(room2s.Count));

            //Console.WriteLine("Selected room with id {0}", megaRoom.getID());

            while (room2s.Count() != 1)
            {
                Random rn = new Random();
                room2s.Remove(megaRoom);
                //Choose one random neighbour and conect
                Room2 neighboutr = new Room2(); //empty constructor (ID is zero and all tiles are empty);
                int roomToChange = megaRoom.getSurrounding().ElementAt(rn.Next(megaRoom.getSurrounding().Count()));

                foreach (var r in room2s)
                {
                    if (r.getID() == roomToChange)     
                    {
                        neighboutr = r;
                    }
                }
                //ckeck if room was changed
                if (neighboutr.getID() == 0)
                {
                    Console.WriteLine("Neighbourt wasn't changed !!!!");
                }

                //remove random border
                string border = megaRoom.getBorderMap()[roomToChange].Last();

                dMap[parseMap(border)[1]][parseMap(border)[0]] = '!';

                //merge tiles
                megaRoom.mergeWith(neighboutr, border);
                room2s.Remove(neighboutr);
                room2s.Add(megaRoom);

            }
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

            //purpose of the megaroom is to celect all conected room
            //Room2 megaRoom = rooms2.Values.ToList().ElementAt(random.Next(rooms2.Count));
            Room2 megaRoom = rooms2[random.Next(rooms2.Count())];

            //Console.WriteLine("Selected room with id {0}", megaRoom.getID());

            while (rooms2.Count() != 1)
            {
                //Random rn = new Random();
                rooms2.Remove(megaRoom.getID());
                //Choose one random neighbour and conect
                Room2 neighboutr = new Room2(); //empty constructor (ID is zero and all tiles are empty);

                
                int roomToChange = megaRoom.getSurrounding().Last();
                //Console.WriteLine(roomToChange);

                //Get Tile
                //List<string> BorderTiles = megaRoom.getBorderMap()[roomToChange];
                /*Console.Write("Borders: ");
                foreach(var v in BorderTiles)
                {
                    Console.Write("{0} ", v);
                }
                Console.WriteLine();*/

                string border = megaRoom.getBorderMap()[roomToChange].Last();
                //string border = BorderTiles.ElementAt(random.Next(BorderTiles.Count()));

                //Console.WriteLine("Choosen Border Tile : {0}", border);

                dMap[parseMap(border)[1]][parseMap(border)[0]] = ' ';

                //merging
                megaRoom.mergeWith2(rooms2[roomToChange], border);
                rooms2.Remove(roomToChange);
                rooms2.Add(megaRoom.getID(), megaRoom);

                //neighboutr = rooms2[roomToChange];
                ////ckeck if room was changed
                //if (neighboutr.getID() == 0)
                //{
                //    Console.WriteLine("Neighbourt wasn't changed !!!!");
                //}

                ////remove random border
                //string border = megaRoom.getBorderMap()[roomToChange].Last();
                //Console.Write("CurrentMegaroomNeighbourts and borders:");
                //foreach(var b in megaRoom.getBorderMap()[roomToChange])
                //{
                //    Console.Write(b + " ");
                //}
                //Console.WriteLine();

                //try
                //{
                //    dMap[parseMap(border)[1]][parseMap(border)[0]] = '!';
                //}
                //catch
                //{
                //    Console.WriteLine("IndexOutOfBoudExeption");
                //}
                


                ////merge tiles
                //megaRoom.mergeWith2(neighboutr, border);
                //room2s.Remove(neighboutr);
                //room2s.Add(megaRoom);

            }
        }

        private void mapRooms()
        {
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
            //int dictionaryCount = allTiles.Count();
            while (allTiles.Count()>0)
            {
                //randomly choose tile and perform flood fill
                List<string> toRemove = floodFill(ID, parseMap(allTiles.Keys.Last())[0], parseMap(allTiles.Keys.Last())[1], 1, roomMap);
                rooms2.Add(ID, new Room2(toRemove, ID));
                ID++;
                foreach(var r in toRemove)
                {
                    allTiles.Remove(r);
                    //Console.Write(r + " ");
                    //dictionaryCount--;
                }
                //Console.WriteLine();
            }


            //printRoomMap();


            //CODE FOR TESTING THE NEW ROOM MAPPING ALGORITH - convert room into image
            //bufferRoomMap();

        }

        private void floodFillTest()
        {
            int[][] test =
            {
                new int[] {0,0,0,0,0,0,0,0,0,0},
                new int[] {0,1,1,1,0,0,1,1,1,0},
                new int[] {0,1,1,1,0,0,1,1,1,0},
                new int[] {0,1,1,1,0,0,1,1,1,0},
                new int[] {0,1,1,1,0,0,1,1,1,0},
                new int[] {0,0,0,0,0,0,1,1,1,0},
                new int[] {0,1,1,1,0,0,1,1,1,0},
                new int[] {0,1,1,1,0,0,1,1,1,0},
                new int[] {0,1,1,1,0,0,1,1,1,0},
                new int[] {0,0,0,0,0,0,0,0,0,0},
            };
            floodFill(2, 2, 3, 1, test);
            floodFill(3, 7, 8, 1, test);

            ImageBuffer buffer = new ImageBuffer(10, 10);
            Dictionary<int, int[]> colors = new Dictionary<int, int[]>();
            for (int i = 0; i < 10; i++)
            {
                Random rn = new Random();
                for (int j = 0; j < 10; j++)
                {
                    if (test[i][j] == 0)
                    {
                        buffer.PlotPixel(j, i, 0, 0, 0);
                    }
                    else
                    {
                        if (colors.ContainsKey(test[i][j]))
                        {
                            byte red = (Byte)colors[test[i][j]][0];
                            byte green = (Byte)colors[test[i][j]][1];
                            byte blue = (Byte)colors[test[i][j]][2];

                            buffer.PlotPixel(j, i, red, green, blue);
                        }
                        else
                        {
                            //Random rn = new Random();
                            byte red = (Byte)rn.Next(0, 255);
                            byte green = (Byte)rn.Next(0, 255);
                            byte blue = (Byte)rn.Next(0, 255);
                            int[] col = { red, green, blue };
                            colors.Add(test[i][j], col);
                            buffer.PlotPixel(j, i, red, green, blue);
                        }

                    }
                }
            }
            buffer.save();

            for(int i = 0; i< 10; i++)
            {
                for (int j = 0; j< 10; j++)
                {
                    Console.Write(test[i][j] + " ");
                }
                Console.WriteLine();
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
                    //Console.Write(W[0] + " ");
                    W[0]--; 
                }

                while (roomMap[E[1]][E[0]] == target)
                {
                    //Console.Write(E[0] + " ");
                    E[0]++;
                }
                //Console.WriteLine();
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
                Random rn = new Random();
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
                            //Random rn = new Random();
                            byte red = (Byte)rn.Next(0, 255);
                            byte green = (Byte)rn.Next(0, 255);
                            byte blue = (Byte)rn.Next(0, 255);
                            int[] col = { red, green, blue };
                            colors.Add(roomMap[i][j], col);
                            buffer.PlotPixel(j, i, red, green, blue);
                        }

                    }
                }
            }
            buffer.save();
        }

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

        private void printMapRoom()
        {
            foreach(var r in room2s)
            {
                var tiles = r.getTiles();
                foreach(var s in tiles)
                {
                    int indexX = parseMap(s)[0];
                    int indexY = parseMap(s)[1];
                    dMap[indexY][indexX] = '.';
                }
                var upperRight = r.getUpperRight();
                int iX = parseMap(upperRight)[0];
                int iY = parseMap(upperRight)[1];
                dMap[iY][iX] = '!';
                //dMap[iY][iX] = (char)(r.getID());
                //Console.WriteLine(room2s.Count());
                //System.Threading.Thread.Sleep(20);
                /*Console.ReadKey();
                print();*/
            }
            
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
            /*foreach(var r in rooms2)
            {
                Console.Write("Room with ID: {0} has following borders", r.Value.getID());
                foreach(var b in r.Value.getBorder2())
                {
                    Console.Write(b.Key + " ");
                }
                Console.WriteLine();
            }*/

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



    }
}