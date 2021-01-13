using System;
using System.Collections.Generic;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.Commando.CommandoWeapon;

namespace EntityStates.Commando.CommandoWeapon.Rux2
{
	// Token: 0x02000AB5 RID: 2741
	public class RuxMissileFire : BaseState
	{
		// Token: 0x06003E65 RID: 15973 RVA: 0x00104AA4 File Offset: 0x00102CA4
		private void FireMissile(GameObject targetObject)
		{
			Ray aimRay = base.GetAimRay();
			if (this.modelTransform && this.modelTransform.GetComponent<ChildLocator>())
			{
				Transform transform = null;
				if (transform)
				{
					aimRay.origin = transform.position;
				}
			}
            _ = FireMicroMissiles.effectPrefab;
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(2f);
			}
			if (this.aimAnimator)
			{
				this.aimAnimator.AimImmediate();
			}
			if (base.isAuthority)
			{
				float x = UnityEngine.Random.Range(FireMicroMissiles.minSpread, FireMicroMissiles.maxSpread);
				float z = UnityEngine.Random.Range(0f, 360f);
				Vector3 up = Vector3.up;
				Vector3 axis = Vector3.Cross(up, aimRay.direction);
				Vector3 vector = Quaternion.Euler(0f, 0f, z) * (Quaternion.Euler(x, 0f, 0f) * Vector3.forward);
				float y = vector.y;
				vector.y = 0f;
				float angle = Mathf.Atan2(vector.z, vector.x) * 57.29578f - 90f;
				float angle2 = Mathf.Atan2(y, vector.magnitude) * 57.29578f + FireMicroMissiles.arcAngle;
				Vector3 forward = Quaternion.AngleAxis(angle, up) * (Quaternion.AngleAxis(angle2, axis) * aimRay.direction);
				ProjectileManager.instance.FireProjectile(FireMicroMissiles.projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(forward), base.gameObject, this.damageStat * 2f, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, targetObject, -1f);
			}
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00104C68 File Offset: 0x00102E68
		public override void OnEnter()
		{
			base.OnEnter();
			this.currentTargetIndex = 0;
			this.fireInterval = FireMicroMissiles.baseFireInterval / this.attackSpeedStat;
			this.modelTransform = base.GetModelTransform();
			this.aimAnimator = (this.modelTransform ? this.modelTransform.GetComponent<AimAnimator>() : null);
			Ray aimRay = base.GetAimRay();
			base.StartAimMode(aimRay, 2f, false);
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x00032FA7 File Offset: 0x000311A7
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00104CD8 File Offset: 0x00102ED8
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.fireTimer -= Time.fixedDeltaTime;
			if (this.fireTimer <= 0f)
			{
				if (this.currentTargetIndex >= this.targetsList.Count && base.isAuthority)
				{
					this.outer.SetNextStateToMain();
					return;
				}
				List<GameObject> list = this.targetsList;
				int num = this.currentTargetIndex;
				this.currentTargetIndex = num + 1;
				GameObject targetObject = list[num];
				this.FireMissile(targetObject);
				this.fireTimer += this.fireInterval;
			}
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x00104D68 File Offset: 0x00102F68
		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write((uint)this.targetsList.Count);
			for (int i = 0; i < this.targetsList.Count; i++)
			{
				writer.Write(this.targetsList[i]);
			}
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x00104DB8 File Offset: 0x00102FB8
		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			uint num = reader.ReadUInt32();
			this.targetsList = new List<GameObject>((int)num);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				this.targetsList.Add(reader.ReadGameObject());
				num2++;
			}
		}

		// Token: 0x040039AD RID: 14765
		public static GameObject effectPrefab;

		// Token: 0x040039AE RID: 14766
		public static GameObject projectilePrefab;

		// Token: 0x040039AF RID: 14767
		public static float damageCoefficient = 1f;

		// Token: 0x040039B0 RID: 14768
		public static float baseFireInterval = 0.1f;

		// Token: 0x040039B1 RID: 14769
		public static float minSpread = 0f;

		// Token: 0x040039B2 RID: 14770
		public static float maxSpread = 5f;

		// Token: 0x040039B3 RID: 14771
		public static float arcAngle = 5f;

		// Token: 0x040039B4 RID: 14772
		public List<GameObject> targetsList;

		// Token: 0x040039B5 RID: 14773
		private Transform modelTransform;

		// Token: 0x040039B6 RID: 14774
		private AimAnimator aimAnimator;

		// Token: 0x040039B7 RID: 14775
		private float fireTimer;

		// Token: 0x040039B8 RID: 14776
		private float fireInterval;

		// Token: 0x040039B9 RID: 14777
		private int currentTargetIndex;
	}
}