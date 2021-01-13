using System;
using RoR2;
using UnityEngine;
using EntityStates.Croco;

namespace EntityStates.Croco.Rux
{
	// Token: 0x02000A9E RID: 2718
	public class RuxCrunch : BasicMeleeAttack
	{
		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06003DE0 RID: 15840 RVA: 0x000E8CAC File Offset: 0x000E6EAC
		protected override bool allowExitFire
		{
			get
			{
				return base.characterBody && !base.characterBody.isSprinting;
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00101CA4 File Offset: 0x000FFEA4
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterDirection.forward = base.GetAimRay().direction;
			this.durationBeforeInterruptable = Bite.baseDurationBeforeInterruptable / this.attackSpeedStat;
			this.crocoDamageTypeController = base.GetComponent<CrocoDamageTypeController>();
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x000E8D72 File Offset: 0x000E6F72
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x00101CF0 File Offset: 0x000FFEF0
		protected override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
		{
			base.AuthorityModifyOverlapAttack(overlapAttack);
			DamageType damageType = this.crocoDamageTypeController ? this.crocoDamageTypeController.GetDamageType() : DamageType.Generic;
			overlapAttack.damageType = (damageType | DamageType.BonusToLowHealth);
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x00101D30 File Offset: 0x000FFF30
		protected override void PlayAnimation()
		{
			float duration = Mathf.Max(this.duration, 0.2f);
			base.PlayCrossfade("Gesture, Additive", "Bite", "Bite.playbackRate", duration, 0.1f);
			base.PlayCrossfade("Gesture, Override", "Bite", "Bite.playbackRate", duration, 0.1f);
			Util.PlaySound(Bite.biteSound, base.gameObject);
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00101D95 File Offset: 0x000FFF95
		protected override void OnMeleeHitAuthority()
		{
			base.OnMeleeHitAuthority();
			base.characterBody.AddSpreadBloom(this.bloom);
			if (!this.hasGrantedBuff)
			{
				this.hasGrantedBuff = true;
				base.characterBody.AddTimedBuffAuthority(BuffIndex.CrocoRegen, 0f);
			}
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x00101DCF File Offset: 0x000FFFCF
		protected override void BeginMeleeAttackEffect()
		{
			base.AddRecoil(0.9f * Bite.recoilAmplitude, 1.1f * Bite.recoilAmplitude, -0.1f * Bite.recoilAmplitude, 0.1f * Bite.recoilAmplitude);
			base.BeginMeleeAttackEffect();
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00101E09 File Offset: 0x00100009
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			if (base.fixedAge >= this.durationBeforeInterruptable)
			{
				return InterruptPriority.Skill;
			}
			return InterruptPriority.Pain;
		}

		// Token: 0x040038DD RID: 14557
		public static float recoilAmplitude;

		// Token: 0x040038DE RID: 14558
		public static float baseDurationBeforeInterruptable;

		// Token: 0x040038DF RID: 14559
		[SerializeField]
		public float bloom;

		// Token: 0x040038E0 RID: 14560
		public static string biteSound;

		// Token: 0x040038E1 RID: 14561
		private string animationStateName;

		// Token: 0x040038E2 RID: 14562
		private float durationBeforeInterruptable;

		// Token: 0x040038E3 RID: 14563
		private CrocoDamageTypeController crocoDamageTypeController;

		// Token: 0x040038E4 RID: 14564
		private bool hasGrantedBuff;
	}
}