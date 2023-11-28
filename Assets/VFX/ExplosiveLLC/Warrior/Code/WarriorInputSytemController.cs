using UnityEngine;

namespace WarriorAnims
{
	/// <summary>
	/// Placeholder script.  Extract the actual script from the "InputSystem Support - Requires InputSystem Package".
	/// </summary>
	public class WarriorInputSystemController:MonoBehaviour
	{
		// Placeholder inputs.
		[HideInInspector] public bool inputAiming;
		[HideInInspector] public float inputAimVertical = 0;
		[HideInInspector] public float inputAimHorizontal = 0;
		[HideInInspector] public bool inputAttack;
		[HideInInspector] public bool inputAttackMove;
		[HideInInspector] public bool inputAttackRanged;
		[HideInInspector] public bool inputAttackSpecial;
		[HideInInspector] public bool inputBlock;
		[HideInInspector] public bool inputDeath;
		[HideInInspector] public bool inputJump;
		[HideInInspector] public bool inputLightHit;
		[HideInInspector] public bool inputRoll;
		[HideInInspector] public bool inputTarget;
		[HideInInspector] public bool inputTargetKey;
		[HideInInspector] public float inputHorizontal = 0;
		[HideInInspector] public float inputVertical = 0;
		[HideInInspector] public Vector3 moveInput;
		[HideInInspector] public Vector2 aimInput;
	}
}
