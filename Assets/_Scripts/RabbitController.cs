using BeautifulTransitions.Scripts.Shake.Components;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using UnityEngine;

namespace InformalPenguins
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RabbitController : MonoBehaviour
    {
        public float MovementSpeed = 1f;
        public float AccFactor = 3.5f;
        public float RequiredSpeedForJump = 5f;
        public int MaxWallJump = 3;
        public float HarmDelay = 0.5f;

        private float _SpeedDelay = 1f; //How long to wait until turbo is re enabled.
        private float _accFactor = 1;
        private float _lastHarm = 0f;
        private Collider2D[] _colliders;
        private Rigidbody2D _MyRigidbody;
        private bool _isJumping = false;
        private int _jumpedWalls = 0;
        private float _ySpeed = 0, _xSpeed = 0;
        private float _lastRunButtonHeld = 0;

        void Start()
        {
            _MyRigidbody = GetComponent<Rigidbody2D>();
            _colliders = GetComponents<Collider2D>();
            _jumpedWalls = MaxWallJump;
        }
        
        private void ResetRunning()
        {
            //Debug.Log("Reset Running Speed");
            _lastRunButtonHeld = 0;
        }

        void Update()
        {
            if (!LevelManager.Instance.IsLevelRunning)
            {
                Stop();
            }
        }
        public void Accelerate(bool isAccelerating) {
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
                _accFactor = Constants.MAX_SPEED;
            }
        }
        public void Break()
        {
            _accFactor = 1;
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
            if (Vector2.zero.Equals(newVelocity)) {
                ResetRunning();
            }
            _MyRigidbody.velocity = newVelocity;
        }
        private bool IsMovementChangeAllowed() {
            if (_isJumping)
            {
                return false;
            }
            return true;
        }
        public void Stop()
        {
            _MyRigidbody.velocity = Vector2.zero;
        }

        public void Jump()
        {
            float velocityScalar = _MyRigidbody.velocity.magnitude;
            if(velocityScalar >= RequiredSpeedForJump)
            {
                ResetRunning();
                SetCollidersActive(false);
            }
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.gameObject.tag == Constants.TAG_WALKABLE)
        //    {
        //        _lastFloor = collision.gameObject;
        //    }

        //    //if (_isJumping)
        //    //{
        //    //    checkJumpTrigger(collision.gameObject);
        //    //}
        //}
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_WALL)
            {
                ResetRunning();
            }
        }
        private void SetCollidersActive(bool enabled)
        {
            _isJumping = !enabled;
            foreach (Collider2D collider in _colliders)
            {
                if (!collider.isTrigger)
                {
                    collider.enabled = enabled;
                }
            }

        }
        //private void checkJumpTrigger(GameObject obj) {
        //    if (obj.tag == Constants.TAG_WALKABLE)
        //    {
        //        //Landing in a floor
        //        SetCollidersActive(true);
        //        // Stop();
        //    } else if (obj.tag == Constants.TAG_WALL) {
        //        _jumpedWalls--;
        //        if (_jumpedWalls <= 0)
        //        {
        //            SetCollidersActive(true);
        //            Stop();
        //            GoTo(_lastFloor);
        //            //Damage?
        //            _jumpedWalls = MaxWallJump;
        //        }
        //    }
        //}
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

            _lastHarm = Time.time;
            GameManager.Instance.Player.Lives -= 1;
            GetComponent<ShakeCamera>().Shake();

            _accFactor = .5f;
        }
    }
}
