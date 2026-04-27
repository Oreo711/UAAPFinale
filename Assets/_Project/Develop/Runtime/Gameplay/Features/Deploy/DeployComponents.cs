using _Project.Develop.Runtime.Gameplay.Features.Deploy;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Deploy
{
	public class MineDeployRequest : IEntityComponent
	{
		public ReactiveEvent<Vector3> Value;
	}

	public class SentryDeployRequest : IEntityComponent
	{
		public ReactiveEvent<Vector3> Value;
	}

	public class PuddleDeployRequest : IEntityComponent
	{
		public ReactiveEvent<Vector3> Value;
	}

	public class CurrentDeployable : IEntityComponent
	{
		public ReactiveVariable<Deployables> Value;
	}
}
