using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>
    
    public class Tile : MonoBehaviour
    {
        public int _x;
        public int _y;
        public TileManager.Tiles _thisTileType;
        public List<Pawn> _pawnsOnTile;
        public int _nbOfPawnOnStart = 0;
        public int _maxPawnOnTile = 4;

        [Header("Pawn Positions")]
        //pawn position transforms lists
        public Transform _onePawn;
        public Transform[] _twoPawns;
        public Transform[] _threePawns;
        public Transform[] _foorPawns;

        [SerializeField] private MeshRenderer tileRenderer;
        [SerializeField] private Material color1;
        [SerializeField] private Material color2;

        //for angle
        public Quaternion _savedRot;
        public int _newSavedRot;

        public void ApplySavedRotation()
        {
            //transform.rotation = _savedRot;
            switch (_newSavedRot)
            {
                case 0:
                    Quaternion q = new Quaternion();
                    q.eulerAngles = new Vector3 (0, 0, 0);
                    transform.rotation = q;
                    break;
                case 1:
                    Quaternion q2 = new Quaternion();
                    q2.eulerAngles = new Vector3(0, 90, 0);
                    transform.rotation = q2;
                    break;
                case 2:
                    Quaternion q3 = new Quaternion();
                    q3.eulerAngles = new Vector3(0, 180, 0);
                    transform.rotation = q3;
                    break;
                case 3:
                    Quaternion q4 = new Quaternion();
                    q4.eulerAngles = new Vector3(0, 270, 0);
                    transform.rotation = q4;
                    break;
            }
        }

        public void SaveRotation()
        {
            ///DEPRETIATED
            //_savedRot = transform.rotation;
            // 0 = north, 1 = east, 2 = south, 3 = east
            //_savedRot = transform.rotation;
        }
        
        public void Initialize(int x, int y, TileManager.Tiles tileType)
        {
            _x = x;
            _y = y;

            if (tileRenderer != null)
            {
                //pair
                if ((_x % 2 == 0 && _y % 2 == 1) ||(_x %2 ==1 && _y % 2 ==0))
                {
                    tileRenderer.material = color1;
                }
                else
                {
                    tileRenderer.material = color2;
                }
            }

            _thisTileType = tileType;

           
        }

        public void UpdatePawnsPositionOnTile()
        {
            //ignore z axis

            switch (_pawnsOnTile.Count)
            {
                case 0:
                    //no pawn on tile
                    break;
                case 1:
                    _pawnsOnTile[0].transform.position = new Vector3(_onePawn.position.x, _pawnsOnTile[0].transform.position.y, _onePawn.position.z);
                    break;
                case 2:
                    _pawnsOnTile[0].transform.position = new Vector3(_twoPawns[0].position.x, _pawnsOnTile[0].transform.position.y, _twoPawns[0].position.z);
                    _pawnsOnTile[1].transform.position = new Vector3(_twoPawns[1].position.x, _pawnsOnTile[1].transform.position.y, _twoPawns[1].position.z);
                    break;
                case 3:
                    _pawnsOnTile[0].transform.position = new Vector3(_threePawns[0].position.x, _pawnsOnTile[0].transform.position.y, _threePawns[0].position.z);
                    _pawnsOnTile[1].transform.position = new Vector3(_threePawns[1].position.x, _pawnsOnTile[1].transform.position.y, _threePawns[1].position.z);
                    _pawnsOnTile[2].transform.position = new Vector3(_threePawns[2].position.x, _pawnsOnTile[2].transform.position.y, _threePawns[2].position.z);
                    break;
                case 4:
                    _pawnsOnTile[0].transform.position = new Vector3(_foorPawns[0].position.x, _pawnsOnTile[0].transform.position.y, _foorPawns[0].position.z);
                    _pawnsOnTile[1].transform.position = new Vector3(_foorPawns[1].position.x, _pawnsOnTile[1].transform.position.y, _foorPawns[1].position.z);
                    _pawnsOnTile[2].transform.position = new Vector3(_foorPawns[2].position.x, _pawnsOnTile[2].transform.position.y, _foorPawns[2].position.z);
                    _pawnsOnTile[3].transform.position = new Vector3(_foorPawns[3].position.x, _pawnsOnTile[3].transform.position.y, _foorPawns[3].position.z);
                    break;
            }
        }
    }
}
