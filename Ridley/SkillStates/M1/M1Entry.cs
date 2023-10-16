using System;
using EntityStates;
using UnityEngine;

namespace Ridley.SkillStates
{
	public class M1Entry : BaseSkillState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			float y = base.inputBank.aimDirection.y;

			if (base.characterBody.isSprinting && base.isGrounded)
			{
				this.outer.SetNextState(new DashAttack());
				return;
			}
			else if (y > 0.5f)
			{
				this.outer.SetNextState(new UpAir());
				return;
			}
			else
			{
				if (!base.characterMotor.isGrounded)
				{
					this.outer.SetNextState(new FAir());
					return;
				}
				else
				{					
					this.outer.SetNextState(new Jab1());
					return;
				}				
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}
	}
}
