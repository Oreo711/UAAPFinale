using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Configs.Gameplay;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer            _container;
        private readonly TimerServiceFactory    _timerServiceFactory;
        private readonly AIBrainsContext        _brainsContext;
        private readonly IInputService          _inputService;
        private readonly EntitiesLifeContext    _entitiesLifeContext;
        private readonly StageProviderService   _stageProviderService;
        private readonly WalletService          _walletService;
        private readonly PlayerDataProvider     _playerDataProvider;
        private readonly ConfigsProviderService _configsProviderService;
        private readonly ICoroutinesPerformer   _coroutinesPerformer;
        private readonly MainHeroHolderService  _mainHeroHolderService;

        public BrainsFactory(DIContainer container)
        {
            _container              = container;
            _timerServiceFactory    = _container.Resolve<TimerServiceFactory>();
            _brainsContext          = _container.Resolve<AIBrainsContext>();
            _inputService           = _container.Resolve<IInputService>();
            _entitiesLifeContext    = _container.Resolve<EntitiesLifeContext>();
            _stageProviderService   = _container.Resolve<StageProviderService>();
            _walletService          = _container.Resolve<WalletService>();
            _playerDataProvider     = _container.Resolve<PlayerDataProvider>();
            _configsProviderService = _container.Resolve<ConfigsProviderService>();
            _coroutinesPerformer    = _container.Resolve<ICoroutinesPerformer>();
            _mainHeroHolderService  = _container.Resolve<MainHeroHolderService>();
        }

        public StateMachineBrain CreateTowerBrain (Entity entity)
        {
            AIStateMachine stateMachine = CreateTowerStateMachine(entity);
            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateTowerStateMachine (Entity entity)
        {
            PointAndClickExplosionState explosionState  = new PointAndClickExplosionState(entity, _inputService);
            DeployState deployState =
                new DeployState(
                entity,
                _inputService,
                _walletService,
                _playerDataProvider,
                _configsProviderService.GetConfig<DeployableCostConfig>(),
                _coroutinesPerformer);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(explosionState);
            stateMachine.AddState(deployState);

            ICondition explosionToMineDeploy = new FuncCondition(() => _stageProviderService.CurrentStageResult.Value == StageResults.Completed);
            ICondition mineDeployToExplosion = new FuncCondition(() => _stageProviderService.CurrentStageResult.Value == StageResults.Uncompleted);

            stateMachine.AddTransition(explosionState, deployState, explosionToMineDeploy);
            stateMachine.AddTransition(deployState, explosionState, mineDeployToExplosion);

            return stateMachine;
        }

        public StateMachineBrain CreateRangerBrain (Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine stateMachine = CreateRangerStateMachine(entity, targetSelector);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateRangerStateMachine (Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine combatState = CreateAutoAttackStateMachine(entity);

            ChaseHeroState chaseState = new ChaseHeroState(entity, _mainHeroHolderService);

            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromChaseToCombatCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value != null));

            ICompositeCondition fromCombatToChaseCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value == null));

            AIStateMachine behaviour = new AIStateMachine();

            behaviour.AddState(chaseState);
            behaviour.AddState(combatState);

            behaviour.AddTransition(chaseState, combatState, fromChaseToCombatCondition);
            behaviour.AddTransition(combatState, chaseState, fromCombatToChaseCondition);

             FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
             AIParallelState parallelState = new AIParallelState(findTargetState, behaviour);

             AIStateMachine rootStateMachine = new AIStateMachine();
             rootStateMachine.AddState(parallelState);

             return rootStateMachine;
        }

        public StateMachineBrain CreateMainHeroBrain(Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine combatState = CreateAutoAttackStateMachine(entity);

            PlayerInputMovementState movementState = new PlayerInputMovementState(entity, _inputService);

            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromMovementToCombatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value != null))
                .Add(new FuncCondition(() => _inputService.Direction == Vector3.zero));

            ICompositeCondition fromCombatToMovementStateCondition = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => currentTarget.Value == null))
                .Add(new FuncCondition(() => _inputService.Direction != Vector3.zero));

            AIStateMachine behaviour = new AIStateMachine();

            behaviour.AddState(movementState);
            behaviour.AddState(combatState);

            behaviour.AddTransition(movementState, combatState, fromMovementToCombatStateCondition);
            behaviour.AddTransition(combatState, movementState, fromCombatToMovementStateCondition);

            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
            AIParallelState parallelState = new AIParallelState(findTargetState, behaviour);

            AIStateMachine rootStateMachine = new AIStateMachine();
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);
            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateSentryBrain (Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine stateMachine = CreateSentryStateMachine(entity, targetSelector);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateSentryStateMachine(Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine combatState = CreateAutoAttackStateMachine(entity);
            EmptyState     emptyState         = new EmptyState();

            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromIdleToCombatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value != null));

            ICompositeCondition fromCombatToIdleStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value == null));

            AIStateMachine behavior = new AIStateMachine();

            behavior.AddState(emptyState);
            behavior.AddState(combatState);

            behavior.AddTransition(emptyState, combatState, fromIdleToCombatStateCondition);
            behavior.AddTransition(combatState, emptyState, fromCombatToIdleStateCondition);

            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
            AIParallelState parallelState   = new AIParallelState(findTargetState, behavior);

            AIStateMachine rootStateMachine = new AIStateMachine();
            rootStateMachine.AddState(parallelState);

            return rootStateMachine;
        }

        public StateMachineBrain CreateBomberBrain (Entity entity)
        {
            AIStateMachine    stateMachine = CreateBomberStateMachine(entity);
            StateMachineBrain brain        = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateBomberStateMachine (Entity entity)
        {
            ChaseHeroState        chaseHeroState    = new ChaseHeroState(entity, _container.Resolve<MainHeroHolderService>());
            SelfDestructState     selfDestructState = new SelfDestructState(entity);
            MainHeroHolderService heroHolderService = _container.Resolve<MainHeroHolderService>();

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(chaseHeroState);
            stateMachine.AddState(selfDestructState);

            ICondition mustSelfDestruct = new FuncCondition(() =>
                (heroHolderService.MainHero.Transform.position - entity.Transform.position).magnitude <= entity.BlastRadius.Value);

            stateMachine.AddTransition(chaseHeroState, selfDestructState, mustSelfDestruct);

            return stateMachine;
        }

        public StateMachineBrain CreateGhostBrain(Entity entity)
        {
            AIStateMachine stateMachine = CreateRandomMovementStateMachine(entity);
            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateRandomMovementStateMachine(Entity entity)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            RandomMovementState randomMovementState = new RandomMovementState(entity, 0.5f);

            EmptyState emptyState = new EmptyState();

            TimerService movementTimer = _timerServiceFactory.Create(2f);
            disposables.Add(movementTimer);
            disposables.Add(randomMovementState.Entered.Subscribe(movementTimer.Restart));

            TimerService idleTimer = _timerServiceFactory.Create(3f);
            disposables.Add(idleTimer);
            disposables.Add(emptyState.Entered.Subscribe(idleTimer.Restart));

            FuncCondition movementTimerEndedCondition = new FuncCondition(() => movementTimer.IsOver);
            FuncCondition idleTimerEndedCondition = new FuncCondition(() => idleTimer.IsOver);

            AIStateMachine stateMachine = new AIStateMachine(disposables);

            stateMachine.AddState(randomMovementState);
            stateMachine.AddState(emptyState);

            stateMachine.AddTransition(randomMovementState, emptyState, movementTimerEndedCondition);
            stateMachine.AddTransition(emptyState, randomMovementState, idleTimerEndedCondition);

            return stateMachine;
        }

        private AIStateMachine CreateAutoAttackStateMachine(Entity entity)
        {
            RotateToTargetState rotateToTargetState = new RotateToTargetState(entity);

            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ICondition canAttack = entity.CanStartAttack;
            Transform transform = entity.Transform;
            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() =>
                {
                    Entity target = currentTarget.Value;

                    if (target == null)
                        return false;

                    float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.Transform.position - transform.position));
                    return angleToTarget < 3f;
                }));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICondition fromAttackToRotateStateCondition = new FuncCondition(() => inAttackProcess.Value == false);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(rotateToTargetState);
            stateMachine.AddState(attackTriggerState);

            stateMachine.AddTransition(rotateToTargetState, attackTriggerState, fromRotateToAttackCondition);
            stateMachine.AddTransition(attackTriggerState, rotateToTargetState, fromAttackToRotateStateCondition);

            return stateMachine;
        }
    }
}
