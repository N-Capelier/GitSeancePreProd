using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace Seance.BoardManagment
{
    [Serializable]
    public class ConnectionDisplay
    {
        public string _fromID;
        public string _toID;

        private RoomDisplay _fromItem;
        private RoomDisplay _toItem;

        public ConnectionDisplay(RoomDisplay from)
        {
            _fromID = from._id;
            _fromItem = from;
            _toItem = null;
        }

        public void ConnectTo(RoomDisplay to)
        {
            _toItem = to;
            _toID = to._id;
        }

        public void InitByIds(FloorProfile floor)
        {
            _fromItem = floor.GetRoomDisplay(_fromID);
            _toItem = floor.GetRoomDisplay(_toID);
        }

        public void Paint()
        {
            if (_fromItem == null || _toItem == null)
                return;


            Handles.color = Color.white;

            Vector2 pStart = new Vector2(_fromItem._Box.width + _fromItem._Box.x, _fromItem._Box.y + _fromItem._Box.height / 2);

            Vector2 pEnd = new Vector2(_toItem._Box.x, _toItem._Box.y + _toItem._Box.height/2);

            Vector3[] points = new Vector3[2];
            points[0] = pStart;
            points[1] = pEnd;

            Handles.DrawAAPolyLine(4, points);

            Rect rect = new Rect(pEnd.x - 5, pEnd.y, 10, 5);

            Handles.DrawSolidRectangleWithOutline(rect, Color.white, Color.gray);
        }

        public void Paint(Vector2 mousePosition)
        {
            if (_fromItem == null)
                return;

            Handles.color = Color.white;
            Vector2 pStart = new Vector2(_fromItem._Box.width + _fromItem._Box.x, _fromItem._Box.y + _fromItem._Box.height / 2);

            Handles.DrawLine(pStart, mousePosition);
        }
    }
}
