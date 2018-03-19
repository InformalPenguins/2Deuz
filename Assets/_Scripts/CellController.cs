using UnityEngine;
namespace InformalPenguins
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CellController : MonoBehaviour
    {
        public Constants.CellType cellType;
        public MapPoint point;

        public bool isTrigger = false;
    }
}