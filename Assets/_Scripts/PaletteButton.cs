using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InformalPenguins {  
    public class PaletteButton : MonoBehaviour {
        public GameObject tilePrefab;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
        public void onClick() {
            MapEditor.SELECTED_ENTITY = tilePrefab;
        }
    }
}