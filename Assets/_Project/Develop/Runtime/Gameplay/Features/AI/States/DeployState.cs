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
		private readonly int                           _mineCost;
		private readonly int                           _sentryCost;
		private readonly int                           _puddleCost;
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
			_mineCost            = costConfig.MineCost;
			_sentryCost          = costConfig.SentryCost;
			_puddleCost          = costConfig.PuddleCost;
			_coroutinesPerformer = coroutinesPerformer;
			_mineDeployRequest   = entity.MineDeployRequest;
			_sentryDeployRequest = entity.SentryDeployRequest;
			_puddleDeployRequest = entity.PuddleDeployRequest;
			_currentDeployable   = entity.CurrentDeployable;
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
					if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Deploy Surface"))
					{
						switch (_currentDeployable.Value)
						{
							case Deployables.Mine:
								Vector3 mineDeployPoint = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
								_mineDeployRequest.Invoke(mineDeployPoint);

								_wallet.Spend(CurrencyTypes.Gold, _mineCost);
								break;

							case Deployables.Sentry:
								Vector3 sentryDeployPoint = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
								_sentryDeployRequest.Invoke(sentryDeployPoint);

								_wallet.Spend(CurrencyTypes.Gold, _sentryCost);
								break;

							case Deployables.Puddle:
								Vector3 puddleDeployPoint = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
								_puddleDeployRequest.Invoke(puddleDeployPoint);

								_wallet.Spend(CurrencyTypes.Gold, _puddleCost);
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
