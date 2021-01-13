using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Commando.CommandoWeapon.Rux3
{
	// Token: 0x02000AB0 RID: 2736
	public class RuxVolatileShell : BaseState
	{
		// Token: 0x06003E4C RID: 15948 RVA: 0x001043C4 File Offset: 0x001025C4
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.SetSpreadBloom(0f, false);
			this.stopwatch = 0f;
			this.duration = this.baseDuration / this.attackSpeedStat;
			this.delayBeforeFiringProjectile = this.baseDelayBeforeFiringProjectile / this.attackSpeedStat;
			this.PlayAnimation(this.duration);
			Util.PlaySound(this.attackSoundString, base.gameObject);
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(2f);
			}
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x00032FA7 File Offset: 0x000311A7
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x00104454 File Offset: 0x00102654
		protected virtual void PlayAnimation(float duration)
		{
			if (base.GetModelAnimator())
			{
				base.PlayAnimation("Gesture, Additive", "FireSweepBarrage", "FireSweepBarrage.playbackRate", this.duration);
				base.PlayAnimation("Gesture, Override", "FireSweepBarrage", "FireSweepBarrage.playbackRate", this.duration);
			}
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x00104490 File Offset: 0x00102690
		protected virtual void Fire()
		{
			string muzzleName = "MuzzleCenter";
			base.AddRecoil(-2f * this.recoilAmplitude, -3f * this.recoilAmplitude, -1f * this.recoilAmplitude, 1f * this.recoilAmplitude);
			if (this.effectPrefab)
			{
				EffectManager.SimpleMuzzleFlash(this.effectPrefab, base.gameObject, muzzleName, false);
			}
			this.firedProjectile = true;
			if (base.isAuthority)
			{
				Ray aimRay = base.GetAimRay();
				aimRay.direction = Util.ApplySpread(aimRay.direction, this.minSpread, this.maxSpread, 1f, 1f, 0f, this.projectilePitchBonus);
				ProjectileManager.instance.FireProjectile(FireRocket.projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageStat * 8f, this.force, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
			}
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x001045A0 File Offset: 0x001027A0
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			if (this.stopwatch >= this.delayBeforeFiringProjectile && !this.firedProjectile)
			{
				this.Fire();
			}
			if (this.stopwatch >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x0000D472 File Offset: 0x0000B672
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x04003987 RID: 14727
		[SerializeField]
		public GameObject effectPrefab;

		// Token: 0x04003988 RID: 14728
		[SerializeField]
		public GameObject projectilePrefab;

		// Token: 0x04003989 RID: 14729
		[SerializeField]
		public float damageCoefficient;

		// Token: 0x0400398A RID: 14730
		[SerializeField]
		public float force;

		// Token: 0x0400398B RID: 14731
		[SerializeField]
		public float minSpread;

		// Token: 0x0400398C RID: 14732
		[SerializeField]
		public float maxSpread;

		// Token: 0x0400398D RID: 14733
		[SerializeField]
		public float baseDuration = 2f;

		// Token: 0x0400398E RID: 14734
		[SerializeField]
		public float recoilAmplitude = 1f;

		// Token: 0x0400398F RID: 14735
		[SerializeField]
		public string attackSoundString;

		// Token: 0x04003990 RID: 14736
		[SerializeField]
		public float projectilePitchBonus;

		// Token: 0x04003991 RID: 14737
		[SerializeField]
		public float baseDelayBeforeFiringProjectile;

		// Token: 0x04003992 RID: 14738
		private float stopwatch;

		// Token: 0x04003993 RID: 14739
		private float duration;

		// Token: 0x04003994 RID: 14740
		private float delayBeforeFiringProjectile;

		// Token: 0x04003995 RID: 14741
		private bool firedProjectile;
	}
}