using _Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.ResultPopups;
using Assets._Project.Develop.Runtime.UI.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;


namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayPresentersFactory
	{
		private readonly DIContainer       _container;
		private readonly GameplayInputArgs _gameplayInputArgs;

		public GameplayPresentersFactory(DIContainer container, GameplayInputArgs gameplayInputArgs)
		{
			_container         = container;
			_gameplayInputArgs = gameplayInputArgs;
		}

		public NextStagePopupPresenter CreateNextStagePopupPresenter (ButtonWithTextView view)
		{
			return new NextStagePopupPresenter(
				_container.Resolve<ICoroutinesPerformer>(),
				view,
				_container.Resolve<StageProviderService>());
		}

		public WinPopupPresenter CreateWinPopupPresenter(WinPopupView view)
		{
			return new WinPopupPresenter(
				_container.Resolve<ICoroutinesPerformer>(),
				view,
				_container.Resolve<SceneSwitcherService>());
		}

		public DefeatPopupPresenter CreateDefeatPopupPresenter(DefeatPopupView view)
		{
			return new DefeatPopupPresenter(
				_container.Resolve<ICoroutinesPerformer>(),
				view,
				_container.Resolve<SceneSwitcherService>(),
				_gameplayInputArgs);
		}

		public GameplayScreenPresenter CreateGameplayScreenPresenter(GameplayScreenView view)
		{
			return new GameplayScreenPresenter(view, _container.Resolve<GameplayPresentersFactory>());
		}

		public StagePresenter CreateStagePresenter(IconTextView view)
		{
			return new StagePresenter(view, _container.Resolve<StageProviderService>());
		}

		public EntityHealthPresenter CreateEntityHealthPresenter(Entity entity, BarWithText view)
		{
			return new EntityHealthPresenter(entity, view);
		}

		public EntitiesHealthDisplayPresenter CreateEntitiesHealthDisplayPresenter(EntitiesHealthDisplay view)
		{
			return new EntitiesHealthDisplayPresenter(
				_container.Resolve<EntitiesLifeContext>(),
				view,
				_container.Resolve<ViewsFactory>(),
				this);
		}
	}
}
