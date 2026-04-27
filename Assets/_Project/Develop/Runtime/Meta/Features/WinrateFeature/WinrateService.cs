using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Meta.Features.Stats
{
	public class WinrateService : IDataReader<PlayerData>, IDataWriter<PlayerData>
	{
		private readonly ReactiveVariable<int> _wins;
		private readonly ReactiveVariable<int> _losses;

		public WinrateService (PlayerDataProvider playerDataProvider)
		{
			_wins   = new ReactiveVariable<int>();
			_losses = new ReactiveVariable<int>();

			playerDataProvider.RegisterWriter(this);
			playerDataProvider.RegisterReader(this);
		}

		public ReactiveVariable<int> Wins   => _wins;
		public ReactiveVariable<int> Losses => _losses;

		public void IncrementWins ()
		{
			_wins.Value++;
		}

		public void IncrementLosses ()
		{
			_losses.Value++;
		}

		public void Reset ()
		{
			_wins.Value   = 0;
			_losses.Value = 0;
		}

		public void ReadFrom (PlayerData data)
		{
			_wins.Value   = data.Wins;
			_losses.Value = data.Losses;
		}

		public void WriteTo (PlayerData data)
		{
			data.Wins   = _wins.Value;
			data.Losses = _losses.Value;
		}
	}
}
