using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
	[RequireComponent(typeof(Animator))]
	public class InstantShootView : EntityView
	{
		private readonly int LoadKey = Animator.StringToHash("Load");
		private readonly int ShootKey = Animator.StringToHash("Shoot");

		[SerializeField] private Animator _animator;

		private ReactiveEvent _startAttackEvent;
		private ReactiveEvent _attackDelayEndEvent;

		private IDisposable[] _disposables = new IDisposable[2];

		private void OnValidate()
		{
			_animator ??= GetComponent<Animator>();
		}

		protected override void OnEntityStartedWork (Entity entity)
		{
			_startAttackEvent = entity.StartAttackEvent;
			_attackDelayEndEvent = entity.AttackDelayEndEvent;

			_disposables[0] = _startAttackEvent.Subscribe(OnStartAttackEvent);
			_disposables[1] = _attackDelayEndEvent.Subscribe(OnAttackDelayEndEvent);
		}

		private void OnStartAttackEvent ()
		{
			_animator.SetTrigger(LoadKey);
		}

		private void OnAttackDelayEndEvent ()
		{
			_animator.SetTrigger(ShootKey);
		}

		public override void Cleanup (Entity entity)
		{
			foreach (IDisposable disposable in _disposables)
			{
				disposable.Dispose();
			}
		}
	}
}
