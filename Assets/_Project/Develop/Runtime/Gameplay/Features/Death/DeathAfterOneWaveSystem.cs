using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace _Project.Develop.Runtime.Gameplay.Features.Death
{
	public class DeathAfterOneWaveSystem : IInitializableSystem, IDisposableSystem
	{
		private readonly StageProviderService   _stageProviderService;
		private          ReactiveVariable<bool> _markedForDeath;

		private bool        _hasBeenInStage;
		private IDisposable _disposable;

		public DeathAfterOneWaveSystem (StageProviderService stageProviderService)
		{
			_stageProviderService = stageProviderService;
		}

		public void OnInit (Entity entity)
		{
			_markedForDeath = entity.MarkedForDeath;
			_hasBeenInStage = false;

			_disposable = _stageProviderService.CurrentStageResult.Subscribe(OnStageResultChanged);
		}

		private void OnStageResultChanged (StageResults _, StageResults result)
		{
			if (result == StageResults.Uncompleted)
			{
				_hasBeenInStage = true;
			}

			if (result == StageResults.Completed && _hasBeenInStage)
			{
				_markedForDeath.Value = true;
			}
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}
	}
}
