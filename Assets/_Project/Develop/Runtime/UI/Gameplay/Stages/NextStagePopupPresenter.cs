using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.UI.Gameplay.Stages
{
	public class NextStagePopupPresenter : PopupPresenterBase
	{
		private readonly ButtonWithTextView _view;
		private readonly StageProviderService _stageProviderService;

		public NextStagePopupPresenter (
			ICoroutinesPerformer coroutinePerformer,
			ButtonWithTextView view,
			StageProviderService stageProviderService)
			: base(coroutinePerformer)
		{
			_view                 = view;
			_stageProviderService = stageProviderService;
		}

		protected override PopupViewBase PopupView => _view;

		public override void Initialize ()
		{
				base.Initialize();

				_view.ButtonClicked += OnStartStageButtonClicked;

				if (_stageProviderService.HasNextStage())
				{
					_view.SetText("Next Stage");
				}
				else if (!_stageProviderService.HasNextStage())
				{
					_view.SetText("Finish Level");
				}
		}

		private void OnStartStageButtonClicked ()
		{
			_stageProviderService.MustSwitchToNext = true;

			Close();
		}

		public override void Dispose ()
		{
			base.Dispose();

			_view.ButtonClicked -= OnStartStageButtonClicked;
		}
	}
}
