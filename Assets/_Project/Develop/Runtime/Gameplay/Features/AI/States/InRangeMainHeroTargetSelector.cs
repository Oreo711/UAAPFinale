using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
	public class InRangeMainHeroTargetSelector : ITargetSelector
	{
		private readonly MainHeroHolderService   _mainHeroHolderService;
		private          Transform               _sourceTransform;
		private          ReactiveVariable<float> _range;

		public InRangeMainHeroTargetSelector (MainHeroHolderService mainHeroHolderService, Entity entity)
		{
			_mainHeroHolderService = mainHeroHolderService;
			_sourceTransform       = entity.Transform;
			_range                 = entity.AttackRange;
		}

		public Entity SelectTargetFrom (IEnumerable<Entity> targets)
		{
			foreach (Entity target in targets)
			{
				if (target == _mainHeroHolderService.MainHero)
				{
					if ((_sourceTransform.position - _mainHeroHolderService.MainHero.Transform.position).magnitude <= _range.Value)
					{
						return target;
					}
				}
			}
				return null;
		}
	}
}
