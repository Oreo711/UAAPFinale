using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.MineDeploy
{
	public class MineDeployRequest : IEntityComponent
	{
		public ReactiveEvent<Vector3> Value;
	}
}
