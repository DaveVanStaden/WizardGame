using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationmanager : MonoBehaviour
{
    public Animator animator;


    public void IdleAnimation()
    {
        animator.SetInteger("AnimationState", 0);
    }
    public void WalkAnimation()
    {
        animator.SetInteger("AnimationState", 1);
    }
    public void JumpAnimation()
    {
        animator.SetInteger("AnimationState", 2);
    }
}
