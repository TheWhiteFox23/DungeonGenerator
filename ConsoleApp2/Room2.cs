using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator

{
    class Room2
    {
        //empty tiles list
        List<string> tiles; //= new List<string>();
        //border tiles list
        List<string> borders; //= new List<string>();
        //id number - smallest is 1, 0 is reserved for walls;
        Dictionary<string, int> border2 = new Dictionary<string, int>();
        int id;
        //Upper right block - only for debgig purpose can be removed
        string upperRight;
        //Map of neighborts and border with them
        Dictionary<int, List<string>> neighbours = new Dictionary<int, List<string>>();

        List<int> conections = new List<int>();

        //constructor
        public Room2(List<string> tiles, List<string> borders, int id)
        {
            this.tiles = tiles;
            this.borders = borders;
            this.id = id;
            upperRight = findUpperRightTile();
            conections.Add(id);
        }
        public Room2(List<string> tiles, int id)
        {
            this.tiles = tiles;
            this.id = id;
            upperRight = findUpperRightTile();
            conections.Add(id);
        }

        //Overload - test of git
        public Room2()
        {
            tiles = new List<string>();
            borders = new List<string>();
            id = 0;
            upperRight = findUpperRightTile();
            conections.Add(id);
        }


        // GET and SET methosd
        public List<string> getBorders()
        {
            return borders;
        }

        public void setBorders(List<string> borders)
        {
            this.borders = borders;
        }

        public List<string> getTiles()
        {
            return tiles;
        }

        public void setTiles(List<string> tiles)
        {
            this.tiles = tiles;
        }

        public int getID()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public string getUpperRight()
        {
            return upperRight;
        }

        private string findUpperRightTile()
        {
            int min = int.MaxValue;
            string tile = "";
            foreach(var s in tiles)
            {
                if (parseMap(s)[0]  + parseMap(s)[1] < min)
                {
                    min = parseMap(s)[0] + parseMap(s)[1];
                    tile = s;
                }
            }

            return tile;
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

        public void cleanRoom(char[][] dMap)
        {
            List<string> obsolent = new List<string>();
            foreach (string key in borders)
            {
                int iX = parseMap(key)[0];
                int iY = parseMap(key)[1];
                //search if tile has at least on side covered
                int[] surronding = new int[8];
                int[] firstIndexes = { 0, -1, -1, -1, 0, 1, 1, 1 };
                int[] secondIndexes = { -1, -1, 0, 1, 1, 1, 0, -1 };


                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        if (dMap[iY + firstIndexes[i]][iX + secondIndexes[i]] == 'o')
                        {
                            surronding[i] = 1;

                        }
                        else
                        {
                            surronding[1] = 0;
                        }
                    }
                    catch
                    {
                        Console.WriteLine(" Index out of bounds  ---- Y index : {0}   X index : {1} ", iY + secondIndexes[i], iX + firstIndexes[i]);
                    }


                }
                //delete if et least 5 ones in the row
                int[] helpArr = new int[16];
                for (int i = 0; i < 16; i++)
                {
                    helpArr[i] = surronding[i % 8];

                }

                int count = 0;
                int maxCount = 0;
                foreach (int i in helpArr)
                {
                    if (i == 1)
                    {
                        count++;
                    }
                    else if (i == 0)
                    {
                        if (count > maxCount) maxCount = count;
                        count = 0;
                    }
                }

                if (maxCount >= 5)
                {
                    obsolent.Add(iY + "." + iX);
                }
            }

            int[][] indexShift =
                {
                    new int[] {0 , 0, 1, 1,  1,-1,-1, -1},
                    new int[] {1 ,-1, 1, 0, -1, 1, 0, -1}
                };

            foreach (var v in obsolent)
            {
                dMap[parseMap(v)[1]][parseMap(v)[0]] = ' ';
                borders.Remove(v);
                tiles.Add(v);

                for (int i = 0; i < indexShift[0].Length; i++)
                {
                    int indexX = parseMap(v)[0] + indexShift[1][i];
                    int indexY = parseMap(v)[1] + indexShift[0][i];
                    if (dMap[indexY][indexX] == 'o')
                    {
                        if (!borders.Contains(indexY + "." + indexX) && (indexY > 0 && indexY < dMap.Length - 1) && (indexX > 0 && indexX < dMap[1].Length - 1)) borders.Add(indexY + "." + indexX);
                    }

                }
            }
            upperRight = getUpperRight();



        }

        public void mapBorder(int[][] roomMap)
        {
            //Console.WriteLine("BordersCount : {0}", borders.Count());
            for (int i = 0; i < borders.Count(); i++)
            {
                string btile = borders.ElementAt(i);
                int iX = parseMap(btile)[0];
                int iY = parseMap(btile)[1];
                //controling values in loop
                int[][] cIncr = new int[][]{
                    new int[] {1, 0, -1, 0},
                    new int[] {-1, 0, 1, 0},
                    new int[] {0, - 1, 0, 1},
                    new int[] {0, 1, 0, -1}
                };
                //Console.WriteLine("iX : {0}   iY : {1}", iX, iY);
                for (int l = 0; l < cIncr.Length; l++)
                {
                    int border = 0;

                    if ((  
                            roomMap[iY + cIncr[l][0]][iX + cIncr[l][1]] == id && roomMap[iY + cIncr[l][2]][iX + cIncr[l][3]] != 0))
                    {
                        int room = roomMap[iY + cIncr[l][2]][iX + cIncr[l][3]];
                        border= roomMap[iY + cIncr[l][2]][iX + cIncr[l][3]];
                        if (!neighbours.ContainsKey(room))
                        {
                            List<string> tileSet = new List<string>();
                            tileSet.Add(btile);
                            neighbours.Add(room, tileSet);
                        }
                        else
                        {
                            List<string> tileSet = neighbours[room];
                            tileSet.Add(btile);
                            neighbours[room] = tileSet;
                        }
                        break;

                    }
                }

            }

        }

        public void mapBorders2(int[][] roomMap)
        {
            foreach(var b in borders)
            {
                Console.Write("{0}   ", b);
            }
            Console.WriteLine();
        }

        public List<int> getSurrounding()
        {
            return neighbours.Keys.ToList();
        }

        public Dictionary<int, List<string>> getBorderMap()
        {
            return neighbours;
        }

        public void setBorderMap(Dictionary<int, List<string>> burderMap)
        {
            neighbours = burderMap;
        }

        public void mergeWith(Room2 roomToMerge, string border)
        {
            //merge tiles
            foreach(var t in roomToMerge.getTiles())
            {
                if(!tiles.Contains(t))tiles.Add(t);
            }
            tiles.Add(border);

            //merge borders
            foreach(var b in roomToMerge.getBorders())
            {
                if (!borders.Contains(b)) borders.Add(b);
            }

            borders.Remove(border);

            conections.Add(roomToMerge.getID());


            foreach(var n in roomToMerge.getBorderMap())
            {
                if (!neighbours.ContainsKey(n.Key))
                {
                    neighbours.Add(n.Key, n.Value);
                }
                else
                {
                    var l = neighbours[n.Key];
                    foreach(var i in n.Value)
                    {
                        if(i != border)
                        {
                            l.Add(i);
                        }
                        
                    }
                    neighbours[n.Key] = l;
                }
                
            }
            foreach(var c in conections)
            {
                if(neighbours.ContainsKey(c))neighbours.Remove(c);
            }
        }
        public void mergeWith2(Room2 roomToMerge, string border)
        {
            //merge tiles
            /*foreach(var t in roomToMerge.getTiles())
            {
                if(!tiles.Contains(t))tiles.Add(t);
            }
            tiles.Add(border);*/

            //merge borders
            /*foreach(var b in roomToMerge.getBorder2())
            {
                if (!border2.ContainsKey(b.Key)) border2.Add(b.Key, b.Value);
            }*/

            border2.Remove(border);

            conections.Add(roomToMerge.getID());


            foreach(var n in roomToMerge.getBorderMap())
            {
                if (!neighbours.ContainsKey(n.Key))
                {
                    neighbours.Add(n.Key, n.Value);
                }
                else
                {
                    var l = neighbours[n.Key];
                    foreach(var i in n.Value)
                    {
                        if(i != border)
                        {
                            l.Add(i);
                        }
                        
                    }
                    neighbours[n.Key] = l;
                }
                
            }
            foreach(var c in conections)
            {
                if(neighbours.ContainsKey(c))neighbours.Remove(c);
            }
        }

        public void addBorder2(string border, int neighbourt)
        {
            if(!border2.ContainsKey(border))border2.Add(border, neighbourt);
            if (neighbours.ContainsKey(neighbourt))
            {
                List<string> add = neighbours[neighbourt];
                add.Add(border);
                neighbours[neighbourt] = add;
            }
            else
            {
                List<string> add = new List<string>();
                add.Add(border);
                neighbours.Add(neighbourt, add);
            }

        }

        public Dictionary<string, int> getBorder2()
        {
            return border2;

        }
    }

}