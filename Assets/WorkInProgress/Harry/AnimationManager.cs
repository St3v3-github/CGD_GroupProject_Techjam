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

    public void toggleAttackingBool(bool attacking)
    {
        playerAnim.SetBool("attacking", attacking);
    }

    public void toggleDamagedBool(bool damaged)
    {
        playerAnim.SetBool("damaged", damaged);
    }

    public void toggleDeadBool(bool dead)
    {
        playerAnim.SetBool("dead", dead);
    }
}
