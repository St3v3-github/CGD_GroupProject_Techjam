using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator playerAnim;

    public void toggleWalkingBool(bool walking)
    {
        playerAnim.SetBool("walking", walking);
    }

    public void toggleJumpingBool(bool jumping)
    {
        playerAnim.SetBool("jumping", jumping);
    }

    public void toggleGroundedBool(bool grounded)
    {
        playerAnim.SetBool("isGrounded", grounded);
    }
}
