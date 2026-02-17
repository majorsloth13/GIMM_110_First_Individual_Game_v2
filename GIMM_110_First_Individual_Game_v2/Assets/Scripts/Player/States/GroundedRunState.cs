using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GroundedRunState : IGroundedSubState//, IPlayerPhysicsState
{
    private PlayerStateMachine machine;
    private GroundedState parent;
    private Rigidbody2D rb;
    //private float speed;
    private Animator anim;
    private float speed;

    public GroundedRunState(PlayerStateMachine machine, GroundedState parent)
    {
        this.machine = machine;
        this.parent = parent;
        rb = machine.Rb;
        //speed = machine.GetHorizontalSpeed();
        anim = machine.GetComponent<Animator>();
        speed = machine.GetHorizontalSpeed();
    }

    public void Enter()
    {
        Debug.Log("entered grounded run");
        anim.SetBool("isRunning", true);
        anim.SetBool("isFalling", false);
    }

    public void Update()
    {

        /* if (!machine.IsGrounded)
         {
             machine.SwitchState(new FallState(machine));
             return;
         }*/

        float input = Input.GetAxisRaw("Horizontal");
        machine.FlipSprite();

        if (Mathf.Abs(input) < 0.01f)
        {
            parent.SetSubState(new GroundedIdleState(machine, parent));
            return;
            //machine.SwitchState(new IdleState(machine));
            //return;
        }

        // walking-away logic (optional)
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float playerX = machine.GetTransform().position.x;

        bool walkingAway =
            (input > 0 && mouseWorld.x < playerX) ||
            (input < 0 && mouseWorld.x > playerX);

        anim.SetBool("isRunning", !walkingAway && Mathf.Abs(input) > 0.01f);
        anim.SetBool("isWalkingAway", walkingAway && Mathf.Abs(input) > 0.01f);
        machine.FlipSprite();
        if (walkingAway)
        {
            speed = 6;
        }
        else
        {
            speed = machine.GetHorizontalSpeed();
        }
        if (Mathf.Abs(input) < 0.01f)
        {
            machine.SwitchState(new GroundedState(machine));
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || machine.TryConsumeJumpBuffer())
        {
            Debug.Log("got jump from run");
            machine.SwitchState(new JumpState(machine));
            return;

            /* if (Input.GetKeyDown(KeyCode.Space) || machine.TryConsumeJumpBuffer())
             {
                 Debug.Log("got jump from run");
                 machine.SwitchState(new JumpState(machine));
                 return;
             }*/

            // Double jump should never trigger on ground, but we still check consistently
            //if (Input.GetKeyDown(KeyCode.Space) && machine.HasDoubleJump)
            //{
            //    machine.SwitchState(new DoubleJumpState(machine));
            //    return;
            //}
        }
    }


    public void FixedUpdate()
    {
        float input = Input.GetAxisRaw("Horizontal");
        float speed = machine.GetHorizontalSpeed();

        rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);
        
    }

    public void Exit() 
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalkingAway", false);
    }
}
