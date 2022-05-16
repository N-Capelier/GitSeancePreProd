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

        public RoomProfile _roomShape;
        public GameObject[] _tilePrefabs;
        public GameObject[] _enemyPrefabs;
        public GameObject[] _characterPrefabs;

        //instances in scene
        public Tile[] _tilesInScene;
        public Pawn[] _pawnsInScene;
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


        void Start()
        {

        }


        void Update()
        {

        }

        [ContextMenu("Generate Room")]
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

            //init array size
            _tilesInScene = new Tile[_roomShape._xLength * _roomShape._yLength];

            //determine grid and tiles margin ratio
            float tileSize = _tilePrefabs[0].transform.lossyScale.x;
            Vector3 originPos = new Vector3(0, 0, 0);

            //generate tile prefabs
            for (int x = 0; x < _roomShape._yLength; x++)
            {
                for (int y = 0; y < _roomShape._xLength; y++)
                {
                    Vector3 thisBlockPos = originPos + new Vector3(tileSize * x, 0, tileSize * y);

                    switch (_roomShape._tiles[y * _roomShape._yLength + x])
                    {
                        case Tiles.empty:
                            _tilesInScene[x * _roomShape._yLength + y] = null;
                            break;
                        //TODO : implement other tile prefabs
                        case Tiles.characterSpawn:
                        case Tiles.enemySpawn:
                        case Tiles.wall:
                        case Tiles.door:
                        case Tiles.basicTile:
                            GameObject go = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity, transform);
                            go.GetComponent<Tile>()._x = x;
                            go.GetComponent<Tile>()._y = y;
                            go.GetComponent<Tile>()._thisTileType = _roomShape._tiles[y * _roomShape._yLength + x];
                            _tilesInScene[x * _roomShape._yLength + y] = go.GetComponent<Tile>();
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
            Vector3 originPos = new Vector3(0, tileSize, 0); //one level upper than ground tiles in scene

            //generate pawns prefabs
            for (int x = 0; x < _roomShape._yLength; x++)
            {
                for (int y = 0; y < _roomShape._xLength; y++)
                {
                    Vector3 thisBlockPos = originPos + new Vector3(tileSize * x, 0, tileSize * y);

                    switch (_roomShape._tiles[y * _roomShape._yLength + x])
                    {
                        case Tiles.characterSpawn:
                            //spawn pawn
                            GameObject characterPawn = Instantiate(_characterPrefabs[0], thisBlockPos, Quaternion.identity, transform);
                            characterPawn.GetComponent<Pawn>()._x = x;
                            characterPawn.GetComponent<Pawn>()._y = y;
                            characterPawn.GetComponent<Pawn>()._pawnID = _currentNbOfPawnInScene;
                            characterPawn.GetComponent<Pawn>()._pawnType = PawnType.character;
                            _pawnsInScene[_currentNbOfPawnInScene++] = characterPawn.GetComponent<Pawn>();
                            break;
                        case Tiles.enemySpawn:
                            //spawn pawn
                            GameObject enemyPawn = Instantiate(_enemyPrefabs[0], thisBlockPos, Quaternion.identity, transform);
                            enemyPawn.GetComponent<Pawn>()._x = x;
                            enemyPawn.GetComponent<Pawn>()._y = y;
                            enemyPawn.GetComponent<Pawn>()._pawnID = _currentNbOfPawnInScene;
                            enemyPawn.GetComponent<Pawn>()._pawnType = PawnType.enemy;
                            _pawnsInScene[_currentNbOfPawnInScene++] = enemyPawn.GetComponent<Pawn>();
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
            for (int i = 0; i< _pawnsInScene.Length; i++)
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
            for (int i = 0; i<_pawnsInScene.Length; i++)
            {
                //if this pawn is of type we're looking for
                if(_pawnsInScene[i]._pawnType == pt)
                {
                    //
                    if(Vector2.Distance(new Vector2(_pawnsInScene[i]._x, _pawnsInScene[i]._y), new Vector2(xOrigin, yOrigin)) < smallestDistanceRecorded)
                    {
                        smallestDistanceRecorded = Vector2.Distance(new Vector2(_pawnsInScene[i]._x, _pawnsInScene[i]._y), new Vector2(xOrigin, yOrigin));
                        closestPawn = _pawnsInScene[i];
                    }
                }
            }

            return closestPawn;
        }

        public enum Tiles
        {
            empty,
            basicTile,
            wall,
            door,
            characterSpawn,
            enemySpawn,
            total //always put this var on last position
        }

        public enum PawnType
        {
            character,
            enemy,
            total
        }
    }
}
