using GameFramework.GameStructure;
using GameFramework.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InformalPenguins
{
    public class LevelInfoPanel : MonoBehaviour
    {
        public InputField levelName;
        public InputField levelId;
        public InputField mapId;
        public Toggle hasBoss;

        // Use this for initialization
        void Start()
        {
            GameManager.Messenger.AddListener<LevelInfoLoadedMessage>(levelInfoAdded);
    
        }
        public bool levelInfoAdded(BaseMessage message)
        {
            LevelInfoLoadedMessage levelInfoLoadedMessage = message as LevelInfoLoadedMessage;

            LevelInfo levelInfo = levelInfoLoadedMessage.levelInfo;

            levelName.text = levelInfo.name;
            levelId.text = levelInfo.id.ToString();
            mapId.text = levelInfo.map.ToString();
            hasBoss.isOn = levelInfo.hasBoss;

            return true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}