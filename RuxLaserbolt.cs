using System;
using RoR2;
using UnityEngine;
using EntityStates.Mage.Weapon;

namespace EntityStates.Mage.Weapon.Rux
{
	// Token: 0x020009AB RID: 2475
	public class RuxLaserbolt : BaseState
	{
		// Token: 0x06003945 RID: 14661 RVA: 0x000EA080 File Offset: 0x000E8280
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = FireLaserbolt.baseDuration / this.attackSpeedStat;
			FireLaserbolt.Gauntlet gauntlet = this.gauntlet;
			if (gauntlet != FireLaserbolt.Gauntlet.Left)
			{
				if (gauntlet == FireLaserbolt.Gauntlet.Right)
				{
					this.muzzleString = "MuzzleRight";
					base.PlayAnimation("Gesture Right, Additive", "FireGauntletRight", "FireGauntlet.playbackRate", this.duration);
				}
			}
			else
			{
				this.muzzleString = "MuzzleLeft";
				base.PlayAnimation("Gesture Left, Additive", "FireGauntletLeft", "FireGauntlet.playbackRate", this.duration);
			}
			base.PlayAnimation("Gesture, Additive", "HoldGauntletsUp", "FireGauntlet.playbackRate", this.duration);
			Util.PlaySound(FireLaserbolt.attackString, base.gameObject);
			this.animator = base.GetModelAnimator();
			base.characterBody.SetAimTimer(2f);
			this.FireGauntlet();
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x00032FA7 File Offset: 0x000311A7
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x000EA154 File Offset: 0x000E8354
		private void FireGauntlet()
		{
			this.hasFiredGauntlet = true;
			Ray aimRay = base.GetAimRay();
			if (FireLaserbolt.muzzleEffectPrefab)
			{
				EffectManager.SimpleMuzzleFlash(FireLaserbolt.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
			}
			if (base.isAuthority)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					maxSpread = base.characterBody.spreadBloomAngle,
					damage = 2f * this.damageStat,
					force = FireLaserbolt.force,
					tracerEffectPrefab = FireLaserbolt.tracerEffectPrefab,
					muzzleName = this.muzzleString,
					hitEffectPrefab = FireLaserbolt.impactEffectPrefab,
					isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
					radius = 0.2f,
					smartCollision = false
				}.Fire();
			}
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x000EA264 File Offset: 0x000E8464
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.animator.GetFloat("FireGauntlet.fire") > 0f && !this.hasFiredGauntlet)
			{
				this.FireGauntlet();
			}
			if (base.fixedAge < this.duration || !base.isAuthority)
			{
				return;
			}
			if (base.inputBank.skill1.down)
			{
				FireLaserbolt fireLaserbolt = new FireLaserbolt();
				fireLaserbolt.gauntlet = ((this.gauntlet == FireLaserbolt.Gauntlet.Left) ? FireLaserbolt.Gauntlet.Right : FireLaserbolt.Gauntlet.Left);
				this.outer.SetNextState(fireLaserbolt);
				return;
			}
			this.outer.SetNextStateToMain();
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		// Token: 0x04003234 RID: 12852
		public static GameObject muzzleEffectPrefab;

		// Token: 0x04003235 RID: 12853
		public static GameObject tracerEffectPrefab;

		// Token: 0x04003236 RID: 12854
		public static GameObject impactEffectPrefab;

		// Token: 0x04003237 RID: 12855
		public static float baseDuration = 2f;

		// Token: 0x04003238 RID: 12856
		public static float damageCoefficient = 1.2f;

		// Token: 0x04003239 RID: 12857
		public static float force = 20f;

		// Token: 0x0400323A RID: 12858
		public static string attackString;

		// Token: 0x0400323B RID: 12859
		private float duration;

		// Token: 0x0400323C RID: 12860
		private bool hasFiredGauntlet;

		// Token: 0x0400323D RID: 12861
		private string muzzleString;

		// Token: 0x0400323E RID: 12862
		private Animator animator;

		// Token: 0x0400323F RID: 12863
		public FireLaserbolt.Gauntlet gauntlet;

		// Token: 0x020009AC RID: 2476
		public enum Gauntlet
		{
			// Token: 0x04003241 RID: 12865
			Left,
			// Token: 0x04003242 RID: 12866
			Right
		}
	}
}