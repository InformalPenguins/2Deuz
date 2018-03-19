using UnityEngine;

namespace InformalPenguins {
    public class MapEditor : MonoBehaviour {
        public const string EMPTY = "EMPTY_CELL_";

        public GameObject emptyCellPrefab;
        public GameObject grassPrefab;
        public GameObject wallPrefab;
        public GameObject carrotPrefab;

        //Map Array is serialized into the Below Json file
        private GameObject[][] mapArray;

        public string MAP_FILENAME = "Assets/Resources/Maps/Map_{0}.json";
        public string LEVEL_FILENAME = "Assets/Resources/LevelInfo/Level_{0}.json";

        string currentLevel = "1";

        LevelInfo levelInfo;

        public GameObject[] carrots = new GameObject[3];
        int currentCarrotIdx = 0;
        public GameObject exitGameObject;
        public GameObject startGameObject;


        public static GameObject SELECTED_ENTITY = null;

        //public GameObject gridCellHighlight;

        private int gridX = 28, gridY = 17;
        // Use this for initialization
        void Start() {
            ResetMap();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (Input.GetMouseButton(0))
            {
                if (hitCollider != null)
                {
                    GameObject oldGameObject = hitCollider.transform.gameObject;
                    CellController oldCellController = oldGameObject.GetComponent<CellController>();
                    Constants.CellType cellType = oldCellController.cellType;
                    GameObject selectedEntity = MapEditor.SELECTED_ENTITY;
                    if (selectedEntity != null)
                    {
                        CellController newCellController = selectedEntity.GetComponent<CellController>();
                        if (newCellController.cellType == oldCellController.cellType) {
                            return;
                        }
                        GameObject triggerInPoint = GetTriggerInPoint(oldCellController.point);
                        Destroy(triggerInPoint);
                        ReplaceCell(oldGameObject, selectedEntity);
                    }
                }
            }
        }
        private GameObject GetTriggerInPoint(MapPoint point) {
            for (int i = 0; i < carrots.Length; i++) {
                GameObject carrot = carrots[i];
                if (carrot != null) {
                    if (isInSamePoint(carrot, point))
                    {
                        return carrot;
                    }
                }
            }

            if (isInSamePoint(startGameObject, point))
            {
                return startGameObject;
            }

            if (isInSamePoint(exitGameObject, point)) {
                return exitGameObject;
            }

            return null;
        }
        public bool isInSamePoint(GameObject checkingGameObject, MapPoint point)
        {
            if (checkingGameObject == null) {
                return false;
            }

            CellController cellController = checkingGameObject.GetComponent<CellController>();
            return cellController != null && cellController.point.x == point.x && cellController.point.y == point.y;
        }

        public void ReplaceCell(GameObject oldObject, GameObject newPrefab)
        {
            CellController oldCellController = oldObject.GetComponent<CellController>();
            CellController newPrefabController = newPrefab.GetComponent<CellController>();

            if (newPrefabController.cellType == oldCellController.cellType) {
                return;
            } else if (newPrefabController.isTrigger) {
                ReplaceCell(oldObject, grassPrefab);

                GameObject newTrigger = AddEntity(oldCellController, newPrefabController);
                switch (newPrefabController.cellType)
                {
                    case Constants.CellType.CARROT:
                        Destroy(carrots[currentCarrotIdx]);
                        carrots[currentCarrotIdx++] = newTrigger;

                        if (currentCarrotIdx >= 3) {
                            currentCarrotIdx = 0;
                        }
                        break;
                    case Constants.CellType.START:
                        Destroy(startGameObject);
                        startGameObject = newTrigger;
                        break;
                    case Constants.CellType.EXIT:
                        Destroy(exitGameObject);
                        exitGameObject = newTrigger;
                        break;
                }
                return;
            }

            GameObject newObject = AddEntity(oldCellController, newPrefabController);

            Destroy(oldObject);

            int x = oldCellController.point.x;
            int y = oldCellController.point.y;
            mapArray[y][x] = newObject;
        }
        private GameObject AddEntity(CellController oldCellController, CellController newPrefabController)
        {
            GameObject newObject = Instantiate(newPrefabController.gameObject);
            newObject.transform.SetParent(transform, false);
            Vector3 position = oldCellController.gameObject.transform.position;
            newObject.transform.position = position;

            int x = oldCellController.point.x;
            int y = oldCellController.point.y;

            CellController newCellController = newObject.GetComponent<CellController>();
            newCellController.point = new MapPoint(x, y);
            return newObject;
        }
        public void DestroyChildren()
        {
            if (mapArray == null)
            {
                return;
            }
            for (int i = 0; i < mapArray.Length; i++)
            {
                for (int j = 0; j < mapArray[0].Length; j++)
                {
                    Destroy(mapArray[i][j]);
                }
            }

            foreach (GameObject carrot in carrots)
            {
                Destroy(carrot);
            }
            Destroy(startGameObject);
            Destroy(exitGameObject);
        }

        public void ResetMap()
        {
            DestroyChildren();
            GameObject newObject = null;
            float currentX = 0, currentY = 0;
            mapArray = new GameObject[gridY][];
            for (int i = 0; i < gridY; i++)
            {
                currentX = 0;
                mapArray[i] = new GameObject[gridX];
                for (int j = 0; j < gridX; j++)
                {
                    newObject = Instantiate(emptyCellPrefab);
                    newObject.name = EMPTY + i + MapGenerator.GRID_SEPARATOR + j;
                    newObject.transform.SetParent(transform, false);
                    newObject.transform.position = new Vector3(currentX, currentY, 0);

                    CellController cellController = newObject.GetComponent<CellController>();
                    cellController.point = new MapPoint(j, i);
                    currentX += EditorConstants.TILE_WIDTH;

                    mapArray[i][j] = newObject;
                }
                currentY += EditorConstants.TILE_HEIGHT;
            }
        }

        public void ExportMap()
        {
            CellInfo[] cellsArray = new CellInfo[mapArray.Length * mapArray[0].Length];
            int k = 0;
            for (int i = 0; i < mapArray.Length; i++)
            {
                for (int j = 0; j < mapArray[0].Length; j++, k++)
                {

                    CellController cellController = mapArray[i][j].GetComponent<CellController>();
                    CellInfo cellInfo = new CellInfo();
                    cellInfo.cellType = cellController.cellType;
                    cellInfo.point = cellController.point;
                    cellsArray[k] = cellInfo;
                }
            }
            MapEditorWrapper wrapper = new MapEditorWrapper();
            wrapper.cells = cellsArray;
            string json = JsonUtility.ToJson(wrapper);

            string filename = getMapFileName(currentLevel);

            FileUtility.writeFile(filename, json);

            BuildLevelInfo();
        }
        private void BuildLevelInfo() {
            levelInfo = new LevelInfo();
            levelInfo.stars = new StarInfo[3];

            for (int i = 0; i < carrots.Length; i++)
            {
                GameObject carrot = carrots[i];
                if (carrot != null)
                {
                    CellController cellController = carrot.GetComponent<CellController>();
                    StarInfo carrotInfo = new StarInfo();
                    carrotInfo.id = i + 1;
                    carrotInfo.point = cellController.point;
                    levelInfo.stars[i] = carrotInfo;
                }
            }

            if (startGameObject != null)
            {
                CellController cellController = startGameObject.GetComponent<CellController>();
                levelInfo.start = cellController.point;
            }
            levelInfo.exit = new MapPoint();
            if (exitGameObject != null)
            {
                CellController cellController = exitGameObject.GetComponent<CellController>();
                levelInfo.exit = cellController.point;
            }
            levelInfo.id = int.Parse(currentLevel);
            levelInfo.name = "<PLACEHOLDER>";
            levelInfo.hasBoss = false;
            levelInfo.map = getMapFileName(currentLevel);

            string levelInfoJson = JsonUtility.ToJson(levelInfo);
            FileUtility.writeFile(getLevelFileName(currentLevel), levelInfoJson);
        }

        public string getMapFileName(string lv)
        {
            return string.Format(MAP_FILENAME, lv);
        }
        public string getLevelFileName(string lv)
        {
            return string.Format(LEVEL_FILENAME, lv);
        }

        public void LoadMap(string lv)
        {
            string filename = getMapFileName(lv);
            MapEditorWrapper wrapper = JsonUtility.FromJson<MapEditorWrapper>(FileUtility.readFile(filename));
            LoadMap(wrapper);
        }

        public void LoadMap()
        {
            LoadMap(currentLevel);
        }

        public void UpdatedTextBox(UnityEngine.UI.Text newValue)
        {
            currentLevel = newValue.text;
        }

        public void LoadMap(MapEditorWrapper wrapper)
        {
            DestroyChildren();

            CellInfo[] cellsArray = wrapper.cells;
            mapArray = new GameObject[gridY][];
            for (int i = 0; i < mapArray.Length; i++) {
                mapArray[i] = new GameObject[gridX];
            }

            for (int i = 0; i < cellsArray.Length; i++)
            {
                CellInfo cell = cellsArray[i];
                MapPoint point = cell.point;

                float currentX = EditorConstants.TILE_WIDTH * point.x;
                float currentY = EditorConstants.TILE_HEIGHT * point.y;

                GameObject newObject = null;

                switch (cell.cellType)
                {
                    case Constants.CellType.EMPTY:
                        newObject = Instantiate(emptyCellPrefab);
                        break;

                    case Constants.CellType.WALL:
                        newObject = Instantiate(wallPrefab);
                        break;

                    case Constants.CellType.GRASS:
                        newObject = Instantiate(grassPrefab);
                        break;
                    case Constants.CellType.CARROT:
                        newObject = Instantiate(grassPrefab);
                        GameObject carrot = Instantiate(carrotPrefab);
                        break;
                }

                newObject.name = newObject.name + point.y + MapGenerator.GRID_SEPARATOR + point.x;
                newObject.transform.SetParent(transform, false);
                newObject.transform.position = new Vector3(currentX, currentY, 0);

                CellController cellController = newObject.GetComponent<CellController>();
                cellController.point = point;
                cellController.cellType = cell.cellType;
                mapArray[point.y][point.x] = newObject;
            }
        }
    }
}