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
    }
}
