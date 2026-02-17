/*using UnityEngine;

public class RunState : IPlayerState, IPlayerPhysicsState
{
    private PlayerStateMachine machine;
    private Rigidbody2D rb;
    private float speed;
    
    private Animator anim;

    public RunState(PlayerStateMachine machine)
    {
        this.machine = machine;
        rb = machine.Rb;
        speed = machine.GetHorizontalSpeed();
        
        anim = machine.GetComponent<Animator>();
    }

    public void Enter() { anim.SetBool("isRunning", true);
       
    }

    public void Update()
    {
        
        

        if (!machine.IsGrounded)
        {
            machine.SwitchState(new FallState(machine));
            return;
        }

        float input = Input.GetAxisRaw("Horizontal");
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float playerX = machine.GetTransform().position.x;

        // Determine if walking away
        bool walkingAway = (input > 0 && mouseWorld.x < playerX) || (input < 0 && mouseWorld.x > playerX);

        // Update animator parameters
        anim.SetBool("isRunning", !walkingAway && Mathf.Abs(input) > 0.01f);
        anim.SetBool("isWalkingAway", walkingAway && Mathf.Abs(input) > 0.01f);
        machine.FlipToGunDirection();
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
            machine.SwitchState(new IdleState(machine));
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || machine.TryConsumeJumpBuffer())
        {
            Debug.Log("got jump from run");
            machine.SwitchState(new JumpState(machine));
            return;
        }

       
    }


    public void FixedUpdate()
    {
        float input = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);
        
    }

    public void Exit() { anim.SetBool("isRunning", false); }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
}*/
