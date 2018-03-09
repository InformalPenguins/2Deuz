using System;
using System.Text.RegularExpressions;

namespace InformalPenguins {
    public class RandomMapUtility
    {
        public static string[][] generateRandomMap() {
            return generateRandomMap(Constants.DEFAULT_WIDTH, Constants.DEFAULT_HEIGHT);
        }
        public static string[][] generateRandomMap(int w, int h)
        {
            Random r = new Random();

            string[][] mapArray = new string[h][];
            for (int i = 0; i < h; i++)
            {
                mapArray[i] = new string[w];
                for (int j = 0; j < w; j++)
                {
                    mapArray[i][j] = MapGenerator.MAP_WALL;

                    if (i == 0 || j == 0 || i == h - 1 || j == w - 1)
                    {
                        //Make all edges non walkable.
                        mapArray[i][j] = MapGenerator.MAP_WALL;
                    }
                    else if (r.Next(0, 2) == 0)
                    {
                        mapArray[i][j] = MapGenerator.MAP_FLOOR;
                    }
                }
            }

            int randomWidth, randomHeight;

            randomWidth = r.Next(1, w - 1);
            randomHeight = r.Next(1, h - 1);
            mapArray[randomHeight][randomWidth] = MapGenerator.MAP_START;

            randomWidth = r.Next(1, w - 1);
            randomHeight = r.Next(1, h - 1);
            mapArray[randomHeight][randomWidth] = MapGenerator.MAP_EXIT;

            randomWidth = r.Next(1, w - 1);
            randomHeight = r.Next(1, h - 1);
            mapArray[randomHeight][randomWidth] = MapGenerator.MAP_STAR_1;

            randomWidth = r.Next(1, w - 1);
            randomHeight = r.Next(1, h - 1);
            mapArray[randomHeight][randomWidth] = MapGenerator.MAP_STAR_2;

            randomWidth = r.Next(1, w - 1);
            randomHeight = r.Next(1, h - 1);
            mapArray[randomHeight][randomWidth] = MapGenerator.MAP_STAR_3;

            return mapArray;
        }
    }
}