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

            onCreate();
        }
        private void onCreate()
        {
            TimeOfExecutionStart(watch);
            fillMap(wall);
            TimeOfExecutionEnd(watch, "Filling the map");
            findValid();
            TimeOfExecutionEnd(watch, "Finding Valid  ");
            showValid();
            TimeOfExecutionEnd(watch, "Showing Valid  ");
            generateRooms();
            TimeOfExecutionEnd(watch, "Generating rooms");
            mapRooms();
            TimeOfExecutionEnd(watch, "Maping Rooms   ");
            printMapRoom();
            /*deleteObsolent();
            TimeOfExecutionEnd(watch, "Deleting Obsolent");
            fillRoomMap();
            TimeOfExecutionEnd(watch, "Filling Room Map");
            mapBorders();
            TimeOfExecutionEnd(watch, "Maping Borders");
            conectRooms();
            TimeOfExecutionEnd(watch, "Conecting Rooms");*/
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

        private void mapRooms()
        {
            /*new map room method - indexing similar then writing the indexes into room list
             *1. Create copy of the dMap byt in integers - walls are 0, floor has ID number of the room
             *2. Generate floor tiles based on id - if no negbourth tiles has id - create new ID else set id to the one of 
             * the neghbourt tile
             *3.create the rooms based on the id - borders can be simultinuosly maped with the rooms
             */
            fillRoomMap();

            int ID = 1;
            for(int i = 0; i< Y; i++)
            {
                for(int j = 0; j< X; j++)
                {
                    //tile check logic
                    //check in a tile is the wall
                    if (dMap[i][j] == 'o')
                    {
                        roomMap[i][j] = 0;
                    }
                    else
                    {
                        //help array with indexes
                        int[][] helpArray =
                        {
                            // X indexes
                            new int[] {0,0,1,-1},
                            //Y indexes
                            new int[] {1,-1,0,0}
                        };
                        for(int t = 0; t<helpArray[0].Length; t++)
                        {
                            int indexX = j + helpArray[0][t];
                            int indexY = i + helpArray[1][t];
                            if (((indexX > 0 && indexX < X) && (indexY > 0 && indexY < Y) && roomMap[indexY][indexX] > 0))
                            {
                                roomMap[i][j] = roomMap[indexY][indexX];
                                break;
                            }
    
                        }
                        if(roomMap[i][j] == 0)
                        {
                            roomMap[i][j] = ID;
                            ID++;

                        }
                    }
                }
            }

            printRoomMap();


            //CODE FOR TESTING THE NEW ROOM MAPPING ALGORITH - convert room into image
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
                    Console.Write(roomMap[i][j] + "   ");

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

    }
}