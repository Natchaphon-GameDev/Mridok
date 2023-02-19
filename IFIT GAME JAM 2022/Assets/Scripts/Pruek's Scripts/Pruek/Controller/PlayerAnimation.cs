using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    public Rigidbody2D rb;
    public PlayerControl characterController;
    
    [SerializeField]private PlayerVariable PlayerVariable;

    public string currentStage;
    public string idle = "Character_Idle";
    public string walk = "Character_Run";
    public string jump = "Character_Jump";
    public string land = "Character_Land";
    public string boostUp = "Character_Boostup";
    public string grab = "Character_Grab";
    public string prepare = "Character_Prepare";
    public string throws = "Character_Throw";

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Animation();
    }

    private void PlayAnim(string newStage)
    {
        if (currentStage == newStage)
        {
            return;
        }
        playerAnimator.Play(newStage);
        currentStage = newStage;
    }

    private IEnumerator WaitForThrow()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerVariable.isThrowing = false;
    }
    
    private IEnumerator WaitForBoost()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerVariable.isBoostUp = false;
    }

    private void Animation() //Animate
    {
        if (PlayerVariable.isBoostUp && (PlayerVariable.isPreparing || PlayerVariable.isGrabbing))
        {
            PlayAnim(boostUp);
            StartCoroutine(WaitForBoost());
            PlayerVariable.isPreparing = false;
        }
        else if (PlayerVariable.isGrabbing && !characterController.isPlayerNearEdge && characterController.onGround)
        {
            PlayAnim(grab);
            PlayerVariable.isPreparing = false;
        }
        else if (PlayerVariable.isThrowing && PlayerVariable.isPreparing)
        {
            PlayAnim(throws);
            PlayerVariable.isPreparing = false;
            StartCoroutine(WaitForThrow());
        }
        else if (rb.velocity.y < 0f && !characterController.onGround && !PlayerVariable.isPreparing)
        {
            PlayAnim(land);
            PlayerVariable.isJumping = false;
            PlayerVariable.isfalling = true;
        }
        else if (PlayerVariable.isPreparing && characterController.onGround && !PlayerVariable.isThrowing)
        {
            PlayAnim(prepare);
            PlayerVariable.isWalking = false;
        }
        else if (PlayerVariable.isWalking && !PlayerVariable.isJumping)
        {
            PlayAnim(walk);
        }
        else if (PlayerVariable.isJumping && rb.velocity.y > 0f )
        {
            PlayAnim(jump);
        }
        else if (PlayerVariable.isIdle && !PlayerVariable.isWalking && !PlayerVariable.isJumping && !PlayerVariable.isThrowing)
        {
            PlayAnim(idle);
        }
    }
}
