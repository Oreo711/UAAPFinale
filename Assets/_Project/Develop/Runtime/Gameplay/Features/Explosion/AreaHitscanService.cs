using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class AreaHitscanService
	{
		private readonly CollidersRegistryService _collidersRegistryService;

		public AreaHitscanService (CollidersRegistryService collidersRegistryService)
		{
			_collidersRegistryService = collidersRegistryService;
		}

		public bool Scan (Vector3 origin, float radius, LayerMask layerMask, Entity source, float damage)
		{
			Collider[] collidersWithinRadius = Physics.OverlapSphere(
      				origin,
				      radius,
				      layerMask);

			if (collidersWithinRadius.Length > 0)
			{
				foreach (Collider hitCollider in collidersWithinRadius)
				{
					Entity entity = _collidersRegistryService.GetBy(hitCollider);

					if (EntitiesHelper.TryTakeDamageFrom(source, entity, damage))
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}
