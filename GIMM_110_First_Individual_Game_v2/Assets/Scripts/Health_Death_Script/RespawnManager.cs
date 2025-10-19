using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform player;
    private Vector3 spawnPoint;
    private Health playerHealth;


    [SerializeField] private float respawnDelay = 1f;


    private void Start()
    {
        if (player != null)
        {
            spawnPoint = player.position;
            playerHealth = player.GetComponent<Health>();
        }
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        //Reset player position
        player.position = spawnPoint;

        //Reactivate components, animations, etc.
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

        Movement2D movement = player.GetComponent<Movement2D>();
        if (movement != null)
        {
            movement.enabled = true;
        }

        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("respawn");
        }
    }
}
