using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.DamageOverTime
{
	public class DamagePerTick : IEntityComponent
	{
		public ReactiveVariable<float> Value;
	}

	public class TicksPerSecond : IEntityComponent
	{
		public ReactiveVariable<int> Value;
	}
}
