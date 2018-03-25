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
                    int extra = (int)(Constants.LIMIT_TIME - LevelManager.Instance.SecondsRunning * 2);
                    if (extra <= 0) {
                        extra = 0;
                    }
                    LevelManager.Instance.Level.Score += extra;
                }

                RabbitController rabbitController = collision.gameObject.GetComponent<RabbitController>();
                rabbitController.GoTo(gameObject);
                LevelManager.Instance.GameOver(true);

            }
        }
    }
}
