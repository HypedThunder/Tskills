using System;
using EntityStates.Captain.Weapon.Rux2;
using RoR2;
using EntityStates.ClayBruiser.Weapon;

namespace EntityStates.Captain.Weapon.Rux3
{
	// Token: 0x02000AC4 RID: 2756
	public class RuxFullAutoSpinDown : RuxFullAutoState
	{
		// Token: 0x06003EBD RID: 16061 RVA: 0x001066D8 File Offset: 0x001048D8
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = MinigunSpinDown.baseDuration / this.attackSpeedStat;
			Util.PlayScaledSound(MinigunSpinDown.sound, base.gameObject, this.attackSpeedStat);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x00106725 File Offset: 0x00104925
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
			}
		}

		// Token: 0x04003A3B RID: 14907
		public static float baseDuration;

		// Token: 0x04003A3C RID: 14908
		public static string sound;

		// Token: 0x04003A3D RID: 14909
		private float duration;
	}
}
