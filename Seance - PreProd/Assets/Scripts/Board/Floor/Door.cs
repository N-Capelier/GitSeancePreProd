using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    public class Door : MonoBehaviour
    {
        public Node _linkedRoom;


        //load next room
        public void OpenDoor()
        {
            // //TODO : change _linkedRoomNumber value in FloorManager.Instance 
            int nextRoomNumber = FloorManager.Instance._rooms.GetNodePositionInTree(_linkedRoom);
            if (nextRoomNumber != -1)
            {
                FloorManager.Instance._playersPositionInFloor = nextRoomNumber;
                TileManager.Instance.GenerateRoom(_linkedRoom._data);
            }
            else
            {
                Debug.Log("Error : Room not found in BinaryTreeOfRoom");
            }

        }
    }
}
