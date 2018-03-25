using UnityEngine;

namespace InformalPenguins
{
    public class FlameController : MonoBehaviour
    {
        private void Start()
        {
        }

        public void Extinguish() {
            Destroy(gameObject);
        }
    }
}