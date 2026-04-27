using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using _Project.Develop.Runtime.Gameplay.Features.Stats;
using _Project.Develop.Runtime.UI.CommonViews;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action OpenLevelsMenuButtonClicked;
        public event Action OpenUpgradesMenuButtonClicked;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }
        [field: SerializeField] public RatioView StatsView { get; private set; }

        [SerializeField] private Button _openLevelsMenuButton;
        [SerializeField] private Button _openUpgradesMenuButton;

        private void OnEnable()
        {
            _openLevelsMenuButton.onClick.AddListener(OnOpenLevelsMenuButtonClicked);
            _openUpgradesMenuButton.onClick.AddListener(OnOpenUpgradesMenuButtonClicked);
        }


        private void OnDisable()
        {
            _openLevelsMenuButton.onClick.RemoveListener(OnOpenLevelsMenuButtonClicked);
            _openUpgradesMenuButton.onClick.RemoveListener(OnOpenUpgradesMenuButtonClicked);
        }

        private void OnOpenLevelsMenuButtonClicked() => OpenLevelsMenuButtonClicked?.Invoke();

        private void OnOpenUpgradesMenuButtonClicked () => OpenUpgradesMenuButtonClicked?.Invoke();
    }
}
