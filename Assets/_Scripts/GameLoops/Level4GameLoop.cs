using GameFramework.GameStructure;
using GameFramework.Messaging;
using UnityEngine;
using UnityEngine.Assertions;

namespace InformalPenguins
{
    public class Level4GameLoop : MonoBehaviour
    {
        void Start()
        {
            MapGenerator mapGenerator = GetComponent<MapGenerator>();
            mapGenerator.WinCarrotsType = Constants.CarrotsType.SCORE;
            Assert.IsNotNull(mapGenerator);
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
            //Maybe this should be added immediatly instead of based on type of level
            GameManager.Instance.Levels.Selected.Score += 100;
            return true;
        }
    }
}
