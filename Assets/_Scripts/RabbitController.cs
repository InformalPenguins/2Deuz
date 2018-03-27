using BeautifulTransitions.Scripts.Shake.Components;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using UnityEngine.Assertions;

namespace InformalPenguins
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RabbitController : MonoBehaviour
    {
        public float MovementSpeed = 1f;
        public float AccFactor = 3.5f;
        public float HarmDelay = 0.5f;

        private float _SpeedDelay = 1f; //How long to wait until turbo is re enabled.
        private float _accFactor = 1;
        private float _lastHarm = 0f;
        private Rigidbody2D _MyRigidbody;
        private float _ySpeed = 0, _xSpeed = 0;
        private float _lastRunButtonHeld = 0;

        //Animation section
        private const string TRIGGER_HIDDEN = "IsHidden";
        private const string TRIGGER_DEAD = "Dead";
        private const string TRIGGER_WALK = "IsWalking";
        private const string TRIGGER_RUN = "IsRunning";
        private const string TRIGGER_HARM = "Harm";

        private int _isVisible = 1;
        private int _isHidden = 0;
        private Animator _animator = null;

        //private bool _facingRight = true;
        //End Animation
        void Start()
        {
            _MyRigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            Assert.IsNotNull(_animator);
        }

        private void ResetRunning()
        {
            //Debug.Log("Reset Running Speed");
            _animator.SetBool(TRIGGER_RUN, false);
            //_animator.SetBool(TRIGGER_WALK, false);
            _lastRunButtonHeld = 0;
        }

        void Update()
        {
            if (!LevelManager.Instance.IsLevelRunning)
            {
                Stop();
            }
        }
        public void Accelerate(bool isAccelerating)
        {
            float _runButtonHeld = 0;
            if (isAccelerating)
            {
                if (_lastRunButtonHeld == 0)
                {
                    _lastRunButtonHeld = Time.time;
                }

                _runButtonHeld = Time.time - _lastRunButtonHeld;
            }
            else
            {
                ResetRunning();
            }

            Accelerate(_runButtonHeld);
        }

        private void Accelerate(float acc)
        {
            if (Time.time - _lastHarm < _SpeedDelay)
            {
                return;
            }

            _accFactor = 1 + AccFactor * acc;
            if (_accFactor < 1)
            {
                _accFactor = 1;
            }
            else if (_accFactor > Constants.MAX_SPEED)
            {
                _animator.SetBool(TRIGGER_RUN, true);
                _accFactor = Constants.MAX_SPEED;
            }
        }
        public void Break()
        {
            _accFactor = 1;
            _animator.SetBool(TRIGGER_RUN, false);
        }
        public void MoveVertical(float vMov)
        {
            _ySpeed = MovementSpeed * vMov * _accFactor;
            SetVelocity();
        }
        public void MoveHorizontal(float hMov)
        {
            _xSpeed = MovementSpeed * hMov * _accFactor;
            SetVelocity();
        }
        private void SetVelocity()
        {
            if (!IsMovementChangeAllowed())
            {
                return;
            }
            Vector2 newVelocity = new Vector2(_xSpeed, _ySpeed);
            if (Constants.VECTOR_2_ZERO.Equals(newVelocity))
            {
                _animator.SetBool(TRIGGER_WALK, false);
                ResetRunning();
            }
            else
            {
                _animator.SetBool(TRIGGER_WALK, true);
            }
            _MyRigidbody.velocity = newVelocity;
        }
        private bool IsMovementChangeAllowed()
        {
            return true;
        }
        public void Stop()
        {
            _MyRigidbody.velocity = Constants.VECTOR_2_ZERO;
            //_animator.SetBool(TRIGGER_WALK, false);
            //_animator.SetBool(TRIGGER_RUN, false);
        }

        public void HideEars(int state)
        {
            _isVisible += state;
            CheckEars();
        }

        public void ShowEars(int state)
        {
            _isHidden += state;
            CheckEars();
        }

        private void CheckEars()
        {
            if (_animator != null)
            {
                _animator.SetBool(TRIGGER_HIDDEN, _isHidden > _isVisible);
            }
        }

        public void GoTo(GameObject obj)
        {
            ResetRunning();
            this.gameObject.transform.position = obj.transform.position;
        }

        public void Harm()
        {
            ResetRunning();
            if (Time.time - _lastHarm < HarmDelay)
            {
                return;
            }
            _animator.SetTrigger(TRIGGER_HARM);

            _lastHarm = Time.time;

            if (GameManager.Instance.Player.Lives == 1)
            {
                _animator.SetTrigger(TRIGGER_DEAD);
            }

            GameManager.Instance.Player.Lives -= 1;

            GetComponent<ShakeCamera>().Shake();

            _accFactor = .5f;
        }


        private void OnCollisionEnter(Collision2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_WALL)
            {
                ResetRunning();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_ARROW)
            {
                ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
                if (ac.CanHarm())
                {
                    Harm();
                }
            }
        }
    }
}
