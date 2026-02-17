using UnityEngine;

public class WallSlideState : IPlayerState, IPlayerPhysicsState
{
    private PlayerStateMachine machine;
    private Rigidbody2D rb;
    private float slideSpeed;
    private float stepTimer;
    private bool hasWallJumped;
    private bool lastWallOnLeft; // true if the last wall we jumped from was the left wall


    public WallSlideState(PlayerStateMachine machine, float slideSpeed = 1.5f)
    {
        this.machine = machine;
        rb = machine.Rb;
        this.slideSpeed = slideSpeed;
    }

    public void Enter()
    {
        stepTimer = 0f;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        hasWallJumped = false;

        lastWallOnLeft = machine.IsTouchingLeftWall;
    }

    public void Update()
    {
        machine.FlipSprite();

        if (machine.IsGrounded)
        {
            machine.SwitchState(new GroundedState(machine));
            return;
        }

        if (!machine.IsTouchingWall)
        {
            machine.SwitchState(new FallState(machine));
            return;
        }

        stepTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool touchingLeftWall = machine.IsTouchingLeftWall;
            bool touchingRightWall = machine.IsTouchingRightWall;

            // Only allow jump if the player hasn’t jumped from this wall yet
            // or if the wall side is opposite to the last jump
            if (!hasWallJumped || (touchingLeftWall != lastWallOnLeft && touchingRightWall != lastWallOnLeft))
            {
                machine.SwitchState(new WallJumpState(machine));
                hasWallJumped = true;
                lastWallOnLeft = touchingLeftWall; // track side we just jumped from
            }
        }

    }

    public void FixedUpdate()
    {
        stepTimer += Time.fixedDeltaTime;
        if (stepTimer >= machine.StepInterval)
        {
            stepTimer = 0f;
            rb.linearVelocity = new Vector2(0f, -slideSpeed);
        }
    }

    public void Exit() { }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
