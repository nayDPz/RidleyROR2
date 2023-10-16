using EntityStates;
using RoR2;
using UnityEngine;
using Ridley.Modules.Components;
using UnityEngine.Networking;
using System.Collections.Generic;
namespace Ridley.SkillStates
{
    public class PullEnemies : RidleyBaseState
    {
        public List<GameObject> hitBodies;
        public float waitTime;
        public Vector3 point;
        private Vector3 direction;
        private bool pullStarted;
        private float duration;

        public Vector3 endPoint;

        public float skewerDuration;
        public float pullDuration;
        public Vector3 destination;

        private Animator animator;
        private float stopwatch;

        private RidleyComponent weaponAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.weaponAnimator = base.GetComponent<RidleyComponent>();

            foreach (GameObject body in this.hitBodies)
            {
                if (body && body.GetComponent<NetworkIdentity>())
                {
                    EntityStateMachine component = body.GetComponent<EntityStateMachine>();
                    if (component && body.GetComponent<SetStateOnHurt>() && body.GetComponent<SetStateOnHurt>().canBeFrozen)
                    {
                        SkeweredState newNextState = new SkeweredState
                        {
                            skewerDuration = (this.waitTime),
                            pullDuration = this.pullDuration,
                            destination = this.point,
                        };
                        component.SetInterruptState(newNextState, InterruptPriority.Vehicle);                      
                    }
                }

            }

            if (base.GetComponent<KinematicCharacterController.KinematicCharacterMotor>())
            {
                base.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().ForceUnground();
            }
            Util.PlaySound("DSpecialHit", base.gameObject);
            base.GetModelAnimator().SetFloat("Slash.playbackRate", 0f);
            //this.weaponAnimator.RotationOverride(base.GetAimRay().GetPoint(Skewer2.range));


        }

        public override void OnExit()
        {
            this.animator.SetFloat("Slash.playbackRate", 1f);
            //this.weaponAnimator.StopRotationOverride();
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterMotor.velocity = Vector3.zero;

            if (base.fixedAge >= this.waitTime)
            {
                if (!this.pullStarted)
                {
                    this.pullStarted = true;
                    base.GetModelAnimator().SetFloat("Slash.playbackRate", 1f);
                    Util.PlaySound("DSpecialPull", base.gameObject);
                    base.PlayAnimation("FullBody, Override", "DownSpecialHit", "Slash.playbackrate", this.pullDuration);
                }
                this.stopwatch += Time.fixedDeltaTime;
                if (this.stopwatch >= this.pullDuration)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
            else
            {
                
                this.animator.SetFloat("Slash.playbackRate", 0f);
            }


        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            int count = 0;
            foreach (GameObject body in this.hitBodies)
            {
                if (body && body.GetComponent<NetworkIdentity>())
                    count++;
            }
            writer.Write(count);
            writer.Write((double)this.waitTime);
            writer.Write((double)this.pullDuration);
            writer.Write(this.point);
            foreach (GameObject body in this.hitBodies)
            {
                if (body && body.GetComponent<NetworkIdentity>())
                    writer.Write(body.GetComponent<NetworkIdentity>().netId);
            }

        }

        public override void OnDeserialize(NetworkReader reader)
        {
            this.hitBodies = new List<GameObject>();
            base.OnDeserialize(reader);
            int count = reader.ReadInt32();
            this.waitTime = (float)reader.ReadDouble();
            this.pullDuration = (float)reader.ReadDouble();
            this.point = reader.ReadVector3();
            
            for (int i = 0; i < count; i++)
            {
                this.hitBodies.Add(NetworkServer.FindLocalObject(reader.ReadNetworkId()));
            }
        }
    }
}