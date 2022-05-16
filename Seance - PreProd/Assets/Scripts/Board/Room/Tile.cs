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
        
        public void Initialize(int x, int y, TileManager.Tiles tileType)
        {
            _x = x;
            _y = y;
            _thisTileType = tileType;
        }
    }
}
