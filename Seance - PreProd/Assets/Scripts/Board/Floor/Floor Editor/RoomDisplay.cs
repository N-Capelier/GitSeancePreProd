using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Seance.BoardManagment
{
    [Serializable]
    public class RoomDisplay
    {
        public string _id;

        public Rect _Box;

        public string _Text;

        public RoomProfile _roomProfile;

        public RoomDisplay(Vector2 position, float width, float height, string text)
        {
            _Box = new Rect(position, new Vector2(width, height));
            _Text = text;
            _id = Guid.NewGuid().ToString();
        }

        public void Paint()
        {
            //TODO : make the visual fancier
            GUI.Box(_Box, _Text);
            //GUI.Label(_Box, )

            //TODO : Display room profile

        }

    }
}
