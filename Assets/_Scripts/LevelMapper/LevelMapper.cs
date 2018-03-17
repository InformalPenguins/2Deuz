using UnityEngine;

namespace InformalPenguins
{
    [System.Serializable]
    public class LevelMapper
    {
        private static LevelMapper _instance;
        private const string LEVEL_MAP_INFO_JSON = "Assets/Resources/LevelMap.json";

        public WorldMapInfo[] worlds;

        private static void CreateFromJSON()
        {
            _instance = JsonUtility.FromJson<LevelMapper>(FileUtility.readFile(LEVEL_MAP_INFO_JSON));
        }
        public static LevelMapper get()
        {
            if (_instance == null)
            {
                CreateFromJSON();
            }
            return _instance;
        }
        public string toJson()
        {
            return JsonUtility.ToJson(_instance);
        }
    }
}