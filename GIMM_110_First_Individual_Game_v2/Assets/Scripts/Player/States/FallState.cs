using UnityEngine;

public class FallState : IPlayerState, IPlayerPhysicsState
{
    private PlayerStateMachine machine;
    public Rigidbody2D rb;
    private Animator anim;

    public FallState(PlayerStateMachine machine)
    {
        this.machine = machine;
        rb = machine.Rb;
        anim = machine.GetComponent<Animator>();
    }

    public void Enter() { anim.SetBool("isFalling", true); }

    public void Update()
    {
        if (machine.IsGrounded)
        {
            machine.SwitchState(new GroundedState(machine));
            return;
        }

        if (machine.IsTouchingWall && rb.linearVelocity.y <= 0f)
        {
            machine.SwitchState(new WallSlideState(machine));
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && machine.HasDoubleJump)
        {
            Debug.Log("double jump!");
            machine.SwitchState(new DoubleJumpState(machine));
            return;
        }

        //if (Input.GetKeyDown(KeyCode.Space) && machine.HasDoubleJump)
        //{
        //    Debug.Log("double jump from fall state");
        //    machine.SwitchState(new DoubleJumpState(machine));
        //    return;
        //}
    }

    public void FixedUpdate()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        machine.FlipSprite();
        rb.linearVelocity = new Vector2(xInput * machine.airMoveSpeed, rb.linearVelocity.y);
    }

    public void Exit() { anim.SetBool("isFalling", false); }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
