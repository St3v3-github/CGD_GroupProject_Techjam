using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator playerAnim; //Animation Support (Do Not Touch)

    public void toggleJumpingBool(bool jumping)
    {
        playerAnim.SetBool("isJumping", jumping);
    }

    public void toggleWalkingBool(bool walking)
    {
        Debug.Log(walking);
        playerAnim.SetBool("isWalking", walking);
    }

    public void toggleRunningBool(bool running)
    {
        playerAnim.SetBool("isRunning", running);
    }

    public void toggleGroundedBool(bool grounded)
    {
        playerAnim.SetBool("isGrounded", grounded);
    }
}
