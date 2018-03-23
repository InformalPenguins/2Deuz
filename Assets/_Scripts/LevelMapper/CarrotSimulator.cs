using UnityEngine;
namespace InformalPenguins
{
    public class CarrotSimulator : MonoBehaviour
    {
        public Sprite[] sprites = new Sprite[3];
        private int _carrotIndex = 0;
        public void SetCarrotIndex(int idx)
        {
            _carrotIndex = idx;

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sprite = sprites[idx];
        }
    }
}