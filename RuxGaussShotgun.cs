﻿using RoR2;
using UnityEngine;
using EntityStates.Commando.CommandoWeapon;
using EntityStates.EngiTurret.EngiTurretWeapon;

namespace EntityStates.Engi.EngiWeapon.Rux
{
	// Token: 0x02000AB9 RID: 2745
	public class RuxGaussShotgun : BaseState
	{
		// Token: 0x06003E81 RID: 16001 RVA: 0x001053D8 File Offset: 0x001035D8
		public override void OnEnter()
		{
			base.OnEnter();
			this.maxDuration = 0.9f / this.attackSpeedStat;
			this.minDuration = 0.7f / this.attackSpeedStat;
			Ray aimRay = base.GetAimRay();
			base.StartAimMode(aimRay, 2f, false);
			Util.PlaySound(FireGauss.attackSoundString, base.gameObject);
			base.PlayCrossfade("Gesture, Right Cannon", "FireGrenadeRight", 0.1f);
			string muzzleName = "MuzzleRight";
			if (RuxGaussShotgun.effectPrefab)
			{
				EffectManager.SimpleMuzzleFlash(FireShotgun.effectPrefab, base.gameObject, muzzleName, false);
			}
			if (base.isAuthority)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 1f,
					maxSpread = 10f,
					bulletCount = 7,
					procCoefficient = 0.7f,
					damage = 0.85f * this.damageStat,
					damageType = DamageType.Generic,
					force = 30f,
					falloffModel = BulletAttack.FalloffModel.DefaultBullet,
					tracerEffectPrefab = FireGauss.tracerEffectPrefab,
					muzzleName = muzzleName,
					hitEffectPrefab = FireGauss.hitEffectPrefab,
					isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
					HitEffectNormal = true,
					radius = 0.8f
				}.Fire();
			}
			base.characterBody.AddSpreadBloom(FireShotgun.spreadBloomValue);
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x00032FA7 File Offset: 0x000311A7
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x001055E0 File Offset: 0x001037E0
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.buttonReleased |= !base.inputBank.skill1.down;
			if (base.fixedAge >= this.maxDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00105635 File Offset: 0x00103835
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			if (this.buttonReleased && base.fixedAge >= this.minDuration)
			{
				return InterruptPriority.Any;
			}
			return InterruptPriority.Skill;
		}

		// Token: 0x040039DC RID: 14812
		public static GameObject effectPrefab = FireShotgun.effectPrefab;

		// Token: 0x040039DD RID: 14813
		public static GameObject hitEffectPrefab = FireShotgun.hitEffectPrefab;

		// Token: 0x040039DE RID: 14814
		public static GameObject tracerEffectPrefab = FirePistol2.tracerEffectPrefab;

		// Token: 0x040039DF RID: 14815
		public static float damageCoefficient = 0.95f;

		// Token: 0x040039E0 RID: 14816
		public static float force = 5f;

		// Token: 0x040039E1 RID: 14817
		public static int bulletCount = 7;

		// Token: 0x040039E2 RID: 14818
		public static float baseMaxDuration = 3f;

		// Token: 0x040039E3 RID: 14819
		public static float baseMinDuration = 3f;

		// Token: 0x040039E5 RID: 14821
		public static float recoilAmplitude = 0.2f;

		// Token: 0x040039E6 RID: 14822
		public static float spreadBloomValue = 0.3f;

		// Token: 0x040039E7 RID: 14823
		private float maxDuration;

		// Token: 0x040039E8 RID: 14824
		private float minDuration;

		// Token: 0x040039E9 RID: 14825
		private bool buttonReleased;
	}
}