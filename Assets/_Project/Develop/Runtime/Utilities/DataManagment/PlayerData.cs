using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Features.Stats;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Utilities.DataManagment
{
    public class PlayerData : ISaveData
    {
        public Dictionary<CurrencyTypes, int> WalletData;
        public List<int>                      CompletedLevels;
        public Dictionary<StatTypes, int>     StatsUpgradesLevels;

        public int Wins;
        public int Losses;
    }
}
