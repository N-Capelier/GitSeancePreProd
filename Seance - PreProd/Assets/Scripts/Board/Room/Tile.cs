using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>
    
    public class Tile : MonoBehaviour
    {
        public int _x;
        public int _y;
        public TileManager.Tiles _thisTileType;
        public List<Pawn> _pawnsOnTile;
        public int _nbOfPawnOnStart = 0;
        public int _maxPawnOnTile = 4;

        [SerializeField] private MeshRenderer tileRenderer;
        [SerializeField] private Material color1;
        [SerializeField] private Material color2;

        //for angle
        public Quaternion _savedRot;

        public void ApplySavedRotation()
        {
            transform.rotation = _savedRot;
        }

        public void SaveRotation()
        {
            _savedRot = transform.rotation;
        }
        
        public void Initialize(int x, int y, TileManager.Tiles tileType)
        {
            _x = x;
            _y = y;

            if (tileRenderer != null)
            {
                //pair
                if ((_x % 2 == 0 && _y % 2 == 1) ||(_x %2 ==1 && _y % 2 ==0))
                {
                    tileRenderer.material = color1;
                }
                else
                {
                    tileRenderer.material = color2;
                }
            }

            _thisTileType = tileType;

           
        }
    }
}
