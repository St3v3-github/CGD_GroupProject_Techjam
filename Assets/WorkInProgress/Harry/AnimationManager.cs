using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void toggleRunningBool(bool Running)
    {
        playerAnim.SetBool("Running", Running);
    }

    public void toggleCastingBool(bool Casting)
    {
        playerAnim.SetBool("Casting", Casting);
    }

    public void toggleEmotingBool()
    {
        int emoteNumb = 1;
        bool Emoting = playerAnim.GetBool("Emoting");
        float randomNumb = Random.Range(0, 1);
        if (randomNumb > 0.5f)
        {
            emoteNumb = 1;
        }
        else
        {
            emoteNumb = 0;
        }
        Debug.Log(emoteNumb);
        playerAnim.SetInteger("EmoteNumb", emoteNumb);
        playerAnim.SetBool("Emoting", !Emoting);
    }

    public void disableEmote()
    {
        playerAnim.SetBool("Emoting", false);
    }
    public void FootL() { }
    public void FootR() { }
    public void Hit() { }
    public void Run() { }
    public void Attack1() { }
}
