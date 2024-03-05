using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComponentRegistry : MonoBehaviour
{
    [Header("Inputs")]
    public InputManager inputManager;
    public PlayerInput playerInput;

    [Header("Camera")]
    public MoveCamera moveCamera;
    public Camera playerCamera;
    //public Camera emoteCamera;
    public GameObject cameraPosition;
    //public Raycast ray;

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

    [Header("Animations")]
    public Animator playerAnimator;
    public AnimationManager animationManager;

    [Header("UI References")]
    public UIController uiController;
    public FullScreenController fullScreenController;
    public PlayerSelector spellIconThing;

}
