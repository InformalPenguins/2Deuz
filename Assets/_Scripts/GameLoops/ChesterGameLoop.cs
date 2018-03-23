using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins
{
    public class ChesterGameLoop : MonoBehaviour
    {
        private void Start()
        {
            switch (GameManager.Instance.Levels.Selected.Number)
            {
                case 4:
                    gameObject.AddComponent<Level4GameLoop>();
                    break;

            }
        }
    }
}
