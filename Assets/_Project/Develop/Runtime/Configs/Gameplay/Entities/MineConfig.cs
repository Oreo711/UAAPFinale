using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using UnityEngine;


namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
	[CreateAssetMenu(fileName = "MineConfig")]
	public class MineConfig : EntityConfig
	{
		[SerializeField] private float _damage;
		[SerializeField] private float _range;
		[SerializeField] private int _cost;

		public float Damage => _damage;
		public float Range  => _range;
		public int Cost => _cost;
	}
}
