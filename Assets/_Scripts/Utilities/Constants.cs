using UnityEngine;

namespace InformalPenguins {
    public class Constants
    {
        public const string TAG_PLAYER = "Player";
        public const string TAG_WALKABLE = "WALKABLE";
        public const string TAG_WALL = "WALL";
        public const string TAG_HAZARD = "HAZARD";
        public const string TAG_HAZARD_FF = "HAZARD_FF";
        public const string TAG_FLAMMABLE = "FLAMMABLE";
        public const string TAG_ARROW = "ARROW";
        
        public const string INPUT_HORIZONTAL = "Horizontal";
        public const string INPUT_VERTICAL = "Vertical";
        public const string INPUT_RUN = "Run";
        public const string INPUT_JUMP = "Jump";
        public const string INPUT_CROUCH = "Crouch";
        public const string INPUT_PAUSE = "Pause";

        public const int DEFAULT_WIDTH = 17;
        public const int DEFAULT_HEIGHT = 15;
        public const int LIMIT_TIME = 50;

        public const float MAX_SPEED = 2.5f;

        public const string RESOURCES_PATH = "Assets/Resources/";
        public const string RESOURCES_EXT= ".json";


        public const string TRIGGER_FLAME = "Fire";

        
        //Vector3.zero keeps generating a new Vector every get
        public static readonly Vector3 VECTOR_3_ZERO = new Vector3(0, 0, 0);
        public static readonly Vector2 VECTOR_2_ZERO = new Vector2(0, 0);

        public enum CellType
        {
            EMPTY, WALL, GRASS,
            START, EXIT,
            CARROT,
            WALKABLE_WALL,
            FLAME, ARCHER, EXTINGUISHER, BONFIRE
        }

        public enum CarrotsType
        {
            NONE,
            TIME,
            SCORE
        }
    }
}