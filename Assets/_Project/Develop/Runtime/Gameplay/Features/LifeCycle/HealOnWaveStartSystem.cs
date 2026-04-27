using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
	public class HealOnWaveStartSystem : IInitializableSystem, IDisposableSystem
	{
		private ReactiveVariable<float> _healOnWaveStart;
		private ReactiveVariable<float> _currentHealth;
		private ReactiveVariable<float> _maxHealth;

		private readonly StageProviderService _stageProviderService;

		private IDisposable _disposable;

		public HealOnWaveStartSystem (StageProviderService stageProviderService)
		{
			_stageProviderService = stageProviderService;
		}

		public void OnInit (Entity entity)
		{
			_healOnWaveStart = entity.HealOnWaveStart;
			_currentHealth   = entity.CurrentHealth;
			_maxHealth       = entity.MaxHealth;

			_disposable = _stageProviderService.CurrentStageResult.Subscribe(OnStageResultChanged);
		}

		private void OnStageResultChanged (StageResults _, StageResults result)
		{
			if (result == StageResults.Completed)
			{
				return;
			}

			Heal();
		}

		private void Heal ()
		{
			if (_currentHealth.Value + _healOnWaveStart.Value < _maxHealth.Value)
			{
				_currentHealth.Value += _healOnWaveStart.Value;

				return;
			}

			_currentHealth.Value = _maxHealth.Value;
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}
	}
}
