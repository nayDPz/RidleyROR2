using System;
using EntityStates.Merc;
using Ridley.Modules;
using UnityEngine;
using RoR2;
using System.Collections.Generic;
namespace Ridley.SkillStates
{
	public class DashAttack : BaseM1
	{
		private bool hasGrantedBuff = false;

		public override void OnEnter()
		{
			this.baseDuration = 0.875f;
			this.attackStartTime = 0.16f;
			this.attackEndTime = 0.6f;
			this.hitStopDuration = 0.025f;
			this.attackRecoil = 2f;
			this.hitHopVelocity = 2f;
			this.damageCoefficient = 3.75f;
			this.damageType = RoR2.DamageType.BonusToLowHealth;
			this.hitStopDuration = 0.25f;
			this.pushForce = 2300f;
			this.launchVectorOverride = true;
			this.earlyExitJump = true;
			this.stackGainAmount = 12;
			this.swingSoundString = "DashAttack";
			this.hitSoundString = "Jab3Hit";
			this.critHitSoundString = "SwordHit3";
			this.muzzleString = "Mouth";
			this.swingEffectPrefab = Assets.biteEffect;
			this.hitEffectPrefab = Assets.biteEffect;
			this.impactSound = Assets.jab3HitSoundEvent.index;
			this.dashSpeedCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 10f),
				new Keyframe(0.5f, 0f),
				new Keyframe(1f, 0f)
			});
			this.isDash = true;
			this.isFlinch = true;
			this.animString = "DashAttack";
			this.hitboxName = "Jab";
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
			
			Vector3 direction = base.characterDirection.forward * 15f + Vector3.up * 7.5f;
			Vector3 launchVector = (direction + base.transform.position) - body.transform.position;
			launchVector = launchVector.normalized;
			
			if (body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>())
			{
				body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().ForceUnground();
			}
			launchVector *= this.pushForce;
			CharacterMotor m = body.characterMotor;
			float force = 0.3f;
			if (m)
			{
				float f = Mathf.Max(150f, m.mass);
				force = f / 150f;
				launchVector *= force;
				m.ApplyForce(launchVector);
			}
			else if (body.rigidbody)
			{
				float f = Mathf.Max(50f, body.rigidbody.mass);
				force = f / 200f;
				launchVector *= force;
				body.rigidbody.AddForce(launchVector, ForceMode.Impulse);
			}
		}

		public override void OnHitEnemyAuthority(List<HurtBox> list)
		{
			base.OnHitEnemyAuthority(list);

			if (!hasGrantedBuff)
			{
				hasGrantedBuff = true;
				characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CrocoRegen.buffIndex, 0.5f);
			}
		}
	}
}
