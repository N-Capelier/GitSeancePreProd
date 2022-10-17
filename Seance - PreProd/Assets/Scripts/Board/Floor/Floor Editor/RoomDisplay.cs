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

        public Rect _Box; //for cell id
        public Rect _Box2; //for cell attributes

        public string _Text;

        public RoomProfile _roomProfile;

        public RoomDisplay(Vector2 position, float width, float height, string text)
        {
            _Box = new Rect(position, new Vector2(width, height));
            _Box2 = new Rect(position + new Vector2(10, 8), new Vector2(width, height));
            _Text = text;
            _id = Guid.NewGuid().ToString();
        }

        public void Paint()
        {
            //TODO : make the visual fancier
            GUI.Box(_Box, _Text);


            if (_roomProfile != null && _roomProfile._name != null)
            {
                GUI.Label(_Box2, "Room name : " + _roomProfile._name);
            }
            else
            {
                GUI.Label(_Box2, "No selected room profile");
            }

            //TODO : add an "EditorGUILayout.Popup" for better room display

        }

    }
}
