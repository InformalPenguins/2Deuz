using GameFramework.UI.Buttons.Components.AbstractClasses;
using UnityEngine;

namespace InformalPenguins {  
    public class PaletteButton : OnButtonClick
    {
        public GameObject HoverImage;
        public GameObject TilePrefab;
        
        private void OnMouseOver()
        {
            HoverImage.SetActive(true);
        }
        private void OnMouseExit()
        {
            HoverImage.SetActive(false);
        }

        public override void OnClick()
        {
            MapEditor.SELECTED_ENTITY = TilePrefab;
        }
    }
}