using UnityEngine;

namespace InformalPenguins { 
    [System.Serializable]
    public class LevelStructure
    {
        private static LevelStructure _instance;
        private const string LEVELS_INFO_JSON = "Assets/Resources/LevelStructure.json";

        public WorldInfo[] worlds;

        private static void CreateFromJSON()
        {
            _instance = JsonUtility.FromJson<LevelStructure>(FileUtility.readFile(LEVELS_INFO_JSON));
        }
        public static LevelStructure get() {
            if (_instance == null) {
                CreateFromJSON();
            }
            return _instance;
        }
        public string toJson() {
            return JsonUtility.ToJson(_instance);
        }

        // Given JSON input:
        // {"name":"Dr Charles","lives":3,"health":0.8}
        // this example will return a PlayerInfo object with
        // name == "Dr Charles", lives == 3, and health == 0.8f.
    }
}