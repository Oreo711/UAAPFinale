using _Project.Develop.Runtime.Gameplay.Features.Stats;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Configs.Meta.Wallet;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack;
using Assets._Project.Develop.Runtime.Gameplay.Features.Stats;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;


namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPresentersFactory
    {
        private readonly DIContainer _container;

        public MainMenuPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public StatsUpgradePopupPresenter CreateStatsUpgradePopupPresenter (StatsUpgradePopupView view)
        {
            return new StatsUpgradePopupPresenter(
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<StatsUpgradeService>(),
                _container.Resolve<ViewsFactory>());
        }

        public MainMenuScreenPresenter CreateMainMenuScreen(MainMenuScreenView view)
        {
            return new MainMenuScreenPresenter(
                view,
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<MainMenuPopupService>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>());
        }
    }
}
