using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using GameFramework.Messaging;
using System;
using UnityEngine;

namespace InformalPenguins
{
    //The INGAME Input Manager, should rename?
    public class InputManager : MonoBehaviour
    {
        private RabbitController _rabbitController;
        void Start()
        {
            GameManager.Messenger.AddListener<RabbitAddedMessage>(OnRabbitAddedMessage);
            GameObject rabbitObject = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER);
            if (rabbitObject != null) {
                _rabbitController = rabbitObject.GetComponent<RabbitController>();
            }
        }
        // Unsubscribe from messages 
        void OnDestroy()
        {
            if (GameManager.Messenger != null) { 
                GameManager.Messenger.RemoveListener<RabbitAddedMessage>(OnRabbitAddedMessage);
            }
        }

        public bool OnRabbitAddedMessage(BaseMessage message)
        {
            RabbitAddedMessage rabbitMessage = message as RabbitAddedMessage;
            GameObject rabbitGameObject = rabbitMessage.rabbit;
            _rabbitController = rabbitGameObject.GetComponent<RabbitController>();
            return _rabbitController != null;
        }

        void Update()
        {
            //Rabbit Context
            if (_rabbitController != null && LevelManager.Instance.IsLevelRunning) {
                RabbitUpdate();
            }
#if UNITY_EDITOR
          //  detectPressedKeyOrButton();
#endif
        }

        public void detectPressedKeyOrButton()
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    Debug.Log("KeyCode down: " + kcode);
            }
        }

        private void checkWalking()
        {
            if (Input.GetButton(Constants.INPUT_HORIZONTAL))
            {
                float horizontalInput = Input.GetAxis(Constants.INPUT_HORIZONTAL);
                _rabbitController.MoveHorizontal(horizontalInput);
            }
            else
            {
                _rabbitController.MoveHorizontal(0);
            }

            if (Input.GetButton(Constants.INPUT_VERTICAL))
            {
                float verticalInput = Input.GetAxis(Constants.INPUT_VERTICAL);
                _rabbitController.MoveVertical(verticalInput);
            }
            else
            {
                _rabbitController.MoveVertical(0);
            }
        }

        void checkRunning()
        {
            _rabbitController.Accelerate(Input.GetButton(Constants.INPUT_RUN));
        }
        private void checkJumping()
        {
            if (Input.GetButtonDown(Constants.INPUT_JUMP))
            {
                _rabbitController.Jump();
            }
        }

        private void checkPause() {

            if (Input.GetButtonDown(Constants.INPUT_PAUSE))
            {
                LevelManager.Instance.PauseLevel(true);
            }
        }
        void RabbitUpdate()
        {
            checkPause();
            checkWalking();
            //checkJumping();
            checkRunning();
        }
    }

}