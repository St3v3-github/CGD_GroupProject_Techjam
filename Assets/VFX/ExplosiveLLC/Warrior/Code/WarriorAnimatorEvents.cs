using UnityEngine;
using UnityEngine.Events;

namespace WarriorAnims
{
	[HelpURL("https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html")]
	public class WarriorCharacterAnimatorEvents:MonoBehaviour
    {
		/// <summary>
		/// Placeholder functions for Animation events.
		/// </summary>
		public UnityEvent OnHit = new UnityEvent();
		public UnityEvent OnFootR = new UnityEvent();
		public UnityEvent OnFootL = new UnityEvent();
		public UnityEvent OnLand = new UnityEvent();
		public UnityEvent OnShoot = new UnityEvent();
		public UnityEvent OnWeaponSwitch = new UnityEvent();

		[HideInInspector] public WarriorController warriorController;

		public void Hit()
		{ OnHit.Invoke(); }

		public void FootR()
		{ OnFootR.Invoke(); }

		public void FootL()
		{ OnFootL.Invoke(); }

		public void Land()
		{ OnLand.Invoke(); }

		public void Shoot()
		{ OnShoot.Invoke(); }

		/// <summary>
		/// Checked when switching weapons to know when to turn on/off weapon models.
		/// </summary>
		public void WeaponSwitch()
		{ warriorController.waitingOnWeapons = false; }
	}
}