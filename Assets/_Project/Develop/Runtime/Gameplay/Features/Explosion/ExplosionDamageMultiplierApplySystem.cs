using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionDamageMultiplierApplySystem : IInitializableSystem, IDisposableSystem
	{
		private ReactiveVariable<float> _explosionDamage;
		private ReactiveVariable<float> _multiplier;
		private ReactiveVariable<float> _modifiedDamage;

		private readonly IDisposable[] _disposables = new IDisposable[2];

		public void OnInit (Entity entity)
		{
			_explosionDamage = entity.ExplosionDamage;
			_multiplier      = entity.ExplosionDamageMultiplier;
			_modifiedDamage  = entity.ModifiedExplosionDamage;

			Apply();

			_disposables[0] = _explosionDamage.Subscribe(OnAnyValueChanged);
			_disposables[1] = _multiplier.Subscribe(OnAnyValueChanged);
		}

		private void OnAnyValueChanged (float _, float __)
		{
			Apply();
		}

		private void Apply ()
		{
			_modifiedDamage.Value = _explosionDamage.Value * _multiplier.Value;
		}

		public void OnDispose ()
		{
			foreach (IDisposable disposable in _disposables)
			{
				disposable.Dispose();
			}
		}
	}
}
