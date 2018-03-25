using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using UnityEngine.Assertions;

namespace InformalPenguins
{
    [RequireComponent(typeof(ArcherController))]
    public class ArcherAI : MonoBehaviour
    {
        //------------- AI core ----------------
        //Triggers 
        public bool isPlayerNearTrigger = false;
        public bool isPlayerInSightTrigger = false;
        public bool isPlayerInShootDistanceTrigger = false;
        //States
        private bool statePassive = false;
        private bool stateAggressive = false;
        private bool stateDefensive = false;
        //--------------------------------------

        private ArcherController _archerController;
        private GameObject _targetPlayer;
        public float PlayerDistThreshold = 1f;
        public float ShootDistThreshold = 3f;
        
        void Start()
        {
            statePassive = true;
            _archerController = GetComponent<ArcherController>();
            Assert.IsNotNull(_archerController);
        }
        void StateUpdate() {
            if (isPlayerNearTrigger)
            {
                stateAggressive = false;
                statePassive = false;
                stateDefensive = true;
            }
            else if (isPlayerInSightTrigger)
            {
                stateAggressive = true;
                statePassive = false;
                stateDefensive = false;
            }
            else
            {
                stateAggressive = false;
                statePassive = true;
                stateDefensive = false;
            }
        }

        // Update is called once per frame
        void TriggerUpdate()
        {
            if (stateDefensive)
            {
                Defensive();
            }
            else if (stateAggressive) {
                Aggressive();
            }
            else if (statePassive)
            {
                Passive();
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (LevelManager.Instance.IsLevelRunning)
            {
                StateUpdate();

                TriggerUpdate();
            }
        }
        private void LateUpdate()
        {
            checkPlayerDistance(_targetPlayer);
        }
        public void DetectPlayer(GameObject player)
        {
            _targetPlayer = player;
            if (player == null)
            {
                isPlayerNearTrigger = false;
                isPlayerInSightTrigger = false;
                isPlayerInShootDistanceTrigger = false;
                return;
            }

            isPlayerInSightTrigger = true;
        }

        public void checkPlayerDistance(GameObject player)
        {
            if (player == null)
            {
                isPlayerNearTrigger = false;
                isPlayerInSightTrigger = false;
                isPlayerInShootDistanceTrigger = false;
                return;
            }

            float dist = Vector3.Distance(player.transform.position, transform.position);
            isPlayerNearTrigger = dist < PlayerDistThreshold;
            isPlayerInShootDistanceTrigger = dist < ShootDistThreshold;
        }

        //Possibilities

        //For wandering purposes
        void walk(Vector3 direction)
        {
            _archerController.Walk(direction);
        }
        //For flee purposes
        void run(Vector3 direction)
        {
            _archerController.Run(direction);
        }
        //Look at methods will register the target to get action into
        void lookAtPoint()
        {
            //_archerController.LookAt();
        }
        void lookAtPlayer(GameObject player)
        {
            _archerController.LookAt(player);
        }
        void Fire(Vector3 spot)
        {
            _archerController.Fire(spot);
        }

        //Goals States
        void Aggressive()
        {
            lookAtPlayer(_targetPlayer);
            if (isPlayerInSightTrigger) {
                if (!isPlayerInShootDistanceTrigger)
                {
                    chase();
                }
                else
                {
                    walk(Vector3.zero);
                    Fire(_targetPlayer.transform.position); //fire arrows to the spot the player is
                }
            }
        }
        private void chase()
        {
            if (_targetPlayer != null)
            {
                Vector3 direction = _targetPlayer.transform.position - transform.position;
                //Debug.Log(direction);
                Debug.Log(direction.normalized);
                walk(direction.normalized); //run towards the flee point
            }
        }

        void Passive()
        {
            //Wander
            //if (!isPlayerInSightTrigger)
            //{
            lookAtPoint();//localize a spot to walk to
            //If no target is set for wandering, then stay there.
            walk(Vector3.zero); //walk to the next spot
            //}
        }

        void Defensive()
        {
            //FLEE
            if (isPlayerNearTrigger)
            {
                //lookAtPoint(); //localize flee point
                if (_targetPlayer != null)
                {
                    lookAtPlayer(_targetPlayer); //localize player
                    Vector3 direction = transform.position - _targetPlayer.transform.position;
                    //Debug.Log(direction.normalized);
                    run(direction.normalized); //run towards the flee point
                }
            }
        }
    }
}