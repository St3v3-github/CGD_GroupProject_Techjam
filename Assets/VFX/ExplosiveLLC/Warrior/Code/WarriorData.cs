/// <summary>
/// Contains enum data for the Warrior’s type, state, and animation triggers.
/// </summary>

namespace WarriorAnims
{
	/// <summary>
	/// The type of Warrior.  Determines which animations can play, and the
	/// timings for those animations in WarriorTiming.cs.
	/// </summary>
	public enum Warrior
	{
		Archer,
		Brute,
		Crossbow,
		Hammer,
		Karate,
		Knight,
		Mage,
		Ninja,
		Sorceress,
		Spearman,
		Swordsman,
		TwoHanded
	}

	/// <summary>
	/// The different movement / situational states the Warrior can be in.
	/// </summary>
	public enum WarriorState
	{
		Idle = 0,
		Move = 1,
		Jump = 2,
		DoubleJump = 3,
		Fall = 4,
		Block = 5,
		Drop = 6
	}

	/// <summary>
	/// Enum to use with the "TriggerNumber" parameter of the animator. Convert to (int) to set.
	/// </summary>
	public enum AnimatorTrigger
	{
		NoTrigger = 0,
		JumpTrigger = 1,
		ActionTrigger = 2,
		DashTrigger = 3,
		AttackTrigger = 4,
		JumpAttackTrigger = 5,
		DeathTrigger = 6,
		ReviveTrigger = 7,
		LightHitTrigger = 8,
		RollTrigger = 9,
		AttackSpecialTrigger = 10,
		AttackMoveTrigger = 11,
		AttackRanged = 12,
		BlockBreakTrigger = 13,
		ReloadTrigger = 14,
		WeaponSwitchTrigger = 15,
		BlockTrigger = 16
	}
}