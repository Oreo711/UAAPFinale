using System;
using Assets._Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Assets._Project.Develop.Runtime.UI.Gameplay.Stages
{
	public class ButtonWithTextView : PopupViewBase
	{
		public event Action ButtonClicked;

		[SerializeField] private Button   _button;
		[SerializeField] private TMP_Text _text;

		private void OnEnable ()
		{
			_button.onClick.AddListener(OnButtonClicked);
		}

		private void OnDisable ()
		{
			_button.onClick.RemoveListener(OnButtonClicked);
		}

		public void SetText (string text)
		{
			_text.text = text;
		}

		private void OnButtonClicked ()
		{
			ButtonClicked?.Invoke();
		}
	}
}
