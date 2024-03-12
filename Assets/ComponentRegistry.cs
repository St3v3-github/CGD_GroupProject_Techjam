using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComponentRegistry : MonoBehaviour
{
    [Header("Inputs")]
    public InputManager inputManager;
    public PlayerInput playerInput;
    public GamepadRumbleController gamepadRumbleController;

    [Header("Camera")]
    public Camera playerCamera;
    //public Camera emoteCamera;
    public GameObject cameraPosition;
    //public Raycast ray;
    public CameraMove moveCamera;

    [Header("Movement")]
    public CapsuleCollider hitboxCollider;
    public CapsuleCollider secondCollider;
    public Rigidbody rigidBody;
    public UpdatedPlayerController playerController;
    public Sliding sliding;
    public Dashing dashing;
    public JetPack jetPack;
    public Grappling grappling;
    public GrappleSwing grappleSwing;

    [Header("Spells")]
    public GameObject firePoint;
    public AttributeManager attributeManager;
    public SpellManagerTemplate spellManager;
    public StatusEffectHandler statusEffectHandler;

    [Header("Animations")]
    public Animator playerAnimator;
    public AnimationManager animationManager;
    public GameObject mainMesh;

    [Header("UI References")]
    public UIController uiController;
    public UIHandler uiHandler;
    public FullScreenController fullScreenController;
    public PlayerSelector spellIconThing;
    public PlayerScoreInfo playerScoreInfo;

}
