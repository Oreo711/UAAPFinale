using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class WinState : EndGameState, IUpdatableState
    {
        private readonly IInputService            _inputService;
        private readonly LevelsProgressionService _levelsProgressionService;
        private readonly GameplayInputArgs        _gameplayInputArgs;
        private readonly PlayerDataProvider       _playerDataProvider;
        private readonly SceneSwitcherService     _sceneSwitcherService;
        private readonly ICoroutinesPerformer     _coroutinesPerformer;
        private readonly WinrateService             _winrateService;
        private readonly WalletService            _walletService;
        private readonly LevelsListConfig         _levelsListConfig;
        private readonly GameplayPopupService     _popupService;

        public WinState(
            IInputService inputService,
            WinrateService winrateService,
            LevelsProgressionService levelsProgressionService,
            GameplayInputArgs gameplayInputArgs,
            PlayerDataProvider playerDataProvider,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            WalletService walletService,
            LevelsListConfig levelsListConfig,
            GameplayPopupService popupService
        ) : base(inputService)
        {
            _inputService             = inputService;
            _levelsProgressionService = levelsProgressionService;
            _winrateService             = winrateService;
            _gameplayInputArgs        = gameplayInputArgs;
            _playerDataProvider       = playerDataProvider;
            _sceneSwitcherService     = sceneSwitcherService;
            _coroutinesPerformer      = coroutinesPerformer;
            _walletService            = walletService;
            _levelsListConfig         = levelsListConfig;
            _popupService             = popupService;
        }

        public override void Enter()
        {
            base.Enter();

            _popupService.OpenWinPopup();

            _levelsProgressionService.AddLevelToCompleted(_gameplayInputArgs.LevelNumber);
            _winrateService.IncrementWins();

            LevelConfig config = _levelsListConfig.GetBy(_gameplayInputArgs.LevelNumber);

            int goldReward    = config.WinGoldReward;
            int diamondReward = config.WinDiamondReward;

            _walletService.Add(CurrencyTypes.Gold, goldReward);
            _walletService.Add(CurrencyTypes.Diamonds, diamondReward);

            _coroutinesPerformer.StartCoroutine(_playerDataProvider.SaveAsync());
        }

        public void Update(float deltaTime)
        {
            if (_inputService.Clicked)
            {
                _coroutinesPerformer.StartCoroutine(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            }
        }
    }
}
