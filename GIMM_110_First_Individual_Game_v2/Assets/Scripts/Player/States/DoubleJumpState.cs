using UnityEngine;

public class DoubleJumpState : IPlayerState, IPlayerPhysicsState
{
    private PlayerStateMachine machine;
    private Rigidbody2D rb;
    private float jumpForce;
    private Animator anim;

    public DoubleJumpState(PlayerStateMachine machine)
    {
        this.machine = machine;
        rb = machine.Rb;
        jumpForce = machine.jumpForce;
        anim = machine.GetComponent<Animator>();
    }

    public void Enter()
    {
        anim.SetBool("isjumping", true);
        Debug.Log("doubleJump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        machine.HasDoubleJump = false;
    }

    public void Update()
    {
        
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
        machine.FlipSprite();
        rb.linearVelocity = new Vector2(xInput * machine.airMoveSpeed, rb.linearVelocity.y);
    }

    public void Exit() { anim.SetBool("isjumping", false); }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
