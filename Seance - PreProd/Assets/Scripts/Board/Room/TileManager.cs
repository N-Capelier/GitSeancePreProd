using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    public class TileManager : MonoBehaviour
    {
        //Singleton
        public static TileManager Instance;

        //origin spawn position
        public Vector3 _originPos = Vector3.zero;

        //intances parent (for hierarchy visibility)
        public GameObject _tilesParent;
        public GameObject _otherInstanceParent;
        public GameObject _pawnsParent;

        public RoomProfile _roomShape;
        public GameObject[] _tilePrefabs;
        public GameObject[] _enemyPrefabs;
        public GameObject[] _characterPrefabs;

        //instances in scene
        public Tile[] _tilesInScene;
        public Pawn[] _pawnsInScene;
        public List<GameObject> _otherInstancesInScene;
        //compteur
        public int _currentNbOfPawnInScene;

        public bool isLastRoom = false;


        void Awake()
        {
            #region Make Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            #endregion
        }

        public void GenerateRoom(RoomProfile rp)
        {
            _roomShape = rp;
            GenerateRoom();
            LoadRotationSave();
        }

        //For Room Decoration Editing purposes
        [ContextMenu("Generate Room")]
        public void GenerateRoomEditor()
        {
            GetComponent<FloorManager>().CreateFloor();


            //empty grid and delete game objects
            if (_tilesInScene != null && _tilesInScene.Length > 0)
            {
                for (int i = 0; i < _tilesInScene.Length; i++)
                {
                    if (_tilesInScene[i] != null)
                        DestroyImmediate(_tilesInScene[i].gameObject);
                }
                _tilesInScene = new Tile[0];
            }

            if (_otherInstancesInScene.Count > 0)
            {
                for (int i = 0; i < _otherInstancesInScene.Count; i++)
                {
                    DestroyImmediate(_otherInstancesInScene[i]);
                }
                _otherInstancesInScene.Clear();
            }


            //get nb of door in room
            int nbOfDoorTotal = 0;
            int nbOfDoorActu = 0;

            if (FloorManager.Instance != null)
            {
                if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode != null) nbOfDoorTotal++;
                else if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode != null) nbOfDoorTotal++;
            }



            isLastRoom = false;
            //init array size
            _tilesInScene = new Tile[_roomShape._xLength * _roomShape._yLength];

            //determine grid and tiles margin ratio
            float tileSize = _tilePrefabs[0].transform.lossyScale.x;



            //generate tile prefabs
            for (int x = 0; x < _roomShape._yLength; x++)
            {
                for (int y = 0; y < _roomShape._xLength; y++)
                {
                    Vector3 thisBlockPos = _originPos + new Vector3(tileSize * x, 0, tileSize * y);

                    switch (_roomShape._tiles[y * _roomShape._yLength + x])
                    {
                        case Tiles.empty:
                            _tilesInScene[y * _roomShape._yLength + x] = null;
                            break;
                        case Tiles.wall:
                            //wall ground
                            GameObject wallGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            wallGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            wallGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = wallGround.GetComponent<Tile>();

                            GameObject wall = Instantiate(_tilePrefabs[1], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _otherInstanceParent.transform);
                            wall.transform.rotation = _tilePrefabs[1].transform.rotation;
                            wall.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _otherInstancesInScene.Add(wall);
                            break;
                        case Tiles.column:
                            //column ground
                            GameObject columnGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            columnGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            columnGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = columnGround.GetComponent<Tile>();

                            GameObject column = Instantiate(_tilePrefabs[3], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _otherInstanceParent.transform);
                            column.transform.rotation = _tilePrefabs[3].transform.rotation;
                            column.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _otherInstancesInScene.Add(column);
                            break;
                        case Tiles.angle:
                            //column ground
                            GameObject angleGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            angleGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            angleGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = angleGround.GetComponent<Tile>();

                            GameObject angle = Instantiate(_tilePrefabs[4], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _otherInstanceParent.transform);
                            angle.transform.rotation = _tilePrefabs[4].transform.rotation;
                            angle.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _otherInstancesInScene.Add(angle);
                            break;
                        case Tiles.door:
                            //block under door
                            GameObject doorBlock = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            doorBlock.transform.rotation = _tilePrefabs[2].transform.rotation;
                            doorBlock.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = doorBlock.GetComponent<Tile>();

                            GameObject door = Instantiate(_tilePrefabs[2], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _otherInstanceParent.transform);
                            door.transform.rotation = _tilePrefabs[2].transform.rotation;
                            _otherInstancesInScene.Add(door);

                            //apply reference of next room to appropriate door
                            if (nbOfDoorTotal == 2)
                            {
                                if (nbOfDoorActu == 0)
                                {
                                    nbOfDoorActu++;
                                    door.GetComponent<Door>()._linkedRoom = FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode;
                                }
                                if (nbOfDoorActu == 1)
                                {
                                    nbOfDoorActu++;
                                    door.GetComponent<Door>()._linkedRoom = FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode;
                                }
                            }
                            else if (nbOfDoorTotal == 1)
                            {
                                if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode != null)
                                {
                                    nbOfDoorActu++;
                                    door.GetComponent<Door>()._linkedRoom = FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode;
                                }
                                else if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode != null)
                                {
                                    nbOfDoorActu++;
                                    door.GetComponent<Door>()._linkedRoom = FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode;
                                }
                            }
                            else
                            {
                                //no door in the room => last room (boos)
                            }

                            break;
                        case Tiles.oil:
                            GameObject oilGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            oilGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            oilGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);

                            //add oil script
                            oilGround.AddComponent<Oil>();
                            _tilesInScene[y * _roomShape._yLength + x] = oilGround.GetComponent<Tile>();

                            break;
                        case Tiles.chest:
                            GameObject chestGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            chestGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            chestGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = chestGround.GetComponent<Tile>();

                            //TODO : add prefab "chest"
                            //waiting for nico's part

                            break;
                        case Tiles.characterSpawn:
                        case Tiles.enemySpawn1:
                        case Tiles.enemySpawn2:
                        case Tiles.basicTile:
                            GameObject go = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            go.transform.rotation = _tilePrefabs[0].transform.rotation;
                            go.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = go.GetComponent<Tile>();
                            break;
                    }
                }
            }
            SpawnPawns();
            NextRoomFeedback();
            LoadRotationSave();
        }

        public void GenerateRoom()
        {
            //empty grid and delete game objects
            if (_tilesInScene.Length > 0)
            {
                for (int i = 0; i < _tilesInScene.Length; i++)
                {
                    if (_tilesInScene[i] != null)
                        DestroyImmediate(_tilesInScene[i].gameObject);
                }
                _tilesInScene = new Tile[0];
            }
            if (_otherInstancesInScene.Count > 0)
            {
                for (int i = 0; i < _otherInstancesInScene.Count; i++)
                {
                    DestroyImmediate(_otherInstancesInScene[i]);
                }
                _otherInstancesInScene.Clear();
            }

            //init var
            isLastRoom = false;

            //get nb of door in room
            int nbOfDoorTotal = 0;
            int nbOfDoorActu = 0;
            if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode != null) nbOfDoorTotal++;
            else if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode != null) nbOfDoorTotal++;

            //init array size
            _tilesInScene = new Tile[_roomShape._xLength * _roomShape._yLength];

            //determine grid and tiles margin ratio
            float tileSize = _tilePrefabs[0].transform.lossyScale.x;

            //generate tile prefabs
            for (int x = 0; x < _roomShape._yLength; x++)
            {
                for (int y = 0; y < _roomShape._xLength; y++)
                {
                    Vector3 thisBlockPos = _originPos + new Vector3(tileSize * x, 0, tileSize * y);

                    switch (_roomShape._tiles[y * _roomShape._yLength + x])
                    {
                        case Tiles.empty:
                            _tilesInScene[y * _roomShape._yLength + x] = null;
                            break;
                        case Tiles.wall:
                            //wall ground
                            GameObject wallGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            wallGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            wallGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = wallGround.GetComponent<Tile>();

                            GameObject wall = Instantiate(_tilePrefabs[1], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _otherInstanceParent.transform);
                            wall.transform.rotation = _tilePrefabs[1].transform.rotation;
                            wall.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _otherInstancesInScene.Add(wall);
                            break;
                        case Tiles.column:
                            //column ground
                            GameObject columnGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            columnGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            columnGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = columnGround.GetComponent<Tile>();

                            GameObject column = Instantiate(_tilePrefabs[3], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _otherInstanceParent.transform);
                            column.transform.rotation = _tilePrefabs[3].transform.rotation;
                            column.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _otherInstancesInScene.Add(column);
                            break;
                        case Tiles.angle:
                            //column ground
                            GameObject angleGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            angleGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            angleGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = angleGround.GetComponent<Tile>();

                            GameObject angle = Instantiate(_tilePrefabs[4], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _otherInstanceParent.transform);
                            angle.transform.rotation = _tilePrefabs[3].transform.rotation;
                            angle.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _otherInstancesInScene.Add(angle);
                            break;
                        case Tiles.door:
                            //block under door
                            GameObject doorBlock = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            doorBlock.transform.rotation = _tilePrefabs[0].transform.rotation;
                            doorBlock.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            doorBlock.transform.rotation = _tilePrefabs[0].transform.rotation;
                            _tilesInScene[y * _roomShape._yLength + x] = doorBlock.GetComponent<Tile>();

                            GameObject door = Instantiate(_tilePrefabs[2], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _otherInstanceParent.transform);
                            door.transform.rotation = _tilePrefabs[2].transform.rotation;
                            _otherInstancesInScene.Add(door);

                            //apply reference of next room to appropriate door
                            if (nbOfDoorTotal == 2)
                            {
                                if (nbOfDoorActu == 0)
                                {
                                    nbOfDoorActu++;
                                    door.GetComponent<Door>()._linkedRoom = FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode;
                                }
                                if (nbOfDoorActu == 1)
                                {
                                    nbOfDoorActu++;
                                    door.GetComponent<Door>()._linkedRoom = FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode;
                                }
                            }
                            else if (nbOfDoorTotal == 1)
                            {
                                if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode != null)
                                {
                                    nbOfDoorActu++;
                                    door.GetComponent<Door>()._linkedRoom = FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode;
                                }
                                else if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode != null)
                                {
                                    nbOfDoorActu++;
                                    door.GetComponent<Door>()._linkedRoom = FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode;
                                }
                            }
                            else
                            {
                                //no door in the room => last room (boss)
                                isLastRoom = true;
                            }
                            break;
                        case Tiles.oil:
                            GameObject oilGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            oilGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            oilGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);

                            //add oil script
                            oilGround.AddComponent<Oil>();
                            _tilesInScene[y * _roomShape._yLength + x] = oilGround.GetComponent<Tile>();

                            break;
                        case Tiles.chest:
                            GameObject chestGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            chestGround.transform.rotation = _tilePrefabs[0].transform.rotation;
                            chestGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = chestGround.GetComponent<Tile>();

                            //TODO : add prefab "chest"
                            //waiting for nico's part

                            break;
                        case Tiles.characterSpawn:
                        case Tiles.enemySpawn1:
                        case Tiles.enemySpawn2:
                        case Tiles.basicTile:
                            GameObject basicTile = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _tilesParent.transform);
                            basicTile.transform.rotation = _tilePrefabs[0].transform.rotation;
                            basicTile.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[y * _roomShape._yLength + x] = basicTile.GetComponent<Tile>();
                            break;
                    }
                }
            }
        }

        [ContextMenu("Spawn Pawns")]
        public void SpawnPawns()
        {
            //empty grid and delete game objects
            if (_pawnsInScene.Length > 0)
            {
                for (int i = 0; i < _pawnsInScene.Length; i++)
                {
                    if (_pawnsInScene[i] != null)
                        DestroyImmediate(_pawnsInScene[i].gameObject);
                }
                _pawnsInScene = new Pawn[0];
            }

            //init array size
            _pawnsInScene = new Pawn[20]; //TODO : Get actual nb of pawn in scene
            _currentNbOfPawnInScene = 0;

            //determine grid and tiles margin ratio
            float tileSize = _tilePrefabs[0].transform.lossyScale.x;

            Vector3 originPos = _originPos + new Vector3(0, tileSize, 0); //one level upper than ground tiles in scene

            //generate pawns prefabs
            for (int x = 0; x < _roomShape._yLength; x++)
            {
                for (int y = 0; y < _roomShape._xLength; y++)
                {
                    Vector3 thisBlockPos = originPos + new Vector3(tileSize * x, 0, tileSize * y);

                    switch (_roomShape._tiles[y * _roomShape._yLength + x])
                    {

                        case Tiles.characterSpawn:
                            Tile _pawnSpawn = GetTile(x, y);

                            for (int i = 0; i < _roomShape._tilesWeight[y * _roomShape._yLength + x]; i++)
                            {
                                //spawn pawn
                                GameObject characterPawn = Instantiate(_characterPrefabs[0], thisBlockPos, Quaternion.identity, _pawnsParent.transform);
                                characterPawn.transform.rotation = _tilePrefabs[2].transform.rotation;
                                characterPawn.GetComponent<CharacterPawn>().Initialize(x, y, 4, 0, 4, HeroType.Wizard, _currentNbOfPawnInScene);
                                _pawnsInScene[_currentNbOfPawnInScene++] = characterPawn.GetComponent<Pawn>();
                                _pawnSpawn._pawnsOnTile.Add(characterPawn.GetComponent<Pawn>());
                            }

                            _pawnSpawn.UpdatePawnsPositionOnTile();
                            
                            break;
                        case Tiles.enemySpawn1:
                            Tile _pawnSpawn2 = GetTile(x, y);

                            for (int i = 0; i < _roomShape._tilesWeight[y * _roomShape._yLength + x]; i++)
                            {
                                //spawn pawn
                                GameObject enemyPawn = Instantiate(_enemyPrefabs[0], thisBlockPos, Quaternion.identity, _pawnsParent.transform);
                                enemyPawn.transform.rotation = _tilePrefabs[2].transform.rotation;
                                enemyPawn.GetComponent<EnemyPawn>().Initialize(x, y, 4, 0, 4, EnemyType.enemy1, _currentNbOfPawnInScene);
                                _pawnsInScene[_currentNbOfPawnInScene++] = enemyPawn.GetComponent<Pawn>();
                                _pawnSpawn2._pawnsOnTile.Add(enemyPawn.GetComponent<Pawn>());
                            }

                            _pawnSpawn2.UpdatePawnsPositionOnTile();
                           
                            break;
                        case Tiles.enemySpawn2:
                            Tile _pawnSpawn3 = GetTile(x, y);

                            for (int i = 0; i < _roomShape._tilesWeight[y * _roomShape._yLength + x]; i++)
                            {
                                //spawn pawn
                                GameObject enemyPawn2 = Instantiate(_enemyPrefabs[1], thisBlockPos, Quaternion.identity, _pawnsParent.transform);
                                enemyPawn2.transform.rotation = _tilePrefabs[2].transform.rotation;
                                enemyPawn2.GetComponent<EnemyPawn>().Initialize(x, y, 4, 0, 4, EnemyType.enemy2, _currentNbOfPawnInScene);
                                _pawnsInScene[_currentNbOfPawnInScene++] = enemyPawn2.GetComponent<EnemyPawn>();
                                _pawnSpawn3._pawnsOnTile.Add(enemyPawn2.GetComponent<Pawn>());
                            }

                            _pawnSpawn3.UpdatePawnsPositionOnTile();
                        
                            break;
                    }
                }
            }
        }

        [ContextMenu("Save Tiles Rotation")]
        public void SaveTileRotation()
        {

            if (_roomShape._tileRotationSave.Length < 1)
            {
                Debug.Log("initialise save list");
                _roomShape._tileRotationSave = new Quaternion[_roomShape._xLength * _roomShape._yLength];
                _roomShape._otherTileRotationSave = new Quaternion[_roomShape._xLength * _roomShape._yLength];
            }


            //ground tile (z = 0)
            for (int i = 0; i < _tilesInScene.Length; i++)
            {
                if (_tilesInScene[i] != null)
                {
                    _tilesInScene[i].SaveRotation();

                    //apply save to room profile
                    _roomShape._tileRotationSave[i] = _tilesInScene[i]._savedRot;
                }
            }

            //tile on top (z = 1)
            for (int i = 0; i < _otherInstancesInScene.Count; i++)
            {
                if (_otherInstancesInScene[i] != null)
                {
                    _otherInstancesInScene[i].GetComponent<Tile>().SaveRotation();

                    //apply save to room profile
                    _roomShape._otherTileRotationSave[i] = _otherInstancesInScene[i].GetComponent<Tile>()._savedRot;
                }
            }

        }

        [ContextMenu("Load Rotation Save")]
        public void LoadRotationSave()
        {
            //ground tile (z = 0)
            for (int i = 0; i < _roomShape._tileRotationSave.Length; i++)
            {
                if (_tilesInScene[i] != null && _roomShape._tileRotationSave[i] != null)
                {
                    //load tile from save
                    _tilesInScene[i]._savedRot = _roomShape._tileRotationSave[i];
                    _tilesInScene[i].ApplySavedRotation();
                }
            }

            //tile on top (z = 1)
            for (int i = 0; i < _otherInstancesInScene.Count; i++)
            {
                if (_otherInstancesInScene[i] != null && _roomShape._otherTileRotationSave.Length > i)
                {
                    //apply save manualy
                    _otherInstancesInScene[i].GetComponent<Tile>()._savedRot = _roomShape._otherTileRotationSave[i];
                    _otherInstancesInScene[i].GetComponent<Tile>().ApplySavedRotation();
                }
            }
        }

        /*[ContextMenu("Apply Saved Rotation to Tiles")]
        public void ApplySavedRotationToTiles()
        {
            for (int i = 0; i < _tilesInScene.Length; i++)
            {
                if (_tilesInScene[i] != null) _tilesInScene[i].ApplySavedRotation();
            }
            for (int i = 0; i < _otherInstancesInScene.Count; i++)
            {
                if (_otherInstancesInScene[i] != null) _otherInstancesInScene[i].GetComponent<Tile>().ApplySavedRotation();
            }
        }*/

        List<GameObject> storedTile = new List<GameObject>();

        public void NextRoomFeedback()
        {
            Debug.Log("called");
            int counter = 0;

            for (int i = 0; i < _pawnsInScene.Length; i++)
            {
                if (_pawnsInScene[i] !=null)
                {
                    if (_pawnsInScene[i]._pawnType == PawnType.Enemy)
                    {
                        counter++;
                    }
                }
            }

            if (counter ==0)
            {
                foreach (Transform item in _otherInstanceParent.transform)
                {
                    storedTile.Add(item.gameObject);
                }

                foreach (var item in storedTile)
                {
                    if(item != null)
                    {
                        if (item.GetComponent<Door>() != null)
                        {
                            item.GetComponent<Door>().UpdateIcon();
                        }
                    }  
                }
            }
        }
        public Tile GetTile(int x, int y)
        {
            if (_tilesInScene.Length >= x * y)
                return _tilesInScene[y * _roomShape._yLength + x];
            else
                return null;
        }

        public Pawn GetPawn(int pawnID)
        {
            for (int i = 0; i < _pawnsInScene.Length; i++)
            {
                if (_pawnsInScene[i]._pawnID == pawnID)
                {
                    return _pawnsInScene[i];
                }
            }
            return null; //when player doesnt exist
        }

        public Pawn GetClosestPawn(int xOrigin, int yOrigin, PawnType pt)
        {
            float smallestDistanceRecorded = float.PositiveInfinity;
            Pawn closestPawn = null;

            //looking between every pawn in scene
            for (int i = 0; i < _pawnsInScene.Length; i++)
            {
                //if this pawn is of type we're looking for
                if (_pawnsInScene[i]._pawnType == pt)
                {
                    //
                    if (Vector2.Distance(new Vector2(_pawnsInScene[i]._x, _pawnsInScene[i]._y), new Vector2(xOrigin, yOrigin)) < smallestDistanceRecorded)
                    {
                        smallestDistanceRecorded = Vector2.Distance(new Vector2(_pawnsInScene[i]._x, _pawnsInScene[i]._y), new Vector2(xOrigin, yOrigin));
                        closestPawn = _pawnsInScene[i];
                    }
                }
            }

            return closestPawn;
        }

        public Pawn GetPawnOn(int x, int y)
        {
            for (int i = 0; i < _pawnsInScene.Length; i++)
            {
                if (_pawnsInScene[i]._x == x && _pawnsInScene[i]._y == y)
                {
                    return _pawnsInScene[i];
                }
            }

            //no pawn on position x,y on board
            return null;
        }

        public Pawn[] GetPawnsOn(int x, int y)
        {
            List<Pawn> _pawnList = new List<Pawn>();

            for (int i = 0; i < _pawnsInScene.Length; i++)
            {
                if (_pawnsInScene[i]._x == x && _pawnsInScene[i]._y == y)
                {
                    _pawnList.Add(_pawnsInScene[i]);
                }
            }

            return _pawnList.ToArray();
        }

        public enum Tiles
        {
            empty,
            basicTile,
            wall,
            door,
            characterSpawn,
            enemySpawn1,
            enemySpawn2,
            oil,
            chest,
            column,
            angle,//always add new tile type on last position
            total //always put this var on last position
        }

    }
}
