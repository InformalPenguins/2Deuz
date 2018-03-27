using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins {

    [RequireComponent(typeof(AudioClip))]
    public class FlameHitbox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_PLAYER || collision.gameObject.tag == Constants.TAG_HAZARD_FF)
            {
                SendMessageUpwards("TargetCollided");
                collision.gameObject.SendMessageUpwards("Harm");
            }
            else if (collision.gameObject.tag == Constants.TAG_ARROW) {
                //ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
                //ac.InteractWithHazard(gameObject);

                Constants.CellType hazardType = gameObject.GetComponentInParent<CellController>().CellType;
                collision.gameObject.SendMessageUpwards("InteractWithHazard", hazardType);
            }
        }
    }
}
