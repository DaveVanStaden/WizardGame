using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneControl : MonoBehaviour
{
    public Animator animator;
    public int blend1Time;
    public int blend2Time;
    
    private void Start()
    {
        animator.SetBool("cutscene1", true);
    }

    private void Update()
    {
        StartCoroutine(CooldownRoutine1());
    }

    IEnumerator CooldownRoutine1()
    {

        yield return new WaitForSeconds(blend1Time + 0.5f);
        animator.SetBool("cutscene1", false); 
        animator.SetBool("mainCameraSwitch", true);
    }

}
