using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    public class Door : MonoBehaviour
    {
        public Node _linkedRoom;

        public GameObject iconDisplay;
        public Sprite[] nextRoomIcone;

        private void Start()
        {
            //UpdateIcon();
        }


        //load next room
        [ContextMenu("Open Door")]
        public void OpenDoor()
        {

            if (FloorManager.Instance != null)
            {
                int nextRoomNumber = FloorManager.Instance._rooms.GetNodePositionInTree(_linkedRoom);
                if (nextRoomNumber != -1)
                {
                    FloorManager.Instance._playersPositionInFloor = nextRoomNumber;
                    TileManager.Instance.GenerateRoom(_linkedRoom._data);

                }
                else
                {
                    Debug.Log("Error : Room not found in BinaryTreeOfRoom");
                }
            }
        }

        public void UpdateIcon()
        {
            if (Application.isPlaying)
            {
                if (TileManager.Instance.isLastRoom)
                {

                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[3];
                    iconDisplay.SetActive(true);

                }
                else
                {
                    for (int i = 0; i < FloorManager.Instance._roomProfiles.Length; i++)
                    {
                        if (FloorManager.Instance._roomProfiles[i] == _linkedRoom._data)
                        {
                            switch (i)
                            {
                                //Osef (init room)
                                case 0:
                                    Debug.Log("0");
                                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[0];
                                    iconDisplay.SetActive(true);
                                    break;
                                //Combat
                                case 1:
                                    Debug.Log("1");
                                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[0];
                                    iconDisplay.SetActive(true);
                                    break;
                                //Trésor
                                case 2:
                                    Debug.Log("2");
                                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[1];
                                    iconDisplay.SetActive(true);
                                    break;
                                //Trésor
                                case 3:
                                    Debug.Log("3");
                                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[1];
                                    iconDisplay.SetActive(true);
                                    break;
                                //Combat
                                case 4:
                                    Debug.Log("4");
                                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[0];
                                    iconDisplay.SetActive(true);
                                    break;
                                //Trésor
                                case 5:
                                    Debug.Log("5");
                                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[1];
                                    iconDisplay.SetActive(true);
                                    break;
                                //Fin
                                case 6:
                                    Debug.Log("6");
                                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[2];
                                    iconDisplay.SetActive(true);
                                    break;
                                default:
                                    Debug.Log("Default");
                                    iconDisplay.GetComponent<SpriteRenderer>().sprite = nextRoomIcone[3];
                                    iconDisplay.SetActive(true);
                                    break;
                            }
                        }

                    }
                }
            }

            

            
        }
    }
}
