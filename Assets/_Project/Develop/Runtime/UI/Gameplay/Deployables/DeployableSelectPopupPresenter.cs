using System;
using Assets._Project.Develop.Runtime.Gameplay.Features.Deploy;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;


namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
	public class DeployableSelectPopupPresenter : PopupPresenterBase
	{
		private readonly DeployableSelectPopupView _view;
		private readonly MainHeroHolderService _mainHeroHoldersService;

		public DeployableSelectPopupPresenter (
			ICoroutinesPerformer coroutinesPerformer,
			DeployableSelectPopupView view,
			MainHeroHolderService mainHeroHoldersService
		)
			: base(coroutinesPerformer)
		{
			_view                        = view;
			_mainHeroHoldersService = mainHeroHoldersService;
		}

		protected override PopupViewBase PopupView => _view;

		public override void Initialize ()
		{
			base.Initialize();

			_view.MineButtonClicked   += OnMineButtonClicked;
			_view.SentryButtonClicked += OnSentryButtonClicked;
			_view.PuddleButtonClicked += OnPuddleButtonClicked;
		}

		private void OnPuddleButtonClicked ()
		{
			_mainHeroHoldersService.MainHero.CurrentDeployable.Value = Deployables.Puddle;
		}

		private void OnMineButtonClicked ()
		{
			_mainHeroHoldersService.MainHero.CurrentDeployable.Value = Deployables.Mine;
		}

		private void OnSentryButtonClicked ()
		{
			_mainHeroHoldersService.MainHero.CurrentDeployable.Value = Deployables.Sentry;
		}

		public override void Dispose ()
		{
			base.Dispose();

			_view.MineButtonClicked -= OnMineButtonClicked;
			_view.SentryButtonClicked -= OnSentryButtonClicked;
			_view.PuddleButtonClicked -= OnPuddleButtonClicked;
		}
	}
}
