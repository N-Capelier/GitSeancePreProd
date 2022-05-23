using FishNet.Object;
using Seance.BoardManagment;
using Seance.CameraManagement;
using Seance.Management;
using UnityEngine;
using Seance.BoardManagment.Dice;
using UnityEngine.Rendering;
using FishNet.Connection;
using System.Collections;

namespace Seance.Player
{
	/// <summary>
	/// Nico
	/// </summary>
	public class PlayerManager : NetworkBehaviour
	{
		[Header("References")]
		GameManager _gManager;
		[SerializeField] PlayerUIMode _playerUI;
		[SerializeField] Volume _cheatPostProcessVolume;

		[Header("Control")]
		bool _isPlaying = false;
		public bool IsPlaying { get => _isPlaying; }

		int _interactions = 0;


		public PlayerCardZones _cardZones;
		[HideInInspector] public CharacterPawn _pawn;
		public CameraSwitch _camera;

		#region Unity messages & Setup

		private void Start()
		{
			_gManager = GameManager.Instance;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();

			if (!IsOwner)
			{
				Destroy(_cardZones.GetComponent<PlayerTileInteraction>()); ;
				return;
			}

			GameManager.Instance._lobby._ownedPlayer = this;

			_camera.transform.parent.gameObject.SetActive(true);
			GameManager.Instance._defaultCamera.gameObject.SetActive(false);
			_playerUI.gameObject.SetActive(true);
			Dice20.Instance.Init(_cheatPostProcessVolume);
		}

		[ServerRpc(RequireOwnership = false)]
		public void ServerRpcSetPawn(int target, int pawnIndex)
		{
			TargetRpcSetPawn(_gManager._lobby._networkConnections[target], pawnIndex);
		}

		[TargetRpc]
		void TargetRpcSetPawn(NetworkConnection conn, int pawnIndex)
		{
			//if (_gManager._lobby._ownedConnection != conn)
			//	return;

			StartCoroutine(SetPawnCoroutine(pawnIndex));
		}

		IEnumerator SetPawnCoroutine(int pawnIndex)
		{
			yield return new WaitForSeconds(.1f);
			_gManager._lobby._ownedPlayer._pawn = TileManager.Instance._pawnsInScene[pawnIndex] as CharacterPawn;
			_gManager._lobby._ownedPlayer._pawn.SetOwnedMaterial();
		}

		[ServerRpc(RequireOwnership = false)]
		public void ServerRpcInitZones(int deckIndex)
		{
			TargetRpcInitZones(_gManager._lobby._networkConnections[deckIndex], deckIndex);
		}

		[TargetRpc]
		void TargetRpcInitZones(NetworkConnection conn, int deckIndex)
		{
			_gManager._lobby._ownedPlayer._cardZones.InitZones(deckIndex);
		}

		#endregion

		#region Turn management

		public void StartTurn()
		{
			if (!IsOwner)
				return;

			_gManager._debugPlayerTurn.text = "Your turn";

			_playerUI.EnableTurnUI();

			StartCoroutine(StartTurnCoroutine());
		}

		IEnumerator StartTurnCoroutine()
		{
			yield return new WaitForSeconds(3f);

			_cardZones.DrawCard();

			_isPlaying = true;
		}

		public void EndTurn()
		{
			if (!_isPlaying)
				return;

			GameManager.Instance._debugPlayerTurn.text = string.Empty;

			_playerUI.DisableTurnUI();

			_isPlaying = false;
			_gManager._turnManager.ServerRpcPlayNextTurn();
		}

		#endregion

		#region Interaction management

		public int AddInteraction()
		{
			_interactions++;
			return _interactions;
		}

		public int RemoveInteraction()
		{
			_interactions--;
			return _interactions;
		}

		public bool CanInteract()
		{
			return _interactions == 0;
		}

		#endregion
	}
}