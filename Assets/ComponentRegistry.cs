using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

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
    public GameObject firePoint;
    public FlameThrower flameThrower;

    public AttributeManager attributeManager;
    public Score score;

    [Header("Animations")]
    public Animator playerAnimator;
    public AnimationManager animationManager;

    [Header("UI References")]
    public UIController uiController;
    public FullScreenController fullScreenController;
    public PlayerSelector spellIconThing;

}
