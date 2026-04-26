using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class PreparationState : State, IUpdatableState
    {
        private readonly GameplayPopupService           _popupService;

        private          DeployableSelectPopupPresenter _deployableSelectPopup;

        public PreparationState (GameplayPopupService popupService)
        {
            _popupService = popupService;
        }

        public override void Enter ()
        {
            base.Enter();

            _popupService.OpenNextStagePopup();
            _deployableSelectPopup = _popupService.OpenDeployableSelectPopup();
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit ()
        {
            base.Exit();

            _deployableSelectPopup.Hide();
        }
    }
}
