using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComponentRegistry : MonoBehaviour
{
    [Header("Camera")]
    public MoveCamera moveCamera;
    public Camera playerCamera;
    public Camera emoteCamera;
    public GameObject cameraPosition;
    public UpdatedCameraController cameraController;
    public Raycast ray;

    [Header("Movement")]
    public CapsuleCollider hitboxCollider;
    public CapsuleCollider secondCollider;
    public PlayerInput playerInput;
    public InputManager inputManager;
    public UpdatedPlayerController playerController;
    public Sliding sliding;
    public Rigidbody rigidBody;
    public Dashing dashing;
    public JetPack jetPack;
    public Grappling grappling;
    public GrappleSwing grappleSwing;

    [Header("Spells")]
    public InventoryEdit inventory;
    public AbilityManager2 abilityManager;
    public AdvancedProjectileSystem advancedProjectileSystem;
    public CastableAOEStrike castableAOEStrike;
    public Summon summonAnimal;
    public SpellCastOnStaff spellCastOnStaff;
    public Heal healSpell;
    public SpellCastOnRay spellCastOnRay;
    public Beam beamSpell;
    public ThrowSpell throwSpell;
    public GameObject firePoint;
    public AttributeManager attributeManager;

    [Header("Animations")]
    public Animator playerAnimator;
    public AnimationManager animationManager;

    [Header("UI References")]
    public UIController uiController;
    public FullScreenController fullScreenController;
    public PlayerSelector spellIconThing;

}
