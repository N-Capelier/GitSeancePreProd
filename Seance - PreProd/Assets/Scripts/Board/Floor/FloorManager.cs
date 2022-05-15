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

            BinaryTreeOfRoom testFloor = new BinaryTreeOfRoom(_roomProfiles[0], 9);

            RoomProfile a = _roomProfiles[1]; 
            RoomProfile b = _roomProfiles[2]; 
            RoomProfile c = _roomProfiles[3]; 
            RoomProfile boss = _roomProfiles[4];

            //1st column
            testFloor.Add(a, testFloor._floor[0]);
            testFloor.Add(b, testFloor._floor[0]);
            Node[] firstFloor = { testFloor._floor[1], testFloor._floor[2] };

            //2nc column
            testFloor.Add(c, testFloor._floor[1]);
            testFloor.Add(c, firstFloor);
            testFloor.Add(c, testFloor._floor[2]);
            Node[] secoundFloor = { testFloor._floor[3], testFloor._floor[4], testFloor._floor[5] };

            //3rd floor
            testFloor.Add(a, secoundFloor);
            testFloor.Add(b, secoundFloor);
            Node[] thirdFloor = { testFloor._floor[6], testFloor._floor[7] };

            //boss
            testFloor.Add(boss, thirdFloor);
        }

        
    }
}
