using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using UnityEngine;


namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
	[CreateAssetMenu(fileName = "PuddleConfig")]
	public class PuddleConfig : EntityConfig
	{
		[SerializeField] private float _damagePerTick;
		[SerializeField] private int _ticksPerSecond;

		public float DamagePerTick  => _damagePerTick;
		public int TicksPerSecond => _ticksPerSecond;
	}
}
