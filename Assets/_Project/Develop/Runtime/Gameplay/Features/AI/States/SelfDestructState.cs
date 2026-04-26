using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
	public class SelfDestructState : State, IUpdatableState
	{
		private ReactiveEvent _explosionRequest;

		public SelfDestructState (Entity entity)
		{
			_explosionRequest = entity.KamikazeExplosionRequest;
		}

		public override void Enter ()
		{
			base.Enter();

			_explosionRequest.Invoke();
		}

		public void Update (float deltaTime)
		{

		}
	}
}
