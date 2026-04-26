using System;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Deploy
{
	public class SentryDeploySystem : IInitializableSystem, IDisposableSystem
	{
		private ReactiveEvent<Vector3>  _deployRequest;
		private ReactiveVariable<Teams> _team;

		private readonly EntitiesFactory     _entitiesFactory;
		private readonly SentryConfig        _sentryConfig;
		private readonly EntitiesLifeContext _entitiesLifeContext;
		private readonly BrainsFactory       _brainsFactory;

		private IDisposable _disposable;

		public SentryDeploySystem (EntitiesFactory entitiesFactory, SentryConfig sentryConfig, EntitiesLifeContext entitiesLifeContext, BrainsFactory brainsFactory)
		{
			_entitiesFactory     = entitiesFactory;
			_sentryConfig        = sentryConfig;
			_entitiesLifeContext = entitiesLifeContext;
			_brainsFactory  = brainsFactory;
		}

		public void OnInit (Entity entity)
		{
			_deployRequest = entity.SentryDeployRequest;
			_team          = entity.Team;

			_disposable = _deployRequest.Subscribe(OnDeployRequest);
		}

		private void OnDeployRequest (Vector3 position)
		{
			Entity entity = _entitiesFactory.CreateSentry(position, _sentryConfig);
			_brainsFactory.CreateSentryBrain(entity, new NearestDamageableTargetSelector(entity));

			entity.AddTeam(new ReactiveVariable<Teams>(_team.Value));

			_entitiesLifeContext.Add(entity);
		}

		public void OnDispose ()
		{
			_disposable.Dispose();
		}
	}
}
