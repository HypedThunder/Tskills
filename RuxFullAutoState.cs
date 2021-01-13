using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.ClayBruiser.Weapon;

namespace EntityStates.Captain.Weapon.Rux2
{
	// Token: 0x02000AC2 RID: 2754
	public class RuxFullAutoState : BaseState
	{
		// Token: 0x06003EB2 RID: 16050 RVA: 0x00106544 File Offset: 0x00104744
		public override void OnEnter()
		{
			base.OnEnter();
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
			if (NetworkServer.active && base.characterBody)
			{
			}
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x00106581 File Offset: 0x00104781
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2f, false);
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x00106595 File Offset: 0x00104795
		public override void OnExit()
		{
			if (NetworkServer.active && base.characterBody)
			{
			}
			base.OnExit();
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06003EB5 RID: 16053 RVA: 0x000EB71E File Offset: 0x000E991E
		protected ref InputBankTest.ButtonState skillButtonState
		{
			get
			{
				return ref base.inputBank.skill1;
			}
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0000D472 File Offset: 0x0000B672
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x04003A33 RID: 14899
		public static string muzzleName;


		// Token: 0x04003A35 RID: 14901
		protected Transform muzzleTransform;
	}
}