using GameFramework.GameStructure.Levels;
using GameFramework.GameStructure.Levels.ObjectModel;
using System;
using UnityEngine;
namespace InformalPenguins
{
    public class ExitCollider : MonoBehaviour
    {
        [NonSerialized]
        public int LimitTime;
        public Constants.CarrotsType winType;

        private void Start()
        {
            LimitTime = Constants.LIMIT_TIME;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (LevelManager.Instance.IsLevelRunning && Constants.TAG_PLAYER.Equals(collision.transform.tag))
            {
                //if (LevelManager.Instance.Level.TimeTarget == 0) {
                LevelManager.Instance.Level.Score += 10;
                int extra = (int)(Constants.LIMIT_TIME - LevelManager.Instance.SecondsRunning * 2);
                if (extra <= 0) {
                    extra = 0;
                }

                LevelManager.Instance.Level.Score += extra;
                switch (MapGenerator.getInstance().WinCarrotsType)
                {
                    case Constants.CarrotsType.TIME:
                        checkCarrotsByTime(LevelManager.Instance.SecondsRunning);
                        break;
                    case Constants.CarrotsType.SCORE:
                        checkCarrotsByScore(LevelManager.Instance.Level.Score);
                        break;
                }
                //}

                RabbitController rabbitController = collision.gameObject.GetComponent<RabbitController>();
                rabbitController.GoTo(gameObject);
                LevelManager.Instance.GameOver(true);
            }
        }


        private void checkCarrotsByTime(float currentValue)
        {
            //Target Score being 0 or -1 is ignored, means the star should be get somehow different.
            Level lv = LevelManager.Instance.Level;
            if (lv.Star1Target > 0 && lv.Star1Target > currentValue)
            {
                lv.StarWon(1, true);
            }

            if (lv.Star2Target > 0 && lv.Star2Target > currentValue)
            {
                lv.StarWon(2, true);
            }

            if (lv.Star3Target > 0 && lv.Star3Target > currentValue)
            {
                lv.StarWon(3, true);
            }
        }


        private void checkCarrotsByScore(float currentValue)
        {
            //Target Score being 0 or -1 is ignored, means the star should be get somehow different.
            Level lv = LevelManager.Instance.Level;
            if (lv.Star1Target > 0 && lv.Star1Target <= currentValue)
            {
                lv.StarWon(1, true);
            }

            if (lv.Star2Target > 0 && lv.Star2Target <= currentValue)
            {
                lv.StarWon(2, true);
            }

            if (lv.Star3Target > 0 && lv.Star3Target <= currentValue)
            {
                lv.StarWon(3, true);
            }
        }
    }
}
