using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins
{
    public class DirtyLevelLoader : MonoBehaviour
    {
        public GameObject[] worlds;

        void Start()
        {
            int idx = GameManager.Instance.Worlds.Selected.Number - 1;
            worlds[idx].SetActive(true);
        }
    }
}
