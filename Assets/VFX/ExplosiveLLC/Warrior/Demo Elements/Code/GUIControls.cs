using UnityEngine;

namespace WarriorAnims
{
	public class GUIControls:MonoBehaviour
	{
		private WarriorController warriorController;
		private bool blockGui;
		private bool blockToggle;

		private void Awake()
		{ warriorController = GetComponent<WarriorController>(); }

		private void OnGUI()
		{
			// Character isn't dead or weapons sheathed.
			if (!warriorController.isDead && !warriorController.sheathed) {
				Attacking();
				Jumping();
				JumpAttack();

				// If can act.
				if (warriorController.canAction) {

					// If grounded.
					if (warriorController.MaintainingGround()) {
						Blocking();

						// Not Blocking.
						if (!warriorController.isBlocking) {
							Dashing();
							Damage();
							WeaponSheath();
						}
					}
				}
			}
			Revive();
			WeaponUnSheath();
			DebugWarrior();
		}

		private void Blocking()
		{
			blockGui = GUI.Toggle(new Rect(25, 215, 100, 30), blockGui, "Block");
			if (blockGui) {
				if (!blockToggle) {
					blockToggle = true;
					warriorController.isBlocking = true;
					warriorController.AllowInput(false);
					warriorController.LockBlock(true);
					warriorController.LockMove(true);
					warriorController.SetAnimatorBool("Blocking", true);
					warriorController.SetAnimatorTrigger(AnimatorTrigger.BlockTrigger);
					if (warriorController.warrior == Warrior.Crossbow && warriorController.ikHands != null)
					{ warriorController.ikHands.BlendIK(false, 0, 0.1f); }
				}
			}
			if (!blockGui) {
				if (blockToggle) {
					warriorController.isBlocking = false;
					blockToggle = false;
					warriorController.SetAnimatorBool("Blocking", false);
					warriorController.LockMove(false);
					warriorController.LockBlock(false);
					warriorController.AllowInput(true);
					if (warriorController.warrior == Warrior.Crossbow && warriorController.ikHands != null)
					{ warriorController.ikHands.BlendIK(true, 0, 0.1f); }
				}
			}
			// Blocking.
			if (blockGui) {
				if (GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")) { warriorController.GetHit(); }
				if (GUI.Button(new Rect(30, 270, 100, 30), "Block Break")) { warriorController.BlockBreak(); }
			}
		}

		private void Attacking()
		{
			if (!warriorController.isBlocking) {
				if (warriorController.MaintainingGround()) {
					if (warriorController.isMoving && warriorController.canRunAttack) {
						if (GUI.Button(new Rect(25, 85, 100, 30), "Running Attack")) { warriorController.RunningAttack(1); }
					}
					else {
						if (warriorController.warrior != Warrior.Archer && warriorController.warrior != Warrior.Crossbow) {
							if (GUI.Button(new Rect(25, 85, 100, 30), "Attack Chain")) { warriorController.AttackChain(); }
						}
						else {
							if (GUI.Button(new Rect(25, 85, 100, 30), "Attack1")) { warriorController.Attack(1); }
						}
					}
				}
				if (warriorController.canAction && warriorController.MaintainingGround()) {

					// Generic extra attacks.
					if (GUI.Button(new Rect(340, 85, 100, 30), "Special Attack1")) { warriorController.SpecialAttack(1); }
					if (GUI.Button(new Rect(340, 145, 100, 30), "Move Attack1")) { warriorController.MoveAttack(1); }
					if (GUI.Button(new Rect(340, 205, 100, 30), "Range Attack1")) { warriorController.RangeAttack(1); }

					// Archer Warrior.
					if (warriorController.warrior == Warrior.Archer) {
						if (GUI.Button(new Rect(340, 175, 100, 30), "Move Attack2")) { warriorController.MoveAttack(2); }
					}
					// Crossbow Warrior.
					if (warriorController.warrior == Warrior.Crossbow) {
						if (GUI.Button(new Rect(340, 235, 100, 30), "Range Attack2")) { warriorController.RangeAttack(2); }
					}
					// Karate Warrior.
					if (warriorController.warrior == Warrior.Karate) {
						if (GUI.Button(new Rect(130, 85, 100, 30), "Attack4")) { warriorController.Attack(4); }
						if (GUI.Button(new Rect(130, 115, 100, 30), "Attack5")) { warriorController.Attack(5); }
						if (GUI.Button(new Rect(130, 145, 100, 30), "Attack6")) { warriorController.Attack(6); }
						if (GUI.Button(new Rect(235, 85, 100, 30), "Attack7")) { warriorController.Attack(7); }
						if (GUI.Button(new Rect(235, 115, 100, 30), "Attack8")) { warriorController.Attack(8); }
						if (GUI.Button(new Rect(235, 145, 100, 30), "Attack9")) { warriorController.Attack(9); }
						if (GUI.Button(new Rect(340, 115, 100, 30), "Special Attack2")) { warriorController.SpecialAttack(2); }
						if (GUI.Button(new Rect(340, 175, 100, 30), "Move Attack2")) { warriorController.MoveAttack(2); }
						if (GUI.Button(new Rect(340, 235, 100, 30), "Range Attack2")) { warriorController.RangeAttack(2); }
					}
					// Knight Warrior.
					if (warriorController.warrior == Warrior.Knight) {
						if (GUI.Button(new Rect(340, 115, 100, 30), "Special Attack2")) { warriorController.SpecialAttack(2); }
					}
					// Mage Warrior.
					if (warriorController.warrior == Warrior.Mage) {
						if (GUI.Button(new Rect(340, 115, 100, 30), "Special Attack2")) { warriorController.SpecialAttack(2); }
						if (GUI.Button(new Rect(340, 235, 100, 30), "Range Attack2")) { warriorController.RangeAttack(2); }
					}
					// Ninja Warrior.
					if (warriorController.warrior == Warrior.Ninja) {
						if (GUI.Button(new Rect(340, 115, 100, 30), "Special Attack2")) { warriorController.SpecialAttack(2); }
						if (GUI.Button(new Rect(340, 235, 100, 30), "Range Attack2")) { warriorController.RangeAttack(2); }
						if (GUI.Button(new Rect(340, 265, 100, 30), "Range Attack3")) { warriorController.RangeAttack(3); }
					}
					// Sorceress Warrior.
					if (warriorController.warrior == Warrior.Sorceress) {
						if (GUI.Button(new Rect(130, 85, 100, 30), "Attack4")) { warriorController.Attack(4); }
						if (GUI.Button(new Rect(130, 115, 100, 30), "Attack5")) { warriorController.Attack(5); }
						if (GUI.Button(new Rect(130, 145, 100, 30), "Attack6")) { warriorController.Attack(6); }
						if (GUI.Button(new Rect(235, 85, 100, 30), "Attack7")) { warriorController.Attack(7); }
						if (GUI.Button(new Rect(235, 115, 100, 30), "Attack8")) { warriorController.Attack(8); }
						if (GUI.Button(new Rect(340, 115, 100, 30), "Special Attack2")) { warriorController.SpecialAttack(2); }
						if (GUI.Button(new Rect(445, 115, 100, 30), "Special Attack3")) { warriorController.SpecialAttack(3); }
						if (GUI.Button(new Rect(340, 175, 100, 30), "Move Attack2")) { warriorController.MoveAttack(2); }
						if (GUI.Button(new Rect(445, 175, 100, 30), "Move Attack3")) { warriorController.MoveAttack(3); }
					}
					// Spearman Warrior.
					if (warriorController.warrior == Warrior.Spearman) {
						if (GUI.Button(new Rect(130, 85, 100, 30), "Attack4")) { warriorController.Attack(4); }
						if (GUI.Button(new Rect(130, 115, 100, 30), "Attack5")) { warriorController.Attack(5); }
					}
				}
			}
		}

		private void Jumping()
		{
			if ((warriorController.canJump || warriorController.canDoubleJump) && (!blockGui || !warriorController.isBlocking)) {
				if (warriorController.MaintainingGround()) {
					if (GUI.Button(new Rect(25, 175, 100, 30), "Jump")) {
						if (warriorController.canJump) { warriorController.inputJump = true; ; }
					}
				}
				if (warriorController.canDoubleJump) {
					if (GUI.Button(new Rect(25, 175, 100, 30), "Double Jump")) { warriorController.inputJump = true; }
				}
			}
		}

		private void JumpAttack()
		{
			if (!warriorController.MaintainingGround()) {
				if (warriorController.warrior == Warrior.Karate
					|| warriorController.warrior == Warrior.Brute
					|| warriorController.warrior == Warrior.Hammer
					|| warriorController.warrior == Warrior.Spearman
					|| warriorController.warrior == Warrior.Swordsman
					|| warriorController.warrior == Warrior.TwoHanded
					|| warriorController.warrior == Warrior.Crossbow
					|| warriorController.warrior == Warrior.Mage) {
					if (GUI.Button(new Rect(25, 85, 100, 30), "Jump Attack")) { warriorController.JumpAttack(); }
				}
			}
		}

		private void Dashing()
		{
			if (GUI.Button(new Rect(25, 15, 100, 30), "Dash Forward")) { warriorController.Dash(1); }
			if (GUI.Button(new Rect(130, 15, 100, 30), "Dash Back")) { warriorController.Dash(3); }
			if (GUI.Button(new Rect(25, 45, 100, 30), "Dash Left")) { warriorController.Dash(4); }
			if (GUI.Button(new Rect(130, 45, 100, 30), "Dash Right")) { warriorController.Dash(2); }

			if (warriorController.warrior == Warrior.Knight) {
				if (GUI.Button(new Rect(255, 15, 100, 30), "Dash Forward2")) { warriorController.Dash(-1); }
				if (GUI.Button(new Rect(360, 15, 100, 30), "Dash Back2")) { warriorController.Dash(-3); }
				if (GUI.Button(new Rect(255, 45, 100, 30), "Dash Left2")) { warriorController.Dash(-4); }
				if (GUI.Button(new Rect(360, 45, 100, 30), "Dash Right2")) { warriorController.Dash(-2); }
			}
		}

		private void Damage()
		{
			if (GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")) { warriorController.GetHit(); }
			if (GUI.Button(new Rect(30, 270, 100, 30), "Death")) { warriorController.Death(); }
		}

		private void Revive()
		{
			if (warriorController.isDead) {
				if (GUI.Button(new Rect(30, 270, 100, 30), "Revive")) { warriorController.Revive(); }
			}
		}

		private void WeaponSheath()
		{
			if (warriorController.warrior == Warrior.Archer
				|| warriorController.warrior == Warrior.Ninja
				|| warriorController.warrior == Warrior.Knight
				|| warriorController.warrior == Warrior.Mage
				|| warriorController.warrior == Warrior.TwoHanded
				|| warriorController.warrior == Warrior.Crossbow
				|| warriorController.warrior == Warrior.Hammer
				|| warriorController.warrior == Warrior.Spearman
				|| warriorController.warrior == Warrior.Swordsman) {
				if (GUI.Button(new Rect(30, 305, 100, 30), "Sheath")) { warriorController.SheathWeapons(); }
			}
		}

		private void WeaponUnSheath()
		{
			if (warriorController.sheathed) {
				if (GUI.Button(new Rect(30, 305, 100, 30), "UnSheath")) { warriorController.SheathWeapons(); }
			}
		}

		private void DebugWarrior()
		{
			if (GUI.Button(new Rect(600, 20, 120, 30), "Debug Controller")) { warriorController.ControllerDebug(); }
			if (GUI.Button(new Rect(600, 60, 120, 30), "Debug Animator")) { warriorController.AnimatorDebug(); }
		}
	}
}