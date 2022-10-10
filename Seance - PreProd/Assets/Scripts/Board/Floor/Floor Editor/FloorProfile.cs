using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    [System.Serializable]

    [CreateAssetMenu(fileName = "New Floor", menuName = "Tool/New Floor")]
    public class FloorProfile : ScriptableObject
    {
        public List<RoomDisplay> _roomDisplays; //rooms node before floor generation
        public List<ConnectionDisplay> _connections; //and their connections

        public BinaryTreeOfRoom _floor; //rooms in this floor when generated
        public RoomProfile[] _roomProfiles; //existing room to put in in floors


#if UNITY_EDITOR
        public FloorManager.RoomType _currentRoomTypeSelected;
        public Color[] _roomTypeColors;
#endif


        public RoomDisplay GetRoomDisplay(string id)
        {
            for (int i = 0; i<_roomDisplays.Count; i++)
            {
                if (_roomDisplays[i]._id == id)
                    return _roomDisplays[i];
            }

            return null;
        }


    }
}
