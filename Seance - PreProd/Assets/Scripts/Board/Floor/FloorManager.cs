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

        public BinaryTreeOfRoom _rooms;

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

        //call this function when player go to another room
        public void OpenDoor(int doorNb)
        {
            //TODO implement function
        }

        
    }
}
