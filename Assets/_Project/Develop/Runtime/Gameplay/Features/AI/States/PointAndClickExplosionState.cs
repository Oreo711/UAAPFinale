using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
	public class PointAndClickExplosionState : State, IUpdatableState
	{
		private IInputService          _inputService;
		private ReactiveEvent<Vector3> _explosionRequest;

		public PointAndClickExplosionState (Entity entity, IInputService inputService)
		{
			_inputService     = inputService;
			_explosionRequest = entity.WorldPointExplosionRequest;
		}

		public void Update (float deltaTime)
		{
			if (_inputService.Clicked)
			{
				Vector3 mousePosition         = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 groundedMousePosition = new Vector3(mousePosition.x, 0, mousePosition.z);
				Vector3 mouseDirection = (groundedMousePosition - Camera.main.transform.position).normalized;
				Ray ray = new Ray(Camera.main.transform.position, mouseDirection);

				if (Physics.Raycast(ray, out RaycastHit hitInfo))
				{
						Vector3 point = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
						_explosionRequest.Invoke(point);
				}
			}
		}
	}
}
