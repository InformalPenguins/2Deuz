using GameFramework.GameStructure.Levels;
using UnityEngine;

namespace InformalPenguins
{
    public class RabbitController : MonoBehaviour
    {
        public float MovementSpeed = 1f;
        private float ySpeed = 0, xSpeed = 0;

        Rigidbody2D MyRigidbody;
        void Start()
        {
            MyRigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            //TODO: Maybe change this to a listener
            if (!LevelManager.Instance.IsLevelRunning) {
                Stop();
            }
        }
        public void MoveVertical(float vMov)
        {
            ySpeed = MovementSpeed * vMov;
            MyRigidbody.velocity = new Vector2(xSpeed, ySpeed);
        }
        public void MoveHorizontal(float hMov)
        {
            xSpeed = MovementSpeed * hMov;
            MyRigidbody.velocity = new Vector2(xSpeed, ySpeed);
        }

        public void Stop()
        {
            MyRigidbody.velocity = new Vector2(0, 0);
        }
    }
}