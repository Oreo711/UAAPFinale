using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionDamage : IEntityComponent
  	{
		  public ReactiveVariable<float> Value;
	  }

	public class BlastRadius : IEntityComponent
	{
		public ReactiveVariable<float> Value;
	}

	public class WorldPointExplosionRequest : IEntityComponent
	{
		public ReactiveEvent<Vector3> Value;
	}

	public class WorldPointExplosionEvent : IEntityComponent
	{
		public ReactiveEvent<Vector3> Value;
	}

	public class KamikazeExplosionRequest : IEntityComponent
	{
		public ReactiveEvent Value;
	}
}
