using System;

namespace InformalPenguins { 
    [Serializable]
    public class LevelInfo
    {
        public int id;
        public string name;
        public int map;
        public StarInfo[] stars;
        public MapPoint exit;
        public MapPoint start;
        public bool hasBoss = false;

        public CellInfo[] extras;
    }
}