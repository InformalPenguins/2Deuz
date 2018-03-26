using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins
{
    public class FlameController : MonoBehaviour
    {
        public AudioClip sfx;
        public void Extinguish() {
            Destroy(gameObject);
        }
        public void TargetCollided() {
            GameManager.Instance.PlayEffect(sfx);
        }
    }
}