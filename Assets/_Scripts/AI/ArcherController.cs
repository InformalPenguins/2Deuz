using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using UnityEngine;

namespace InformalPenguins
{
    public class ArcherController : MonoBehaviour
    {
        public int Lives = 3;
        public int MovSpeed = 3;
        private float _lastHarm = 0f;
        public float _HarmDelay = 0.5f;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (LevelManager.Instance.IsLevelRunning)
            {
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject collisionObj = collision.gameObject;
            if (collisionObj.tag == Constants.TAG_HAZARD) {
                Harm();
            }
        }


        public void Harm()
        {
            if (Time.time - _lastHarm < _HarmDelay)
            {
                return;
            }

            _lastHarm = Time.time;

            Lives -= 1;
            if (Lives <= 0)
            {
                ArcherDefeatedMessage msg = new ArcherDefeatedMessage(gameObject);
                GameManager.Messenger.TriggerMessage(msg);
                Destroy(gameObject);
            }
        }

        //Triggers 
        bool isPlayerNear = false;
        bool isPlayerLocated = false;
        //States
        bool isWandering = false;
        //Possibilities
        void walk()
        {
        }
        void run()
        {
        }
        void lookAtPoint()
        {
        }
        void shoot()
        {
        }
        //Goals States
        void AttackPlayer()
        {
            if (isPlayerLocated) {
                shoot();
            }
        }

        void Flee()
        {
            //If Rabbit is near, move back.
            //Dynamic is that this can get harmed by the flame.
        }

    }
}