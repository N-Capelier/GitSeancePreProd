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
            float tileSize = _tilePrefabs[0].transform.lossyScale.x;
            Vector3 originPos = new Vector3(0,0,0);

            for (int x = 0; x < _roomShape._xLength; x++)
            {
                for (int y = 0; y < _roomShape._yLength; y++)
                {
                    Vector3 thisBlockPos = originPos + new Vector3(tileSize* x, 0, tileSize * y) ;

                    switch (_roomShape._tiles[y * _roomShape._yLength + x])
                    {
                        case Tiles.empty:
                            //Do nothing;
                            break;
                        case Tiles.characterSpawn:
                        case Tiles.enemySpawn:
                        case Tiles.basicTile:
                            GameObject go = Instantiate(_tilePrefabs[0], thisBlockPos, Quaternion.identity);
                            break;
                        case Tiles.wall:
                            //TODO : wall
                            break;
                    }
                }
            }
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
