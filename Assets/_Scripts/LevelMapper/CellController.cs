using UnityEngine;
namespace InformalPenguins
{
    public class CellController : MonoBehaviour
    {
        public Constants.CellType cellType;
        public MapPoint point;

        public bool isTrigger = false;
        public bool isExtra = false; //extra objects can only be erased using the eraser
        public int reference = -1;
    }
}