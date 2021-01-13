using System;
using RoR2;
using UnityEngine;
using EntityStates.Croco;
using EntityStates.Mage.Weapon;

namespace EntityStates.Croco.Rux2
{
	// Token: 0x020009AE RID: 2478
	public class RuxExtinctionBreath : BaseState
	{
		// Token: 0x06003956 RID: 14678 RVA: 0x000EA888 File Offset: 0x000E8A88
		public override void OnEnter()
		{
			base.OnEnter();
			this.stopwatch = 0f;
			this.entryDuration = Flamethrower.baseEntryDuration / this.attackSpeedStat;
			this.flamethrowerDuration = Flamethrower.baseFlamethrowerDuration;
			this.crocoDamageTypeController = base.GetComponent<CrocoDamageTypeController>();
			Transform modelTransform = base.GetModelTransform();
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(this.entryDuration + this.flamethrowerDuration + 1f);
			}
			if (modelTransform)
			{
				this.childLocator = modelTransform.GetComponent<ChildLocator>();
				this.leftMuzzleTransform = this.childLocator.FindChild("MouthMuzzle");
				this.rightMuzzleTransform = this.childLocator.FindChild("MouthMuzzle");
			}
			float num = this.flamethrowerDuration * Flamethrower.tickFrequency;
			this.tickDamageCoefficient = Flamethrower.totalDamageCoefficient / num;
			if (base.isAuthority && base.characterBody)
			{
				this.isCrit = Util.CheckRoll(this.critStat, base.characterBody.master);
			}
			base.PlayAnimation("Gesture, Mouth", "FireSpit", "FireSpit.playbackRate", this.entryDuration);
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x000EA99C File Offset: 0x000E8B9C
		public override void OnExit()
		{
			Util.PlaySound(Flamethrower.endAttackSoundString, base.gameObject);
			if (this.leftFlamethrowerTransform)
			{
				EntityState.Destroy(this.leftFlamethrowerTransform.gameObject);
			}
			if (this.rightFlamethrowerTransform)
			{
				EntityState.Destroy(this.rightFlamethrowerTransform.gameObject);
			}
			base.OnExit();
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x000EAA10 File Offset: 0x000E8C10
		private void FireGauntlet(string muzzleString)
		{
			Ray aimRay = base.GetAimRay();
			if (base.isAuthority)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					damage = this.tickDamageCoefficient * this.damageStat,
					force = Flamethrower.force,
					muzzleName = muzzleString,
					hitEffectPrefab = EntityStates.Croco.Slash.comboFinisherSwingEffectPrefab,
					isCrit = this.isCrit,
					radius = Flamethrower.radius,
					falloffModel = BulletAttack.FalloffModel.None,
					stopperMask = LayerIndex.world.mask,
					procCoefficient = Flamethrower.procCoefficientPerTick,
					maxDistance = 30f,
					smartCollision = true,
					damageType = this.crocoDamageTypeController ? this.crocoDamageTypeController.GetDamageType() : DamageType.Generic,
			}.Fire();
				if (base.characterMotor)
				{
					base.characterMotor.ApplyForce(aimRay.direction * -Flamethrower.recoilForce, false, false);
				}
			}
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x000EAB48 File Offset: 0x000E8D48
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			if (this.stopwatch >= this.entryDuration && !this.hasBegunFlamethrower)
			{
				this.hasBegunFlamethrower = true;
				Util.PlaySound(Flamethrower.startAttackSoundString, base.gameObject);
				if (this.childLocator)
				{
					Transform transform = this.childLocator.FindChild("MouthMuzzle");
					Transform transform2 = this.childLocator.FindChild("MouthMuzzle");
					if (transform)
					{
						this.leftFlamethrowerTransform = UnityEngine.Object.Instantiate<GameObject>(this.flamethrowerEffectPrefab, transform).transform;
					}
					if (transform2)
					{
						this.rightFlamethrowerTransform = UnityEngine.Object.Instantiate<GameObject>(this.flamethrowerEffectPrefab, transform2).transform;
					}
					if (this.leftFlamethrowerTransform)
					{
						this.leftFlamethrowerTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = this.flamethrowerDuration;
					}
					if (this.rightFlamethrowerTransform)
					{
						this.rightFlamethrowerTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = this.flamethrowerDuration;
					}
				}
				this.FireGauntlet("MouthMuzzle");
			}
			if (this.hasBegunFlamethrower)
			{
				this.flamethrowerStopwatch += Time.deltaTime;
				if (this.flamethrowerStopwatch > 1f / Flamethrower.tickFrequency)
				{
					this.flamethrowerStopwatch -= 1f / Flamethrower.tickFrequency;
					this.FireGauntlet("MouthMuzzle");
				}
				this.UpdateFlamethrowerEffect();
			}
			if (this.stopwatch >= this.flamethrowerDuration + this.entryDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x000EACFC File Offset: 0x000E8EFC
		private void UpdateFlamethrowerEffect()
		{
			Ray aimRay = base.GetAimRay();
			Vector3 direction = aimRay.direction;
			Vector3 direction2 = aimRay.direction;
			if (this.leftFlamethrowerTransform)
			{
				this.leftFlamethrowerTransform.forward = direction;
			}
			if (this.rightFlamethrowerTransform)
			{
				this.rightFlamethrowerTransform.forward = direction2;
			}
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		// Token: 0x04003266 RID: 12902
		[SerializeField]
		public GameObject flamethrowerEffectPrefab;

		// Token: 0x04003267 RID: 12903
		public static GameObject impactEffectPrefab;

		// Token: 0x04003268 RID: 12904
		public static GameObject tracerEffectPrefab;

		// Token: 0x04003269 RID: 12905
		[SerializeField]
		public float maxDistance;

		// Token: 0x0400326A RID: 12906
		public static float radius;

		// Token: 0x0400326B RID: 12907
		public static float baseEntryDuration = 1f;

		// Token: 0x0400326C RID: 12908
		public static float baseFlamethrowerDuration = 2f;

		// Token: 0x0400326D RID: 12909
		public static float totalDamageCoefficient = 1.2f;

		// Token: 0x0400326E RID: 12910
		public static float procCoefficientPerTick;

		// Token: 0x0400326F RID: 12911
		public static float tickFrequency;

		// Token: 0x04003270 RID: 12912
		public static float force = 20f;

		// Token: 0x04003271 RID: 12913
		public static string startAttackSoundString;

		// Token: 0x04003272 RID: 12914
		public static string endAttackSoundString;

		// Token: 0x04003273 RID: 12915
		public static float ignitePercentChance;

		// Token: 0x04003274 RID: 12916
		public static float recoilForce;

		// Token: 0x04003275 RID: 12917
		private float tickDamageCoefficient;

		// Token: 0x04003276 RID: 12918
		private float flamethrowerStopwatch;

		private CrocoDamageTypeController crocoDamageTypeController;

		// Token: 0x04003277 RID: 12919
		private float stopwatch;

		// Token: 0x04003278 RID: 12920
		private float entryDuration;

		// Token: 0x04003279 RID: 12921
		private float flamethrowerDuration;

		// Token: 0x0400327A RID: 12922
		private bool hasBegunFlamethrower;

		// Token: 0x0400327B RID: 12923
		private ChildLocator childLocator;

		// Token: 0x0400327C RID: 12924
		private Transform leftFlamethrowerTransform;

		// Token: 0x0400327D RID: 12925
		private Transform rightFlamethrowerTransform;

		// Token: 0x0400327E RID: 12926
		private Transform leftMuzzleTransform;

		// Token: 0x0400327F RID: 12927
		private Transform rightMuzzleTransform;

		// Token: 0x04003280 RID: 12928
		private bool isCrit;

		// Token: 0x04003281 RID: 12929
		private const float flamethrowerEffectBaseDistance = 16f;
	}
}