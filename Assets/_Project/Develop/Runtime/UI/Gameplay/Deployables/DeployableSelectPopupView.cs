using System;
using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;
using UnityEngine.UI;


namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
	public class DeployableSelectPopupView : PopupViewBase
	{
		public event Action MineButtonClicked;
		public event Action SentryButtonClicked;
		public event Action PuddleButtonClicked;

		[SerializeField] private Button _mineButton;
		[SerializeField] private Button _sentryButton;
		[SerializeField] private Button _puddleButton;

		private void OnEnable ()
		{
			_mineButton.onClick.AddListener(OnMineButtonClicked);
			_sentryButton.onClick.AddListener(OnSentryButtonClicked);
			_puddleButton.onClick.AddListener(OnPuddleButtonClicked);
		}

		private void OnPuddleButtonClicked ()
		{
			PuddleButtonClicked?.Invoke();
		}

		private void OnMineButtonClicked ()
		{
			MineButtonClicked?.Invoke();
		}

		private void OnSentryButtonClicked ()
		{
			SentryButtonClicked?.Invoke();
		}

		private void OnDisable ()
		{
			_mineButton.onClick.RemoveListener(OnMineButtonClicked);
			_sentryButton.onClick.RemoveListener(OnSentryButtonClicked);
		}
	}
}
