using System;
using _Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.UI.Core;


namespace _Project.Develop.Runtime.UI.MainMenu.Stats
{
	public class WinratePresenter : IPresenter
	{
		private readonly WinrateService _winrateService;

		private readonly RatioView _view;

		private readonly IDisposable[] _disposables = new IDisposable[2];

		public WinratePresenter (RatioView view, WinrateService winrateService)
		{
			_view = view;
			_winrateService = winrateService;
		}

		public RatioView View => _view;

		public void Initialize ()
		{
			UpdateViewNumerator(0, _winrateService.Wins.Value);
			UpdateViewDenominator(0, _winrateService.Losses.Value);

			_disposables[0] = _winrateService.Wins.Subscribe(UpdateViewNumerator);
			_disposables[1] = _winrateService.Losses.Subscribe(UpdateViewDenominator);
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
