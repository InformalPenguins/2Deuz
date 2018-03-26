using UnityEngine;
using UnityEngine.Assertions;

namespace InformalPenguins
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class SightCollider : MonoBehaviour
    {
        ArcherAI archerAI;

        private void Start()
        {
            archerAI = GetComponentInParent<ArcherAI>();
            Assert.IsNotNull(archerAI);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject collisionObj = collision.gameObject;

            if (collisionObj.tag == Constants.TAG_PLAYER)
            {
                if (archerAI != null)
                {
                    //This Null check is to fix the NPE thrown when the game starts and the trigger acts before the Start function.
                    archerAI.DetectPlayer(collisionObj);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            GameObject collisionObj = collision.gameObject;

            if (collisionObj.tag == Constants.TAG_PLAYER)
            {
                archerAI.DetectPlayer(null);
            }
        }
    }
}