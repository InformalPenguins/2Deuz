using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins
{
    public class LevelLoader : MonoBehaviour
    {
        private const string LEVEL_BUTTON_PREFIX = "LEVEL_";
        public GameObject LevelButtonPrefab;

        public void LoadWorld(int worldId)
        {
            LevelMapper _instance = LevelMapper.get();
            WorldMapInfo selectedWorld = null;
            foreach (WorldMapInfo wi in _instance.worlds)
            {
                if (worldId == wi.id)
                {
                    selectedWorld = wi;
                }
            }

            if (selectedWorld != null)
            {
                foreach (LevelMapInfo levelMapInfo in selectedWorld.levels)
                {

                    GameObject levelButton = Instantiate(LevelButtonPrefab, transform, true);
                    levelButton.transform.parent = transform;
                    levelButton.transform.name = LEVEL_BUTTON_PREFIX + levelMapInfo.id;
                    levelButton.transform.position = new Vector3(levelMapInfo.point.x, levelMapInfo.point.y, transform.position.z);
                }
            }
        }

        void Start()
        {
            Init();
        }
        void Init()
        {
            int currentWorld = GameManager.Instance.Worlds.Selected.Number;
            Debug.Log("CURRENT WORLD: " + currentWorld);
            LoadWorld(currentWorld);
        }

    }
}
