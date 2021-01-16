using System;
using System.Collections.Generic;
using System.Linq;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.Mage.Weapon;

namespace EntityStates.Commando.CommandoWeapon.Rux3
{
	// Token: 0x02000ABC RID: 2748
	public class RuxSweepBarrage : BaseState
	{
		// Token: 0x06003E91 RID: 16017 RVA: 0x00105998 File Offset: 0x00103B98
		public override void OnEnter()
		{
			base.OnEnter();
			this.totalDuration = FireSweepBarrage.baseTotalDuration / this.attackSpeedStat;
			this.firingDuration = FireSweepBarrage.baseFiringDuration / this.attackSpeedStat;
			base.characterBody.SetAimTimer(3f);
			base.PlayAnimation("Gesture, Additive", "FireSweepBarrage", "FireSweepBarrage.playbackRate", this.totalDuration);
			base.PlayAnimation("Gesture, Override", "FireSweepBarrage", "FireSweepBarrage.playbackRate", this.totalDuration);
			Util.PlaySound(FireSweepBarrage.enterSound, base.gameObject);
			Ray aimRay = base.GetAimRay();
			BullseyeSearch bullseyeSearch = new BullseyeSearch();
			bullseyeSearch.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
			bullseyeSearch.maxAngleFilter = FireSweepBarrage.fieldOfView * 0.5f;
			bullseyeSearch.maxDistanceFilter = 1000f;
			bullseyeSearch.searchOrigin = aimRay.origin;
			bullseyeSearch.searchDirection = aimRay.direction;
			bullseyeSearch.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
			bullseyeSearch.filterByLoS = true;
			bullseyeSearch.RefreshCandidates();
			this.targetHurtboxes = bullseyeSearch.GetResults().Where(new Func<HurtBox, bool>(Util.IsValid)).Distinct(default(HurtBox.EntityEqualityComparer)).ToList<HurtBox>();
			this.totalBulletsToFire = Mathf.Max(this.targetHurtboxes.Count, 1);
			this.timeBetweenBullets = 0.1f;
			this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
			this.muzzleIndex = this.childLocator.FindChildIndex(FireSweepBarrage.muzzle);
			this.muzzleTransform = this.childLocator.FindChild(this.muzzleIndex);
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x00105B30 File Offset: 0x00103D30
		private void Fire()
		{
			if (this.totalBulletsFired < this.totalBulletsToFire)
			{
				if (!string.IsNullOrEmpty(FireSweepBarrage.muzzle))
				{
					EffectManager.SimpleMuzzleFlash(FireSweepBarrage.muzzleEffectPrefab, base.gameObject, FireSweepBarrage.muzzle, false);
				}
				Util.PlaySound(FireSweepBarrage.fireSoundString, base.gameObject);
				base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
				if (NetworkServer.active && this.targetHurtboxes.Count > 0)
				{
					DamageInfo damageInfo = new DamageInfo();
					damageInfo.damage = this.damageStat * 4f;
					damageInfo.attacker = base.gameObject;
					damageInfo.procCoefficient = 1f;
					damageInfo.crit = Util.CheckRoll(this.critStat, base.characterBody.master);
					if (this.targetHurtboxIndex >= this.targetHurtboxes.Count)
					{
						this.targetHurtboxIndex = 0;
					}
					HurtBox hurtBox = this.targetHurtboxes[this.targetHurtboxIndex];
					if (hurtBox)
					{
						HealthComponent healthComponent = hurtBox.healthComponent;
						if (healthComponent)
						{
							this.targetHurtboxIndex++;
							Vector3 normalized = (hurtBox.transform.position - base.characterBody.corePosition).normalized;
							damageInfo.force = FireSweepBarrage.force * normalized;
							damageInfo.position = hurtBox.transform.position;
							EffectManager.SimpleImpactEffect(FireSweepBarrage.impactEffectPrefab, hurtBox.transform.position, normalized, true);
							healthComponent.TakeDamage(damageInfo);
							GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
						}
						if (FireLaserbolt.tracerEffectPrefab && this.childLocator)
						{
							int childIndex = this.childLocator.FindChildIndex(FireSweepBarrage.muzzle);
							this.childLocator.FindChild(childIndex);
							EffectData effectData = new EffectData
							{
								origin = hurtBox.transform.position,
								start = this.muzzleTransform.position
							};
							effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
							EffectManager.SpawnEffect(FireLaserbolt.tracerEffectPrefab, effectData, true);
						}
					}
				}
				this.totalBulletsFired++;
			}
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x00105D50 File Offset: 0x00103F50
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.fireTimer -= Time.fixedDeltaTime;
			if (this.fireTimer <= 0f)
			{
				this.Fire();
				this.fireTimer += this.timeBetweenBullets;
			}
			if (base.fixedAge >= this.totalDuration)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0000D472 File Offset: 0x0000B672
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x040039FA RID: 14842
		public static string enterSound;

		// Token: 0x040039FB RID: 14843
		public static string muzzle;

		// Token: 0x040039FC RID: 14844
		public static string fireSoundString;

		// Token: 0x040039FD RID: 14845
		public static GameObject muzzleEffectPrefab;

		// Token: 0x040039FE RID: 14846
		public static GameObject tracerEffectPrefab;

		// Token: 0x040039FF RID: 14847
		public static float baseTotalDuration;

		// Token: 0x04003A00 RID: 14848
		public static float baseFiringDuration;

		// Token: 0x04003A01 RID: 14849
		public static float fieldOfView;

		// Token: 0x04003A02 RID: 14850
		public static float maxDistance;

		// Token: 0x04003A03 RID: 14851
		public static float damageCoefficient;

		// Token: 0x04003A04 RID: 14852
		public static float procCoefficient;

		// Token: 0x04003A05 RID: 14853
		public static float force;

		// Token: 0x04003A06 RID: 14854
		public static int minimumFireCount;

		// Token: 0x04003A07 RID: 14855
		public static GameObject impactEffectPrefab;

		// Token: 0x04003A08 RID: 14856
		private float totalDuration;

		// Token: 0x04003A09 RID: 14857
		private float firingDuration;

		// Token: 0x04003A0A RID: 14858
		private int totalBulletsToFire;

		// Token: 0x04003A0B RID: 14859
		private int totalBulletsFired;

		// Token: 0x04003A0C RID: 14860
		private int targetHurtboxIndex;

		// Token: 0x04003A0D RID: 14861
		private float timeBetweenBullets;

		// Token: 0x04003A0E RID: 14862
		private List<HurtBox> targetHurtboxes = new List<HurtBox>();

		// Token: 0x04003A0F RID: 14863
		private float fireTimer;

		// Token: 0x04003A10 RID: 14864
		private ChildLocator childLocator;

		// Token: 0x04003A11 RID: 14865
		private int muzzleIndex;

		// Token: 0x04003A12 RID: 14866
		private Transform muzzleTransform;
	}
}
