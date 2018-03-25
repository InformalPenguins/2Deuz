using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using GameFramework.Messaging;
using UnityEngine;

namespace InformalPenguins
{
    public class Level4GameLoop : MonoBehaviour
    {
        void Start()
        {
            GameManager.Messenger.AddListener<ArcherDefeatedMessage>(ArcherDefeated);
        }
        void OnDestroy()
        {
            if (GameManager.Messenger != null)
            {
                GameManager.Messenger.RemoveListener<ArcherDefeatedMessage>(ArcherDefeated);
            }
        }

        public bool ArcherDefeated(BaseMessage message)
        {
            GameManager.Instance.Levels.Selected.Score += 100;
            return true;
        }
    }
}
