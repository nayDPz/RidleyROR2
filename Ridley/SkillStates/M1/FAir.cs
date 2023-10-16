using System;
using EntityStates.Merc;
using Ridley.Modules;
using UnityEngine;
using RoR2;
namespace Ridley.SkillStates
{
	public class FAir : BaseM1
	{
		public override void OnEnter()
		{
			this.anim = 1.45f;
			this.baseDuration = 0.7f;
			this.attackStartTime = 0.225f;
			this.attackEndTime = 0.65f;
			this.hitStopDuration = 0.03f;
			this.attackRecoil = 2f;
			this.hitHopVelocity = 5f;
			this.damageCoefficient = 2.4f;
			this.attackResetInterval = 0.11f;
			this.hitStopDuration = 0.08f;
			this.stackGainAmount = 3;
			this.pushForce = 1800f;
			this.isMultiHit = true;
			this.isAerial = true;
			this.isSus = true;
			this.launchVectorOverride = true;
			this.swingSoundString = "FAir";
			this.hitSoundString = "SwordHit";
			this.critHitSoundString = "SwordHit2";
			this.swingEffectPrefab = Assets.ridleySwingEffect;
			this.hitEffectPrefab = GroundLight.finisherHitEffectPrefab;
			this.impactSound = Assets.sword1HitSoundEvent.index;
			this.animString = "FAir";
			this.hitboxName = "Jab";
			base.OnEnter();
		}

		public override void LaunchEnemy(CharacterBody body)
		{
			Vector3 direction = base.characterDirection.forward * 15f + Vector3.up * 10f;
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
				float f = Mathf.Max(150f, m.mass);
				force = f / 150f;
				launchVector *= force;
				m.ApplyForce(launchVector);
			}
			else if (body.rigidbody)
			{
				body.rigidbody.velocity = Vector3.zero;
				float f = Mathf.Max(50f, body.rigidbody.mass);
				force = f / 200f;
				launchVector *= force;
				body.rigidbody.AddForce(launchVector, ForceMode.Impulse);
			}
		}
	}
}
