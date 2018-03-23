using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins
{
    [RequireComponent(typeof(AudioClip))]
    public class FlameCollider : MonoBehaviour
    {
        public AudioClip sfx;
        private void Start()
        {
            sfx = GetComponent<AudioClip>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_PLAYER)
            {
                GameManager.Instance.PlayEffect(sfx);
                collision.GetComponent<RabbitController>().Harm();
            }
        }
    }
}