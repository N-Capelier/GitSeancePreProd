using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    public class MyShlagScript : MonoBehaviour
    {
        public Door doorScript;

        private void OnMouseDown()
        {
            Debug.Log("called open door");
            doorScript.OpenDoor();
        }


    }
}
