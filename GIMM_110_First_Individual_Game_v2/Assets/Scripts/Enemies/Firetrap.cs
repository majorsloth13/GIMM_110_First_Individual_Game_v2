using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay = 1f;
    [SerializeField] private float activeTime = 2f;

    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; // when trap gets triggered
    private bool active;    // when trap is active and can hurt the player

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Ensure continuous damage if player stays in fire while active
        TryDamage(collision);
    }

    private void TryDamage(Collider2D collision)
    {
        if (collision.CompareTag("Player") && active)
        {
            collision.GetComponent<Health>()?.TakeDamage(damage);
            Debug.Log($"Firetrap damaged player for {damage}");
        }
    }

    // Exposed method to let HybridTrapTrigger activate this trap
    public void ActivateTrap()
    {
        if (!triggered)
            StartCoroutine(ActivateFiretrap());
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);

        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);

        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
