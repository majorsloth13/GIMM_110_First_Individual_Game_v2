using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Movement Speeds (units/second)")]
    public float verticalSpeed = 10f;
    public float horizontalSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float airMoveSpeed = 8f;

    [Header("Movement Step Rate")]
    [Tooltip("How many movement steps per second (12 ≈ slow, visible stepping).")]
    public float movementFps = 12f;
    public float StepInterval => 1f / movementFps;

    [Header("Environment Checks")]
    public GroundCheck GroundCheck;
    public WallCheck WallCheck;

    public Rigidbody2D Rb;

    [Header("Coyote & Buffer")]
    [Tooltip("How long after leaving ground the player can still jump.")]
    public float coyoteTime = 0.12f;
    [Tooltip("How long a jump press is buffered before landing.")]
    public float jumpBufferTime = 0.12f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSoundClip;

    /*[Header("Drop Through Platform")]
    [SerializeField] private float dropDuration = 0.25f;
    private bool isDropping = false;*/
    //[SerializeField]private float dropForce = 10f;

    internal float coyoteTimer = 0f;
    internal float jumpBufferTimer = 0f;

    //public LayerMask enemyLayer;

    // states
    private IPlayerState currentState;
    public IPlayerState previousState;

    // public flags
    public bool HasDoubleJump = true;   // rset when landing
    public bool isIdle = false;

    private GroundCheck groundCheck;

    //public LayerMask dropMask;

    //internal PlatformEffector2D currentEffector = null;
    //internal Collider2D currentPlatformCollider = null;

    // Helpers (exposed for states)
    public bool IsGrounded => GroundCheck != null && GroundCheck.IsGrounded();
    public bool IsTouchingWall => WallCheck != null && WallCheck.IsTouchingWall;
    public bool IsTouchingLeftWall => WallCheck != null && WallCheck.IsTouchingLeftWall;
    public bool IsTouchingRightWall => WallCheck != null && WallCheck.IsTouchingRightWall;

    //public IPlayerState FallState;
    //public IPlayerState WallSlideState;
    //public IPlayerState DoubleJumpState;
    //public IPlayerState GroundedState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Rb == null) Rb = GetComponent<Rigidbody2D>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        SwitchState(new GroundedState(this)); // start inside grounded parent
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current State: " + currentState?.GetType().Name);

        if (!IsGrounded)
        {

            coyoteTimer -= Time.deltaTime;
        }
        else
        {

            coyoteTimer = coyoteTime; // rest whenever grounded
        }


        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // capture jump input (buffer)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = jumpBufferTime;
        }

        void FixedUpdate()
        {
            if (IsGrounded)
            {
                Rb.gravityScale = 3f;
            }

            // Let the state apply physics ONLY here
            if (currentState is IPlayerPhysicsState physState)
            {
                physState.FixedUpdate();
            }

        }
    }

    /*public IEnumerator DropRoutine()
    {
        if (currentPlatformCollider == null) yield break;

        // 1. CAPTURE the platform in a local variable
        Collider2D platformToFix = currentPlatformCollider;
        Collider2D playerCollider = GetComponent<Collider2D>();

        isDropping = true;

        // 2. Use the local variable to ignore
        Physics2D.IgnoreCollision(playerCollider, platformToFix, true);

        Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, -5f);

        yield return new WaitForSeconds(dropDuration);

        // 3. Use the local variable to re-enable
        // This works even if machine.currentPlatformCollider was set to null!
        if (platformToFix != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platformToFix, false);
        }

        isDropping = false;

        // Only clear these if they haven't been changed by a new platform
        if (currentPlatformCollider == platformToFix)
        {
            currentPlatformCollider = null;
            currentEffector = null;
        }

        // Only switch to fall if we are actually in the air
        if (!IsGrounded)
        {
            SwitchState(new FallState(this));
        }
    }*/

    public void SwitchState(IPlayerState newState)
    {
        if (newState == null)
        {
            Debug.LogWarning("SwitchState called with null!");
            return;
        }

        currentState?.Exit();
        previousState = currentState; // << assign previous
        currentState = newState;
        currentState.Enter();
    }


    // Called by states to check & consume buffered jump
    public bool TryConsumeJumpBuffer()
    {
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer = 0f;
            return true;
        }
        return false;
    }

    // Called by states to check if coyote time still allows jump
    public bool IsCoyoteAvailable()
    {
        return coyoteTimer > 0f;
    }

    public void FlipSprite()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        if (xInput > 0.1f)
        {
            transform.localScale = new Vector3(2f, 2f, 1f);
        }
        else if (xInput < -0.1f)
        {
            transform.localScale = new Vector3(-2f, 2f, 1f);
        }
    }


    //Accessors used by state objects (encapulates internal details)
    public Transform GetTransform() => transform;
    public float GetVerticalSpeed() => verticalSpeed;
    public float GetHorizontalSpeed() => horizontalSpeed;
    //public Rigidbody2D GetRb() => GetComponent<Rigidbody2D>();
    public Rigidbody2D GetRb() => Rb;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState is IPlayerPhysicsState physState)
        {
            physState.OnCollisionEnter2D(collision);
        }
    }

    // Update your existing OnCollisionEnter2D or add these:
    /*private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if what we are touching is on the "Drop" layer
        // (Ensure your platforms are on the layer you assigned to dropMask)
        if (((1 << collision.gameObject.layer) & dropMask) != 0)
        {
            currentPlatformCollider = collision.collider;
            currentEffector = collision.gameObject.GetComponent<PlatformEffector2D>();
        }
    }*/

    /*private void OnCollisionExit2D(Collision2D collision)
    {
        // Clear the references when we leave the platform
        if (collision.collider == currentPlatformCollider)
        {
            currentPlatformCollider = null;
            currentEffector = null;
        }
    }*/

}

