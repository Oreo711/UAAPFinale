using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class CollisionExplosionSystem : IInitializableSystem, IUpdatableSystem
	{
		private ReactiveVariable<float> _explosionDamage;
		private ReactiveVariable<float> _blastRadius;
		private Transform               _transform;
		private ReactiveVariable<bool>  _markedForDeath;
		private Entity                  _entity;

		private readonly AreaHitscanService _areaHitscanService;

		public CollisionExplosionSystem (AreaHitscanService areaHitscanService)
		{
			_areaHitscanService = areaHitscanService;
		}


		public void OnInit (Entity entity)
		{
			_explosionDamage = entity.ExplosionDamage;
			_blastRadius     = entity.BlastRadius;
			_transform       = entity.Transform;
			_markedForDeath  = entity.MarkedForDeath;
			_entity          = entity;
		}

		public void OnUpdate (float deltaTime)
		{
			if (_areaHitscanService.Scan(
				    _transform.position,
				    _blastRadius.Value,
				    LayerMask.GetMask("Characters"),
				    _entity, _explosionDamage.Value))
			{
				_markedForDeath.Value = true;
			}
		}
	}
}
