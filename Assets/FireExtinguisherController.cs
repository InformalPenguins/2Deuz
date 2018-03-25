﻿using GameFramework.GameStructure;
using UnityEngine;

namespace InformalPenguins {  

    [RequireComponent(typeof(AudioClip))]
    public class FireExtinguisherController : MonoBehaviour
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
                //collision.gameObject.SendMessageUpwards("Harm");
                //ExtinguisherUsedMessage msg = new ExtinguisherUsedMessage(gameObject);
                //GameManager.Messenger.TriggerMessage(msg);
                ExtinguishFlames();
            }
        }
        private void ExtinguishFlames() {
            //Destroy all Flames so you can go to the exit button
            GameObject[] hazards = GameObject.FindGameObjectsWithTag(Constants.TAG_HAZARD);
            foreach (GameObject hazard in hazards)
            {
                hazard.SendMessage("Extinguish");
            }
        }
    }
}
