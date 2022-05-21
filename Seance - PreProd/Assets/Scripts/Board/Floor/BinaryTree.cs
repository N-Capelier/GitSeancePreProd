using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was Julien Haigron
    /// </summary>

    public class Node
    {
        public List<Node> _parents;

        public Node _leftNode { get; set; }
        public Node _rightNode { get; set; }
        public RoomProfile _data { get; set; }

        public Node(RoomProfile init)
        {
            _data = init;
            _parents = new List<Node>();
            _parents = null;
        }

        public Node(RoomProfile rp, Node parent)
        {
            _data = rp;
            _parents = new List<Node>();
            _parents.Add(parent);
        }

        public Node(RoomProfile rp, Node[] parents)
        {
            _data = rp;
            _parents = new List<Node>();
            for (int i = 0; i < parents.Length; i++)
            {
                _parents.Add(parents[i]);
            }
        }
    }

    public class BinaryTreeOfRoom
    {
        public Node _root { get; set; }
        public List<Node> _floor;   

        public BinaryTreeOfRoom(RoomProfile init, int nbOfNode)
        {
            _root = new Node(init);
            _floor = new List<Node>();
            _floor.Add(_root);
        }

        public bool Add(RoomProfile newRoom, Node nodeParent)
        {
            Node newNode = new Node(newRoom, nodeParent);

            for (int i = 0; i < _floor.Count; i++)
            {
                if (_floor[i].Equals(nodeParent))
                {
                    if (_floor[i]._leftNode == null)
                    {
                        _floor[i]._leftNode = newNode;
                        _floor.Add(newNode);
                        return true;
                    }
                    else if (_floor[i]._rightNode == null)
                    {
                        _floor[i]._rightNode = newNode;
                        _floor.Add(newNode);
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

        public bool Add(RoomProfile newRoom, Node[] nodesParent)
        {
            Node newNode = new Node(newRoom, nodesParent);
            int parentsFound = 0;

            for (int parentIterator = 0; parentIterator < nodesParent.Length; parentIterator++)
            {
                for (int i = 0; i < _floor.Count; i++)
                {
                    if (_floor[i].Equals(nodesParent[parentIterator]))
                    {
                        if (_floor[i]._leftNode == null)
                        {
                            _floor[i]._leftNode = newNode;
                            parentsFound++;
                        }
                        else if (_floor[i]._rightNode == null)
                        {
                            _floor[i]._rightNode = newNode;
                            parentsFound++;
                        }

                    }
                }
            }

            if (parentsFound == nodesParent.Length)
            {
                _floor.Add(newNode);
                return true;
            }

            else
                //One ore more node in arguments dont exist in the tree
                return false;
        }

        public int GetNodePositionInTree(Node n)
        {
            for (int i = 0; i < _floor.Count; i++)
            {
                if (_floor[i].Equals(n)) return i;
            }
            return -1;
        }

        public Node GetNextRoom(Node actualRoom, bool direction)
        {
            for (int i = 0; i < _floor.Count; i++)
            {
                if (_floor[i].Equals(actualRoom))
                {
                    //left
                    if (!direction)
                        return _floor[i]._leftNode;
                    else
                        return _floor[i]._rightNode;
                }
            }

            //last room
            return null;
        }
    }
}
