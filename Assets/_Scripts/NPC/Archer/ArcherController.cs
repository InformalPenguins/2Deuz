using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using UnityEngine.Assertions;

namespace InformalPenguins
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ArcherController : MonoBehaviour
    {
        public GameObject arrowPrefab;
        private Transform _arrowsTransform;

        public float CurrentHP = 1;

        public float MovSpeed = .7f;
        public float WalkSpeed = .15f;
        public float FireDelay = 0.1f;
        public float HarmDelay = 1f;

        private Rigidbody2D _MyRigidbody;
        //private Vector3 _initialPoint; //For wandering
        private float _lastFire = 0f;
        private float _lastHarm = 0f;

        public GameObject HpPrefab;
        HazardHpBarController hpController;
        public GameObject SightObject;

        public static readonly string _arrowsTransformName = "ArcherArrows";

        void Start()
        {
            //_initialPoint = transform.position;
            _MyRigidbody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(arrowPrefab);
            GameObject HpObj = Instantiate(HpPrefab, transform, false);
            hpController = HpObj.GetComponent<HazardHpBarController>();
            hpController.MaxHP = CurrentHP;
            hpController.CurrentHP = CurrentHP;
            SightObject.SetActive(true);

            initArrowsParent();
        }

        private void initArrowsParent()
        {
            GameObject arrowsParent = GameObject.Find(_arrowsTransformName);
            if (arrowsParent == null)
            {
                arrowsParent = new GameObject(_arrowsTransformName);
            }
            _arrowsTransform = arrowsParent.transform;
        }
        public void Harm()
        {
            if (Time.time - _lastHarm < HarmDelay)
            {
                return;
            }

            _lastHarm = Time.time;

            CurrentHP -= 0.334f; //Harm takes 1/2 of the HP
            //CurrentHP -= 1f; //TODO: DEBUG HP 
            hpController.CurrentHP = CurrentHP;
            if (CurrentHP <= 0)
            {
                ArcherDefeatedMessage msg = new ArcherDefeatedMessage(gameObject);
                GameManager.Messenger.QueueMessage(msg);
                Destroy(gameObject);
            }
        }

        public void Walk(Vector2 direction)
        {
            _MyRigidbody.velocity = direction * WalkSpeed;
        }

        public void LookAt(GameObject obj)
        {
            //transform.LookAt(obj.transform);
        }

        public void Fire(Vector3 targetPosition)
        {
            //Vector3 will NEVER be null (just Zero)
            //if (targetPosition == null)
            //{
            //    return;
            //}

            float currentTime = Time.time;

            if (currentTime - _lastFire < FireDelay)
            {
                return;
            }

            _lastFire = Time.time;
            GameObject arrow = Instantiate(arrowPrefab, transform.position, arrowPrefab.transform.rotation,  _arrowsTransform);

            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            if (arrow != null)
            {
                arrowController.SetTargetPosition(targetPosition);
            }
        }

        public void Run(Vector2 direction)
        {
            _MyRigidbody.velocity = direction * MovSpeed;
        }
    }
}
