using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Gameplay.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
	[CreateAssetMenu(fileName = "StatsUpgradeConfig")]
	public class StatsUpgradeConfig : ScriptableObject
	{
		[SerializeField] private List<StatUpgradeCostConfig> _stats = new List<StatUpgradeCostConfig>();

		public StatUpgradeCostConfig GetStatConfig(StatTypes type)
			=> _stats.First(s => s.Type == type);
	}

	[Serializable]
	public class StatUpgradeCostConfig
	{
		[field: SerializeField] public StatTypes     Type                   { get; private set; }
		[field: SerializeField] public List<float>   StatValues             { get; private set; }
		[field: SerializeField] public CurrencyTypes CostType               { get; private set; } = CurrencyTypes.Diamonds;
		[field: SerializeField] public List<int>     UpgradeToNextLevelCost { get; private set; }
	}

}

