using UnityEngine;

namespace InformalPenguins
{
    [System.Serializable]
    public class LevelStructure
    {
        private static LevelStructure _instance;
        private const string LEVELS_INFO_JSON = "Assets/Resources/LevelStructure.json";

        public WorldInfo[] worlds;

        private static void CreateFromJSON()
        {
            _instance = JsonUtility.FromJson<LevelStructure>(FileUtility.ReadFile(LEVELS_INFO_JSON));
        }
        public static LevelStructure get()
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