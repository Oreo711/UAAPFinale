using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionEffectView : EntityView
	{
		[SerializeField] private ParticleSystem _explosionEffectPrefab;

		private ReactiveEvent<Vector3> _explosionEvent;

		private IDisposable _disposable;

		protected override void OnEntityStartedWork (Entity entity)
		{
			_explosionEvent = entity.WorldPointExplosionEvent;
			_disposable       = _explosionEvent.Subscribe(OnExplosionRequest);
		}

		private void OnExplosionRequest (Vector3 position)
		{
			Instantiate(_explosionEffectPrefab, position, Quaternion.identity);
		}

		public override void Cleanup (Entity entity)
		{
			base.Cleanup(entity);

			_disposable.Dispose();
		}
	}
}
