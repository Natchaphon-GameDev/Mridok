using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Self Assign")]
    [SerializeField] Rigidbody theRB;
    [SerializeField] SpriteRenderer theSR;
    [SerializeField] Transform groundPoint;
    [SerializeField] private InteractionRays interRay;

    [Header("Move Setting")]
    [SerializeField] float moveSpeed, jumpForce;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] private bool moveDependsOnCamera;
    [HideInInspector] public bool enabledInput = true;
    
    private Vector2 moveInput;
    private bool isGrounded;
    private bool movingBackwards;

    [Header("Sprite Relate")]
    [SerializeField] Animator anim;
    [SerializeField] Animator flipAnim;
    
    [Header("Camera")]
    [SerializeField] private Camera activeCamera;
    private Vector3 camFront;
    private Vector3 camRight;
    
    [Header("Animation ")]
    public string currentStage;
    public string idle = "Character_Idle";
    public string walk = "Character_Run";
    private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = theSR.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(enabledInput)Walk();
        
        Jump();
        
        Animate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(activeCamera.GetComponent<CameraMode>()._cameraMode == CameraMode.CamMode.Perspective)
            switch (other.tag)
            {
                case "Building":
                    other.GetComponent<StructureCamera>().Focus(theSR);
                    break;
            }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(activeCamera.GetComponent<CameraMode>()._cameraMode == CameraMode.CamMode.Perspective)
            switch (other.tag)
            {
                case "Building":
                    other.GetComponent<StructureCamera>().DeFocus(theSR);
                    break;
            }
    }

    private void Walk()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            PlayAnim(walk);
        }
        else
        {
            PlayAnim(idle);
        }
        
        Facing();
        
        if(!moveDependsOnCamera)theRB.velocity = new Vector3(moveInput.x * moveSpeed, theRB.velocity.y, moveInput.y * moveSpeed);
        else
        {
            CameraAffect();
            theRB.velocity = (camFront * moveInput.y + camRight * moveInput.x) *moveSpeed;
        }
        theRB.velocity *= Time.deltaTime;
    }
    
    private void Jump()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, whatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            theRB.velocity += new Vector3(0f, jumpForce, 0f);
        }
    }
    
    private void Animate()
    {
        if(anim != null) anim.SetFloat("moveSpeed", theRB.velocity.magnitude);
        
        if(anim != null) anim.SetBool("onGround", isGrounded);

        if (theSR.flipX && moveInput.x < 0)
        {
            theSR.flipX = false;
            if(flipAnim != null) flipAnim.SetTrigger("Flip");
        }
        else if (!theSR.flipX && moveInput.x > 0)
        {
            theSR.flipX = true;
            if(flipAnim != null) flipAnim.SetTrigger("Flip");
        }

        if (!movingBackwards && moveInput.y > 0)
        {
            movingBackwards = true;
            if(flipAnim != null) flipAnim.SetTrigger("Flip");
        }
        else if (movingBackwards && moveInput.y < 0)
        {
            movingBackwards = false;
            if(flipAnim != null) flipAnim.SetTrigger("Flip");
        }
        
        if(anim != null) anim.SetBool("movingBackwards", movingBackwards);
    }
    
    private void CameraAffect()
    {
        camFront = activeCamera.transform.forward;
        camRight = activeCamera.transform.right;

        camFront.y = 0f;
        camRight.y = 0f;
        
        camFront.Normalize();
        camRight.Normalize();
    }

    private void Facing()
    {
        if(moveInput.y > 0) interRay.Facing(transform.forward);
        else if(moveInput.y < 0) interRay.Facing(-transform.forward);
        else if(moveInput.x > 0) interRay.Facing(transform.right);
        else if(moveInput.x < 0) interRay.Facing(-transform.right);
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

    public void EmergencyBreak()
    {
        moveInput = new Vector2(0f, 0f);
        PlayAnim(idle);
        theRB.velocity = Vector3.zero;
    }
}
