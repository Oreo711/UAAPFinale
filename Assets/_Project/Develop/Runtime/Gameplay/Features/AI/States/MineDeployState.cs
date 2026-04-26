using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
	public class MineDeployState : State, IUpdatableState
	{
		private IInputService          _inputService;
		private WalletService          _wallet;
		private PlayerDataProvider     _playerDataProvider;
		private ReactiveEvent<Vector3> _deployRequest;
		private int                  _mineCost;
		private ICoroutinesPerformer _coroutinesPerformer;

		public MineDeployState (Entity entity, IInputService inputService, WalletService wallet, PlayerDataProvider playerDataProvider, int mineCost, ICoroutinesPerformer coroutinesPerformer)
		{
			_inputService             = inputService;
			_wallet                   = wallet;
			_playerDataProvider       = playerDataProvider;
			_mineCost                 = mineCost;
			_coroutinesPerformer      = coroutinesPerformer;
			_deployRequest            = entity.MineDeployRequest;
		}

		public void Update (float deltaTime)
		{
			if (_inputService.Clicked)
			{
				if (!_wallet.Enough(CurrencyTypes.Gold, _mineCost))
				{
					return;
				}

				Vector3 mousePosition         = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 groundedMousePosition = new Vector3(mousePosition.x, 0, mousePosition.z);
				Vector3 mouseDirection        = (groundedMousePosition - Camera.main.transform.position).normalized;
				Ray     ray                   = new Ray(Camera.main.transform.position, mouseDirection);

				if (Physics.Raycast(ray, out RaycastHit hitInfo))
				{
					if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Mine Deploy Surface"))
					{
						Vector3 point = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
						_deployRequest.Invoke(point);

						_wallet.Spend(CurrencyTypes.Gold, _mineCost);
						_coroutinesPerformer.StartCoroutine(_playerDataProvider.SaveAsync());
					}
				}
			}
		}
	}
}
