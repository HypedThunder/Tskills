using System;
using System.Collections.Generic;
using System.Linq;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Loader;

namespace EntityStates.Loader.Rux
{
	// Token: 0x020009C2 RID: 2498
	public class RuxElectroboom : LoaderMeleeAttack
	{
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060039B7 RID: 14775 RVA: 0x000ECB30 File Offset: 0x000EAD30
		private Vector3 punchVector
		{
			get
			{
				return base.characterDirection.forward.normalized;
			}
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x000ECB50 File Offset: 0x000EAD50
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterMotor.velocity.y = BigPunch.shorthopVelocityOnEnter;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x000ECB6D File Offset: 0x000EAD6D
		protected override void PlayAnimation()
		{
			base.PlayAnimation();
			base.PlayAnimation("FullBody, Override", "BigPunch", "BigPunch.playbackRate", this.duration);
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x000ECB90 File Offset: 0x000EAD90
		protected override void AuthorityFixedUpdate()
		{
			base.AuthorityFixedUpdate();
			if (this.hasHit && !this.hasKnockbackedSelf && !base.authorityInHitPause)
			{
				this.hasKnockbackedSelf = true;
				base.healthComponent.TakeDamageForce(this.punchVector * -BigPunch.knockbackForce, true, false);
			}
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x000ECBE0 File Offset: 0x000EADE0
		protected override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
		{
			base.AuthorityModifyOverlapAttack(overlapAttack);
			overlapAttack.maximumOverlapTargets = 1;
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x000ECBF0 File Offset: 0x000EADF0
		protected override void OnMeleeHitAuthority()
		{
			if (this.hasHit)
			{
				return;
			}
			base.OnMeleeHitAuthority();
			this.hasHit = true;
			if (base.FindModelChild(this.swingEffectMuzzleString))
			{
				FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
				fireProjectileInfo.position = base.GetAimRay().origin;
				fireProjectileInfo.rotation = Quaternion.LookRotation(this.punchVector);
				fireProjectileInfo.crit = base.RollCrit();
				fireProjectileInfo.damage = 1f * this.damageStat;
				fireProjectileInfo.owner = base.gameObject;
				fireProjectileInfo.projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/LoaderZapCone");
				ProjectileManager.instance.FireProjectile(fireProjectileInfo);
			}
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x000ECCA0 File Offset: 0x000EAEA0
		private void FireSecondaryRaysServer()
		{
			Ray aimRay = base.GetAimRay();
			TeamIndex team = base.GetTeam();
			BullseyeSearch bullseyeSearch = new BullseyeSearch();
			bullseyeSearch.teamMaskFilter = TeamMask.GetEnemyTeams(team);
			bullseyeSearch.maxAngleFilter = BigPunch.maxShockFOV * 0.5f;
			bullseyeSearch.maxDistanceFilter = BigPunch.maxShockDistance;
			bullseyeSearch.searchOrigin = aimRay.origin;
			bullseyeSearch.searchDirection = this.punchVector;
			bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
			bullseyeSearch.filterByLoS = false;
			bullseyeSearch.RefreshCandidates();
			List<HurtBox> list = bullseyeSearch.GetResults().Where(new Func<HurtBox, bool>(Util.IsValid)).ToList<HurtBox>();
			Transform transform = base.FindModelChild(this.swingEffectMuzzleString);
			if (transform)
			{
				for (int i = 0; i < Mathf.Min(list.Count, BigPunch.maxShockCount); i++)
				{
					HurtBox hurtBox = list[i];
					if (hurtBox)
					{
						LightningOrb lightningOrb = new LightningOrb();
						lightningOrb.bouncedObjects = new List<HealthComponent>();
						lightningOrb.attacker = base.gameObject;
						lightningOrb.teamIndex = team;
						lightningOrb.damageValue = this.damageStat * BigPunch.shockDamageCoefficient;
						lightningOrb.isCrit = base.RollCrit();
						lightningOrb.origin = transform.position;
						lightningOrb.bouncesRemaining = 0;
						lightningOrb.lightningType = LightningOrb.LightningType.Loader;
						lightningOrb.procCoefficient = BigPunch.shockProcCoefficient;
						lightningOrb.target = hurtBox;
						OrbManager.instance.AddOrb(lightningOrb);
					}
				}
			}
		}

		// Token: 0x04003324 RID: 13092
		public static int maxShockCount;

		// Token: 0x04003325 RID: 13093
		public static float maxShockFOV;

		// Token: 0x04003326 RID: 13094
		public static float maxShockDistance;

		// Token: 0x04003327 RID: 13095
		public static float shockDamageCoefficient;

		// Token: 0x04003328 RID: 13096
		public static float shockProcCoefficient;

		// Token: 0x04003329 RID: 13097
		public static float knockbackForce;

		// Token: 0x0400332A RID: 13098
		public static float shorthopVelocityOnEnter;

		// Token: 0x0400332B RID: 13099
		private bool hasHit;

		// Token: 0x0400332C RID: 13100
		private bool hasKnockbackedSelf;
	}
}