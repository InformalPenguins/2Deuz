using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using GameFramework.Messaging;
using System;
using UnityEngine;

namespace InformalPenguins
{
    //The INGAME Input Manager, should rename?
    public class MobileInputManager : MonoBehaviour
    {
        private RabbitController _rabbitController;


        private Vector3 fp;   //First touch position
        private Vector3 lp;   //Last touch position
        [RangeAttribute(0, 1)]
        public float dragDistanceFactor = 0.05f;  //minimum distance for a swipe to be registered
        private float dragDistance;  //minimum distance for a swipe to be registered

        private Boolean isHorizontal = false;
        private Boolean isVertical = false;
        private Boolean isPressed = false;
        private Boolean isUp = false;
        private Boolean isRight = false;
        private Boolean isRunning = false;
        private float lastDifH = 0;
        private  float lastDifV = 0;
        private  Vector3 lastReferencePoint;

        void Start()
        {
            GameManager.Messenger.AddListener<RabbitAddedMessage>(OnRabbitAddedMessage);
            GameObject rabbitObject = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER);
            if (rabbitObject != null) {
                _rabbitController = rabbitObject.GetComponent<RabbitController>();
            }

            dragDistance = Screen.height * dragDistanceFactor; //dragDistance is 5% height of the screen
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
                MobileInputUpdate();

                RabbitUpdate();
            }
        }

        private void checkWalking()
        {
            if (isHorizontal)
            {
                //float horizontalInput = Input.GetAxis(Constants.INPUT_HORIZONTAL);
                _rabbitController.MoveHorizontal(isRight ? 1:-1);
            }
            else
            {
                _rabbitController.MoveHorizontal(0);
            }

            if (isVertical)
            {
                //float verticalInput = Input.GetAxis(Constants.INPUT_VERTICAL);
                _rabbitController.MoveVertical(isUp ? 1:-1);
            }
            else
            {
                _rabbitController.MoveVertical(0);
            }
        }

        void checkRunning()
        {
            _rabbitController.Accelerate(isRunning);
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

        
        private void resetMoveFlags(){
            isHorizontal = false;
            isPressed = false;
            isVertical = false;
            isUp = false;
            isRight = false;
            isRunning = false; //use drag distance for this.
        }
        // Este men: @MD_Reptile
        // https://forum.unity.com/threads/simple-swipe-and-tap-mobile-input.376160/
        void MobileInputUpdate()
        {
            if (Input.touchCount >= 1)
            {
                // if (Input.touchCount > 1) // user is touching the screen with a single touch
                // {
                //     isRunning = true;
                // }

                isPressed = true;
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;


                    float currentDifH = Mathf.Abs(lp.x - fp.x);
                    float currentDifV = Mathf.Abs(lp.y - fp.y);

                    if(isHorizontal){
                        if(currentDifH < lastDifH - 1){ //initial swipe right and started going to the left
                            fp = lastReferencePoint;
                            isRight = !isRight;
                        }
                    }
                    if(isVertical){
                        if(currentDifV < lastDifV - 1){ //initial swipe right and started going to the left
                            fp = lastReferencePoint;
                            isUp = !isUp;
                        }
                    }
                    // difH = currentDifH;
                    // difV = currentDifV;
                    // referencePoint = touch.position;
                    // float currentDiff Mathf.Abs(lp.x - fp.x)

                    //Check if drag distance is greater than dragDistance a % of the screen height
                    if (currentDifH > dragDistance || currentDifV > dragDistance)
                    {
                        //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lp.x - fp.x) > dragDistance)
                        {
                            //If the horizontal movement is greater than the vertical movement...
                            //isVertical = false;
                            isHorizontal = true;
                            if ((lp.x > fp.x))  //If the movement was to the right)
                            {   //Right swipe
                                Debug.Log("Right Swipe");
                                isRight = true;
                            }
                            else
                            {   //Left swipe
                                Debug.Log("Left Swipe");
                                isRight = false;
                            }
                        } else {
                            isHorizontal = false;
                        }

                        if(Mathf.Abs(lp.y - fp.y) > dragDistance)
                        {   //the vertical movement is greater than the horizontal movement
                                isVertical = true;
                                //isHorizontal = false;
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                Debug.Log("Up Swipe");
                                isUp = true;
                            }
                            else
                            {   //Down swipe
                                Debug.Log("Down Swipe");
                                isUp = false;
                            }
                        } else {
                            isVertical = false;
                        }

                        
                        lastDifH = currentDifH;
                        lastDifV = currentDifV;
                        lastReferencePoint = touch.position;
                    }
                    else
                    {   //It's a tap as the drag distance is less than 20% of the screen height
                        isVertical = false;
                        isHorizontal = false;
                        lastDifH = 0;
                        lastDifV = 0;
                    }
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    resetMoveFlags();
                }
            }
        }

    }

}