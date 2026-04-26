using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using Unity.VisualScripting;
using UnityEngine;
using State = Assets._Project.Develop.Runtime.Utilities.StateMachineCore.State;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
	public class ChaseHeroState : State, IUpdatableState
	{
		private ReactiveVariable<Vector3> _moveDirection;
		private ReactiveVariable<Vector3> _rotationDirection;
		private Transform _transform;
		private Entity _entity;
		private MainHeroHolderService     _mainHeroHolderService;

		public ChaseHeroState (Entity entity, MainHeroHolderService mainHeroHolderService)
		{
			_moveDirection         = entity.MoveDirection;
			_rotationDirection     = entity.RotationDirection;
			_transform             = entity.Transform;
			_entity = entity;
			_mainHeroHolderService = mainHeroHolderService;
		}

		public void Update (float deltaTime)
		{
			if (!_mainHeroHolderService.MainHero.IsDead.Value && _entity != null)
			{
				_moveDirection.Value     = (_mainHeroHolderService.MainHero.Transform.position - _transform.position).normalized;
				_rotationDirection.Value = _moveDirection.Value;
			}
		}

		public override void Exit ()
		{
			base.Exit();

			_moveDirection.Value = Vector3.zero;
		}
	}
}
