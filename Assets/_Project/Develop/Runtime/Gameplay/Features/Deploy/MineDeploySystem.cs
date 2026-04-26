using System;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Mines
{
	public class MineDeploySystem : IInitializableSystem, IDisposableSystem
	{
		private ReactiveEvent<Vector3>  _deployRequest;
		private ReactiveVariable<Teams> _team;

		private readonly EntitiesFactory _entitiesFactory;
		private readonly MineConfig _mineConfig;
		private readonly EntitiesLifeContext _entitiesLifeContext;

		private IDisposable _disposable;

		public MineDeploySystem (EntitiesFactory entitiesFactory, MineConfig mineConfig, EntitiesLifeContext entitiesLifeContext)
		{
			_entitiesFactory          = entitiesFactory;
			_mineConfig               = mineConfig;
			_entitiesLifeContext = entitiesLifeContext;
		}

		public void OnInit (Entity entity)
		{
			_deployRequest = entity.MineDeployRequest;
			_team          = entity.Team;

			_disposable = _deployRequest.Subscribe(OnDeployRequest);
		}

		private void OnDeployRequest (Vector3 position)
		{
			Entity entity = _entitiesFactory.CreateMine(position, _mineConfig);

			entity.AddTeam(new ReactiveVariable<Teams>(_team.Value));

			_entitiesLifeContext.Add(entity);
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}
	}
}
