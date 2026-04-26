using System;
using _Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.UI.Core;


namespace _Project.Develop.Runtime.UI.MainMenu.Stats
{
	public class StatsPresenter : IPresenter
	{
		private readonly StatsService _statsService;

		private readonly RatioView _view;

		private readonly IDisposable[] _disposables = new IDisposable[2];

		public StatsPresenter (RatioView view, StatsService statsService)
		{
			_view = view;
			_statsService = statsService;
		}

		public RatioView View => _view;

		public void Initialize ()
		{
			UpdateViewNumerator(0, _statsService.Wins.Value);
			UpdateViewDenominator(0, _statsService.Losses.Value);

			_disposables[0] = _statsService.Wins.Subscribe(UpdateViewNumerator);
			_disposables[1] = _statsService.Losses.Subscribe(UpdateViewDenominator);
		}

		public void Dispose ()
		{
			foreach (IDisposable disposable in _disposables) {
				disposable.Dispose();
			}
		}

		private void UpdateViewNumerator (int _, int newValue)
		{
			_view.SetNumerator(newValue);
		}

		private void UpdateViewDenominator (int _, int newValue)
		{
			_view.SetDenomenator(newValue);
		}
	}
}
