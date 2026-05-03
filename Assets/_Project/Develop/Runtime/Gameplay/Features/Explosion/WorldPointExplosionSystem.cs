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

		private readonly AreaHitscanService _areaHitscanService;

		private IDisposable _disposable;

		public WorldPointExplosionSystem (AreaHitscanService areaHitscanService)
		{
			_areaHitscanService = areaHitscanService;
		}


		public void OnInit (Entity entity)
		{
			_explosionDamage  = entity.ModifiedExplosionDamage;
			_blastRadius      = entity.BlastRadius;
			_explosionRequest = entity.WorldPointExplosionRequest;
			_explosionEvent   = entity.WorldPointExplosionEvent;
			_entity           = entity;

			_disposable = _explosionRequest.Subscribe(OnExplosionRequest);
		}

		private void OnExplosionRequest (Vector3 position)
		{
			_areaHitscanService.Scan(
				position,
				_blastRadius.Value,
				LayerMask.GetMask("Characters"),
				_entity,
				_explosionDamage.Value);

			_explosionEvent.Invoke(position);
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}
	}
}
