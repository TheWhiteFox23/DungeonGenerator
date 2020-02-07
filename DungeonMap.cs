using System;
using System.Collections.Generic;
using System.Linq;


public class DungeonMap
{
    int X;
    int Y;
    char[][] dMap;
    char wall;

    public DungeonMap(int X, int Y)
	{
        this.X = X;
        this.Y = Y;
        dMap = new char[Y][];
        this.wall = 'x';
        fillMap(wall);
        generateRooms();
	}

    private void fillMap(char wall)
    {
        for(int i = 0; i< Y; i++)
        {
            char[] arr2 = new char[X];
            for(int j = 0; j< X; j++)
            {
                arr2[j] = wall;
            }
            dMap[i] = arr2;
        }
    }

    private void generateRooms()
    {
        //map of valid starting points
        Dictionary<String, int> validStarts = new Dictionary<string, int>();
        validStarts.Add("test", 0);

        //filling maps with valid start positions
        for(int i = 1; i< Y-1; i++)
        {
            for(int j = 1; j < X -1; j++)
            {
                if(dMap[i - 1][j - 1] == wall &&
                   dMap[i - 1][j] == wall &&
                   dMap[i - 1][j +1] == wall &&
                   dMap[i][j - 1] == wall &&
                   dMap[i][j + 1] == wall &&
                   dMap[i + 1][j - 1] == wall &&
                   dMap[i + 1][j] == wall &&
                   dMap[i + 1][j + 1] == wall)
                {
                    validStarts.Add(("{0},{i}", i, j), 1);
                }
            }
        }
        var val = validStarts.Keys.ToList();
        foreach(var key in val)
        {
            Console.WriteLine(key);
        }
        
    }

    public void print()
    {
        /*for(int i = 0; i< Y; i++)
        {
            for(int j = 0; j< X; j++)
            {
                Console.Write("{0} ", dMap[i][j]);
            }
            Console.WriteLine();
        }*/
        Dictionary<string, int> d = new Dictionary<string, int>();

        d.Add("keyboard", 1);
        d.Add("mouse", 2);

        var vald = d.Keys.ToList();

        foreach (var key in vald)
        {
            Console.WriteLine(key + "Zadek");
        }

        /*Dictionary<String, int> validStarts = new Dictionary<string, int>();
        validStarts.Add("test", 0);
        var val = validStarts.Keys.ToList();
        foreach (var key in val)
        {
            Console.WriteLine(key + "Zadek");
        }*/

    }


}
