using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.DamageOverTime
{
	public class ContactDamageOverTimeSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
	{
		private Entity                  _entity;
		private ReactiveVariable<float> _damagePerTick;
		private ReactiveVariable<int>   _ticksPerSecond;
		private Buffer<Entity>          _contacts;

		private float _damageInterval;
		private float _currentDamageInterval;
		private IDisposable _disposable;

		public void OnInit (Entity entity)
		{
			_entity         = entity;
			_damagePerTick  = entity.DamagePerTick;
			_ticksPerSecond = entity.TicksPerSecond;
			_contacts       = entity.ContactEntitiesBuffer;

			RecalculateInterval();
			_currentDamageInterval = _damageInterval;
			_disposable = _ticksPerSecond.Subscribe(OnTickRateChanged);
		}

		private void OnTickRateChanged (int arg1, int arg2)
		{
			RecalculateInterval();
		}

		public void OnUpdate (float deltaTime)
		{
			_currentDamageInterval -= deltaTime;

			if (_currentDamageInterval <= 0)
			{
				for (int i = 0; i < _contacts.Count; i++)
				{
					Entity contactEntity = _contacts.Items[i];

					EntitiesHelper.TryTakeDamageFrom(_entity, contactEntity, _damagePerTick.Value);
				}

				_currentDamageInterval = _damageInterval;
			}
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}

		private void RecalculateInterval ()
		{
			int interval = 1 / _ticksPerSecond.Value;

			_damageInterval = interval;
		}
	}
}
