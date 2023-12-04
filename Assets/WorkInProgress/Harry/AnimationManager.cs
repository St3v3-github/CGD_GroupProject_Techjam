using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator playerAnim;
    public Camera emoteCam;
    public Camera playerCam;

    private void Awake()
    {
        playerAnim.fireEvents = false;
    }

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

    public void toggleEmotingBool(bool Emoting)
    {
        /*
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
        playerCam.enabled = false;
        emoteCam.enabled = true;
        */

        if (playerAnim.GetBool("Emoting"))
        {
            if (Emoting)
            {
                Emoting = !Emoting;
            }
        }
        if (Emoting)
        {
            int emoteNumb = 1;
            float randomNumb = Random.Range(0, 10);
            if(randomNumb >= 5)
            {
                emoteNumb = 1;
            }
            else
            {
                emoteNumb = 0;
            }
            Debug.Log(emoteNumb);
            playerAnim.SetInteger("EmoteNumb", emoteNumb);
            playerAnim.SetBool("Emoting", Emoting);
            if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) //prevents 3rd person camera from activating when emote won't trigger
            {
                playerCam.enabled = false;
                emoteCam.enabled = true;
            }
        }
        else
        {
            playerAnim.SetBool("Emoting", Emoting);
            emoteCam.enabled = false;
            playerCam.enabled = true;
        }
    }

    public void disableEmote()
    {
        playerAnim.SetBool("Emoting", false);
        emoteCam.enabled = false;
        playerCam.enabled = true;
    }
    public void FootL() { }
    public void FootR() { }
    public void Hit() { }
    public void Run() { }
    public void Attack1() { }
}
