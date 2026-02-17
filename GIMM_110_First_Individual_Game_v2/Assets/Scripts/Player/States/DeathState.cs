using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class DeathState : IPlayerState
{
    private PlayerStateMachine machine;
    private Rigidbody2D rb;
    private Animator anim;
    private RespawnManager respawn;

    public DeathState(PlayerStateMachine machine)
    {
        this.machine = machine;
        rb = machine.Rb;
        anim = machine.GetComponent<Animator>();
        respawn = machine.GetComponent<RespawnManager>();
        if (respawn == null)
        {
            respawn = Object.FindAnyObjectByType<RespawnManager>();
        }
    }

    public void Enter()
    {
        rb.linearVelocity = Vector2.zero;

        if (machine.audioSource != null && machine.deathSoundClip != null)
        {
            machine.audioSource.PlayOneShot(machine.deathSoundClip);
        }

        /*if (anim != null)
        {
            anim.SetBool("isHurt", true);
            //anim.SetTrigger("hurt");
        }*/

        machine.StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        rb.simulated = false;

        if (respawn != null)
        {
            // This is the magic line: it waits for the RespawnManager timer to finish
            yield return machine.StartCoroutine(respawn.RespawnCoroutine());
        }

        rb.simulated = true;
        //respawn.RespawnPlayer();
        machine.SwitchState(new GroundedState(machine));

        
      
       
        //rb.simulated = true; // Turn physics back on
       
    }


    public void Update() { }
    public void Exit()
    {
        if (anim != null)
        {
            // 1. Force the animator back to the Idle Anim state immediately
            anim.Play("Idle Anim", 0, 0f);

            // 2. Clear any lingering parameters
            anim.SetBool("isHurt", false);
            //anim.SetBool("isIdle", true);

            // ResetTrigger only works if the transition hasn't happened yet.
            // To be safe, clear it here so it doesn't fire later.
            //anim.ResetTrigger("hurt");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision){}
}
