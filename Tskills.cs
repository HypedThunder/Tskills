using System;
using BepInEx;
using EntityStates;
using EntityStates.Commando.CommandoWeapon.Rux;
using EntityStates.Commando.CommandoWeapon.Rux3;
using EntityStates.Croco.Rux;
using EntityStates.Croco.Rux2;
using EntityStates.Engi.EngiWeapon.Rux;
using EntityStates.Rux;
using EntityStates.Mage.Weapon.Rux;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace TsunamiSkills
{
	// Token: 0x02000002 RID: 2
	[BepInDependency("com.bepis.r2api")]
	[BepInPlugin("com.Ruxbieno.TsunamiSkills", "Tsunami Skills", "0.1.0")]
	[R2APISubmoduleDependency(new string[]
	{
		"LoadoutAPI",
		"SurvivorAPI",
		"AssetPlus"
	})]
	public class TsunamiSkillsBase : BaseUnityPlugin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Awake()
		{
			Resources.Load<GameObject>("prefabs/projectiles/Rocket").AddComponent<CapsuleCollider>();
			GameObject gameObject = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/MageBody");
			GameObject gameObject3 = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");
			GameObject gameObject4 = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/EngiBody");
			GameObject gameObject5 = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CrocoBody");
			GameObject gameObject6 = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/LoaderBody");
			LanguageAPI.Add("MAGE_PRIMARY_LASER_NAME", "Laser Bolt");
			LanguageAPI.Add("MAGE_PRIMARY_LASER_DESCRIPTION", "Fire a laser bolt, dealing <style=cIsDamage>200% damage</style>.");
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(RuxLaserbolt));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = false;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.isBullets = false;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.noSprint = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.shootDelay = 0f;
			skillDef.stockToConsume = 1;
			skillDef.skillDescriptionToken = "MAGE_PRIMARY_LASER_DESCRIPTION";
			skillDef.skillName = "MAGE_PRIMARY_LASER_NAME";
			skillDef.skillNameToken = "MAGE_PRIMARY_LASER_NAME";

			LoadoutAPI.AddSkillDef(skillDef);
			SkillLocator component = gameObject.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.primary.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
			skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "1",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			LanguageAPI.Add("COMMANDO_SECONDARY_MICROMISSILES_NAME", "Micro Missiles");
			LanguageAPI.Add("COMMANDO_SECONDARY_MICROMISSILES_DESCRIPTION", "Charge up and fire a barrage of missiles, dealing <style=cIsDamage>200% damage</style> each with a maximum of 8.");
			SkillDef skillDef3 = ScriptableObject.CreateInstance<SkillDef>();
			skillDef3.activationState = new SerializableEntityStateType(typeof(RuxMissilePaint));
			skillDef3.activationStateMachineName = "Weapon";
			skillDef3.baseMaxStock = 1;
			skillDef3.baseRechargeInterval = 3f;
			skillDef3.beginSkillCooldownOnSkillEnd = true;
			skillDef3.canceledFromSprinting = false;
			skillDef3.fullRestockOnAssign = true;
			skillDef3.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef3.isBullets = true;
			skillDef3.isCombatSkill = true;
			skillDef3.mustKeyPress = false;
			skillDef3.noSprint = true;
			skillDef3.rechargeStock = 1;
			skillDef3.requiredStock = 1;
			skillDef3.shootDelay = 0f;
			skillDef3.stockToConsume = 1;
			skillDef3.skillDescriptionToken = "COMMANDO_SECONDARY_MICROMISSILES_DESCRIPTION";
			skillDef3.skillName = "COMMANDO_SECONDARY_MICROMISSILES_NAME";
			skillDef3.skillNameToken = "COMMANDO_SECONDARY_MICROMISSILES_NAME";
			LoadoutAPI.AddSkillDef(skillDef3);
			SkillLocator component3 = gameObject3.GetComponent<SkillLocator>();
			SkillFamily skillFamily3 = component3.secondary.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily3.variants, skillFamily3.variants.Length + 1);
			skillFamily3.variants[skillFamily3.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef3,
				unlockableName = "3",
				viewableNode = new ViewablesCatalog.Node(skillDef3.skillNameToken, false, null)
			};
			LanguageAPI.Add("ENGI_PRIMARY_SHOTGUN_NAME", "Gauss Shotgun");
			LanguageAPI.Add("ENGI_PRIMARY_SHOTGUN_DESCRIPTION", "Fire a blast of gauss bullets, dealing <style=cIsDamage>7x80% damage</style>.");
			SkillDef skillDef4 = ScriptableObject.CreateInstance<SkillDef>();
			skillDef4.activationState = new SerializableEntityStateType(typeof(RuxGaussShotgun));
			skillDef4.activationStateMachineName = "Weapon";
			skillDef4.baseMaxStock = 1;
			skillDef4.baseRechargeInterval = 0f;
			skillDef4.beginSkillCooldownOnSkillEnd = true;
			skillDef4.canceledFromSprinting = false;
			skillDef4.fullRestockOnAssign = false;
			skillDef4.interruptPriority = InterruptPriority.Any;
			skillDef4.isBullets = false;
			skillDef4.isCombatSkill = true;
			skillDef4.mustKeyPress = false;
			skillDef4.noSprint = true;
			skillDef4.rechargeStock = 1;
			skillDef4.requiredStock = 1;
			skillDef4.shootDelay = 0f;
			skillDef4.stockToConsume = 1;
			skillDef4.skillDescriptionToken = "ENGI_PRIMARY_SHOTGUN_DESCRIPTION";
			skillDef4.skillName = "ENGI_PRIMARY_SHOTGUN_NAME";
			skillDef4.skillNameToken = "ENGI_PRIMARY_SHOTGUN_NAME";

			LoadoutAPI.AddSkillDef(skillDef4);
			SkillLocator component4 = gameObject4.GetComponent<SkillLocator>();
			SkillFamily skillFamily4 = component4.primary.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily4.variants, skillFamily4.variants.Length + 1);
			skillFamily4.variants[skillFamily4.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef4,
				unlockableName = "4",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			LanguageAPI.Add("COMMANDO_ULTIMATE_PILL_NAME", "Sweep Barrage");
			LanguageAPI.Add("COMMANDO_ULTIMATE_PILL_DESCRIPTION", "Fire a heavy shot for each enemy in your vision, dealing <style=cIsDamage>400% damage</style>.");
			SkillDef skillDef5 = ScriptableObject.CreateInstance<SkillDef>();
			skillDef5.activationState = new SerializableEntityStateType(typeof(RuxSweepBarrage));
			skillDef5.activationStateMachineName = "Weapon";
			skillDef5.baseMaxStock = 1;
			skillDef5.baseRechargeInterval = 8f;
			skillDef5.beginSkillCooldownOnSkillEnd = true;
			skillDef5.canceledFromSprinting = false;
			skillDef5.fullRestockOnAssign = false;
			skillDef5.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef5.isBullets = false;
			skillDef5.isCombatSkill = true;
			skillDef5.mustKeyPress = false;
			skillDef5.noSprint = true;
			skillDef5.rechargeStock = 1;
			skillDef5.requiredStock = 1;
			skillDef5.shootDelay = 0f;
			skillDef5.stockToConsume = 1;
			skillDef5.skillDescriptionToken = "COMMANDO_ULTIMATE_PILL_DESCRIPTION";
			skillDef5.skillName = "COMMANDO_ULTIMATE_PILL_NAME";
			skillDef5.skillNameToken = "COMMANDO_ULTIMATE_PILL_NAME";

			LoadoutAPI.AddSkillDef(skillDef5);
			SkillLocator component5 = gameObject3.GetComponent<SkillLocator>();
			SkillFamily skillFamily5 = component5.special.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily5.variants, skillFamily5.variants.Length + 1);
			skillFamily5.variants[skillFamily5.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef5,
				unlockableName = "5",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			LanguageAPI.Add("CROCO_SPECIAL_BLIGHT_NAME", "Extinction Breath");
			LanguageAPI.Add("CROCO_SPECIAL_BLIGHT_DESCRIPTION", "Release a poisonous blast of envenoming spit from your mouth, dealing <style=cIsDamage>60% damage</style> per tick and poisoning enemies hit.");
			SkillDef skillDef7 = ScriptableObject.CreateInstance<SkillDef>();
			skillDef7.activationState = new SerializableEntityStateType(typeof(RuxExtinctionBreath));
			skillDef7.activationStateMachineName = "Weapon";
			skillDef7.baseMaxStock = 1;
			skillDef7.baseRechargeInterval = 8f;
			skillDef7.beginSkillCooldownOnSkillEnd = true;
			skillDef7.canceledFromSprinting = false;
			skillDef7.fullRestockOnAssign = false;
			skillDef7.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef7.isBullets = false;
			skillDef7.isCombatSkill = true;
			skillDef7.mustKeyPress = false;
			skillDef7.noSprint = true;
			skillDef7.rechargeStock = 1;
			skillDef7.requiredStock = 1;
			skillDef7.shootDelay = 0f;
			skillDef7.stockToConsume = 1;
			skillDef7.skillDescriptionToken = "CROCO_SPECIAL_BLIGHT_DESCRIPTION";
			skillDef7.skillName = "CROCO_SPECIAL_BLIGHT_NAME";
			skillDef7.skillNameToken = "CROCO_SPECIAL_BLIGHT_NAME";

			LoadoutAPI.AddSkillDef(skillDef7);
			SkillLocator component7 = gameObject5.GetComponent<SkillLocator>();
			SkillFamily skillFamily7 = component7.special.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily7.variants, skillFamily7.variants.Length + 1);
			skillFamily7.variants[skillFamily7.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef7,
				unlockableName = "7",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			LanguageAPI.Add("LOADER_ULTIMATE_PUNCH_NAME", "Electroboom");
			LanguageAPI.Add("LOADER_ULTIMATE_PUNCH_DESCRIPTION", "Swing your fist with extreme force, dealing <style=cIsDamage>1000% damage</style> in a large aoe while releasing chain lightning.");
			SkillDef skillDef8 = ScriptableObject.CreateInstance<SkillDef>();
			skillDef8.activationState = new SerializableEntityStateType(typeof(RuxElectroboom));
			skillDef8.activationStateMachineName = "Weapon";
			skillDef8.baseMaxStock = 1;
			skillDef8.baseRechargeInterval = 8f;
			skillDef8.beginSkillCooldownOnSkillEnd = true;
			skillDef8.canceledFromSprinting = false;
			skillDef8.fullRestockOnAssign = false;
			skillDef8.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef8.isBullets = false;
			skillDef8.isCombatSkill = true;
			skillDef8.mustKeyPress = false;
			skillDef8.noSprint = true;
			skillDef8.rechargeStock = 1;
			skillDef8.requiredStock = 1;
			skillDef8.shootDelay = 0f;
			skillDef8.stockToConsume = 1;
			skillDef8.skillDescriptionToken = "LOADER_ULTIMATE_PUNCH_DESCRIPTION";
			skillDef8.skillName = "LOADER_ULTIMATE_PUNCH_NAME";
			skillDef8.skillNameToken = "LOADER_ULTIMATE_PUNCH_NAME";

			LoadoutAPI.AddSkillDef(skillDef8);
			SkillLocator component8 = gameObject6.GetComponent<SkillLocator>();
			SkillFamily skillFamily8 = component8.special.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily8.variants, skillFamily8.variants.Length + 1);
			skillFamily8.variants[skillFamily8.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef8,
				unlockableName = "8",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}
	}
}
