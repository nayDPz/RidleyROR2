using EntityStates;
using RoR2;
using UnityEngine;
using Ridley.Modules.Components;
using Ridley.SkillStates;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Ridley.SkillStates
{
    public class Skewer2 : RidleyBaseState
    {
        public static float cooldownOnMiss = 0.5f;
        public static float pullForce = 3f;
        public static float damageCoefficient = 7.5f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.25f;
        public static float recoil = 1f;
        public static float range = 60f;

        private float pullTime = 0.25f;
        private float skewerTime = 0.375f;

        private Ray aimRay;
        private Vector3 pullPoint;
        private RidleyComponent weaponAnimator;

        private bool hitWorld;
        private float stopwatch;
        private Vector3 hitPoint;
        private float duration;
        private float fireTime;
        private bool hasFired;
        private bool hasHit;
        private float hitTime;
        private Animator animator;
        private string muzzleString;
        private static float antigravityStrength;

        public override void OnEnter()
        {
            base.OnEnter();

            this.weaponAnimator = base.GetComponent<RidleyComponent>();

            base.StartAimMode(2f);
            this.skewerTime /= this.attackSpeedStat;
            this.pullTime /= this.attackSpeedStat;
            this.duration = Skewer2.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.4f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("FullBody, Override", "DownSpecial", "Slash.playbackrate", this.duration);
            Util.PlaySound("DSpecialStart", base.gameObject);

            base.characterDirection.forward = base.inputBank.aimDirection;

        }

        public override void OnExit()
        {
            //this.weaponAnimator.StopRotationOverride();
            this.animator.SetFloat("Slash.playbackRate", 1f);
            base.OnExit();
        }



        private void Fire()
        {
            if (!this.hasFired)
            {

                this.hasFired = true;

                this.aimRay = base.GetAimRay();
                this.pullPoint = aimRay.GetPoint(3f);
                this.pullPoint.y = base.transform.position.y + 1f;
                this.hasFired = true;
                base.characterBody.AddSpreadBloom(1.5f);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * Skewer2.recoil, -2f * Skewer2.recoil, -0.5f * Skewer2.recoil, 0.5f * Skewer2.recoil);
                    bool hitEnemy = false;
                    List<GameObject> hitBodies = new List<GameObject>();
                    BulletAttack bulletAttack = new BulletAttack
                    {
                        bulletCount = 1U,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = this.damageStat * Skewer2.damageCoefficient,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Stun1s,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = Skewer2.range,
                        force = 0f,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = this.muzzleString,
                        smartCollision = true,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = Skewer2.procCoefficient,
                        radius = 3.25f,
                        sniper = false,
                        stopperMask = LayerIndex.world.collisionMask,
                        weapon = null,
                        tracerEffectPrefab = Skewer.tracerEffectPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = Skewer.muzzleEffectPrefab
                    };
                    bulletAttack.hitCallback = (BulletAttack bullet, ref BulletAttack.BulletHit hitInfo) =>
                    {
                        bool result = BulletAttack.defaultHitCallback(bullet, ref hitInfo);


                        this.hitPoint = hitInfo.point;
                        if (hitInfo.hitHurtBox)
                        {
                            HurtBox hurtBox = hitInfo.hitHurtBox;
                            if (hurtBox)
                            {
                                HealthComponent h = hurtBox.healthComponent;
                                if (h && h.body)
                                {
                                    hitEnemy = true;
                                    hitBodies.Add(h.body.gameObject);

                                }
                            }
                        }

                        return result;
                    };
                    bulletAttack.Fire();

                    if (hitEnemy)
                    {
                        this.outer.SetNextState(new PullEnemies
                        {
                            waitTime = this.skewerTime,
                            pullDuration = this.pullTime,
                            point = this.pullPoint,
                            hitBodies = hitBodies,
                        });
                        this.OnHitAnyAuthority();
                        //this.weaponAnimator.RotationOverride(base.GetAimRay().GetPoint(range));
                    }
                    else
                    {
                        Util.PlaySound("DSpecialSwing", base.gameObject);
                        base.activatorSkillSlot.rechargeStopwatch += base.activatorSkillSlot.CalculateFinalRechargeInterval() - Skewer2.cooldownOnMiss;

                    }

                    
                }
            }
        }
      
        private void OnHitAnyAuthority()
        {

            this.animator.SetFloat("Slash.playbackRate", 0f);
            
            base.characterMotor.velocity = Vector3.zero;

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.stopwatch += Time.fixedDeltaTime;
            if (base.fixedAge < this.fireTime)
            {
                base.characterDirection.forward = base.inputBank.aimDirection;
            }
            if (base.fixedAge >= this.fireTime * 0.85f && !this.hasFired)
            {
                float f = (base.fixedAge - (this.fireTime * .85f) / this.fireTime);
                //this.weaponAnimator.RotationOverride(base.GetAimRay().GetPoint(f * range));
            }
            
            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
                base.characterDirection.forward = this.hitPoint != Vector3.zero ? this.hitPoint - base.transform.position : base.characterDirection.forward;
            }

            if (this.hasHit)
            {
                base.characterMotor.velocity = Vector3.zero;
                this.animator.SetFloat("Slash.playbackRate", 0f);
            }


            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.weaponAnimator.StopRotationOverride();
                this.outer.SetNextStateToMain();


                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}