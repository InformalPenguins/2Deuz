using System;
using UnityEngine;

namespace InformalPenguins
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class ArrowController : MonoBehaviour
    {
        public float Speed = 2;
        public float LifeSpan = 2;

        [NonSerialized]
        public bool HasFire = false;

        private float _createdTime;
        private Vector3 _targetPosition;
        private Vector3 _direction;
        private bool _arrived = false;
        private Rigidbody2D _MyRigidbody;

        //Animator
        private Animator _animator;
        //End Animator

        void Start()
        {
            _MyRigidbody = GetComponent<Rigidbody2D>();
            _createdTime = Time.time;
            _animator = GetComponent<Animator>();
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
            string tag = collision.gameObject.tag;
            CellController cellController = null;
            switch (tag)
            {
                case Constants.TAG_PLAYER:
                    collision.gameObject.SendMessage("Harm");
                    break;
                case Constants.TAG_HAZARD:
                    cellController = collision.gameObject.GetComponentInParent<CellController>();
                    interactWithHazard(cellController.cellType);
                    break;
                case Constants.TAG_FLAMMABLE:
                    if (HasFire) { 
                        cellController = collision.gameObject.GetComponentInParent<CellController>();
                        interactWithFlammable(collision.gameObject, cellController.cellType);
                    }
                    break;
            }
        }
        private void interactWithFlammable(GameObject flammable, Constants.CellType flammableType)
        {
            switch (flammableType)
            {
                case Constants.CellType.BONFIRE:
                    flammable.SendMessage("AddFire");
                    break;
            }
        }
        private void interactWithHazard(Constants.CellType hazardType)
        {
            switch (hazardType) {
                case Constants.CellType.FLAME:
                    AddFire();
                    break;
            }
        }
        private void AddFire() {
            HasFire = true;
            _animator.SetTrigger(Constants.TRIGGER_FLAME);
        }
    }
}
