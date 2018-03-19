using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using GameFramework.Messaging;
using UnityEngine;

namespace InformalPenguins
{
    public class InputManager : MonoBehaviour
    {
        private float rabbitMoveDelayTimer = 0F;
        public float rabbitMoveDelay = .05F;
        RabbitController rabbitController;

        void Start()
        {
            GameManager.Messenger.AddListener<RabbitAddedMessage>(OnRabbitAddedMessage);
            GameObject rabbitObject = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER);
            if (rabbitObject != null) {
                rabbitController = rabbitObject.GetComponent<RabbitController>();
            }
        }
        /*
        // Unsubscribe from messages 
        void OnDestroy()
        {
            GameManager.Messenger.RemoveListener<RabbitAddedMessage>(OnRabbitAddedMessage);
        }*/

        public bool OnRabbitAddedMessage(BaseMessage message)
        {
            RabbitAddedMessage rabbitMessage = message as RabbitAddedMessage;
            GameObject rabbitGameObject = rabbitMessage.rabbit;
            rabbitController = rabbitGameObject.GetComponent<RabbitController>();
            return rabbitController != null;
        }

        void Update()
        {
            //Rabbit Context
            if (rabbitController != null && LevelManager.Instance.IsLevelRunning) {
                RabbitUpdate();
            }
        }
        void RabbitUpdate() {
            //bool isMoving = false;
            if (Time.time > rabbitMoveDelayTimer)
            {
                if (Input.GetButton(Constants.INPUT_HORIZONTAL))
                {
                   // isMoving = true;
                    float horizontalInput = Input.GetAxis(Constants.INPUT_HORIZONTAL);
                    rabbitController.MoveHorizontal(horizontalInput);
                    rabbitMoveDelayTimer = Time.time + rabbitMoveDelay;
                } else {
                    rabbitController.MoveHorizontal(0);
                }

                if (Input.GetButton(Constants.INPUT_VERTICAL))
                {
                  //  isMoving = true;
                    float verticalInput = Input.GetAxis(Constants.INPUT_VERTICAL);
                    rabbitController.MoveVertical(verticalInput);
                    rabbitMoveDelayTimer = Time.time + rabbitMoveDelay;
                } else {
                    rabbitController.MoveVertical(0);
                }
                /*
                if (!isMoving) {
                    //No Movements: 
                    rabbitController.Stop();
                }
                */
            }
        }
    }

}