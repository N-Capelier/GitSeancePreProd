using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    public class FloorManager : MonoBehaviour
    {
        //Singleton
        public static FloorManager Instance;

        private TileManager _tm;

        public RoomProfile[] _roomProfiles;
        public BinaryTreeOfRoom _rooms;

        public int _playersPositionInFloor;

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

        private void Start()
        {
            _tm = GetComponent<TileManager>();

            //players start in the first room
            _playersPositionInFloor = 0;
        }

        public void CreateFloor()
        {
            //TODO : make a tool for this so its modulable in inspector

            _rooms = new BinaryTreeOfRoom(_roomProfiles[0], 9);


            RoomProfile a = _roomProfiles[1];
            RoomProfile aBoss = _roomProfiles[2];
            RoomProfile b = _roomProfiles[3];
            RoomProfile bBoss = _roomProfiles[4];
            RoomProfile c = _roomProfiles[5];
            RoomProfile boss = _roomProfiles[6];

            //1st column
            _rooms.Add(a, _rooms._rooms[0]);
            _rooms.Add(b, _rooms._rooms[0]);
            Room[] firstFloor = { _rooms._rooms[1], _rooms._rooms[2] };

            //2nc column
            _rooms.Add(c, _rooms._rooms[1]);
            _rooms.Add(c, firstFloor);
            _rooms.Add(c, _rooms._rooms[2]);
            Room[] secoundFloor = { _rooms._rooms[3], _rooms._rooms[4], _rooms._rooms[5] };

            //3rd floor
            _rooms.Add(aBoss, secoundFloor);
            _rooms.Add(bBoss, secoundFloor);
            Room[] thirdFloor = { _rooms._rooms[6], _rooms._rooms[7] };

            //boss
            _rooms.Add(boss, thirdFloor);
        }

        public enum RoomType
        {
            start,
            normal,
            boss,
            total
        }


    }
}
