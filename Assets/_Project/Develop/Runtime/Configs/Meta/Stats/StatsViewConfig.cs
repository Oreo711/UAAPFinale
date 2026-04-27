using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Gameplay.Features.Stats;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
	[CreateAssetMenu(menuName = "Configs/Meta/StatsViewConfig", fileName = "StatsViewConfig")]
	public class StatsViewConfig : ScriptableObject
	{
		[SerializeField] private List<StatViewConfig> _statShowDatas;

		public StatViewConfig GetStatViewData(StatTypes statType) => _statShowDatas.First(s => s.Type == statType);
	}

	[Serializable]
	public class StatViewConfig
	{
		[field: SerializeField] public StatTypes Type   { get; private set; }
		[field: SerializeField] public string    Name   { get; private set; }
		[field: SerializeField] public Sprite    Sprite { get; private set; }
	}
}
