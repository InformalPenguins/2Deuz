namespace InformalPenguins {
    [System.Serializable]
    public class MapPoint
    {
        public int x = -1;
        public int y = -1;
        public MapPoint() { }
        public MapPoint(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
}