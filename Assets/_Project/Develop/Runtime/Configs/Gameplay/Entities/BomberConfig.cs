using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
	[CreateAssetMenu(fileName = ("BomberConfig"))]
	public class BomberConfig : EntityConfig
	{
		[SerializeField] private float _damage;
		[SerializeField] private float _range;
		[SerializeField] private float _health;
		[SerializeField] private float _speed;
		[SerializeField] private float _spawnProcessTime;


		public float Damage           => _damage;
		public float Range            => _range;
		public float Health           => _health;
		public float Speed            => _speed;
		public float SpawnProcessTime => _spawnProcessTime;
	}
}
