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
        public GameObject _instanceParent;

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
        }

        //For Room Decoration Editing purposes
        [ContextMenu("Generate Room")]
        public void GenerateRoomEditor()
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

            /*Transform[] oldRoomTiles = _instanceParent.GetComponentsInChildren<Transform>();
            for (int i = 0; i<oldRoomTiles.Length; i++)
            {
                DestroyImmediate(oldRoomTiles[i].gameObject);
            }*/

            //get nb of door in room
            /*int nbOfDoorTotal = 0;
            int nbOfDoorActu = 0;
            if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._leftNode != null) nbOfDoorTotal++;
            else if (FloorManager.Instance._rooms._floor[FloorManager.Instance._playersPositionInFloor]._rightNode != null) nbOfDoorTotal++;
            */
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
                            _tilesInScene[x * _roomShape._yLength + y] = null;
                            break;
                        case Tiles.wall:
                            //block under wall
                            GameObject wallBlock = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, transform);
                            wallBlock.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[x * _roomShape._yLength + y] = wallBlock.GetComponent<Tile>();

                            GameObject wall = Instantiate(_tilePrefabs[1], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _instanceParent.transform);
                            _otherInstancesInScene.Add(wall);
                            break;
                        case Tiles.door:
                            //block under door
                            GameObject doorBlock = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, transform);
                            doorBlock.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[x * _roomShape._yLength + y] = doorBlock.GetComponent<Tile>();

                            GameObject door = Instantiate(_tilePrefabs[2], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _instanceParent.transform);
                            _otherInstancesInScene.Add(door);

                            //apply reference of next room to appropriate door
                            /*if(nbOfDoorTotal == 2)
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
                            */
                            break;
                        case Tiles.characterSpawn:
                        case Tiles.enemySpawn1:
                        case Tiles.enemySpawn2:
                        case Tiles.basicTile:
                            GameObject go = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, transform);
                            go.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[x * _roomShape._yLength + y] = go.GetComponent<Tile>();
                            break;
                    }
                }
            }
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
                            _tilesInScene[x * _roomShape._yLength + y] = null;
                            break;
                        case Tiles.wall:
                            //block under wall
                            GameObject wallBlock = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _instanceParent.transform);
                            wallBlock.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[x * _roomShape._yLength + y] = wallBlock.GetComponent<Tile>();

                            GameObject wall = Instantiate(_tilePrefabs[1], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _instanceParent.transform);
                            _otherInstancesInScene.Add(wall);
                            break;
                        case Tiles.door:
                            //block under door
                            GameObject doorBlock = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _instanceParent.transform);
                            doorBlock.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[x * _roomShape._yLength + y] = doorBlock.GetComponent<Tile>();

                            GameObject door = Instantiate(_tilePrefabs[2], thisBlockPos + new Vector3(0, tileSize, 0), Quaternion.identity, _instanceParent.transform);
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
                                //no door in the room => last room (boos)
                            }
                            break;
                        case Tiles.oil:
                            GameObject oilGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _instanceParent.transform);
                            oilGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);

                            //add oil script
                            oilGround.AddComponent<Oil>();
                            _tilesInScene[x * _roomShape._yLength + y] = oilGround.GetComponent<Tile>();

                            break;
                        case Tiles.chest:
                            GameObject chestGround = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _instanceParent.transform);
                            chestGround.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[x * _roomShape._yLength + y] = chestGround.GetComponent<Tile>();

                            //TODO : add prefab "chest"
                            //waiting for nico's part

                            break;
                        case Tiles.characterSpawn:
                        case Tiles.enemySpawn1:
                        case Tiles.enemySpawn2:
                        case Tiles.basicTile:
                            GameObject basicTile = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, _instanceParent.transform);
                            basicTile.GetComponent<Tile>().Initialize(x, y, _roomShape._tiles[y * _roomShape._yLength + x]);
                            _tilesInScene[x * _roomShape._yLength + y] = basicTile.GetComponent<Tile>();
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
                            //TODO : change pawn place
                            for (int i = 0; i < _roomShape._tilesWeight[y * _roomShape._yLength + x]; i++)
                            {
                                //spawn pawn
                                GameObject characterPawn = Instantiate(_characterPrefabs[0], thisBlockPos, Quaternion.identity, transform);
                                characterPawn.GetComponent<CharacterPawn>().Initialize(x, y, 4, 0, 4, HeroType.Wizard, _currentNbOfPawnInScene);
                                _pawnsInScene[_currentNbOfPawnInScene++] = characterPawn.GetComponent<Pawn>();
                            }
                            break;
                        case Tiles.enemySpawn1:
                            //TODO : change pawn place
                            for (int i = 0; i < _roomShape._tilesWeight[y * _roomShape._yLength + x]; i++)
                            {
                                //spawn pawn
                                GameObject enemyPawn = Instantiate(_enemyPrefabs[0], thisBlockPos, Quaternion.identity, transform);
                                enemyPawn.GetComponent<EnemyPawn>().Initialize(x, y, 4, 0, 4, EnemyType.enemy1, _currentNbOfPawnInScene);
                                _pawnsInScene[_currentNbOfPawnInScene++] = enemyPawn.GetComponent<Pawn>();
                            }

                            break;
                        case Tiles.enemySpawn2:
                            //TODO : change pawn place
                            for (int i = 0; i < _roomShape._tilesWeight[y * _roomShape._yLength + x]; i++)
                            {
                                //spawn pawn
                                GameObject enemyPawn2 = Instantiate(_enemyPrefabs[1], thisBlockPos, Quaternion.identity, transform);
                                enemyPawn2.GetComponent<EnemyPawn>().Initialize(x, y, 4, 0, 4, EnemyType.enemy2, _currentNbOfPawnInScene);
                                _pawnsInScene[_currentNbOfPawnInScene++] = enemyPawn2.GetComponent<EnemyPawn>();
                            }

                            break;
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
            chest,//always add new tile type on last position
            total //always put this var on last position
        }

    }
}
