using UnityEngine;

public class WallJumpState : IPlayerState, IPlayerPhysicsState
{
    private PlayerStateMachine machine;
    private Rigidbody2D rb;

    private float jumpVerticalForce;
    private float jumpHorizontalForce;
    private float duration;
    private float timer;
    private Vector2 initialVelocity;

    public WallJumpState(PlayerStateMachine machine, float verticalForce = 10f, float horizontalForce = 6f, float duration = 0.18f)
    {
        this.machine = machine;
        rb = machine.Rb;
        this.jumpVerticalForce = verticalForce;
        this.jumpHorizontalForce = horizontalForce;
        this.duration = duration;
    }

    public void Enter()
    {
        timer = 0f;
        float dir = machine.IsTouchingLeftWall ? 1f : -1f;
        initialVelocity = new Vector2(dir * jumpHorizontalForce, jumpVerticalForce);
        rb.linearVelocity = initialVelocity;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        machine.FlipSprite();

        if (timer >= duration)
        {
            machine.SwitchState(new FallState(machine));
            return;
        }
    }

    public void FixedUpdate()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(xInput * machine.airMoveSpeed, rb.linearVelocity.y);
    }

    public void Exit() { }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
