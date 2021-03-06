using BepInEx;
using R2API.Utils;
using RoR2;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Projectile;
using Ridley.Modules.Components;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace Ridley
{
    [BepInDependency("com.TeamMoonstorm.Starstorm2", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KingEnderBrine.ScrollableLobbyUI", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]

    public class RidleyPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.ndp.RidleyBeta";
        public const string MODNAME = "RidleyBeta";
        public const string MODVERSION = "0.0.1";

        public const string developerPrefix = "NDP";

        public static bool scepterInstalled = false;


        public static RidleyPlugin instance;

        private void Awake()
        {
            instance = this;

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter")) scepterInstalled = true;

            // load assets and read config
            Modules.Assets.PopulateAssets();
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules
            Modules.CameraParams.InitializeParams();
            Modules.Survivors.Ridley.CreateCharacter();
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Start()
        {
            Modules.Survivors.Ridley.SetItemDisplays();
        }

        private void Hook()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            bool addInvuln = false;
            if (damageInfo != null && damageInfo.attacker && damageInfo.attacker.GetComponent<CharacterBody>())
            {
                if (self.GetComponent<CharacterBody>().baseNameToken == "NDP_RIDLEY_BODY_NAME")
                {
                    if(true)//(damageInfo.damageType & DamageType.DoT) != DamageType.DoT)
                    {
                        //float num = Mathf.Min(self.body.armor, (self.body.baseArmor + self.body.levelArmor * self.body.level) * 2f);
                        float num = self.body.armor;
                        if(self.combinedHealthFraction < 0.5f && (damageInfo.damageType & DamageType.DoT) != DamageType.DoT)
                        {
                            damageInfo.damage -= num;
                            if (damageInfo.damage < 0f)
                            {
                                self.Heal(Mathf.Abs(damageInfo.damage), default(RoR2.ProcChainMask), true);
                                damageInfo.damage = 0f;
                            }
                        }
                        else
                        {
                            damageInfo.damage = Mathf.Max(1f, damageInfo.damage - num);
                        }
                        
                    }
                    


                }
            }

            




            orig(self, damageInfo);

            if(addInvuln)
                self.body.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 3f);
        }
    }
}