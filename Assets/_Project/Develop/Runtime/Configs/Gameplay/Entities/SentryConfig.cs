using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using UnityEngine;


namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
	[CreateAssetMenu(fileName = "SentryConfig")]
	public class SentryConfig : EntityConfig
	{
		[SerializeField, Min(0)] private float _attackProcessTime   = 1.5f;
		[SerializeField, Min(0)] private float _attackDelayTime     = 0.75f;
		[SerializeField, Min(0)] private float _attackCooldown      = 1f;
		[SerializeField, Min(0)] private float _instantAttackDamage = 50;
		[SerializeField, Min(0)] private float _attackRange         = 5;
		[SerializeField, Min(0)] private float _rotationSpeed       = 900;

		public float AttackProcessTime   => _attackProcessTime;
		public float AttackDelayTime     => _attackDelayTime;
		public float AttackCooldown      => _attackCooldown;
		public float InstantAttackDamage => _instantAttackDamage;
		public float AttackRange         => _attackRange;
		public float RotationSpeed       => _rotationSpeed;
	}
}
