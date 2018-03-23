using GameFramework.GameStructure;
using GameFramework.Messaging;
using UnityEngine;

namespace InformalPenguins
{
    public class Level4GameLoop : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GameManager.Messenger.AddListener<ArcherDefeatedMessage>(ArcherDefeated);
        }

        private bool ArcherDefeated(BaseMessage message)
        {
            GameObject[] hazards = GameObject.FindGameObjectsWithTag(Constants.TAG_HAZARD);
            foreach (GameObject hazard in hazards) {
                hazard.SetActive(false);
            }
            
            return true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
