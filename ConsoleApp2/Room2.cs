using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator

{
    class Room2
    {
        //id number - smallest is 1, 0 is reserved for walls;
        Dictionary<string, int> border2 = new Dictionary<string, int>();
        int id;
        //Map of neighborts and border with them
        Dictionary<int, List<string>> neighbours = new Dictionary<int, List<string>>();

        List<int> conections = new List<int>();

        //constructor
        public Room2(List<string> tiles, List<string> borders, int id)
        {
            this.id = id;
            conections.Add(id);
        }
        public Room2(List<string> tiles, int id)
        {
            this.id = id;
            conections.Add(id);
        }

        //Overload - test of git
        public Room2()
        {
            id = 0;
            conections.Add(id);
        }


        // GET and SET methosd

        public int getID()
        {
            return id;
        }

        //private int[] parseMap(string mapOutput)
        //{
        //    string X = "";
        //    string Y = "";
        //    bool dot = false;
        //    var arr = mapOutput.ToCharArray();
        //    for (int i = 0; i < mapOutput.Length; i++)
        //    {
        //        if (!dot && arr[i] != '.')
        //        {
        //            Y = Y + arr[i];
        //        }
        //        else if (arr[i] == '.')
        //        {
        //            dot = true;
        //        }
        //        else
        //        {
        //            X = X + arr[i];
        //        }
        //    }
        //    int[] ret = { int.Parse(X), int.Parse(Y) };
        //    return ret;

        //}

        //public void cleanRoom(char[][] dMap)
        //{
        //    List<string> obsolent = new List<string>();
        //    foreach (string key in borders)
        //    {
        //        int iX = parseMap(key)[0];
        //        int iY = parseMap(key)[1];
        //        //search if tile has at least on side covered
        //        int[] surronding = new int[8];
        //        int[] firstIndexes = { 0, -1, -1, -1, 0, 1, 1, 1 };
        //        int[] secondIndexes = { -1, -1, 0, 1, 1, 1, 0, -1 };


        //        for (int i = 0; i < 8; i++)
        //        {
        //            try
        //            {
        //                if (dMap[iY + firstIndexes[i]][iX + secondIndexes[i]] == 'o')
        //                {
        //                    surronding[i] = 1;

        //                }
        //                else
        //                {
        //                    surronding[1] = 0;
        //                }
        //            }
        //            catch
        //            {
        //                Console.WriteLine(" Index out of bounds  ---- Y index : {0}   X index : {1} ", iY + secondIndexes[i], iX + firstIndexes[i]);
        //            }


        //        }
        //        //delete if et least 5 ones in the row
        //        int[] helpArr = new int[16];
        //        for (int i = 0; i < 16; i++)
        //        {
        //            helpArr[i] = surronding[i % 8];

        //        }

        //        int count = 0;
        //        int maxCount = 0;
        //        foreach (int i in helpArr)
        //        {
        //            if (i == 1)
        //            {
        //                count++;
        //            }
        //            else if (i == 0)
        //            {
        //                if (count > maxCount) maxCount = count;
        //                count = 0;
        //            }
        //        }

        //        if (maxCount >= 5)
        //        {
        //            obsolent.Add(iY + "." + iX);
        //        }
        //    }

        //    int[][] indexShift =
        //        {
        //            new int[] {0 , 0, 1, 1,  1,-1,-1, -1},
        //            new int[] {1 ,-1, 1, 0, -1, 1, 0, -1}
        //        };

        //    foreach (var v in obsolent)
        //    {
        //        dMap[parseMap(v)[1]][parseMap(v)[0]] = ' ';
        //        borders.Remove(v);
        //        tiles.Add(v);

        //        for (int i = 0; i < indexShift[0].Length; i++)
        //        {
        //            int indexX = parseMap(v)[0] + indexShift[1][i];
        //            int indexY = parseMap(v)[1] + indexShift[0][i];
        //            if (dMap[indexY][indexX] == 'o')
        //            {
        //                if (!borders.Contains(indexY + "." + indexX) && (indexY > 0 && indexY < dMap.Length - 1) && (indexX > 0 && indexX < dMap[1].Length - 1)) borders.Add(indexY + "." + indexX);
        //            }

        //        }
        //    }



        //}

        public List<int> getSurrounding()
        {
            return neighbours.Keys.ToList();
        }

        public Dictionary<int, List<string>> getBorderMap()
        {
            return neighbours;
        }

        public void mergeWith2(Room2 roomToMerge, string border)
        {

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