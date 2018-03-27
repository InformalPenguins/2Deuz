using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins
{
    public class BonfireController : MonoBehaviour
    {
        public float FiredTimeLimit = 15f;
        private float _TimeFired = 0;

        private bool _isTurnedOn = false;
        private Animator _animator;
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            float ellapsedTime = Time.time - _TimeFired;
            if (ellapsedTime >= FiredTimeLimit) {
                TurnOff();
            }
        }
        public void AddFire()
        {
            _TimeFired = Time.time;
            if (_isTurnedOn) {
                return;
            }
            _isTurnedOn = true;
            checkAnimation();
        }
        public void TurnOff()
        {
            if (!_isTurnedOn)
            {
                return;
            }
            _isTurnedOn = false;
            checkAnimation();
        }

        private void checkAnimation()
        {
            _animator.SetBool(Constants.TRIGGER_FLAME, _isTurnedOn);
            BonfireChangedMessage msg = new BonfireChangedMessage(_isTurnedOn);
            GameManager.Messenger.QueueMessage(msg);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_ARROW)
            {
                if (collision.gameObject.GetComponentInParent<ArrowController>().HasFire)
                {
                    //collision.gameObject.SendMessageUpwards("InteractWithFlammable", gameObject);
                    AddFire();
                }
            }
        }
    }
}
