using UnityEngine;
using UnityEngine.Assertions;


namespace InformalPenguins
{
    [RequireComponent(typeof(Collider2D))]
    public class ArcherHitbox : MonoBehaviour
    {
        ArcherController archer;
        private void Start()
        {
            archer = GetComponentInParent<ArcherController>();
            Assert.IsNotNull(archer);
        }
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    GameObject collisionObj = collision.gameObject;
        //    if (collisionObj.tag == Constants.TAG_HAZARD)
        //    {
        //        archer.Harm();
        //    }
        //}
    }
}
