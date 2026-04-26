using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class DefeatState : EndGameState, IUpdatableState
    {
        private readonly IInputService        _inputService;
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly PlayerDataProvider   _playerDataProvider;
        private readonly StatsService         _statsService;
        private readonly GameplayPopupService _popupService;

        public DefeatState(
            IInputService inputService,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            PlayerDataProvider playerDataProvider,
            StatsService statsService,
            GameplayPopupService popupService
        ) : base(inputService)
        {
            _inputService         = inputService;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer  = coroutinesPerformer;
            _playerDataProvider   = playerDataProvider;
            _statsService         = statsService;
            _popupService         = popupService;
        }

        public override void Enter()
        {
            base.Enter();

            _popupService.OpenDefeatPopup();

            _statsService.IncrementLosses();

            _coroutinesPerformer.StartCoroutine(_playerDataProvider.SaveAsync());
        }

        public void Update(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _coroutinesPerformer.StartCoroutine(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            }
        }
    }
}
