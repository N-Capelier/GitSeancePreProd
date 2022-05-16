using FishNet.Object;
using Seance.BoardManagment;
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
		[SerializeField] GameObject _playerUI;
		[SerializeField] Transform _cardsParent;

		[Header("Control")]
		bool _isPlaying = false;
		public bool IsPlaying { get => _isPlaying; }

		int _interactions = 0;


		public PlayerCardZones _cardZones;
		[HideInInspector] public CharacterPawn _pawn;

		#region Unity messages & Setup

		private void Start()
		{
			_gManager = GameManager.Instance;
		}

		private void Update()
		{
			if (!IsOwner)
				return;

			//debug
			if (Input.GetKeyDown(KeyCode.Space))
			{
				EndTurn();
			}
		}
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!IsOwner)
				return;
			GameManager.Instance._lobby._ownedPlayer = this;
			_playerUI?.SetActive(true);
		}

		#endregion

		#region Turn management

		public void StartTurn()
		{
			if (!IsOwner)
				return;

			_gManager._debugTurnIndex.text = $"turn index: {_gManager._turnManager.TotalTurns % 4}";

			_gManager._debugPlayerTurn.text = "player turn: true";

			_cardZones.DrawCard();

			_isPlaying = true;
		}

		public void EndTurn()
		{
			if (!_isPlaying)
				return;

			GameManager.Instance._debugPlayerTurn.text = "player turn: false";
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