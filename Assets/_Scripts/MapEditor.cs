using GameFramework.GameStructure.Colliders;
using UnityEngine;

namespace InformalPenguins {
    public class MapEditor : MonoBehaviour {
        public const string EMPTY = "EMPTY_CELL_";

        public GameObject emptyCellPrefab;
        public GameObject grassPrefab;
        public GameObject wallPrefab;
        public GameObject carrotPrefab;
        public GameObject exitGameObjectPrefab;
        public GameObject startGameObjectPrefab;

        //Map Array is serialized into the Below Json file
        private GameObject[][] mapArray;

        public string MAP_FILENAME = "Assets/Resources/Maps/Map_{0}.json";
        public string LEVEL_FILENAME = "Assets/Resources/LevelInfo/Level_{0}.json";

        int currentLevelId = 1;
        int currentMapId = 1;
        string currentLevelName = "LEVEL_NAME";
        bool currentHasBoss = false;
        public bool IsEditor = false;
        public LevelInfo LevelInfo;

        private int currentCarrotIdx = 0;
        private GameObject[] carrotsGameObjects = new GameObject[3];
        private GameObject exitGameObject;
        private GameObject startGameObject;
        //private GameObject tilesTransformObject;
        private Transform tilesTransform;

        public static GameObject SELECTED_ENTITY = null;

        //public GameObject gridCellHighlight;

        private int gridX = 28, gridY = 17;

        // Use this for initialization
        void Start()
        {
            IsEditor = true;

            ResetMap();

            Init();
        }
        public void Init()
        {
            tilesTransform = new GameObject("TilesParent").transform;
            //tilesTransform.SetParent(transform);
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
            for (int i = 0; i < carrotsGameObjects.Length; i++) {
                GameObject carrot = carrotsGameObjects[i];
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
                ReplaceCell(oldObject, grassPrefab); //TODO: Check if walkable is present before replacing with grass

                GameObject newTrigger = AddEntity(oldCellController, newPrefabController);
                switch (newPrefabController.cellType)
                {
                    case Constants.CellType.CARROT:
                        Destroy(carrotsGameObjects[currentCarrotIdx]);
                        carrotsGameObjects[currentCarrotIdx++] = newTrigger;

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
            newObject.transform.SetParent(tilesTransform, false);
            Vector3 position = oldCellController.gameObject.transform.position;
            newObject.transform.position = position;

            int x = oldCellController.point.x;
            int y = oldCellController.point.y;

            CellController newCellController = newObject.GetComponent<CellController>();
            newCellController.point = new MapPoint(x, y);
            return newObject;
        }
        public void DestroyMapChildren()
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
        }
        private void DestroyLevelChildren()
        {
            foreach (GameObject carrot in carrotsGameObjects)
            {
                Destroy(carrot);
            }
            Destroy(startGameObject);
            Destroy(exitGameObject);
        }

        public void ResetMap()
        {
            DestroyMapChildren();
            DestroyLevelChildren();

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
                    newObject.name = EMPTY + i + EditorConstants.GRID_SEPARATOR + j;
                    newObject.transform.SetParent(tilesTransform, false);
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

            string filename = getMapFileName(currentLevelId);

            FileUtility.writeFile(filename, json);

            BuildLevelInfo();
        }
        private void BuildLevelInfo() {
            LevelInfo = new LevelInfo();
            LevelInfo.stars = new StarInfo[3];

            for (int i = 0; i < carrotsGameObjects.Length; i++)
            {
                GameObject carrot = carrotsGameObjects[i];
                if (carrot != null)
                {
                    CellController cellController = carrot.GetComponent<CellController>();
                    StarInfo carrotInfo = new StarInfo();
                    carrotInfo.id = i + 1;
                    carrotInfo.point = cellController.point;
                    LevelInfo.stars[i] = carrotInfo;
                }
            }

            if (startGameObject != null)
            {
                CellController cellController = startGameObject.GetComponent<CellController>();
                LevelInfo.start = cellController.point;
            }

            LevelInfo.exit = new MapPoint();
            if (exitGameObject != null)
            {
                CellController cellController = exitGameObject.GetComponent<CellController>();
                LevelInfo.exit = cellController.point;
            }
            LevelInfo.id = currentLevelId;
            LevelInfo.name = currentLevelName;
            LevelInfo.map = getMapFileName(currentMapId);
            LevelInfo.hasBoss = currentHasBoss;

            string levelInfoJson = JsonUtility.ToJson(LevelInfo);
            FileUtility.writeFile(getLevelFileName(currentLevelId), levelInfoJson);
        }

        public string getMapFileName(int lv)
        {
            return string.Format(MAP_FILENAME, lv);
        }
        public string getLevelFileName(int lv)
        {
            return string.Format(LEVEL_FILENAME, lv);
        }

        public void LoadLevel(int levelId)
        {
            string levelFilename = getLevelFileName(levelId);
            LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(FileUtility.readFile(levelFilename));
            string mapFilename = levelInfo.map;
            MapEditorWrapper wrapper = JsonUtility.FromJson<MapEditorWrapper>(FileUtility.readFile(mapFilename));
            LoadMap(wrapper);
            LoadLevel(levelInfo);
        }

        public void LoadMap()
        {
            LoadLevel(currentLevelId);
        }


        private float getWorldY(int pointY)
        {
            return EditorConstants.TILE_HEIGHT * pointY;
        }
        private float getWorldX(int pointX)
        {
            return EditorConstants.TILE_WIDTH * pointX;
        }

        public void LoadLevel(LevelInfo levelInfo)
        {
            this.LevelInfo = levelInfo;

            DestroyLevelChildren();

            StarInfo[] levelInfoCarrots = levelInfo.stars;
            AddStart(levelInfo.start);
            AddExit(levelInfo.exit);

            for (int i = 0; i < levelInfoCarrots.Length; i++) {
                StarInfo levelInfoCarrot = levelInfoCarrots[i];
                GameObject newCarrot = AddElement(carrotPrefab, levelInfoCarrot.point);
                if (newCarrot != null)
                {
                    StarCollider carrotStarCollider = newCarrot.GetComponent<StarCollider>();
                    carrotStarCollider.StarNumber = levelInfoCarrot.id;
                    carrotsGameObjects[i] = newCarrot;
                }
            }
        }
        public void AddStart(MapPoint startPoint)
        {
            startGameObject = AddElement(startGameObjectPrefab, startPoint);
        }

        public void AddExit(MapPoint exitPoint)
        {
            exitGameObject = AddElement(exitGameObjectPrefab, exitPoint);

            if (!IsEditor && LevelInfo.hasBoss) {
                ToggleExit(false);
            }
        }
        public void ToggleExit(bool isActive)
        {
            exitGameObject.SetActive(isActive);
        }

        private GameObject AddElement(GameObject prefab, MapPoint point)
        {
            if (point.x < 0 || point.y < 0) {
                return null;
            }

            GameObject newGameObject = Instantiate(prefab);
            newGameObject.transform.SetParent(tilesTransform);
            newGameObject.transform.position = new Vector3(getWorldX(point.x), getWorldY(point.y), 0);
            CellController cellController = newGameObject.GetComponent<CellController>();
            cellController.point = point;
            return newGameObject;
        }
        public void LoadMap(MapEditorWrapper wrapper)
        {
            DestroyMapChildren();

            CellInfo[] cellsArray = wrapper.cells;
            mapArray = new GameObject[gridY][];
            for (int i = 0; i < mapArray.Length; i++) {
                mapArray[i] = new GameObject[gridX];
            }

            for (int i = 0; i < cellsArray.Length; i++)
            {
                CellInfo cell = cellsArray[i];
                MapPoint point = cell.point;

                float currentX = getWorldX(point.x);
                float currentY = getWorldY(point.y);

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
                }

                newObject.name = newObject.name + point.y + EditorConstants.GRID_SEPARATOR + point.x;
                newObject.transform.SetParent(tilesTransform, false);
                newObject.transform.position = new Vector3(currentX, currentY, 0);

                CellController cellController = newObject.GetComponent<CellController>();
                cellController.point = point;
                cellController.cellType = cell.cellType;
                mapArray[point.y][point.x] = newObject;
            }
        }

        public void UpdatedLevelId(UnityEngine.UI.Text newValue)
        {
            currentLevelId = int.Parse(newValue.text);
        }

        public void UpdatedMapId(UnityEngine.UI.Text newValue)
        {
            currentMapId = int.Parse(newValue.text);
        }

        public void UpdatedLevelName(UnityEngine.UI.Text newValue)
        {
            currentLevelName = newValue.text;
        }
        public void UpdatedHasBoss(UnityEngine.UI.Toggle newValue)
        {
            currentHasBoss = newValue.isOn;
        }
        public Vector3 getStartTransformPosition() {
            return startGameObject.transform.position;
        }
    }
}