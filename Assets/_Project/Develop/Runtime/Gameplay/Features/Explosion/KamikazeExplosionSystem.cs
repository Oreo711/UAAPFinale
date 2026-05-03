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

		private readonly AreaHitscanService _areaHitscanService;

		private IDisposable _disposable;

		public KamikazeExplosionSystem(AreaHitscanService areaHitscanService)
		{
			_areaHitscanService = areaHitscanService;
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
			_areaHitscanService.Scan(
				_transform.position,
				_blastRadius.Value,
				LayerMask.GetMask("Characters"),
				_entity,
				_explosionDamage.Value);

			_markedForDeath.Value = true;
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}
	}
}
