using System;
using EntityStates.Captain.Weapon.Rux2;
using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Captain.Weapon.Rux;

namespace EntityStates.Captain.Weapon.Rux4
{
	// Token: 0x02000AC3 RID: 2755
	public class RuxFullAutoSpinUp : RuxFullAutoState
	{
		// Token: 0x06003EB9 RID: 16057 RVA: 0x001065C4 File Offset: 0x001047C4
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = MinigunSpinUp.baseDuration / this.attackSpeedStat;
			Util.PlaySound(MinigunSpinUp.sound, base.gameObject);
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			if (this.muzzleTransform && MinigunSpinUp.chargeEffectPrefab)
			{
				this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(MinigunSpinUp.chargeEffectPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
				this.chargeInstance.transform.parent = this.muzzleTransform;
				ScaleParticleSystemDuration component = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
				if (component)
				{
					component.newDuration = this.duration;
				}
			}
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x00106680 File Offset: 0x00104880
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextState(new RuxFullAutoFire());
			}
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x001066AE File Offset: 0x001048AE
		public override void OnExit()
		{
			base.OnExit();
			if (this.chargeInstance)
			{
				EntityState.Destroy(this.chargeInstance);
			}
		}

		// Token: 0x04003A36 RID: 14902
		public static float baseDuration;

		// Token: 0x04003A37 RID: 14903
		public static string sound;

		// Token: 0x04003A38 RID: 14904
		public static GameObject chargeEffectPrefab;

		// Token: 0x04003A39 RID: 14905
		private GameObject chargeInstance;

		// Token: 0x04003A3A RID: 14906
		private float duration;
	}
}
