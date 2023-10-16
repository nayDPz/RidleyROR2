﻿using System;
using R2API;
using Ridley.SkillStates;

namespace Ridley.Modules
{
	// Token: 0x02000026 RID: 38
	internal static class Tokens
	{
		// Token: 0x060000BE RID: 190 RVA: 0x0000908C File Offset: 0x0000728C
		internal static void AddTokens()
		{
			string str = "NDP_RIDLEY_BODY_";
			string value = "RIDLEY RIDLEY RIDLEY";
			string value2 = "..and so he left, searching for a new identity.";
			string value3 = "..and so he vanished, forever a blank slate.";
			LanguageAPI.Add(str + "NAME", "Ridley");
			LanguageAPI.Add(str + "DESCRIPTION", value);
			LanguageAPI.Add(str + "SUBTITLE", "The");
			LanguageAPI.Add(str + "LORE", "?");
			LanguageAPI.Add(str + "OUTRO_FLAVOR", value2);
			LanguageAPI.Add(str + "OUTRO_FAILURE", value3);
			LanguageAPI.Add(str + "PASSIVE_NAME", "Adaptive Exoskeleton");
			LanguageAPI.Add(str + "PASSIVE_DESCRIPTION", "Gain <style=cIsUtility>flat damage reduction</style> equal to your <style=cIsUtility>armor</style>. When you are below half health, damage can be reduced <style=cIsHealing>below zero.</style> ");
			LanguageAPI.Add(str + "PRIMARY_SLASH_NAME", "Wicked Claws");
			LanguageAPI.Add(str + "PRIMARY_SLASH_DESCRIPTION", $"<style=cIsDamage>Melee attack</style> with effects based on your aim input.");
			LanguageAPI.Add(str + "SECONDARY_GUN_NAME", "Plasma Breath");
			LanguageAPI.Add(str + "SECONDARY_GUN_DESCRIPTION", string.Format("Charge <style=cIsDamage>3</style> fireballs that deal <style=cIsDamage>{0}% damage</style>. <style=cIsUtility>Can be used during other skills.</style>", 100f * FireFireballs.damageCoefficient));
			LanguageAPI.Add(str + "UTILITY_ROLL_NAME", "Space Pirate Rush");
			LanguageAPI.Add(str + "UTILITY_ROLL_DESCRIPTION", "<style=cIsUtility>Grab</style> an enemy and drag them along the ground, dealing <style=cIsDamage>500% damage</style> per second. Deal <style=cIsDamage>250% damage</style> in an area on impact, <style=cIsDamage>scaling massively with airtime</style>.");
			LanguageAPI.Add(str + "SPECIAL_BOMB_NAME", "Skewer");
			LanguageAPI.Add(str + "SPECIAL_BOMB_DESCRIPTION", string.Format("Stab in a line, dealing <style=cIsDamage>{0}% damage</style> to all enemies and pulling them to you.", 100f * Skewer2.damageCoefficient));
			LanguageAPI.Add(str + "DEFAULT_SKIN_NAME", "Default");
			LanguageAPI.Add(str + "META_SKIN_NAME", "Meta");
			LanguageAPI.Add(str + "MECHA_SKIN_NAME", "Mecha");
			LanguageAPI.Add(str + "PURPLE_SKIN_NAME", "Purple");
			LanguageAPI.Add(str + "YELLOW_SKIN_NAME", "Yellow");
			LanguageAPI.Add(str + "RED_SKIN_NAME", "Red");
			LanguageAPI.Add(str + "GREEN_SKIN_NAME", "Green");
			LanguageAPI.Add(str + "BLUE_SKIN_NAME", "Blue");

		}
	}
}
