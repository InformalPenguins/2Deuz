using UnityEngine;

namespace InformalPenguins
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class ArrowController : MonoBehaviour
    {
        public float Speed = 2;
        public float LifeSpan = 2;
        private float _createdTime;
        private Vector3 _targetPosition;
        private Vector3 _direction;
        private bool _arrived = false;
        private Rigidbody2D _MyRigidbody;
        void Start()
        {
            _MyRigidbody = GetComponent<Rigidbody2D>();
            _createdTime = Time.time;
        }
        void Update()
        {
            if (Time.time > _createdTime + LifeSpan)
            {
                Destroy(gameObject);
            }
            if (!_arrived) { 
                updateMove();
            }
        }
        private void updateMove()
        {
            if (_targetPosition == null)
            {
                return;
            }
            float dist = Vector3.Distance(_targetPosition, transform.position);
            if (dist <= 0.1f) {
                _arrived = true;
                return;
            }

            _MyRigidbody.velocity = _direction * Speed;
        }
        public void SetTargetPosition(Vector3 position)
        {
            //For some weird reason setting velocity here is not working.
            //_MyRigidbody.AddForce(direction.normalized);
            _targetPosition = position;
            _direction = (_targetPosition - transform.position).normalized;
            transform.up = _direction;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_PLAYER)
            {
                collision.gameObject.SendMessage("Harm");
            }
        }
    }
}
