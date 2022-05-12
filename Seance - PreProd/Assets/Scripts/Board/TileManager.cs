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

        public Tile[] _tilesInScene;

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

        [ContextMenu("Spawn")]
        public void GenerateRoom()
        {
            if (_tilesInScene.Length > 0)
            {
                for (int i = 0; i< _tilesInScene.Length; i++)
                {
                    if (_tilesInScene[i] != null) 
                        DestroyImmediate(_tilesInScene[i].gameObject);
                }
                _tilesInScene = new Tile[0];
            }


            _tilesInScene = new Tile[_roomShape._xLength * _roomShape._yLength];

            float tileSize = _tilePrefabs[0].transform.lossyScale.x;
            Vector3 originPos = new Vector3(0,0,0);

            for (int x = 0; x < _roomShape._yLength; x++)
            {
                for (int y = 0; y < _roomShape._xLength; y++)
                {
                    Vector3 thisBlockPos = originPos + new Vector3(tileSize* x, 0, tileSize * y) ;

                    switch (_roomShape._tiles[y * _roomShape._yLength + x])
                    {
                        case Tiles.empty:
                            _tilesInScene[x * _roomShape._yLength + y] = null;
                            break;
                        //TODO : implement other tile prefabs
                        case Tiles.characterSpawn:
                        case Tiles.enemySpawn:
                        case Tiles.wall:
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

        public Tile GetTile(int x, int y)
        {
            if (_tilesInScene.Length > x * y)
                return _tilesInScene[x * _roomShape._yLength + y];
            else
                return null;
        }

        public enum Tiles
        {
            empty,
            basicTile,
            wall,
            characterSpawn,
            enemySpawn,
            total //always put this var on last position
        }
    }
}
