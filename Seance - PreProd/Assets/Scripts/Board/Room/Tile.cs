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
        
        public void Initialize(int x, int y, TileManager.Tiles tileType)
        {
            _x = x;
            _y = y;
            _thisTileType = tileType;
        }
    }
}
