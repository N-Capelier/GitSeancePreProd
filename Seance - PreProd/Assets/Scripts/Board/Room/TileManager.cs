using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Seance.CardSystem;
using Seance.Management;
using FishNet.Object;
using System.Linq;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    public class TileManager : NetworkBehaviour
    {
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
        public List<CharacterPawn> _characterPawnsInScene;
        public List<GameObject> _otherInstancesInScene;
        //compteur
        public int _currentNbOfPawnInScene;

        public bool isLastRoom = false;

        static TileManager _instance;
        static public TileManager Instance
        {
            get { return _instance; }
        }

        GameManager _gManager;

        public Deck _rangerDeck;
        public Deck _wizardDeck;
        public Deck _knightDeck;

        void Awake()
        {
            CreateSingleton();
        }

        private void Start()
        {
            _gManager = GameManager.Instance;
        }

        void CreateSingleton()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void GenerateRoom(RoomProfile rp)
        {
            _roomShape = rp;
            GenerateRoom();
            SpawnPawns();
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
            bool isInPlayMode = false;
            if (FloorManager.Instance != null)
            {
                isInPlayMode = true;
            }

            if (_pawnsInScene.Length > 0 && !isInPlayMode)
            {
                for (int i = 0; i < _pawnsInScene.Length; i++)
                {
                    if (_pawnsInScene[i] != null)
                        DestroyImmediate(_pawnsInScene[i].gameObject);
                }
                _pawnsInScene = new Pawn[50];
            }
            else if (_pawnsInScene.Length == 0)
            {
                Debug.Log("enter here");
                _pawnsInScene = new Pawn[50];
            }
            _currentNbOfPawnInScene = 0;

            //get nb of pawn in already in _pawnInScene
            int nbOfPawn = 0;
            int nbOfPawnCurr = 0;
            for (int i = 0; i < _pawnsInScene.Length; i++)
            {
                if (_pawnsInScene[i] != null && _pawnsInScene[i]._pawnType == PawnType.Character)
                {
                    _characterPawnsInScene.Add(_pawnsInScene[i] as CharacterPawn);
                    nbOfPawn++;
                }
            }

            //determine grid and tiles margin ratio
            float tileSize = _tilePrefabs[0].transform.lossyScale.x;

            Vector3 originPos = _originPos + new Vector3(0, tileSize, 0); //one level upper than ground tiles in scene

            int spawnedCharacterPawnsCount = 0;
            HeroType heroType;

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

                            if (!isInPlayMode || (nbOfPawn == 0 && isInPlayMode))
                            {
                                for (int i = 0; i < _roomShape._tilesWeight[y * _roomShape._yLength + x]; i++)
                                {
                                    //spawn pawn
                                    GameObject characterPawn = Instantiate(_characterPrefabs[0], thisBlockPos, Quaternion.identity, _pawnsParent.transform);
                                    characterPawn.transform.rotation = _tilePrefabs[2].transform.rotation;

                                    switch (spawnedCharacterPawnsCount)
                                    {
                                        case 0:
                                            heroType = HeroType.Ranger;
                                            break;
                                        case 1:
                                            heroType = HeroType.Wizard;
                                            break;
                                        case 2:
                                            heroType = HeroType.Knight;
                                            break;
                                        default:
                                            throw new System.ArgumentOutOfRangeException("Wrong character pawn index.");
                                    }

                                    characterPawn.GetComponent<CharacterPawn>().Initialize(x, y, 4, 0, 4, heroType, _currentNbOfPawnInScene);
                                    _pawnsInScene[_currentNbOfPawnInScene++] = characterPawn.GetComponent<Pawn>();
                                    _pawnSpawn._pawnsOnTile.Add(characterPawn.GetComponent<Pawn>());

                                    int newPawnIndex = _currentNbOfPawnInScene - 1;

                                    //for editor testing purposes
                                    if (_gManager != null)
                                    {
                                        //network
                                        if (_gManager._lobby.IsServer)
                                        {
                                            _gManager._lobby._ownedPlayer.ServerRpcSetPawn(spawnedCharacterPawnsCount, newPawnIndex);
                                            _gManager._lobby._ownedPlayer.ServerRpcInitZones(spawnedCharacterPawnsCount);
                                        }
                                        spawnedCharacterPawnsCount++;
                                    }
                                }
                            }
                            else if (isInPlayMode && nbOfPawn > 0)
                            {
                                //read when fnct called from OpenDoor (to resume)
                                for (int i = 0; i < _roomShape._tilesWeight[y * _roomShape._yLength + x]; i++)
                                {
                                    GameObject characterPawn = _characterPawnsInScene[nbOfPawnCurr++].gameObject;
                                    characterPawn.transform.position = thisBlockPos;
                                    characterPawn.GetComponent<CharacterPawn>().ResetPosition(x, y);
                                    _pawnSpawn._pawnsOnTile.Add(characterPawn.GetComponent<Pawn>());

                                    int newPawnIndex = _currentNbOfPawnInScene - 1;

                                    //for editor testing purposes
                                    if (_gManager != null)
                                    {
                                        //network
                                        if (_gManager._lobby.IsServer)
                                        {
                                            _gManager._lobby._ownedPlayer.ServerRpcSetPawn(spawnedCharacterPawnsCount, newPawnIndex);
                                            _gManager._lobby._ownedPlayer.ServerRpcInitZones(spawnedCharacterPawnsCount);
                                        }
                                        spawnedCharacterPawnsCount++;
                                    }
                                }
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

            for (int i = 0; i < _pawnsInScene.Length; i++)
            {
                if (_pawnsInScene[i] == null)
                    continue;
                _pawnsInScene[i]._pawnID = i;
            }

        }

        /*
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

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_roomShape);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif

        }*/

        [ContextMenu("Load Rotation Save")]
        public void LoadRotationSave()
        {

            for (int x = 0; x < _roomShape._yLength; x++)
            {
                for (int y = 0; y < _roomShape._xLength; y++)
                {
                    if (_tilesInScene[y * _roomShape._yLength + x] != null && (y * _roomShape._yLength + x >= _roomShape._yLength * _roomShape._xLength))
                    {
                        //Debug.Log(y * _roomShape._yLength + x + " : " + _roomShape._newTileRotation[y * _roomShape._yLength + x]);
                        _tilesInScene[y * _roomShape._yLength + x]._newSavedRot = _roomShape._newTileRotation[y * _roomShape._yLength + x];
                        _tilesInScene[y * _roomShape._yLength + x].ApplySavedRotation();
                    }
                }
            }

            for (int i = 0; i < _otherInstancesInScene.Count; i++)
            {
                Tile thisTile = _otherInstancesInScene[i].GetComponent<Tile>();
                thisTile._newSavedRot = _roomShape._newOtherTileRotation[thisTile._y * _roomShape._yLength + thisTile._x];
                thisTile.ApplySavedRotation();
                //Debug.Log(i + " : " + thisTile._newSavedRot);
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
            int counter = 0;

            for (int i = 0; i < _pawnsInScene.Length; i++)
            {
                if (_pawnsInScene[i] != null)
                {
                    if (_pawnsInScene[i]._pawnType == PawnType.Enemy)
                    {
                        counter++;
                    }
                }
            }

            if (counter == 0)
            {
                foreach (Transform item in _otherInstanceParent.transform)
                {
                    storedTile.Add(item.gameObject);
                }

                foreach (var item in storedTile)
                {
                    if (item != null)
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
                if (_pawnsInScene[i] == null)
                    continue;

                if (_pawnsInScene[i]._x == x && _pawnsInScene[i]._y == y)
                {
                    _pawnList.Add(_pawnsInScene[i]);
                }
            }

            return _pawnList.ToArray();
        }

        #region Network Methods

        [ServerRpc(RequireOwnership = false)]
        public void ServerRpcChangePositionTo(int id, int x, int y)
        {
            ObserverRpcChangePawnPositionTo(id, x, y);
        }

        [ObserversRpc]
        void ObserverRpcChangePawnPositionTo(int id, int x, int y)
        {
            foreach (Pawn pawn in _pawnsInScene)
            {
                if (pawn == null)
                    continue;
                if (pawn._pawnID == id)
                {
                    pawn.ChangePositionTo(x, y);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerRpcPawnTakeDamage(int id, int damages)
        {
            ObserverRpcPawnTakeDamage(id, damages);
        }

        [ObserversRpc]
        void ObserverRpcPawnTakeDamage(int id, int damages)
        {
            foreach (Pawn pawn in _pawnsInScene)
            {
                if (pawn == null)
                    continue;
                if (pawn._pawnID == id)
                {
                    pawn.TakeDamage(damages);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerRpcPawnDeath(int id)
        {
            ObserverRpcPawnDeath(id);
        }

        [ObserversRpc]
        void ObserverRpcPawnDeath(int id)
        {
            foreach (Pawn pawn in _pawnsInScene)
            {
                if (pawn == null)
                    continue;
                if (pawn._pawnID == id)
                {
                    pawn.Die();
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerRpcPawnHeal(int id, int amount)
        {
            ObserverRpcPawnHeal(id, amount);
        }

        [ObserversRpc]
        void ObserverRpcPawnHeal(int id, int amount)
        {
            foreach (Pawn pawn in _pawnsInScene)
            {
                if (pawn == null)
                    continue;
                if (pawn._pawnID == id)
                {
                    pawn.Heal(amount);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerRpcPawnGainArmor(int id, int amount)
        {
            ObserverRpcPawnGainArmor(id, amount);
        }

        [ObserversRpc]
        void ObserverRpcPawnGainArmor(int id, int amount)
        {
            foreach (Pawn pawn in _pawnsInScene)
            {
                if (pawn == null)
                    continue;
                if (pawn._pawnID == id)
                {
                    pawn.GainArmor(amount);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerRpcPawnDecreaseArmor(int id, int amount)
        {
            ObserverRpcPawnDecreaseArmor(id, amount);
        }

        [ObserversRpc]
        void ObserverRpcPawnDecreaseArmor(int id, int amount)
        {
            foreach (Pawn pawn in _pawnsInScene)
            {
                if (pawn == null)
                    continue;
                if (pawn._pawnID == id)
                {
                    pawn.DecreaseArmor(amount);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerRpcPawnPurify(int id, int amount)
        {
            ObserverRpcPawnPurify(id, amount);
        }

        [ObserversRpc]
        void ObserverRpcPawnPurify(int id, int amount)
        {
            foreach (Pawn pawn in _pawnsInScene)
            {
                if (pawn == null)
                    continue;
                if (pawn._pawnID == id)
                {
                    CharacterPawn character = pawn as CharacterPawn;
                    character.Purify(amount);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerRpcPawnCorrupt(int id, int amount)
        {
            ObserverRpcPawnCorrupt(id, amount);
        }

        [ObserversRpc]
        void ObserverRpcPawnCorrupt(int id, int amount)
        {
            foreach (Pawn pawn in _pawnsInScene)
            {
                if (pawn == null)
                    continue;
                if (pawn._pawnID == id)
                {
                    CharacterPawn character = pawn as CharacterPawn;
                    character.Corrupt(amount);
                }
            }
        }

        #endregion

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
