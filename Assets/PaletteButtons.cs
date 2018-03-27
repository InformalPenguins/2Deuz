using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InformalPenguins
{
    public class PaletteButtons : MonoBehaviour
    {
        private static readonly string separatorName = "-----";
        public GameObject PaletteButton;
        public PaletteGroup[] PaletteGroups;
        public void Start()
        {
            foreach (PaletteGroup group in PaletteGroups)
            {
                GameObject[] tilesPrefabs = group.Prefabs;

                foreach (GameObject tilePrefab in tilesPrefabs)
                {

                    SpriteRenderer spriteRenderer = tilePrefab.GetComponentInChildren<SpriteRenderer>();

                    GameObject newObject = Instantiate(PaletteButton, transform);
                    PaletteButton newButton = newObject.GetComponent<PaletteButton>();
                    Image newImage = newObject.GetComponent<Image>();

                    newButton.TilePrefab = tilePrefab;
                    newImage.sprite = spriteRenderer.sprite;
                }

                AddSeparator();
            }
        }
        private void AddSeparator()
        {
            GameObject empty = new GameObject();
            empty.name = separatorName;
            empty.AddComponent<RectTransform>();
            empty.transform.parent = transform;
        }
    }
}