using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;           // Pooled arrow gameobjects (inactive by default)

    [Header("Firing")]
    [Tooltip("If true the trap will fire toward the player when triggered. Otherwise it fires in the trap's facing direction.")]
    [SerializeField] private bool aimAtPlayer = true;

    private float cooldownTimer;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    /// <summary>
    /// External call to trigger the trap (used by HybridTrapTrigger).
    /// </summary>
    public void ActivateTrap()
    {
        if (cooldownTimer >= attackCooldown)
        {
            Attack();
            cooldownTimer = 0f;
        }
        else
        {
            // Optional: you can queue or ignore extra activations during cooldown
            // Debug.Log("ArrowTrap on cooldown");
        }
    }

    private void Attack()
    {
        int idx = FindArrow();
        if (idx < 0)
        {
            Debug.LogWarning("ArrowTrap: no available arrow in pool.");
            return;
        }

        GameObject arrow = arrows[idx];
        arrow.transform.position = firePoint.position;

        // Determine firing direction
        Vector2 dir;
        if (aimAtPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                dir = (player.transform.position - firePoint.position).normalized;
            }
            else
            {
                // fallback to trap facing direction
                dir = transform.right * (transform.localScale.x >= 0 ? 1f : -1f);
            }
        }
        else
        {
            // Use localScale.x as facing (assumes sprite flips by scale)
            dir = transform.right * (transform.localScale.x >= 0 ? 1f : -1f);
        }

        // Ensure arrow is active then call the new ActivateProjectile(Vector2)
        arrow.SetActive(true);

        EnemyProjectile proj = arrow.GetComponent<EnemyProjectile>();
        if (proj != null)
        {
            proj.ActivateProjectile(dir);
        }
        else
        {
            Debug.LogError("ArrowTrap: pooled object missing EnemyProjectile component.");
        }

        // optional debug
        // Debug.Log($"ArrowTrap fired arrow idx={idx} dir={dir}");
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }

        return -1; // no free arrow
    }
}
