using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    [CreateAssetMenu(fileName = "New Room", menuName = "Tool/New Room")]
    public class RoomProfile : ScriptableObject
    {
        public int _roomID;
        public int _xLength;
        public int _yLength;
        public TileManager.Tiles[] _tiles;
        public int[] _tilesWeight;
    #if UNITY_EDITOR
        public TileManager.Tiles _currentTileSelected;
        public Color[] _tilesColor;
    #endif
    }
}
