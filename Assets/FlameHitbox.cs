using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins {

    [RequireComponent(typeof(AudioClip))]
    public class FlameHitbox : MonoBehaviour
    {
        public AudioClip sfx;
        private void Start()
        {
            sfx = GetComponent<AudioClip>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_PLAYER || collision.gameObject.tag == Constants.TAG_HAZARD_FF)
            {
                GameManager.Instance.PlayEffect(sfx);
                collision.gameObject.SendMessageUpwards("Harm");
            }
        }
    }
}
