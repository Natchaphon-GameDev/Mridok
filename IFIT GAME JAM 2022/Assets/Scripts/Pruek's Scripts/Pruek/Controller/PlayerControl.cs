using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Player
{
    A,
    B
}

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float putForce;
    [SerializeField] private float putForwardForce;
    private Controller controller;
    [SerializeField] private Player player;
    [SerializeField] private float movementAcceleration;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float airLinearDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpFallMultiplier;
    [SerializeField] private float groundLinearDrag;
    
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private float fontRaycastLength;
    [SerializeField] private float backRaycastLength;
    [SerializeField] private float underRaycastLength;
    [SerializeField] private Vector3 groundRaycastOffset;
    [SerializeField] private Vector3 fontRaycastOffset;
    [SerializeField] private Vector3 backRaycastOffset;
    [SerializeField] private Vector3 underRaycastOffset;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private PlayerVariable PlayerVariable;

    private Rigidbody2D playerA;
    private Rigidbody2D playerB;

    private float InputPlayer;

    private bool toggle;

    public List<Transform> crucifixs;

    private bool canJump => onGround && !PlayerVariable.isCantMove;
    private bool canMove => !PlayerVariable.isCantMove;

    private bool waitForFriend = false;

    private bool changingDirection =>
        (rb.velocity.x > 0f && InputPlayer < 0f) || (rb.velocity.x < 0f && InputPlayer > 0f);

    public bool onGround;
    public bool isPlayerNearEdge;
    public bool isFaceRight = true;


    public bool isPlayerOnFont;
    public bool isPlayerOnBack;
    public bool isPlayerOnUnder;


    [FormerlySerializedAs("jump")] [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip landed;
    public AudioClip walk;

    private Rigidbody2D rb;

    private void Awake()
    {
        controller = new Controller();
        rb = GetComponent<Rigidbody2D>();
        playerA = GameObject.FindWithTag("PlayerA").GetComponent<Rigidbody2D>();
        playerB = GameObject.FindWithTag("PlayerB").GetComponent<Rigidbody2D>();

        if (player == Player.A)
        {
            InputPlayer = controller.PlayerA.Move.ReadValue<float>();
            controller.PlayerA.Jump.started += ctx => Jump();
        }
        else if (player == Player.B)
        {
            InputPlayer = controller.PlayerB.Move.ReadValue<float>();
            controller.PlayerB.Jump.started += ctx => Jump();
        }
    }

    private void Start()
    {
        foreach (var crucifix in crucifixs)
        {
            Physics2D.IgnoreCollision(crucifix.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void FixedUpdate()
    {
        Move();
        FallMultiplier();
        CheckCollisions();
        if (PlayerVariable.isWalking && !toggle && !PlayerVariable.isJumping && !PlayerVariable.isfalling)
        {
            InvokeRepeating("WalkSound", 0, 0.3f);
            toggle = true;
        }
        else if (!PlayerVariable.isWalking || PlayerVariable.isJumping || PlayerVariable.isfalling)
        {
            CancelInvoke("WalkSound");
            toggle = false;
        }
    }

    private void WalkSound()
    {
        SoundManager.instance.Play(SoundManager.SoundName.Walk);
    }

    private IEnumerator WaitForPush(float time)
    {
        PlayerVariable.isThrowing = true;
        yield return new WaitForSeconds(time);
        if (player == Player.A)
        {
            if (isFaceRight)
            {
                playerB.AddForce(Vector2.right * putForce * putForwardForce, ForceMode2D.Force);
            }
            else
            {
                playerB.AddForce(Vector2.left * putForce * putForwardForce, ForceMode2D.Force);
            }
        }
        else if (player == Player.B)
        {
            if (isFaceRight)
            {
                playerA.AddForce(Vector2.right * putForce * putForwardForce, ForceMode2D.Force);
            }
            else
            {
                playerA.AddForce(Vector2.left * putForce * putForwardForce, ForceMode2D.Force);
            }
        }
    }

    private void HelpOtherPlayer()
    {

        if (!onGround)
        {
            return; 
        }
        
        if (player == Player.A)
        {
            if (controller.PlayerA.Interaction.IsPressed() && onGround && !isPlayerNearEdge)
            {
                PlayerVariable.isGrabbing = true;
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
            else if (controller.PlayerA.Interaction.IsPressed() && onGround)
            {
                PlayerVariable.isPreparing = true;
                waitForFriend = true;
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
            else
            {
                PlayerVariable.isPreparing = false;
                waitForFriend = false;
                PlayerVariable.isGrabbing = false;
            }
            if (isPlayerOnFont && controller.PlayerA.Interaction.inProgress)
            {
                playerB.AddForce(Vector2.up * putForce, ForceMode2D.Force);
                PlayerVariable.isBoostUp = true;
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
            else if (isPlayerOnBack && controller.PlayerA.Interaction.inProgress)
            {
                playerB.AddForce(Vector2.up * putForce, ForceMode2D.Force);
                StartCoroutine(WaitForPush(0.1f));
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
            else if (isPlayerOnUnder && !isPlayerNearEdge && controller.PlayerA.Interaction.inProgress)
            {
                playerB.AddForce(Vector2.up * putForce/3, ForceMode2D.Force);
                PlayerVariable.isBoostUp = true;
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
        }
        else if (player == Player.B)
        {
            if (controller.PlayerB.Interaction.IsPressed() && onGround)
            {
                PlayerVariable.isPreparing = true;
                waitForFriend = true;
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
            else
            {
                PlayerVariable.isPreparing = false;
                waitForFriend = false;
            }
            if (isPlayerOnFont && controller.PlayerB.Interaction.inProgress)
            {
                playerA.AddForce(Vector2.up * putForce, ForceMode2D.Force);
                PlayerVariable.isBoostUp = true;
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
            else if (isPlayerOnBack && controller.PlayerB.Interaction.inProgress)
            {
                playerA.AddForce(Vector2.up * putForce, ForceMode2D.Force);
                StartCoroutine(WaitForPush(0.1f));
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
            else if (isPlayerOnUnder && !isPlayerNearEdge && controller.PlayerB.Interaction.inProgress)
            {
                playerA.AddForce(Vector2.up * putForce/3, ForceMode2D.Force);
                PlayerVariable.isBoostUp = true;
                SoundManager.instance.Play(SoundManager.SoundName.BoostJump);
            }
        }
    }

    private void Update()
    {
        Flip();
        HelpOtherPlayer();
        if (onGround && PlayerVariable.isfalling)
        {
            //SoundManager.instance.Play();
        }

        if (onGround)
        {
            ApplyGroundLinearDrag();
            PlayerVariable.isfalling = false;
        }
        else
        {
            ApplyAirLinearDrag();
        }
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(InputPlayer) < 0.4f || changingDirection)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void FallMultiplier()
    {
        if (player == Player.A)
        {
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !controller.PlayerA.Jump.triggered)
            {
                rb.gravityScale = lowJumpFallMultiplier;
            }
            else
            {
                rb.gravityScale = 1f;
            }
        }
        else if (player == Player.B)
        {
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !controller.PlayerB.Jump.triggered)
            {
                rb.gravityScale = lowJumpFallMultiplier;
            }
            else
            {
                rb.gravityScale = 1f;
            }
        }
    }

    private void Flip()
    {
        if (CamTrigger.isFontStage)
        {
            if (player == Player.A)
            {
                if (controller.PlayerA.Move.ReadValue<float>() == -1)
                {
                    isFaceRight = true;
                    fontRaycastLength = 0.5f;
                    backRaycastLength = 0.5f;
                    underRaycastLength = 0.2f;
                    underRaycastOffset.x = -0.33f;
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                else if (controller.PlayerA.Move.ReadValue<float>() == 1)
                {
                    isFaceRight = false;
                    fontRaycastLength = -0.5f;
                    backRaycastLength = -0.5f;
                    underRaycastLength = -0.2f;
                    underRaycastOffset.x = 0.41f;
                    transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                }
            }
            else if (player == Player.B)
            {
                if (controller.PlayerB.Move.ReadValue<float>() == -1)
                {
                    isFaceRight = true;
                    fontRaycastLength = 0.5f;
                    backRaycastLength = 0.5f;
                    underRaycastLength = 0.2f;
                    underRaycastOffset.x = -0.33f;
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                else if (controller.PlayerB.Move.ReadValue<float>() == 1)
                {
                    isFaceRight = false;
                    fontRaycastLength = -0.5f;
                    backRaycastLength = -0.5f;
                    underRaycastLength = -0.2f;
                    underRaycastOffset.x = 0.41f;
                    transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                }
            }
            return;
        }
        if (player == Player.A)
        {
            if (controller.PlayerA.Move.ReadValue<float>() == 1)
            {
                isFaceRight = true;
                fontRaycastLength = 0.5f;
                backRaycastLength = 0.5f;
                underRaycastLength = 0.2f;
                underRaycastOffset.x = -0.33f;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else if (controller.PlayerA.Move.ReadValue<float>() == -1)
            {
                isFaceRight = false;
                fontRaycastLength = -0.5f;
                backRaycastLength = -0.5f;
                underRaycastLength = -0.2f;
                underRaycastOffset.x = 0.41f;
                transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            }
        }
        else if (player == Player.B)
        {
            if (controller.PlayerB.Move.ReadValue<float>() == 1)
            {
                isFaceRight = true;
                fontRaycastLength = 0.5f;
                backRaycastLength = 0.5f;
                underRaycastLength = 0.2f;
                underRaycastOffset.x = -0.33f;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else if (controller.PlayerB.Move.ReadValue<float>() == -1)
            {
                isFaceRight = false;
                fontRaycastLength = -0.5f;
                backRaycastLength = -0.5f;
                underRaycastLength = -0.2f;
                underRaycastOffset.x = 0.41f;
                transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            }
        }
    }

    private void Move()
    {
        if (canMove && !waitForFriend)
        {
            if (player == Player.A)
            {
                if (controller.PlayerA.Move.ReadValue<float>() != 0)
                {
                    PlayerVariable.isWalking = true;
                    PlayerVariable.isIdle = false;
                }
                else
                {
                    PlayerVariable.isIdle = true;
                    PlayerVariable.isWalking = false;
                }

                if (CamTrigger.isFontStage)
                {
                    rb.AddForce(new Vector2(-controller.PlayerA.Move.ReadValue<float>(), 0f) * movementAcceleration);

                    if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
                    {
                        rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
                    }
                }
                else
                {
                    rb.AddForce(new Vector2(controller.PlayerA.Move.ReadValue<float>(), 0f) * movementAcceleration);

                    if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
                    {
                        rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
                    }
                }
            }
            else if (player == Player.B)
            {
                if (controller.PlayerB.Move.ReadValue<float>() != 0)
                {
                    PlayerVariable.isWalking = true;
                    PlayerVariable.isIdle = false;
                }
                else
                {
                    PlayerVariable.isIdle = true;
                    PlayerVariable.isWalking = false;
                }

                if (CamTrigger.isFontStage)
                {
                    rb.AddForce(new Vector2(-controller.PlayerB.Move.ReadValue<float>(), 0f) * movementAcceleration);

                    if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
                    {
                        rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
                    }
                }
                else
                {
                    rb.AddForce(new Vector2(controller.PlayerB.Move.ReadValue<float>(), 0f) * movementAcceleration);

                    if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
                    {
                        rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
                    }
                }
            }
        }
    }

    private void Jump()
    {
        if (canJump && !waitForFriend)
        {
            SoundManager.instance.Play(SoundManager.SoundName.Jump);
            PlayerVariable.isJumping = true;
            ApplyAirLinearDrag();
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void ApplyAirLinearDrag()
    {
        rb.drag = airLinearDrag;
    }

    private void CheckCollisions()
    {
        onGround = Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength,
            groundLayer);

        isPlayerOnFont = Physics2D.Raycast(transform.position - fontRaycastOffset, Vector2.right, fontRaycastLength,
            playerLayer);

        isPlayerOnBack = Physics2D.Raycast(transform.position - backRaycastOffset, Vector2.left, backRaycastLength,
            playerLayer);
        isPlayerOnUnder = Physics2D.Raycast(transform.position - underRaycastOffset, Vector2.right, underRaycastLength,
            playerLayer);
        isPlayerNearEdge = Physics2D.Raycast(transform.position - underRaycastOffset, Vector2.right, underRaycastLength,
            groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position - groundRaycastOffset,
            transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);
        //Font
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - fontRaycastOffset,
            transform.position - fontRaycastOffset + Vector3.right * fontRaycastLength);
        //Back
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - backRaycastOffset,
            transform.position - backRaycastOffset + Vector3.left * backRaycastLength);
        //Under
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position - underRaycastOffset,
            transform.position - underRaycastOffset + Vector3.right * underRaycastLength);
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }
}