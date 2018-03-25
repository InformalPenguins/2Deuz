using GameFramework.GameStructure.Colliders;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using GameFramework.GameStructure;
using System;
using System.Collections.Generic;

namespace InformalPenguins {
    public class MapEditor : MonoBehaviour {
        public const string EMPTY = "EMPTY_CELL_";
        public static GameObject SELECTED_ENTITY = null;
        private const string MAP_FILENAME = "Assets/Resources/Maps/Map_{0}.json";
        private const string LEVEL_FILENAME = "Assets/Resources/LevelInfo/Level_{0}.json";

        private GameObject[][] mapArray;
        private int _currentLevelId = 1;
        private int _currentMapId = 1;
        private string _currentLevelName = "LEVEL_NAME";
        private bool _currentHasBoss = false;

        private int _currentCarrotIdx = 0;
        private GameObject[] _carrotsGameObjects = new GameObject[3];
        private GameObject _exitGameObject;
        private GameObject _startGameObject;
        private Transform _tilesTransform;
        private int _gridX = 28, _gridY = 17;

        private List<GameObject> _extrasObjects = new List<GameObject>();

        [NonSerialized]
        public bool IsEditor = false;
        [NonSerialized]
        public LevelInfo LevelInfo;

        public GameObject emptyCellPrefab;
        public GameObject grassPrefab;
        public GameObject wallPrefab;
        public GameObject walkableWallPrefab;
        public GameObject carrotPrefab;
        public GameObject exitGameObjectPrefab;
        public GameObject startGameObjectPrefab;
        public GameObject flamePrefab;
        public GameObject archerPrefab;
        public GameObject extinguisherPrefab;
        
        //Editor handlers
        public GameObject carrotSimulatorPrefab;
        public GameObject archerSimulator;
        public GameObject flameSimulator;

        void Start()
        {
            IsEditor = true;

            Init();
        }
        public void Init()
        {
            _tilesTransform = new GameObject("TilesParent").transform;

            ResetMap();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsEditor) {
                EditorUpdate();
            }
        }


        private void addSelectedObjectOver(GameObject overGameObject)
        {
            addSelectedObjectOver(overGameObject, SELECTED_ENTITY);
        }
        private CellController getCellController(GameObject obj) {
            CellController cellController = obj.GetComponent<CellController>();
            if (cellController != null) {
                return cellController;
            }
            return obj.GetComponentInParent<CellController>();
        }
        private void addSelectedObjectOver(GameObject oldGameObject, GameObject selectedEntity) {
            CellController oldCellController = getCellController(oldGameObject);
            if (selectedEntity != null)
            {
                CellController newCellController = selectedEntity.GetComponent<CellController>();

                if (newCellController.cellType == Constants.CellType.EMPTY) {

                    DestroyExtra(oldCellController.point);
                }

                //if (newCellController.cellType == Constants.CellType.EMPTY)
                //{
                //}
                GameObject triggerInPoint = GetTriggerInPoint(oldCellController.point);
                Destroy(triggerInPoint);

                ReplaceCell(oldGameObject, selectedEntity);

                if (newCellController.cellType == oldCellController.cellType)
                {
                    return;
                }
            }
        }
        private void DestroyExtra(MapPoint point) {
            GameObject foundExtra = null;
            foreach (GameObject extra in _extrasObjects) {
                if(isInPoint(extra, point))
                {
                    foundExtra = extra;
                    break;
                }
            }
            if (foundExtra != null) {
                DestroyExtra(foundExtra);
            }
        }
        private void DestroyExtra(GameObject extra)
        {
            if(_extrasObjects != null) { 
                _extrasObjects.Remove(extra);
            }

            Destroy(extra);
        }
        private bool DestroyExtra(int refIdx) {
            foreach(GameObject extra in _extrasObjects)
            {
                if (extra != null) {
                    CellController cellController = extra.GetComponent<CellController>();
                    if (cellController.reference == refIdx) {
                        DestroyExtra(extra);
                        return true;
                    }
                }
            }
            return false;
        }
        void EditorUpdate() {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    addSelectedObjectOver(hitCollider.gameObject);
                } else if (Input.GetMouseButton(0) && !SELECTED_ENTITY.GetComponent<CellController>().isTrigger && !SELECTED_ENTITY.GetComponent<CellController>().isExtra) {
                    addSelectedObjectOver(hitCollider.gameObject);
                }
            }
        }
        private GameObject GetTriggerInPoint(MapPoint point) {
            for (int i = 0; i < _carrotsGameObjects.Length; i++) {
                GameObject carrot = _carrotsGameObjects[i];
                if (carrot != null) {
                    if (isInPoint(carrot, point))
                    {
                        return carrot;
                    }
                }
            }

            if (isInPoint(_startGameObject, point))
            {
                return _startGameObject;
            }

            if (isInPoint(_exitGameObject, point)) {
                return _exitGameObject;
            }

            return null;
        }
        public bool isInPoint(GameObject checkingGameObject, MapPoint point)
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

            if (newPrefabController == null || oldCellController == null || newPrefabController.cellType == oldCellController.cellType)
            {
                return;
            }
            else if (newPrefabController.isTrigger)
            {
                //TODO: Check if walkable is present before replacing with grass
                //ReplaceCell(oldObject, grassPrefab); 

                GameObject newTrigger = AddEntity(oldCellController, newPrefabController);
                switch (newPrefabController.cellType)
                {
                    case Constants.CellType.CARROT:
                        Destroy(_carrotsGameObjects[_currentCarrotIdx]);
                        _carrotsGameObjects[_currentCarrotIdx] = newTrigger;

                        if (IsEditor)
                        {
                            newTrigger.GetComponent<CarrotSimulator>().SetCarrotIndex(_currentCarrotIdx);
                        }

                        _currentCarrotIdx++;

                        if (_currentCarrotIdx >= 3)
                        {
                            _currentCarrotIdx = 0;
                        }
                        break;
                    case Constants.CellType.START:
                        Destroy(_startGameObject);
                        _startGameObject = newTrigger;
                        break;
                    case Constants.CellType.EXIT:
                        Destroy(_exitGameObject);
                        _exitGameObject = newTrigger;
                        break;
                }
            }
            else if (newPrefabController.isExtra)
            {
                GameObject newExtra = AddEntity(oldCellController, newPrefabController);
                CellController cellController = newExtra.GetComponent<CellController>();
                cellController.reference = _extrasObjects.Count;
                _extrasObjects.Add(newExtra);
            }
            else {
                GameObject newObject = AddEntity(oldCellController, newPrefabController);
                if (!oldCellController.isExtra) {
                    Destroy(oldObject);
                }

                int x = oldCellController.point.x;
                int y = oldCellController.point.y;
                mapArray[y][x] = newObject;
            }
        }


        private GameObject AddElement(GameObject prefab, MapPoint point)
        {
            if (point.x < 0 || point.y < 0)
            {
                return null;
            }

            GameObject newGameObject = Instantiate(prefab);
            newGameObject.transform.SetParent(_tilesTransform);
            newGameObject.transform.position = new Vector3(getWorldX(point.x), getWorldY(point.y), 0);
            CellController cellController = newGameObject.GetComponent<CellController>();
            cellController.point = point;
            return newGameObject;
        }

        private GameObject AddEntity(CellController oldCellController, CellController newPrefabController)
        {
            GameObject newObject = Instantiate(newPrefabController.gameObject);
            newObject.transform.SetParent(_tilesTransform, false);
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
            foreach (GameObject carrot in _carrotsGameObjects)
            {
                Destroy(carrot);
            }
            Destroy(_startGameObject);
            Destroy(_exitGameObject);
        }
        private void DestroyExtrasChildren()
        {
            if (_extrasObjects != null)
            {
                GameObject[] extrasArr = _extrasObjects.ToArray();
                for (int i =0;i<_extrasObjects.Count;i++)
                {
                    DestroyExtra(extrasArr[1]);
                }
                _extrasObjects = new List<GameObject>();
            }

            //Array got out of sync, remove from the scene using tags search.
            GameObject[] hazardsObjects = GameObject.FindGameObjectsWithTag(Constants.TAG_HAZARD);
            foreach (GameObject extra in hazardsObjects)
            {
                DestroyExtra(extra);
            }
        }

        public void ResetMap()
        {
            DestroyMapChildren();
            DestroyLevelChildren();
            DestroyExtrasChildren();

            GameObject newObject = null;
            float currentX = 0, currentY = 0;
            mapArray = new GameObject[_gridY][];
            for (int i = 0; i < _gridY; i++)
            {
                currentX = 0;
                mapArray[i] = new GameObject[_gridX];
                for (int j = 0; j < _gridX; j++)
                {
                    newObject = Instantiate(emptyCellPrefab);
                    newObject.name = EMPTY + i + EditorConstants.GRID_SEPARATOR + j;
                    newObject.transform.SetParent(_tilesTransform, false);
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

            string filename = getMapFileName(_currentLevelId);

            FileUtility.writeFile(filename, json);

            BuildLevelInfo();
        }
        private void BuildLevelInfo() {
            LevelInfo = new LevelInfo();
            LevelInfo.stars = new StarInfo[3];

            for (int i = 0; i < _carrotsGameObjects.Length; i++)
            {
                GameObject carrot = _carrotsGameObjects[i];
                if (carrot != null)
                {
                    CellController cellController = carrot.GetComponent<CellController>();
                    StarInfo carrotInfo = new StarInfo();
                    carrotInfo.id = i + 1;
                    carrotInfo.point = cellController.point;
                    LevelInfo.stars[i] = carrotInfo;
                }
            }

            if (_startGameObject != null)
            {
                CellController cellController = _startGameObject.GetComponent<CellController>();
                LevelInfo.start = cellController.point;
            }

            //LevelInfo.exit = new MapPoint();
            if (_exitGameObject != null)
            {
                CellController cellController = _exitGameObject.GetComponent<CellController>();
                LevelInfo.exit = cellController.point;
            }
            CellInfo[] cellInfoArray = null;
            if (_extrasObjects != null)
            {
                cellInfoArray = new CellInfo[_extrasObjects.Count];

                for (int i = 0; i < cellInfoArray.Length; i++)
                {
                    CellController cellController = _extrasObjects[i].GetComponent<CellController>();

                    CellInfo cellInfo = new CellInfo();
                    cellInfo.cellType = cellController.cellType;
                    cellInfo.point = cellController.point;

                    cellInfoArray[i] = cellInfo;
                }

            }


            LevelInfo.id = _currentLevelId;
            LevelInfo.name = _currentLevelName;
            LevelInfo.map = _currentMapId;
            LevelInfo.hasBoss = _currentHasBoss;
            LevelInfo.extras = cellInfoArray;

            string levelInfoJson = JsonUtility.ToJson(LevelInfo);
            FileUtility.writeFile(getLevelFileName(_currentLevelId), levelInfoJson);
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
            LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(FileUtility.LoadResource(levelFilename));

            string mapFilename = getMapFileName(levelInfo.map);
            MapEditorWrapper wrapper = JsonUtility.FromJson<MapEditorWrapper>(FileUtility.LoadResource(mapFilename));
            LoadMap(wrapper);
            LoadLevel(levelInfo);
        }

        public void LoadMap()
        {
            LoadLevel(_currentLevelId);
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
            LevelInfo = levelInfo;

            DestroyLevelChildren();

            StarInfo[] levelInfoCarrots = levelInfo.stars;

            AddStart(levelInfo.start);
            AddExit(levelInfo.exit);

            for (int i = 0; i < levelInfoCarrots.Length; i++) {
                StarInfo levelInfoCarrot = levelInfoCarrots[i];
                GameObject newCarrot = AddCarrot(levelInfoCarrot.point, levelInfoCarrot.id);
                _carrotsGameObjects[i] = newCarrot;
            }

            CellInfo[] extrasArray = levelInfo.extras;
            if (extrasArray != null) {
                loadExtras(extrasArray);
            }

            _currentMapId = levelInfo.map;
            _currentLevelName = levelInfo.name;
            _currentHasBoss = levelInfo.hasBoss;

            GameManager.Messenger.TriggerMessage(new LevelInfoLoadedMessage(levelInfo));
        }
        private void loadExtras(CellInfo[] extrasArray) {
            _extrasObjects = new List<GameObject>();
            for (int i = 0; i < extrasArray.Length; i++)
            {
                CellInfo cellInfo = extrasArray[i];
                GameObject newExtra = AddElement(getPrefabByCellType(cellInfo.cellType), cellInfo.point);

                CellController cellController = newExtra.GetComponent<CellController>();
                cellController.isExtra = true;
                cellController.reference = i;
                cellController.point = cellInfo.point;
                cellController.cellType = cellInfo.cellType;

                _extrasObjects.Add(newExtra);
                //_carrotsGameObjects[i] = newCarrot;
            }
        }
        public GameObject AddCarrot(MapPoint carrotPoint, int carrotId) {
            GameObject newCarrot = null;
            if (IsEditor)
            {
                newCarrot = AddElement(carrotSimulatorPrefab, carrotPoint);
                if (newCarrot != null) { 
                    newCarrot.GetComponent<CarrotSimulator>().SetCarrotIndex(carrotId - 1);
                }
            } else if (!LevelManager.Instance.Level.IsStarWon(carrotId))
            {
                newCarrot = AddElement(carrotPrefab, carrotPoint);
                if (newCarrot != null)
                {
                    StarCollider carrotStarCollider = newCarrot.GetComponent<StarCollider>();
                    carrotStarCollider.StarNumber = carrotId;
                }
            }
            return newCarrot;
        }
        public void AddStart(MapPoint startPoint)
        {
            _startGameObject = AddElement(startGameObjectPrefab, startPoint);
        }

        public void AddExit(MapPoint exitPoint)
        {
            _exitGameObject = AddElement(exitGameObjectPrefab, exitPoint);

            if (!IsEditor && LevelInfo.hasBoss) {
                ToggleExit(false);
            }
        }
        public void ToggleExit(bool isActive)
        {
            _exitGameObject.SetActive(isActive);
        }
        private GameObject getEditorPrefabByCellType(Constants.CellType type)
        {
            switch (type)
            {
                default:
                case Constants.CellType.GRASS:
                    return grassPrefab;
                case Constants.CellType.EMPTY:
                    return emptyCellPrefab;
                case Constants.CellType.WALL:
                    return wallPrefab;
                case Constants.CellType.WALKABLE_WALL:
                    return walkableWallPrefab;
                case Constants.CellType.FLAME:
                    return flameSimulator;
                case Constants.CellType.ARCHER:
                    return archerSimulator;
                case Constants.CellType.EXTINGUISHER:
                    return extinguisherPrefab;
            }
        }
        private GameObject getPrefabByCellType(Constants.CellType type) {
            if (IsEditor)
            {
                return getEditorPrefabByCellType(type);
            }
            else
            {
                switch (type)
                {
                    default:
                    case Constants.CellType.GRASS:
                        return grassPrefab;
                    //case Constants.CellType.EMPTY:
                    //    return emptyCellPrefab;
                    case Constants.CellType.WALL:
                        return wallPrefab;
                    case Constants.CellType.WALKABLE_WALL:
                        return walkableWallPrefab;
                    case Constants.CellType.FLAME:
                        return flamePrefab;
                    case Constants.CellType.ARCHER:
                        return archerPrefab;
                    case Constants.CellType.EXTINGUISHER:
                        return extinguisherPrefab;
                }
            }
        }
        public void LoadMap(MapEditorWrapper wrapper)
        {
            DestroyMapChildren();

            CellInfo[] cellsArray = wrapper.cells;
            mapArray = new GameObject[_gridY][];
            for (int i = 0; i < mapArray.Length; i++) {
                mapArray[i] = new GameObject[_gridX];
            }

            for (int i = 0; i < cellsArray.Length; i++)
            {
                CellInfo cell = cellsArray[i];
                MapPoint point = cell.point;

                float currentX = getWorldX(point.x);
                float currentY = getWorldY(point.y);

                GameObject prefab = getPrefabByCellType(cell.cellType);
                if (prefab == null) {
                    return;
                }

                GameObject newObject = Instantiate(prefab);

                newObject.name = newObject.name + point.y + EditorConstants.GRID_SEPARATOR + point.x;
                newObject.transform.SetParent(_tilesTransform, false);
                newObject.transform.position = new Vector3(currentX, currentY, 0);

                CellController cellController = newObject.GetComponent<CellController>();
                cellController.point = point;
                cellController.cellType = cell.cellType;
                mapArray[point.y][point.x] = newObject;
            }
        }

        public void UpdatedLevelId(UnityEngine.UI.Text newValue)
        {
            _currentLevelId = int.Parse(newValue.text);
        }

        public void UpdatedMapId(UnityEngine.UI.Text newValue)
        {
            _currentMapId = int.Parse(newValue.text);
        }

        public void UpdatedLevelName(UnityEngine.UI.Text newValue)
        {
            _currentLevelName = newValue.text;
        }
        public void UpdatedHasBoss(UnityEngine.UI.Toggle newValue)
        {
            _currentHasBoss = newValue.isOn;
        }
        public Vector3 getStartTransformPosition() {
            return _startGameObject.transform.position;
        }
    }
}