using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using EntityStates.Commando.CommandoWeapon;
using EntityStates.Commando.CommandoWeapon.Rux2;

namespace EntityStates.Commando.CommandoWeapon.Rux
{
	// Token: 0x02000ABE RID: 2750
	public class RuxMissilePaint : BaseState
	{
		// Token: 0x06003E9C RID: 16028 RVA: 0x00105F32 File Offset: 0x00104132
		public override void OnEnter()
		{
			base.OnEnter();
			this.targetsList = new List<GameObject>();
			this.targetIndicators = new Dictionary<GameObject, GameObject>();
			this.retargetTimer = 0f;
			this.lastTarget = null;
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x00105F64 File Offset: 0x00104164
		public override void OnExit()
		{
			base.OnExit();
			foreach (KeyValuePair<GameObject, GameObject> keyValuePair in this.targetIndicators)
			{
				if (keyValuePair.Value)
				{
					EntityState.Destroy(keyValuePair.Value);
				}
			}
			this.targetIndicators = null;
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x00105FD8 File Offset: 0x001041D8
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority)
			{
				Ray aimRay = base.GetAimRay();
				base.StartAimMode(aimRay, 2f, false);
				if (!base.inputBank || !base.inputBank.skill2.down)
				{
					if (this.targetsList.Count == 0)
					{
						this.outer.SetNextStateToMain();
						return;
					}
					this.outer.SetNextState(new RuxMissileFire
					{
						targetsList = this.targetsList
					});
					return;
				}
				else
				{
					LayerMask mask = LayerIndex.world.mask | LayerIndex.entityPrecise.mask;
					float maxDistance = 100f;
					GameObject gameObject = null;
					RaycastHit raycastHit;
					if (Physics.Raycast(aimRay, out raycastHit, maxDistance, mask, QueryTriggerInteraction.Ignore))
					{
						HurtBox component = raycastHit.collider.GetComponent<HurtBox>();
						if (component)
						{
							HealthComponent healthComponent = component.healthComponent;
							if (healthComponent)
							{
								gameObject = healthComponent.gameObject;
							}
						}
					}
					int num = 10;
					if (this.targetsList.Count < num)
					{
						this.retargetTimer -= Time.fixedDeltaTime;
						if (this.lastTarget != gameObject || this.retargetTimer <= 0f)
						{
							if (gameObject)
							{
								this.AddTarget(gameObject);
							}
							this.retargetTimer = this.retargetInterval;
							this.lastTarget = gameObject;
						}
					}
				}
			}
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x00106140 File Offset: 0x00104340
		private void AddTarget(GameObject target)
		{
			this.targetsList.Add(target);
			GameObject value = null;
			if (!this.targetIndicators.TryGetValue(target, out value))
			{
				value = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ShieldTransferIndicator"), target.transform.position, Quaternion.identity, target.transform);
				this.targetIndicators[target] = value;
			}
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x0000D472 File Offset: 0x0000B672
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x04003A1B RID: 14875
		private List<GameObject> targetsList;

		// Token: 0x04003A1C RID: 14876
		private Dictionary<GameObject, GameObject> targetIndicators;

		// Token: 0x04003A1D RID: 14877
		private GameObject lastTarget;

		// Token: 0x04003A1E RID: 14878
		private float retargetTimer;

		// Token: 0x04003A1F RID: 14879
		private float retargetInterval = 0.2f;
	}
}