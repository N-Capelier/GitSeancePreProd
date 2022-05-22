using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.CardSystem;
using Seance.BoardManagment;
using Seance.Management;

namespace Seance.Player
{
    public class PlayerTileInteraction : MonoBehaviour
    {
		[SerializeField] LayerMask _interactableLayerMask;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
				RayCastTileInteraction();
		}

		void RayCastTileInteraction()
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, _interactableLayerMask))
			{
				if(hit.transform.parent.GetComponent<Tile>() != null && GameManager.Instance._lobby._ownedPlayer._cardZones._selectedCardIndex != -1)
				{
					Tile targetTile = hit.transform.parent.GetComponent<Tile>();
					GameManager.Instance._lobby._ownedPlayer._cardZones.UseCard(GameManager.Instance._lobby._ownedPlayer._pawn, targetTile, TileManager.Instance.GetPawnsOn(targetTile._x, targetTile._y));
				}
			}
		}
	}
}
