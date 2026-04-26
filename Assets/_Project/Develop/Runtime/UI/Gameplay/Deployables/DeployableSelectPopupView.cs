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

		[SerializeField] private Button _mineButton;
		[SerializeField] private Button _sentryButton;

		private void OnEnable ()
		{
			_mineButton.onClick.AddListener(OnMineButtonClicked);
			_sentryButton.onClick.AddListener(OnSentryButtonClicked);
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
