using System;
using EntityStates.Merc;
using Ridley.Modules;
using UnityEngine;
using RoR2;
using System.Collections.Generic;

namespace Ridley.SkillStates
{
	// Token: 0x02000012 RID: 18
	public class UpAir : BaseM1
	{
		// Token: 0x0600003A RID: 58 RVA: 0x000040D0 File Offset: 0x000022D0
		public override void OnEnter()
		{
			this.baseDuration = 0.7f;
			this.attackStartTime = 0f;
			this.attackEndTime = 0.4f;
			this.hitStopDuration = 0.025f;
			this.attackRecoil = 2f;
			this.hitHopVelocity = 2f;
			this.damageCoefficient = 3f;
			this.stackGainAmount = 7;
			this.hitStopDuration = .15f;
			this.damageType = RoR2.DamageType.BonusToLowHealth;
			//this.bonusForce = Vector3.up * 2000f;
			this.pushForce = 2000f;
			this.launchVectorOverride = true;
			this.isAerial = true;
			this.swingSoundString = "UpAir";
			this.hitSoundString = "SwordHit2";
			this.critHitSoundString = "SwordHit3";
			this.swingEffectPrefab = Assets.ridleySwingEffect;
			this.hitEffectPrefab = GroundLight.finisherHitEffectPrefab;
			this.impactSound = Assets.sword2HitSoundEvent.index;
			this.animString = "UpAir";
			this.hitboxName = "UpTilt";
			base.OnEnter();
		}

        public override void OnHitEnemyAuthority(List<HurtBox> list)
        {
			foreach (HurtBox hurtBox in list)
			{
				HealthComponent h = hurtBox.healthComponent;
				if (h && h.combinedHealthFraction < 0.45f)
				{
					this.hitSoundString = "SwordHit3";
					break;
				}

			}
			base.OnHitEnemyAuthority(list);
        }
        public override void LaunchEnemy(CharacterBody body)
		{
			
			//Vector3 launchVector = (Vector3.up * 15f + base.transform.position) - hurtBox.healthComponent.body.footPosition;
			//launchVector = launchVector.normalized;

			Vector3 direction = base.characterDirection.forward * 4f + Vector3.up * 15f;
			Vector3 launchVector = (direction + base.transform.position) - body.transform.position;
			launchVector = launchVector.normalized;
			launchVector *= this.pushForce;

			if (body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>())
			{
				body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().ForceUnground();
			}

			CharacterMotor m = body.characterMotor;

			float force = 0.25f;
			if (m)
			{
				m.velocity = Vector3.zero;
				float f = Mathf.Max(100f, m.mass);
				force = f / 100f;
				launchVector *= force;
				m.ApplyForce(launchVector);
			}
			else if (body.rigidbody)
			{
				body.rigidbody.velocity = Vector3.zero;
				float f = Mathf.Max(50f, body.rigidbody.mass);
				force = f / 140f;
				launchVector *= force;
				body.rigidbody.AddForce(launchVector, ForceMode.Impulse);
			}

			DamageInfo info = new DamageInfo
			{
				attacker = base.gameObject,
				inflictor = base.gameObject,
				damage = 0,
				damageColorIndex = DamageColorIndex.Default,
				damageType = DamageType.Generic,
				crit = false,
				dotIndex = DotController.DotIndex.None,
				force = launchVector,
				position = base.transform.position,
				procChainMask = default(ProcChainMask),
				procCoefficient = 0
			};
			//body.healthComponent.TakeDamageForce(info, true, true);
		}
	}
}
