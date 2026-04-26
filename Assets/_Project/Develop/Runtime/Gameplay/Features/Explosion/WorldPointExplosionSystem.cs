using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class WorldPointExplosionSystem : IInitializableSystem, IDisposableSystem
	{
		private ReactiveVariable<float> _explosionDamage;
		private ReactiveVariable<float> _blastRadius;
		private ReactiveEvent<Vector3>  _explosionRequest;
		private ReactiveEvent<Vector3>  _explosionEvent;
		private Entity                  _entity;

		private readonly CollidersRegistryService _collidersRegistryService;

		private IDisposable _disposable;

		public WorldPointExplosionSystem (CollidersRegistryService collidersRegistryService)
		{
			_collidersRegistryService = collidersRegistryService;
		}


		public void OnInit (Entity entity)
		{
			_explosionDamage  = entity.ExplosionDamage;
			_blastRadius      = entity.BlastRadius;
			_explosionRequest = entity.WorldPointExplosionRequest;
			_explosionEvent   = entity.WorldPointExplosionEvent;
			_entity           = entity;

			_disposable = _explosionRequest.Subscribe(OnExplosionRequest);
		}

		private void OnExplosionRequest (Vector3 position)
		{
			Collider[] hitColliders = Physics.OverlapSphere(position, _blastRadius.Value, LayerMask.GetMask("Characters"));

			foreach (Collider hitCollider in hitColliders)
			{
				Entity entity = _collidersRegistryService.GetBy(hitCollider);

				EntitiesHelper.TryTakeDamageFrom(_entity, entity, _explosionDamage.Value);
			}

			_explosionEvent.Invoke(position);
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}
	}
}
