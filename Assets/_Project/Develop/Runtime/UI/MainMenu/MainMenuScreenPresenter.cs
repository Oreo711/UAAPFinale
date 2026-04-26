using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System.Collections.Generic;
using _Project.Develop.Runtime.UI.MainMenu.Stats;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Random = UnityEngine.Random;


namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _screen;

        private readonly ProjectPresentersFactory _projectPresentersFactory;
				private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinePerformer;
        private readonly LevelsListConfig _levelsListConfig;

        private readonly MainMenuPopupService _popupService;

        private readonly List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView screen,
            ProjectPresentersFactory projectPresentersFactory,
            MainMenuPopupService popupService,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinePerformer,
            LevelsListConfig levelsListConfig
        )
        {
            _screen                   = screen;
            _projectPresentersFactory = projectPresentersFactory;
            _popupService             = popupService;
            _sceneSwitcherService     = sceneSwitcherService;
            _coroutinePerformer       = coroutinePerformer;
            _levelsListConfig    = levelsListConfig;
        }

        public void Initialize()
        {
            _screen.OpenLevelsMenuButtonClicked += OnOpenLevelsMenuButtonClicked;

            CreateWallet();
            CreateStats();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            _screen.OpenLevelsMenuButtonClicked -= OnOpenLevelsMenuButtonClicked;

            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_screen.WalletView);

            _childPresenters.Add(walletPresenter);
        }

        private void CreateStats ()
        {
            StatsPresenter statsPresenter = _projectPresentersFactory.CreateStatsPresenter(_screen.StatsView);

            _childPresenters.Add(statsPresenter);
        }

        private void OnOpenLevelsMenuButtonClicked()
        {
            int levelNumber = Random.Range(1, _levelsListConfig.Levels.Count);

            _coroutinePerformer.StartCoroutine(_sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(levelNumber)));
        }
    }
}
