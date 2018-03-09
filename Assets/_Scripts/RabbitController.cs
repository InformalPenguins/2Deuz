using UnityEngine;

namespace InformalPenguins
{
    public class RabbitController : MonoBehaviour
    {
        public float speed = 1f;
        private float DelayTimer = 0F;
        public float InputDelay = .05F;

        void Start()
        {
        }

        void Update()
        {
            //TODO Move to control
            float horizontalInput = Input.GetAxis(Constants.INPUT_HORIZONTAL);
            float verticalInput = Input.GetAxis(Constants.INPUT_VERTICAL);
            if (Time.time > DelayTimer)
            {
                if (Input.GetButton(Constants.INPUT_HORIZONTAL))
                {
                    MoveHorizontal(horizontalInput > 0);
                    DelayTimer = Time.time + InputDelay;
                }
                if (Input.GetButton(Constants.INPUT_VERTICAL))
                {
                    MoveVertical(verticalInput > 0);
                    DelayTimer = Time.time + InputDelay;
                }
            }
        }

        public void MoveVertical(bool up)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (up ? speed : -speed), transform.position.z);
        }
        public void MoveHorizontal(bool right)
        {
            transform.position = new Vector3(transform.position.x + (right ? speed : -speed), transform.position.y, transform.position.z);
        }
    }
}