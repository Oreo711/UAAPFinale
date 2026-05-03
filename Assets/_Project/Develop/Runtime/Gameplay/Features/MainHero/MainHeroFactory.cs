using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Features.Stats;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack;
using Assets._Project.Develop.Runtime.Gameplay.Features.Explosion;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MainHero
{
    public class MainHeroFactory
    {
        private readonly DIContainer _container;

        private readonly EntitiesFactory          _entitiesFactory;
        private readonly BrainsFactory            _brainsFactory;
        private readonly ConfigsProviderService   _configsProviderService;
        private readonly EntitiesLifeContext      _entitiesLifeContext;
        private readonly StatsUpgradeService      _statsUpgradeService;

        public MainHeroFactory(DIContainer container)
        {
            _container                     = container;
            _entitiesFactory               = _container.Resolve<EntitiesFactory>();
            _brainsFactory                 = _container.Resolve<BrainsFactory>();
            _configsProviderService        = _container.Resolve<ConfigsProviderService>();
            _entitiesLifeContext           = _container.Resolve<EntitiesLifeContext>();
            _statsUpgradeService           = _container.Resolve<StatsUpgradeService>();
        }

        public Entity Create(Vector3 position, int levelNumber)
        {
            LevelsListConfig config = _configsProviderService.GetConfig<LevelsListConfig>();

            LevelConfig levelConfig = config.GetBy(levelNumber);

            Entity entity = _entitiesFactory.CreateTower(position, levelConfig.TowerConfig);
            _brainsFactory.CreateTowerBrain(entity);

            Dictionary<StatTypes, float> stats = GetStats();

            entity
                .AddIsMainHero()
                .AddTeam(new ReactiveVariable<Teams>(Teams.MainHero))
                .AddModifiedExplosionDamage()
                .AddExplosionDamageMultiplier(new ReactiveVariable<float>(stats[StatTypes.ExplosionDamageMultiplier]))
                .AddHealOnWaveStart(new ReactiveVariable<float>(stats[StatTypes.HealOnWaveStart]))
                .AddDamageToSpawnedEnemies(new ReactiveVariable<float>(stats[StatTypes.DamageToSpawnedEnemies]));

            entity
                .AddSystem(new ExplosionDamageMultiplierApplySystem())
                .AddSystem(new HealOnWaveStartSystem(_container.Resolve<StageProviderService>()))
                .AddSystem(new DamageToSpawnedEnemiesSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Dictionary<StatTypes, float> GetStats()
        {
            Dictionary<StatTypes, float> stats = new();

            foreach (StatTypes statType in Enum.GetValues(typeof(StatTypes)))
                stats.Add(statType, _statsUpgradeService.GetCurrentStatValueFor(statType));

            return stats;
        }
    }
}
