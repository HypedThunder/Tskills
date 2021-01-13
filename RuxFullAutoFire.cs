using System;
using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Captain.Weapon.Rux2;
using EntityStates.Captain.Weapon.Rux3;
using EntityStates.Commando.CommandoWeapon;

namespace EntityStates.Captain.Weapon.Rux
{
	// Token: 0x02000AC5 RID: 2757
	public class RuxFullAutoFire : RuxFullAutoState
	{
		// Token: 0x06003EC0 RID: 16064 RVA: 0x00106750 File Offset: 0x00104950
		public override void OnEnter()
		{
			base.OnEnter();
			if (this.muzzleTransform && ChargeCaptainShotgun.holdChargeVfxPrefab)
			{
				this.muzzleVfxTransform = UnityEngine.Object.Instantiate<GameObject>(ChargeCaptainShotgun.holdChargeVfxPrefab, this.muzzleTransform).transform;
			}
			this.baseFireRate = 1f / RuxFullAutoFire.baseFireInterval;
			this.baseBulletsPerSecond = 1f * this.baseFireRate;
			this.critEndTime = Run.FixedTimeStamp.negativeInfinity;
			this.lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
			Util.PlaySound(MinigunFire.startSound, base.gameObject);
			base.PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", 0.4f, 0.1f);
			base.PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", 0.4f, 0.1f);
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x001067F7 File Offset: 0x001049F7
		private void UpdateCrits()
		{
			if (this.lastCritCheck.timeSince >= 1f)
			{
				this.lastCritCheck = Run.FixedTimeStamp.now;
				if (base.RollCrit())
				{
					this.critEndTime = Run.FixedTimeStamp.now + 2f;
				}
			}
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x00106834 File Offset: 0x00104A34
		public override void OnExit()
		{
			Util.PlaySound(MinigunFire.endSound, base.gameObject);
			if (this.muzzleVfxTransform)
			{
				EntityState.Destroy(this.muzzleVfxTransform.gameObject);
				this.muzzleVfxTransform = null;
			}
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.OnExit();
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x00106891 File Offset: 0x00104A91
		private void OnFireShared()
		{
			Util.PlaySound(MinigunFire.fireSound, base.gameObject);
			if (base.isAuthority)
			{
				this.OnFireAuthority();
			}
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x001068B4 File Offset: 0x00104AB4
		private void OnFireAuthority()
		{
			this.UpdateCrits();
			bool isCrit = !this.critEndTime.hasPassed;
			float damage = 0.5f * this.damageStat;
			float force = MinigunFire.baseForcePerSecond / this.baseBulletsPerSecond;
			float procCoefficient = MinigunFire.baseProcCoefficientPerSecond / this.baseBulletsPerSecond;
			Ray aimRay = base.GetAimRay();
			new BulletAttack
			{
				bulletCount = 1,
				aimVector = aimRay.direction,
				origin = aimRay.origin,
				damage = damage,
				damageColorIndex = DamageColorIndex.Default,
				damageType = DamageType.Generic,
				falloffModel = BulletAttack.FalloffModel.None,
				maxDistance = MinigunFire.bulletMaxDistance,
				force = force,
				hitMask = LayerIndex.CommonMasks.bullet,
				minSpread = 0f,
				maxSpread = 8f,
				isCrit = isCrit,
				owner = base.gameObject,
				muzzleName = MinigunState.muzzleName,
				smartCollision = false,
				procChainMask = default(ProcChainMask),
				procCoefficient = procCoefficient,
				radius = 0f,
				sniper = false,
				stopperMask = LayerIndex.CommonMasks.bullet,
				weapon = null,
				tracerEffectPrefab = FirePistol2.tracerEffectPrefab,
				spreadPitchScale = 1f,
				spreadYawScale = 1f,
				queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
				hitEffectPrefab = FirePistol2.hitEffectPrefab,
				HitEffectNormal = FirePistol2.hitEffectPrefab
			}.Fire();
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x00106A24 File Offset: 0x00104C24
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.fireTimer -= Time.fixedDeltaTime;
			if (this.fireTimer <= 0f)
			{
				float num = MinigunFire.baseFireInterval / this.attackSpeedStat;
				this.fireTimer += num;
				this.OnFireShared();
			}
			if (base.isAuthority && !base.skillButtonState.down)
			{
				this.outer.SetNextState(new RuxFullAutoSpinDown());
				return;
			}
		}

		// Token: 0x04003A3E RID: 14910
		public static GameObject muzzleVfxPrefab;

		// Token: 0x04003A3F RID: 14911
		public static float baseFireInterval;

		// Token: 0x04003A40 RID: 14912
		public static int baseBulletCount;

		// Token: 0x04003A41 RID: 14913
		public static float baseDamagePerSecondCoefficient;

		// Token: 0x04003A42 RID: 14914
		public static float baseForcePerSecond;

		// Token: 0x04003A43 RID: 14915
		public static float baseProcCoefficientPerSecond;

		// Token: 0x04003A44 RID: 14916
		public static float bulletMinSpread;

		// Token: 0x04003A45 RID: 14917
		public static float bulletMaxSpread;

		// Token: 0x04003A46 RID: 14918
		public static GameObject bulletTracerEffectPrefab;

		// Token: 0x04003A47 RID: 14919
		public static GameObject bulletHitEffectPrefab;

		// Token: 0x04003A48 RID: 14920
		public static bool bulletHitEffectNormal;

		// Token: 0x04003A49 RID: 14921
		public static float bulletMaxDistance;

		// Token: 0x04003A4A RID: 14922
		public static string fireSound;

		// Token: 0x04003A4B RID: 14923
		public static string startSound;

		// Token: 0x04003A4C RID: 14924
		public static string endSound;

		// Token: 0x04003A4D RID: 14925
		private float fireTimer;

		// Token: 0x04003A4E RID: 14926
		private Transform muzzleVfxTransform;

		// Token: 0x04003A4F RID: 14927
		private float baseFireRate;

		// Token: 0x04003A50 RID: 14928
		private float baseBulletsPerSecond;

		// Token: 0x04003A51 RID: 14929
		private Run.FixedTimeStamp critEndTime;

		// Token: 0x04003A52 RID: 14930
		private Run.FixedTimeStamp lastCritCheck;
	}
}