using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace Ridley.Modules
{
    public static class EmoteAPI
    {
        private static bool? _enabled;

        public static bool enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI");
                }
                return (bool)_enabled;
            }
        }

        public static void SetupSkeleton()
        {
            On.RoR2.SurvivorCatalog.Init += (orig) =>
            {
                orig();
                foreach (var item in SurvivorCatalog.allSurvivorDefs)
                {
                    DebugClass.Log($"----------bodyprefab: [{item.bodyPrefab}]");
                    if (item.bodyPrefab.name == "RidleyBody")
                    {
                        var skele = Assets.mainAssetBundle.LoadAsset<GameObject>("RidleyHumanoid");
                        EmotesAPI.CustomEmotesAPI.ImportArmature(item.bodyPrefab, skele);
                    }
                }
            };
        }
    }
}
