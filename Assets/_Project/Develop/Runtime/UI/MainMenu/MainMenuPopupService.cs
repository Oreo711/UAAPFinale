using System;
using _Project.Develop.Runtime.Gameplay.Features.Stats;
using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPopupService : PopupService
    {
        private readonly MainMenuUIRoot _uiRoot;
        private readonly MainMenuPresentersFactory _menuPresentersFactory;

        public MainMenuPopupService(
            ViewsFactory viewsFactory,
            ProjectPresentersFactory presentersFactory,
            MainMenuPresentersFactory menuPresentersFactory,
            MainMenuUIRoot uiRoot)
            : base(viewsFactory, presentersFactory)
        {
            _uiRoot                = uiRoot;
            _menuPresentersFactory = menuPresentersFactory;
        }

        protected override Transform PopupLayer => _uiRoot.PopupsLayer;

        public StatsUpgradePopupPresenter OpenStatsUpgradePopup (Action closedCallback = null)
        {
            StatsUpgradePopupView view = ViewsFactory.Create<StatsUpgradePopupView>(ViewIDs.StatsUpgradePopupView, PopupLayer);

            StatsUpgradePopupPresenter popup = _menuPresentersFactory.CreateStatsUpgradePopupPresenter(view);

            OnPopupCreated(popup, view, closedCallback);

            return popup;
        }
    }
}
