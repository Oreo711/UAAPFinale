using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
	public class DamageToSpawnedEnemiesSystem : IInitializableSystem, IDisposableSystem
	{
		private ReactiveVariable<float> _damage;
		private Entity                  _entity;

		private readonly EntitiesLifeContext _entitiesLifeContext;
		private readonly StageProviderService _stageProviderService;
		private          int                 _count;
		private          int                 _currentWaveCount;

		private IDisposable _disposable;

		public DamageToSpawnedEnemiesSystem (EntitiesLifeContext entitiesLifeContext, StageProviderService stageProviderService, int count)
		{
			_entitiesLifeContext  = entitiesLifeContext;
			_stageProviderService = stageProviderService;
			_count                = count;
		}

		public void OnInit (Entity entity)
		{
			_damage = entity.DamageToSpawnedEnemies;
			_entity = entity;
			_currentWaveCount = _count;

			_entitiesLifeContext.Added += OnEntityAdded;
			_disposable                =  _stageProviderService.CurrentStageResult.Subscribe(OnStageResultChanged);
		}

		private void OnStageResultChanged (StageResults _, StageResults result)
		{
			if (result == StageResults.Completed)
			{
				_currentWaveCount = _count;
			}
		}

		private void OnEntityAdded (Entity entity)
		{
			if (_currentWaveCount >= 0)
			{
				EntitiesHelper.TryTakeDamageFrom(_entity, entity, _damage.Value);
			}

			_currentWaveCount--;
		}

		public void OnDispose ()
		{
			_entitiesLifeContext.Added -= OnEntityAdded;
		}
	}
}
