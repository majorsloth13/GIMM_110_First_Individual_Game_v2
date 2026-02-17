using UnityEngine;

public class JumpState : IPlayerState, IPlayerPhysicsState
{
    private PlayerStateMachine machine;
    private Rigidbody2D rb;
    private float jumpForce = 5;
    private Animator anim;

    public JumpState(PlayerStateMachine machine)
    {
        this.machine = machine;
        rb = machine.Rb;
        jumpForce = machine.jumpForce;
        anim = machine.GetComponent<Animator>();
    }

    public void Enter()
    {
        Debug.Log("jumped");
        anim.SetBool("isjumping", true);
        // Reset vertical velocity
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // Directly set the upward velocity to make the player jump
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, machine.jumpForce); // use jumpHeight instead of jumpForce

    }

    public void Update()
    {

        // --- Always check double jump first ---
        if (Input.GetKeyDown(KeyCode.Space) && machine.HasDoubleJump)
        {
            Debug.Log("double jump!");
            machine.SwitchState(new DoubleJumpState(machine));
            return;
        }

        if (rb.linearVelocity.y <= 0f)
        {
            machine.SwitchState(new FallState(machine));
            return;
        }

        if (machine.IsTouchingWall && rb.linearVelocity.y <= 0f)
        {
            machine.SwitchState(new WallSlideState(machine));
            return;
        }
    }


    public void FixedUpdate()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(xInput * machine.airMoveSpeed, rb.linearVelocity.y);
    }

    public void Exit() { Debug.Log("JumpState ended");
        anim.SetBool("isjumping", false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
    
    
}
