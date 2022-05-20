using FishNet.Object;
using Seance.BoardManagment;
using Seance.CameraManagement;
using Seance.Management;
using UnityEngine;

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

		private void Update()
		{
			if (!IsOwner)
				return;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();

			if (!IsOwner)
				return;

			GameManager.Instance._lobby._ownedPlayer = this;

			_playerUI?.gameObject.SetActive(true);
			_cardZones.InitZones();
		}

		#endregion

		#region Turn management

		public void StartTurn()
		{
			if (!IsOwner)
				return;

			_gManager._debugPlayerTurn.text = "Your turn";

			_playerUI.EnableTurnUI();

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