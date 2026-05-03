using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Features.Stats;
using Assets._Project.Develop.Runtime.Configs.Meta.Wallet;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack;
using Assets._Project.Develop.Runtime.Gameplay.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Stats
{
	public class UpgradableStatPresenter : IPresenter
    {
        private readonly UpgradableStatView   _view;
        private readonly StatsViewConfig      _statsViewConfig;
        private readonly StatsUpgradeService  _upgradeStatsService;
        private readonly WalletService        _walletService;
        private readonly StatTypes            _statType;
        private readonly CurrencyIconsConfig  _currencyIconsConfig;

        private List<IDisposable> _disposables = new();

        public UpgradableStatPresenter(
            UpgradableStatView view,
            StatTypes type,
            StatsViewConfig statsShowConfig,
            StatsUpgradeService upgradeStatsService,
            WalletService walletService,
            PlayerDataProvider playerDataProvider,
            CurrencyIconsConfig currencyIconsConfig,
            ICoroutinesPerformer coroutinesPerformer
        )
        {
            _view                = view;
            _statsViewConfig     = statsShowConfig;
            _upgradeStatsService = upgradeStatsService;
            _walletService       = walletService;
            _statType            = type;
            _currencyIconsConfig = currencyIconsConfig;
        }

        public UpgradableStatView View => _view;

        public void Initialize()
        {
            StatViewConfig statShowData = _statsViewConfig.GetStatViewData(_statType);

            _view.Initialize(statShowData.Name, statShowData.Sprite, GetStatValueText());

            UpdateBuyButtonState();

            _view.BuyButtonView.Click += OnBuyButtonClicked;

            IReadOnlyVariable<int> statLevel = _upgradeStatsService.GetStatLevelFor(_statType);
            _disposables.Add(statLevel.Subscribe(OnStatUpgradeLevelChanged));

            IReadOnlyVariable<int> currency = _walletService.GetCurrency(_upgradeStatsService.GetUpgradeCostTypeFor(_statType));
            _disposables.Add(currency.Subscribe(OnWalletChanged));
        }

        public void Dispose()
        {
            _view.BuyButtonView.Click -= OnBuyButtonClicked;

            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();
        }

        private void OnStatUpgradeLevelChanged(int arg1, int arg2) => _view.SetStatValueText(GetStatValueText());


        private void OnWalletChanged(int arg1, int arg2) => UpdateBuyButtonState();


        private void OnBuyButtonClicked()
        {
           _upgradeStatsService.TryBuyUpgrade(_statType);
        }

        private void UpdateBuyButtonState()
        {
            if(_upgradeStatsService.TryGetUpgradeCostFor(_statType, out CurrencyTypes currencyType, out int cost))
            {
                _view.BuyButtonView.SetPriceText(cost.ToString());
                _view.BuyButtonView.ShowIcon();
                _view.BuyButtonView.SetIcon(_currencyIconsConfig.GetSpriteFor(currencyType));

                if (_walletService.Enough(currencyType, cost))
                    _view.BuyButtonView.Unlock();
                else
                    _view.BuyButtonView.Lock();
            }
            else
            {
                _view.BuyButtonView.HideIcon();
                _view.BuyButtonView.Lock();
                _view.BuyButtonView.SetPriceText("MAX");
            }
        }

        private string GetStatValueText()
        {
            float statValue = _upgradeStatsService.GetCurrentStatValueFor(_statType);
            string result = statValue.ToString();

            if(_upgradeStatsService.TryGetStatValueForNextLevel(_statType, out float nextStatValue))
            {
                result += $"<color=green>>{nextStatValue}</color>";
            }

            return result;
        }
    }
}
