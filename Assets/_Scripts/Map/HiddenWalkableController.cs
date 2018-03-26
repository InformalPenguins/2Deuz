using UnityEngine;

namespace InformalPenguins
{
    public class HiddenWalkableController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_PLAYER)
            {
                collision.gameObject.GetComponent<RabbitController>().ShowEars(1);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_PLAYER)
            {
                collision.gameObject.GetComponent<RabbitController>().ShowEars(-1);
            }
        }
    }
}