using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Seance.BoardManagment
{

    public class FloorGeneratorWindow : EditorWindow
    {
        SerializedObject _serializedObject;
        FloorProfile _currentFloor;

        SerializedProperty _roomDisplays, _connections, _floor, _roomProfiles, _currentRoomTypeSelected, _roomTypeColors;

        ConnectionDisplay _pendingConnection;

        public void InitializeWindow(FloorProfile correspondingFloor)
        {
            _currentFloor = correspondingFloor;
            _serializedObject = new SerializedObject(_currentFloor);

            _roomDisplays = _serializedObject.FindProperty("_roomDisplays");
            _connections = _serializedObject.FindProperty("_connections");
            _floor = _serializedObject.FindProperty("_floor");
            _roomProfiles = _serializedObject.FindProperty("_roomProfiles");
            _currentRoomTypeSelected = _serializedObject.FindProperty("_currentRoomTypeSelected");
            _roomTypeColors = _serializedObject.FindProperty("_roomTypeColors");

            if (_currentFloor._roomDisplays.Count == 0)
            {
                OnAddStartingRoom(new Vector2(40, 200));
            }

            _pendingConnection = null;

        }

        private void OnGUI()
        {
            _serializedObject.Update();

            //
            ProcessEvents(Event.current);

            PaintRooms();

            PaintConnections();
            //

            _serializedObject.ApplyModifiedProperties();

            Repaint(); // pour la mouse_position (Event)
        }

        private void PaintRooms()
        {
            foreach (var room in _currentFloor._roomDisplays)
            {
                room.Paint();
            }

        }

        private void PaintConnections()
        {
            if (_pendingConnection != null)
            {
                _pendingConnection.Paint(Event.current.mousePosition);
            }

            foreach (var connection in _currentFloor._connections)
            {
                connection.Paint();
            }

        }
        public enum EItemAction
        {
            None,
            ContextMenu,
            Select
        }

        private void ProcessEvents(Event e)
        {
            bool isInARoom = false;
            foreach (var room in _currentFloor._roomDisplays)
            {
                switch (e.type)
                {
                    case EventType.MouseDrag:
                        if (room._Box.Contains(e.mousePosition))
                        {
                            room._Box.position += e.delta;
                            isInARoom = true;
                        }
                        break;
                    case EventType.MouseDown:
                        if (room._Box.Contains(e.mousePosition) && e.button == 1)
                        {
                            //right clic
                            OpenContextMenuForNode(room, e.mousePosition);
                            isInARoom = true;
                        }
                        else if (room._Box.Contains(e.mousePosition) && e.button == 0 && _pendingConnection != null)
                        {
                            LinkPendingConnectionTo(room);
                            isInARoom = true;
                        }
                        break;
                }

            }

            if (!isInARoom)
            {
                switch (e.type)
                {
                    case EventType.MouseDown:

                        //right mouse button
                        if (e.button == 1)
                        {
                            OpenContextMenu(e.mousePosition);
                        }
                        //left mouse button
                        else if (e.button == 0)
                        {
                            DeletePendingConnection();
                        }
                        break;
                    case EventType.KeyDown:

                        if (e.keyCode == KeyCode.Escape)
                        {
                            DeletePendingConnection();
                        }
                        break;
                }
            }
        }

        private void DeletePendingConnection()
        {
            if (_pendingConnection != null)
            {
                _pendingConnection = null;
            }
        }

        private void OpenContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add room item"),
                false, () => OnAddRoom(mousePosition));

            genericMenu.ShowAsContext();
        }

        private void OpenContextMenuForNode(RoomDisplay node, Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Delete Room"), false, () => OnDeleteRoom(node));

            genericMenu.AddItem(new GUIContent("Create Room Connection"), false, () => OnCreateRoomConnection(node));

            genericMenu.ShowAsContext();
        }

        void OnAddStartingRoom(Vector2 mousePosition)
        {
            RoomDisplay newRoom = new RoomDisplay(mousePosition, 200, 30, "< Starting room > ");

            _currentFloor._roomDisplays.Add(newRoom);
            /*_roomDisplays.arraySize++;
            _roomDisplays.GetArrayElementAtIndex(_roomDisplays.arraySize - 1).objectReferenceValue = newRoom;*/
        }

        void OnAddRoom(Vector2 mousePosition)
        {
            RoomDisplay newRoom = new RoomDisplay(mousePosition, 200, 30, "Item " + _currentFloor._roomDisplays.Count);

            _currentFloor._roomDisplays.Add(newRoom);
        }

        void OnDeleteRoom(RoomDisplay room)
        {
            for (int i = 0; i < _currentFloor._connections.Count; i++)
            {
                if (_currentFloor._connections[i]._fromID == room._id || _currentFloor._connections[i]._toID == room._id)
                {
                    _currentFloor._connections.RemoveAt(i);
                }
            }

            _currentFloor._roomDisplays.Remove(room);
        }

        void OnCreateRoomConnection(RoomDisplay room)
        {
            _pendingConnection = new ConnectionDisplay(room);
        }

        void LinkPendingConnectionTo(RoomDisplay destination)
        {
            _pendingConnection.ConnectTo(destination);
            _currentFloor._connections.Add(_pendingConnection);
            _pendingConnection = null;
        }

    }
}
