using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was Julien Haigron
    /// </summary>

    public class Room
    {
        public List<Room> _parents;

        public Room _leftNode { get; set; }
        public Room _rightNode { get; set; }
        public RoomProfile _data { get; set; }

        public FloorManager.RoomType _roomType { get; set; }


        public Room(RoomProfile init)
        {
            _data = init;
            _parents = new List<Room>();
            _parents = null;
        }

        public Room(RoomProfile rp, Room parent)
        {
            _data = rp;
            _parents = new List<Room>();
            _parents.Add(parent);
        }

        public Room(RoomProfile rp, Room[] parents)
        {
            _data = rp;
            _parents = new List<Room>();
            for (int i = 0; i < parents.Length; i++)
            {
                _parents.Add(parents[i]);
            }
        }

        public void HandleEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:

                    break;
            }
        }
    }

    public class BinaryTreeOfRoom
    {
        public Room _root { get; set; }
        public List<Room> _rooms;

        public BinaryTreeOfRoom(RoomProfile init, int nbOfNode)
        {
            _root = new Room(init);
            _rooms = new List<Room>();
            _rooms.Add(_root);
        }

        public bool Add(RoomProfile newRoom, Room nodeParent)
        {
            Room newNode = new Room(newRoom, nodeParent);

            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i].Equals(nodeParent))
                {
                    if (_rooms[i]._leftNode == null)
                    {
                        _rooms[i]._leftNode = newNode;
                        _rooms.Add(newNode);
                        return true;
                    }
                    else if (_rooms[i]._rightNode == null)
                    {
                        _rooms[i]._rightNode = newNode;
                        _rooms.Add(newNode);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }

            //the parent node doesnt exist in the tree
            return false;
        }

        public bool Add(RoomProfile newRoom, Room[] nodesParent)
        {
            Room newNode = new Room(newRoom, nodesParent);
            int parentsFound = 0;

            for (int parentIterator = 0; parentIterator < nodesParent.Length; parentIterator++)
            {
                for (int i = 0; i < _rooms.Count; i++)
                {
                    if (_rooms[i].Equals(nodesParent[parentIterator]))
                    {
                        if (_rooms[i]._leftNode == null)
                        {
                            _rooms[i]._leftNode = newNode;
                            parentsFound++;
                        }
                        else if (_rooms[i]._rightNode == null)
                        {
                            _rooms[i]._rightNode = newNode;
                            parentsFound++;
                        }

                    }
                }
            }

            if (parentsFound == nodesParent.Length)
            {
                _rooms.Add(newNode);
                return true;
            }

            else
                //One ore more node in arguments dont exist in the tree
                return false;
        }

        public int GetNodePositionInTree(Room n)
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i].Equals(n)) return i;
            }
            return -1;
        }

        public Room GetNextRoom(Room actualRoom, bool direction)
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i].Equals(actualRoom))
                {
                    //left
                    if (!direction)
                        return _rooms[i]._leftNode;
                    else
                        return _rooms[i]._rightNode;
                }
            }

            //last room
            return null;
        }
    }
}
