using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace _Project.Develop.Runtime.Gameplay.Features.Stats
{
    public class StatsUpgradeService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private readonly ConfigsProviderService _configsProviderService;
        private readonly WalletService          _walletService;
        private readonly PlayerDataProvider     _playerDataProvider;
        private readonly ICoroutinesPerformer   _coroutinesPerformer;

        private readonly Dictionary<StatTypes, ReactiveVariable<int>> _statLevels = new();

        public StatsUpgradeService (PlayerDataProvider playerDataProvider, ConfigsProviderService configsProviderService, WalletService walletService, ICoroutinesPerformer coroutinesPerformer)
        {
            _configsProviderService = configsProviderService;
            _walletService          = walletService;
            _playerDataProvider     = playerDataProvider;
            _coroutinesPerformer    = coroutinesPerformer;

            playerDataProvider.RegisterReader(this);
            playerDataProvider.RegisterWriter(this);
        }

        public List<StatTypes> AvailableStats => _statLevels.Keys.ToList();

        public IReadOnlyVariable<int> GetStatLevelFor (StatTypes statType)
            => _statLevels[statType];

        private StatsUpgradeConfig PlayerStatsByLevelConfig => _configsProviderService.GetConfig<StatsUpgradeConfig>();

        public float GetCurrentStatValueFor (StatTypes type)
        {
            return PlayerStatsByLevelConfig.GetStatConfig(type).StatValues[_statLevels[type].Value - 1];
        }

        public CurrencyTypes GetUpgradeCostTypeFor (StatTypes type)
        {
            return PlayerStatsByLevelConfig.GetStatConfig(type).CostType;
        }

        public bool TryGetStatValueForNextLevel (StatTypes type, out float statValue)
        {
            StatUpgradeCostConfig statData = PlayerStatsByLevelConfig.GetStatConfig(type);

            if (statData.StatValues.Count <= _statLevels[type].Value)
            {
                statValue = 0;
                return false;
            }

            statValue = statData.StatValues[_statLevels[type].Value];
            return true;
        }

        public bool TryGetUpgradeCostFor (StatTypes type, out CurrencyTypes costType, out int cost)
        {
            StatUpgradeCostConfig statData = PlayerStatsByLevelConfig.GetStatConfig(type);

            if (statData.UpgradeToNextLevelCost.Count <= _statLevels[type].Value - 1)
            {
                costType = default(CurrencyTypes);
                cost     = 0;
                return false;
            }

            costType = statData.CostType;
            cost     = statData.UpgradeToNextLevelCost[_statLevels[type].Value - 1];
            return true;
        }

        public void TryBuyUpgrade (StatTypes type)
        {
            if (TryGetUpgradeCostFor(type, out CurrencyTypes currencyType, out int cost))
            {
                if (_walletService.Enough(currencyType, cost))
                {
                    if (TryUpgradeStat(type) == false)
                        throw new Exception();

                    _walletService.Spend(currencyType, cost);

                    _coroutinesPerformer.StartCoroutine(_playerDataProvider.SaveAsync());
                }
            }
        }

        public bool TryUpgradeStat (StatTypes type)
        {
            StatUpgradeCostConfig statData = PlayerStatsByLevelConfig.GetStatConfig(type);

            if (statData.StatValues.Count <= _statLevels[type].Value)
                return false;

            _statLevels[type].Value += 1;
            return true;
        }


        public void ReadFrom (PlayerData data)
        {
            foreach (var statLevel in data.StatsUpgradesLevels)
            {
                if (_statLevels.ContainsKey(statLevel.Key))
                    _statLevels[statLevel.Key].Value = statLevel.Value;
                else
                    _statLevels.Add(statLevel.Key, new ReactiveVariable<int>(statLevel.Value));
            }
        }

        public void WriteTo (PlayerData data)
        {
            foreach (var stat in _statLevels)
            {
                if (data.StatsUpgradesLevels.ContainsKey(stat.Key))
                    data.StatsUpgradesLevels[stat.Key] = stat.Value.Value;
                else
                    data.StatsUpgradesLevels.Add(stat.Key, stat.Value.Value);
            }
        }
    }
}
