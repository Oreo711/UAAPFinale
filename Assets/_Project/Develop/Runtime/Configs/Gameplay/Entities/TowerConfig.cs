using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore
{
	[CreateAssetMenu(fileName = "TowerConfig")]
	public class TowerConfig : EntityConfig
	{
		[SerializeField] private float _health;
		[SerializeField] private float _damage;
		[SerializeField] private float _radius;

		public float Health => _health;
		public float Damage => _damage;
		public float Radius => _radius;
	}
}
