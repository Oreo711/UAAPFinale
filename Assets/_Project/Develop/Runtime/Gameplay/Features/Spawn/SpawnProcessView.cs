using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
		public class SpawnProcessView : EntityView
		{
			[SerializeField] private ParticleSystem _spawnEffectPrefab;

			private ReactiveVariable<bool> _inSpawnProcess;
			private Transform              _entityTransform;

			private IDisposable _inSpawnProcessChangedDisposable;

			protected override void OnEntityStartedWork(Entity entity)
			{
				_inSpawnProcess  = entity.InSpawnProcess;
				_entityTransform = entity.Transform;

				_inSpawnProcessChangedDisposable = _inSpawnProcess.Subscribe(OnSpawnProcessChanged);
				UpdateSpawnProcessKey(_inSpawnProcess.Value);
			}

			public override void Cleanup(Entity entity)
			{
				base.Cleanup(entity);

				_inSpawnProcessChangedDisposable.Dispose();
			}

			private void OnSpawnProcessChanged(bool arg1, bool newValue) => UpdateSpawnProcessKey(newValue);

			private void UpdateSpawnProcessKey(bool value)
			{
				if (value)
					Instantiate(_spawnEffectPrefab, _entityTransform.position, _spawnEffectPrefab.transform.rotation, null);
			}
		}
}
