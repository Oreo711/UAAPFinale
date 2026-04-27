using UnityEngine;


namespace _Project.Develop.Runtime.Configs.Gameplay
{
	[CreateAssetMenu(fileName = "DeployableCostConfig")]
	public class DeployableCostConfig : ScriptableObject
	{
		[SerializeField] private int _mineCost;
		[SerializeField] private int _sentryCost;
		[SerializeField] private int _puddleCost;

		public int MineCost   => _mineCost;
		public int SentryCost => _sentryCost;
		public int PuddleCost => _puddleCost;
	}
}
