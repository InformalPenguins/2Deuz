using GameFramework.GameStructure;
using GameFramework.UI.Other.Components;
using UnityEngine;
using UnityEngine.UI;

namespace InformalPenguins
{
    [RequireComponent(typeof(TimeRemaining), typeof(Text))]
    public class ChesterTimer : MonoBehaviour
    {
        private TimeRemaining _timeRemaining;
        // Use this for initialization
        void Start()
        {
            _timeRemaining = GetComponent<TimeRemaining>();
            Text text = GetComponent<Text>();
            _timeRemaining.Text = text;

            // _level = (CustomLevel)GameManager.Instance.Levels.Selected;
            float timeTarget = GameManager.Instance.Levels.Selected.TimeTarget;
            if (timeTarget > 0)
            {
                _timeRemaining.Limit = (int)timeTarget;
                _timeRemaining.CounterMode = TimeRemaining.CounterModeType.Down;
            }
            else
            {
                _timeRemaining.Limit = Constants.LIMIT_TIME;
                _timeRemaining.CounterMode = TimeRemaining.CounterModeType.Up;
            }
        }
    }
}
