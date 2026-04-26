using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Death
{
	public class DeathEffectView : EntityView
	{
		[SerializeField] private ParticleSystem _deathEffectPrefab;
		[SerializeField] private Transform      _effectSpawnPoint;

		private ReactiveVariable<bool> _isDead;

		private IDisposable _disposable;

		protected override void OnEntityStartedWork (Entity entity)
		{
			_isDead     = entity.IsDead;
			_disposable = _isDead.Subscribe(OnIsDeadChanged);
		}

		private void OnIsDeadChanged (bool _, bool isDead)
		{
			if (isDead)
			{
				Instantiate(_deathEffectPrefab, _effectSpawnPoint.position, Quaternion.identity);
			}
		}

		public override void Cleanup (Entity entity)
		{
			base.Cleanup(entity);

			_disposable.Dispose();
		}
	}
}
