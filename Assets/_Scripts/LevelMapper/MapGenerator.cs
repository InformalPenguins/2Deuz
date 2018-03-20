using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels.ObjectModel;
using UnityEngine;

namespace InformalPenguins
{
    [RequireComponent(typeof(MapEditor))]
    public class MapGenerator : MonoBehaviour
    {
        /// <summary>
        /// Player prefabs
        /// </summary>
        [Tooltip("A prefab for the Player.")]
        public GameObject RabbitPrefab;
        public MapEditor mapEditor;

        private GameObject rabbitGameObject;

        void Start()
        {
            Init();
        }

        void Init()
        {
            mapEditor.Init();

            Debug.Log("CURRENT LEVEL: " + GameManager.Instance.Levels.Selected.Number);
            LoadLevel(GameManager.Instance.Levels.Selected.Number);
        }


        public void LoadLevel(int levelId)
        {
            mapEditor.LoadLevel(levelId);

            AddPlayer();
        }
        private void AddPlayer() {
            if (rabbitGameObject == null) { 
                rabbitGameObject = Instantiate(RabbitPrefab);
                rabbitGameObject.transform.SetParent(transform.parent);

                GameManager.Messenger.TriggerMessage(new RabbitAddedMessage(rabbitGameObject));
            }

            rabbitGameObject.transform.position = mapEditor.getStartTransformPosition();
        }

        private void loadlevel(int index)
        {
            Level foundLevel = GameManager.Instance.Levels.GetItem(index);
            if (foundLevel != null)
            {
                GameManager.Instance.Levels.Selected = foundLevel;
                LoadLevel(index);
            }
        }

        public void previousLevel() {
            Level level = GameManager.Instance.Levels.Selected;
            int index = level.Number;
            int newIndex = index - 1;
            loadlevel(newIndex);
        }

        public void nextLevel()
        {
            Level level = GameManager.Instance.Levels.Selected;
            int index = level.Number;
            int newIndex = index + 1;
            loadlevel(newIndex);
        }

        public void DefeatBoss()
        {
            mapEditor.ToggleExit(true); 
        }
    }

}
