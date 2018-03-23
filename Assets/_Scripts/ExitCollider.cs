using GameFramework.GameStructure.Levels;
using UnityEngine;

namespace InformalPenguins
{
    public class ExitCollider : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (LevelManager.Instance.IsLevelRunning && Constants.TAG_PLAYER.Equals(collision.transform.tag))
            {
                if (LevelManager.Instance.Level.TimeTarget == 0) {
                    LevelManager.Instance.Level.Score += 10;
                    LevelManager.Instance.Level.Score += (int)(Constants.LIMIT_TIME/2 - LevelManager.Instance.SecondsRunning*2);
                }

                RabbitController rabbitController = collision.gameObject.GetComponent<RabbitController>();
                rabbitController.GoTo(gameObject);
                LevelManager.Instance.GameOver(true);

            }
        }
    }
}
