using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using UnityEngine;
using UnityEngine.Serialization;


namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
	[CreateAssetMenu(fileName = "RangerConfig")]
	public class RangerConfig : EntityConfig
	{
		[SerializeField, Min(0)] private float _moveSpeed           = 9;
		[SerializeField, Min(0)] private float _rotationSpeed       = 900;
		[SerializeField, Min(0)] private float _attackProcessTime   = 1.5f;
		[SerializeField, Min(0)] private float _attackDelayTime     = 0.75f;
		[SerializeField, Min(0)] private float _attackCooldown      = 1f;
		[SerializeField, Min(0)] private float _instantAttackDamage = 50;
		[SerializeField, Min(0)] private float _maxHealth           = 100;
		[SerializeField, Min(0)] private float _attackRange         = 5;

		public float MoveSpeed           => _moveSpeed;
		public float RotationSpeed       => _rotationSpeed;
		public float AttackProcessTime   => _attackProcessTime;
		public float AttackDelayTime     => _attackDelayTime;
		public float AttackCooldown      => _attackCooldown;
		public float InstantAttackDamage => _instantAttackDamage;
		public float MaxHealth           => _maxHealth;
		public float AttackRange         => _attackRange;
	}
}
