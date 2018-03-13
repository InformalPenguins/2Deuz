using UnityEngine;

namespace InformalPenguins { 
    [System.Serializable]
    public class LevelInfo
    {
        public int id;
        public string name;
        public string map;
        public StarInfo[] stars;
        public MapPoint exit;
        public MapPoint start;
        public bool hasBoss = false;
    }
}