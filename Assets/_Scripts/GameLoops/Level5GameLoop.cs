using GameFramework.GameStructure;
using GameFramework.Messaging;
using UnityEngine;
using UnityEngine.Assertions;

namespace InformalPenguins
{
    public class Level5GameLoop : MonoBehaviour
    {
        private MapGenerator _mapGenerator;
        public int BonfireCounter = 5;
        void Start()
        {
            _mapGenerator = GetComponent<MapGenerator>();
            _mapGenerator.WinCarrotsType = Constants.CarrotsType.TIME;
            Assert.IsNotNull(_mapGenerator);
            GameManager.Messenger.AddListener<BonfireChangedMessage>(BonfireTurnedOn);
        }
        void OnDestroy()
        {
            if (GameManager.Messenger != null)
            {
                GameManager.Messenger.RemoveListener<BonfireChangedMessage>(BonfireTurnedOn);
            }
        }

        public bool BonfireTurnedOn(BaseMessage message)
        {
            BonfireChangedMessage msg = message as BonfireChangedMessage;
            if (msg.IsTurnedOn)
            {
                BonfireCounter--; //TODO: Increase and use counter vs 5 instead of 0
            }
            else {
                BonfireCounter++;
            }
            if (BonfireCounter <= 0) {
                _mapGenerator.DefeatBoss();
                GameManager.Instance.Levels.Selected.Score += 150 * 5;
            }
            return true;
        }
    }
}
