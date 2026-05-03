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
		private readonly PlayerDataProvider            _playerDataProvider;
		private readonly ICoroutinesPerformer          _coroutinesPerformer;
		private readonly DeployablePurchaseService     _deployablePurchaseService;
		private          ReactiveVariable<Deployables> _currentDeployable;
		private          Entity                        _entity;

		public DeployState (
			Entity entity,
			IInputService inputService,
			PlayerDataProvider playerDataProvider,
			ICoroutinesPerformer coroutinesPerformer,
			DeployablePurchaseService deployablePurchaseService
		)
		{
			_inputService              = inputService;
			_playerDataProvider        = playerDataProvider;
			_coroutinesPerformer       = coroutinesPerformer;
			_deployablePurchaseService = deployablePurchaseService;
			_currentDeployable         = entity.CurrentDeployable;
			_entity                    = entity;
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

						_deployablePurchaseService.PurchaseAndDeploy(_currentDeployable.Value, deployPoint, _entity);
						_coroutinesPerformer.StartCoroutine(_playerDataProvider.SaveAsync());
					}
				}
			}
		}
	}
}
