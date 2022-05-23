using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    public class MyShlagScript : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;
        public Door doorScript;

        private void OnMouseDown()
        {
            doorScript.OpenDoor();
        }


        private void Update()
        {
			if (Input.GetMouseButtonDown(0))
				RayCastTileInteraction();
		}

		void RayCastTileInteraction()
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
			{
				doorScript.OpenDoor();
			}
		}

	}
}
