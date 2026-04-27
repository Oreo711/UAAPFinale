using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Stats
{
	public class StatsUpgradePopupView : PopupViewBase
	{
		[SerializeField] private TMP_Text _title;

		[field: SerializeField] public IconTextListView       CurrencyListView       { get; private set; }
		[field: SerializeField] public UpgradableStatListView UpgradableStatListView { get; private set; }

		public void SetTitle(string title) => _title.text = title;
	}
}
