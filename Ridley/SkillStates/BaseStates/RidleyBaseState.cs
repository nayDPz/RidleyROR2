using EntityStates;
using RoR2;
using UnityEngine;
using Ridley.Modules.Components;
using UnityEngine.Networking;
using System.Collections.Generic;
namespace Ridley.SkillStates
{
    public class RidleyBaseState : BaseSkillState
    {
        private bool skill2InputReceived;

        public override void OnEnter()
        {
            base.OnEnter();


        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.GatherInputs();
            this.PerformInputs();
        }

        private void GatherInputs()
        {
            this.skill2InputReceived = base.inputBank.skill2.down;
            
        }
        private void PerformInputs()
        {
            this.HandleSkill(base.skillLocator.secondary, ref this.skill2InputReceived, base.inputBank.skill2.justPressed);
        }
        private void HandleSkill(GenericSkill skillSlot, ref bool inputReceived, bool justPressed)
		{
			bool flag = inputReceived;
            inputReceived = false;
			if (!skillSlot)
			{
				return;
			}
			if ((justPressed || inputReceived))
			{
				skillSlot.ExecuteIfReady();
			}
        }
    }
}