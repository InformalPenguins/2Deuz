using GameFramework.GameStructure;
using GameFramework.GameStructure.Colliders;
using GameFramework.GameStructure.Levels;
using UnityEngine;

namespace InformalPenguins
{
    public class MapGenerator : MonoBehaviour
    {
        //TODO: move to another config.
        public string FILENAME = "Assets/Resources/Maps/Level_1.tile";

        public const string TILE_PREFIX = "FLOOR_";
        public const string WALL_PREFIX = "WALL_";
        public const string EXIT_PREFIX = "EXIT_";
        public const string START_PREFIX = "START_";
        public const string GRID_SEPARATOR = ", ";

        public const string MAP_WALL = "0";
        public const string MAP_FLOOR = "1";
        public const string MAP_START = "S";
        public const string MAP_EXIT = "E";

        public const string MAP_STAR_1 = "C1";
        public const string MAP_STAR_2 = "C2";
        public const string MAP_STAR_3 = "C3";

        /// <summary>
        /// Tiles prefabs
        /// </summary>
        [Tooltip("A prefab for the walkable tile.")]
        public GameObject PrefabTile;

        [Tooltip("A prefab for the non walkable wall.")]
        public GameObject PrefabWall;

        [Tooltip("A prefab for the respawn point.")]
        public GameObject PrefabStart;

        [Tooltip("A prefab for the exit door.")]
        public GameObject PrefabExit;

        /// <summary>
        /// Player prefabs
        /// </summary>
        [Tooltip("A prefab for the Player.")]
        public GameObject RabbitPrefab;

        [Tooltip("A prefab for the Carrot (a.k.a. Star).")]
        public GameObject CarrotPrefab;

        public int gridX = 16;
        public int gridY = 12;

        public float initialX = 0;
        public float initialY = 0;

        public float tileWidth = .53f;
        public float tileHeight = .53f;
        private GameObject rabbitGameObject;
        private string[][] levelArray;
        private Transform tilesTransform;
        void Start() {
            Init();
            Randomize();
        }
        void Init() {
            Debug.Log("CURRENT LEVEL: " + GameManager.Instance.Levels.Selected.Number);

            GameObject empty = new GameObject("TilesContainer");
            empty.transform.SetParent(transform, false);
            tilesTransform = empty.transform;
        }

        public void DestroyChildren() {
            foreach (Transform child in tilesTransform)
            {
                Destroy(child.gameObject);
            }
        }

        private GameObject addTile(int i, int j) {
            GameObject tileObject = Instantiate(PrefabTile);
            tileObject.name = TILE_PREFIX + i + GRID_SEPARATOR + j;
            return tileObject;
        }
        private GameObject addCarrot(int currentStar, float currentX, float currentY) {
            GameObject carrotObject = Instantiate(CarrotPrefab);
            carrotObject.transform.position = new Vector3(currentX, currentY, 0);

            StarCollider carrotStarCollider = carrotObject.GetComponent<StarCollider>();
            carrotStarCollider.StarNumber = currentStar;

            carrotObject.transform.SetParent(tilesTransform, false);
            return carrotObject;
        }
        public void ResetMap(string[][] levelArray)
        {
            this.levelArray = levelArray;

            DestroyChildren();

            bool hasStart = false, hasExit = false;
            float currentY = initialY;
            /*
            //TODO: Move to an external config
            TODO: MOVE INSIDE THE SWITCH SO FILES CAN HAVE THE STARS LOCATIONS
            if (!LevelManager.Instance.Level.IsStarWon(1))
            {
                levelArray[13][1] = MAP_STAR_1;
            }
            if (!LevelManager.Instance.Level.IsStarWon(2))
            {
                levelArray[4][9] = MAP_STAR_2;
            }
            if (!LevelManager.Instance.Level.IsStarWon(3))
            {
                levelArray[2][16] = MAP_STAR_3;
            }
            */
            //NOTE: This can get nasty, beware.
            for (int i = levelArray.Length - 1; i >= 0; i--)
            {
                float currentX = initialX;
                for (int j = 0; j < levelArray[0].Length; j++)
                {
                    GameObject newObject = null;
                    switch (levelArray[i][j])
                    {
                        case MAP_WALL:
                            //Note: If modified, C case must be modified too.
                            newObject = Instantiate(PrefabWall);
                            newObject.name = TILE_PREFIX + i + GRID_SEPARATOR + j;
                            break;
                        case MAP_FLOOR:
                            newObject = Instantiate(PrefabTile);
                            newObject.name = WALL_PREFIX + i + GRID_SEPARATOR + j;
                            break;
                        case MAP_STAR_1:
                        case MAP_STAR_2:
                        case MAP_STAR_3:
                            string strNum = levelArray[i][j].Substring(1, 1);
                            int starNumber;
                            int.TryParse(strNum, out starNumber);
                            newObject = addTile(i, j);
                            addCarrot(starNumber, currentX, currentY);
                            break;
                        case MAP_START:
                            if (!hasStart)
                            {
                                hasStart = true;
                                newObject = Instantiate(PrefabStart);
                                newObject.name = START_PREFIX + i + GRID_SEPARATOR + j;

                                if (rabbitGameObject == null)
                                {
                                    rabbitGameObject = Instantiate(RabbitPrefab);
                                }
                                rabbitGameObject.transform.position = new Vector3(currentX, currentY, 0);
                                rabbitGameObject.transform.SetParent(transform, false);
                            }
                            break;
                        case MAP_EXIT:
                            hasExit = true;
                            newObject = Instantiate(PrefabExit);
                            newObject.name = EXIT_PREFIX + i + GRID_SEPARATOR + j;
                            break;
                    }
                    newObject.transform.SetParent(tilesTransform, false);
                    newObject.transform.position = new Vector3(currentX, currentY, 0);
                    currentX += tileWidth;
                }
                currentY += tileHeight;
            }

            if (!hasStart)
            {
                Debug.LogError("Map Tile does not have Start point, cannot add player.");
            }
            if (!hasExit)
            {
                Debug.LogError("Map Tile does not have Exit point.");
            }

            PrintMap();
        }
        public void ResetMapFromFile() {
            string[][] levelArray = FileUtility.readFile(FILENAME);
            ResetMap(levelArray);
        }

        public void Randomize()
        {
            string[][] levelArray = RandomMapUtility.generateRandomMap();
            ResetMap(levelArray);
        }
        public void PrintMap()
        {
            string concat = "";
            int jLen = levelArray[0].Length;
            for (int i = levelArray.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < jLen; j++)
                {
                    concat += levelArray[i][j] + (j == jLen - 1 ? "" : " ");
                }
                concat += "\n";
            }
            Debug.Log(concat);
        }
    }
}
