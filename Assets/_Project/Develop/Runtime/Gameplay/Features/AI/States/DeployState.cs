using System;
using _Project.Develop.Runtime.Configs.Gameplay;
using _Project.Develop.Runtime.Gameplay.Features.Deploy;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Deploy;
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
	public class DeployState : State, IUpdatableState
	{
		private readonly IInputService                 _inputService;
		private readonly WalletService                 _wallet;
		private readonly PlayerDataProvider            _playerDataProvider;
		private readonly DeployableCostConfig          _costConfig;
		private readonly ICoroutinesPerformer          _coroutinesPerformer;
		private          ReactiveEvent<Vector3>        _mineDeployRequest;
		private          ReactiveEvent<Vector3>        _sentryDeployRequest;
		private          ReactiveEvent<Vector3>        _puddleDeployRequest;
		private          ReactiveVariable<Deployables> _currentDeployable;

		public DeployState (
			Entity entity,
			IInputService inputService,
			WalletService wallet,
			PlayerDataProvider playerDataProvider,
			DeployableCostConfig costConfig,
			ICoroutinesPerformer coroutinesPerformer)
		{
			_inputService        = inputService;
			_wallet              = wallet;
			_playerDataProvider  = playerDataProvider;
			_coroutinesPerformer = coroutinesPerformer;
			_costConfig          = costConfig;
			_mineDeployRequest   = entity.MineDeployRequest;
			_sentryDeployRequest = entity.SentryDeployRequest;
			_puddleDeployRequest = entity.PuddleDeployRequest;
			_currentDeployable   = entity.CurrentDeployable;
		}

		public void Update (float deltaTime)
		{
			if (_inputService.Clicked)
			{
				Vector3 mousePosition         = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 groundedMousePosition = new Vector3(mousePosition.x, 0, mousePosition.z);
				Vector3 mouseDirection        = (groundedMousePosition - Camera.main.transform.position).normalized;
				Ray     ray                   = new Ray(Camera.main.transform.position, mouseDirection);

				if (Physics.Raycast(ray, out RaycastHit hitInfo))
				{
					if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Deploy Surface"))
					{
						Vector3 deployPoint = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
						switch (_currentDeployable.Value)
						{
							case Deployables.Mine:
								if (!_wallet.Enough(CurrencyTypes.Gold, _costConfig.MineCost))
								{
									return;
								}

								_mineDeployRequest.Invoke(deployPoint);

								_wallet.Spend(CurrencyTypes.Gold, _costConfig.MineCost);
								break;

							case Deployables.Sentry:
								if (!_wallet.Enough(CurrencyTypes.Gold, _costConfig.SentryCost))
								{
									return;
								}

								_sentryDeployRequest.Invoke(deployPoint);

								_wallet.Spend(CurrencyTypes.Gold, _costConfig.SentryCost);
								break;

							case Deployables.Puddle:
								if (!_wallet.Enough(CurrencyTypes.Gold, _costConfig.PuddleCost))
								{
									return;
								}

								_puddleDeployRequest.Invoke(deployPoint);

								_wallet.Spend(CurrencyTypes.Gold, _costConfig.PuddleCost);
								break;
							default:
								throw new ArgumentException("This type of deployable is not supported.");
						}
						_coroutinesPerformer.StartCoroutine(_playerDataProvider.SaveAsync());
					}
				}
			}
		}
	}
}
