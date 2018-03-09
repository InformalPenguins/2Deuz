using GameFramework.GameStructure.Levels;
using UnityEngine;

namespace InformalPenguins
{
    public class ExitCollider : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("OnCollisionEnter2D " + collision.transform.name);
            if (Constants.TAG_PLAYER.Equals(collision.transform.tag))
            {
                LevelManager.Instance.GameOver(true);
            }
        }
    }
}
