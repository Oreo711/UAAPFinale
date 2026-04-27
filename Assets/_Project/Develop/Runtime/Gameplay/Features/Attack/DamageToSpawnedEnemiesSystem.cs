using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
	public class DamageToSpawnedEnemiesSystem : IInitializableSystem, IDisposableSystem
	{
		private ReactiveVariable<float> _damage;
		private Entity                  _entity;

		private readonly EntitiesLifeContext _entitiesLifeContext;

		public DamageToSpawnedEnemiesSystem (EntitiesLifeContext entitiesLifeContext)
		{
			_entitiesLifeContext = entitiesLifeContext;
		}

		public void OnInit (Entity entity)
		{
			_damage = entity.DamageToSpawnedEnemies;
			_entity = entity;

			_entitiesLifeContext.Added += OnEntityAdded;
		}

		private void OnEntityAdded (Entity entity)
		{
			EntitiesHelper.TryTakeDamageFrom(_entity, entity, _damage.Value);
		}

		public void OnDispose ()
		{
			_entitiesLifeContext.Added -= OnEntityAdded;
		}
	}
}
