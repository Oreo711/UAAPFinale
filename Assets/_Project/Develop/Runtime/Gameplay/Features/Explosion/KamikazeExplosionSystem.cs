using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class KamikazeExplosionSystem : IInitializableSystem, IDisposableSystem
	{
		private ReactiveVariable<float> _explosionDamage;
		private ReactiveVariable<float> _blastRadius;
		private ReactiveEvent           _explosionRequest;
		private Transform               _transform;
		private ReactiveVariable<bool>  _markedForDeath;
		private Entity                  _entity;

		private readonly CollidersRegistryService _collidersRegistryService;

		private IDisposable _disposable;

		public KamikazeExplosionSystem(CollidersRegistryService collidersRegistryService)
		{
			_collidersRegistryService = collidersRegistryService;
		}

		public void OnInit (Entity entity)
		{
			_explosionDamage  = entity.ExplosionDamage;
			_blastRadius      = entity.BlastRadius;
			_explosionRequest = entity.KamikazeExplosionRequest;
			_transform        = entity.Transform;
			_markedForDeath   = entity.MarkedForDeath;
			_entity           = entity;

			_disposable = _explosionRequest.Subscribe(OnExplosionRequest);
		}

		private void OnExplosionRequest ()
		{
			Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _blastRadius.Value, LayerMask.GetMask("Characters"));

			foreach (Collider hitCollider in hitColliders)
			{
				Entity entity = _collidersRegistryService.GetBy(hitCollider);

				EntitiesHelper.TryTakeDamageFrom(_entity, entity, _explosionDamage.Value);
			}

			_markedForDeath.Value = true;
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}
	}
}
