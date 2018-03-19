using GameFramework.GameStructure;
using GameFramework.GameStructure.Colliders;
using GameFramework.GameStructure.Levels;
using GameFramework.GameStructure.Levels.ObjectModel;
using UnityEngine;

namespace InformalPenguins
{
    public class MapGenerator : MonoBehaviour
    {
        //More like for debugging purposes, not used on game flow
        public string FILENAME = "Assets/Resources/Maps/Level_1.tile";

        public const string FLOOR_PREFIX = "FLOOR_";
        public const string WALL_PREFIX = "WALL_";
        public const string EXIT_PREFIX = "EXIT_";
        public const string START_PREFIX = "START_";
        public const string GRID_SEPARATOR = ", ";

        public const string MAP_WALL = "0";
        public const string MAP_FLOOR = "1";
        public const string MAP_START = "S";
        public const string MAP_EXIT = "E";

        public const string MAP_STAR_X = "C{0}";
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

        public float initialX = 0;
        public float initialY = 0;

        private GameObject rabbitGameObject;
        private string[][] levelArray;
        private LevelInfo levelInfo;
        private Transform tilesTransform;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 1);
        }

        void Start() {
            Init();
        }

        void Init()
        {
            Debug.Log("CURRENT LEVEL: " + GameManager.Instance.Levels.Selected.Number);

            GameObject empty = new GameObject("TilesContainer");
            empty.transform.SetParent(transform, false);
            tilesTransform = empty.transform;

            LoadLevel(GameManager.Instance.Levels.Selected.Number);
        }

        public void LoadLevel(int worldId, int levelId)
        {
            LevelStructure levelStructure = LevelStructure.get();
            LevelInfo selectedLevelInfo = null;
            WorldInfo selectedWorld = null;
            foreach (WorldInfo wi in levelStructure.worlds)
            {
                selectedWorld = wi;
                if (worldId == wi.id) {
                    foreach (LevelInfo li in wi.levels)
                    {
                        if (levelId == li.id)
                        {
                            selectedLevelInfo = li;
                            break;
                        }
                    }
                }
            }

            if (selectedLevelInfo != null) {
                string filename = selectedLevelInfo.map;

                ResetMapFromFile(filename, selectedLevelInfo);
            }
        }
        public void LoadLevel(int levelId) {
            this.LoadLevel(GameManager.Instance.Worlds.Selected.Number, levelId);
        }

        public void DestroyChildren() {
            foreach (Transform child in tilesTransform)
            {
                Destroy(child.gameObject);
            }
        }

        private GameObject addTile(int i, int j) {
            GameObject tileObject = Instantiate(PrefabTile);
            tileObject.name = FLOOR_PREFIX + i + GRID_SEPARATOR + j;
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
        public void ResetMap(string[][] levelArray, LevelInfo levelInfo = null)
        {
            this.levelArray = levelArray;

            DestroyChildren();

            bool hasStart = false, hasExit = false;
            float currentY = initialY;

            if (levelInfo != null)
            {
                this.levelInfo = levelInfo;
                levelArray[levelInfo.start.y][levelInfo.start.x] = MAP_START;
                if (levelInfo.hasBoss)
                {
                    hasExit = true;
                }
                else
                {
                    levelArray[levelInfo.exit.y][levelInfo.exit.x] = MAP_EXIT;
                }

                if (levelInfo.stars.Length > 0) {
                    foreach (StarInfo si in levelInfo.stars)
                    {
                        if (!LevelManager.Instance.Level.IsStarWon(si.id))
                        {
                            levelArray[si.point.y][si.point.x] = string.Format(MAP_STAR_X, si.id);
                        }
                    }
                }
            }

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
                            newObject.name = WALL_PREFIX + i + GRID_SEPARATOR + j;
                            break;
                        case MAP_FLOOR:
                            newObject = Instantiate(PrefabTile);
                            newObject.name = FLOOR_PREFIX + i + GRID_SEPARATOR + j;
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
                                    GameManager.Messenger.TriggerMessage(new RabbitAddedMessage(rabbitGameObject));
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
                    currentX += EditorConstants.TILE_WIDTH;
                }
                currentY += EditorConstants.TILE_HEIGHT;
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
        public void ResetMapFromFile()
        {
            this.ResetMapFromFile(FILENAME);
        }
        public void ResetMapFromFile(string filename, LevelInfo levelInfo = null)
        {
            string[][] levelArray = FileUtility.readFileAsArray(filename);
            ResetMap(levelArray, levelInfo);
        }

        public void Randomize()
        {
            string[][] levelArray = RandomMapUtility.generateRandomMap();
            ResetMap(levelArray);
        }
        public void PrintMap()
        {
            string concat = "";
            //string concatCoords = "";
            string space = " ";
            string newLine = "\n";
            int jLen = levelArray[0].Length;

            for (int i = levelArray.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < jLen; j++)
                {
                    string currentChar = levelArray[i][j];
                    space = (j == jLen - 1 ? "" : " ");
                    concat += currentChar + space;
                    if (currentChar != MAP_WALL && currentChar != MAP_FLOOR) {
                        Debug.Log((levelArray[i][j] + "(" + i + ", " + j + ")"));
                    }
                }

                //concatCoords += newLine;
                concat += newLine;
            }
            //Debug.Log(concatCoords);
            Debug.Log(concat);
        }

        public void ExportMap()
        {
            LevelInfo levelInfo = new LevelInfo();
            levelInfo.id = GameManager.Instance.Levels.Selected.Number;
            levelInfo.name = GameManager.Instance.Levels.Selected.Name;
            levelInfo.map = string.Format("Assets/Resources/Maps/Level_{0}.tile", levelInfo.id);

            int jLen = levelArray[0].Length;

            StarInfo[] starsArray = new StarInfo[3];

            for (int i = levelArray.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < jLen; j++)
                {
                    switch (levelArray[i][j])
                    {
                        case MAP_STAR_1:
                            StarInfo star1 = new StarInfo();
                            star1.id = 1;
                            MapPoint star1Point = new MapPoint();
                            star1Point.x = j;
                            star1Point.y = i;
                            star1.point = star1Point;
                            starsArray[0] = star1;
                            break;
                        case MAP_STAR_2:
                            StarInfo star2 = new StarInfo();
                            star2.id = 2;
                            MapPoint star2Point = new MapPoint();
                            star2Point.x = j;
                            star2Point.y = i;
                            star2.point = star2Point;
                            starsArray[1] = star2;
                            break;
                        case MAP_STAR_3:
                            StarInfo star3 = new StarInfo();
                            star3.id = 2;
                            MapPoint star3Point = new MapPoint();
                            star3Point.x = j;
                            star3Point.y = i;
                            star3.point = star3Point;
                            starsArray[2] = star3;
                            break;
                        case MAP_START:
                            MapPoint startPoint = new MapPoint();
                            startPoint.x = j;
                            startPoint.y = i;

                            levelInfo.start = startPoint;
                            break;
                        case MAP_EXIT:
                            MapPoint exitPoint = new MapPoint();
                            exitPoint.x = j;
                            exitPoint.y = i;

                            levelInfo.exit = exitPoint;
                            break;
                    }
                }
            }

            levelInfo.stars = starsArray;
            Debug.Log(JsonUtility.ToJson(levelInfo));
        }
        public void previousLevel() {
            Level level = GameManager.Instance.Levels.Selected;
            int index = level.Number;
            int newIndex = index - 1;
            loadlevel(newIndex);
        }
        public void nextLevel()
        {
            Level level = GameManager.Instance.Levels.Selected;
            int index = level.Number;
            int newIndex = index + 1;
            loadlevel(newIndex);
        }
        private void loadlevel(int index)
        {
            Level foundLevel = GameManager.Instance.Levels.GetItem(index);
            if (foundLevel != null)
            {
                GameManager.Instance.Levels.Selected = foundLevel;
                LoadLevel(index);
            }
        }
        public void DefeatBoss()
        {
            int h = levelInfo.exit.x;
            int w = levelInfo.exit.y;

            GameObject newObject = Instantiate(PrefabExit);
            newObject.transform.SetParent(tilesTransform, false);
            newObject.name = EXIT_PREFIX + h + GRID_SEPARATOR + w;

            newObject.transform.position = new Vector3(w * EditorConstants.TILE_WIDTH + initialX, h * EditorConstants.TILE_HEIGHT + initialY, 0);
        }
    }

}
