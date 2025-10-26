using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to collectible prefab (DarkMatter prefab). Requires a Collider2D set to "Is Trigger".
/// On player trigger it notifies the CoinCollector and ScoreCounter, then destroys itself.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class MatterPickup : MonoBehaviour
{
    // The data object representing this pickup.
    // Option A: add a specific Matter-derived component on the prefab (recommended)
    // Option B: create it here (we create DarkMatter by default).
    private Matter matterData;

    private void Awake()
    {
        // If you already have a Matter-derived component attached to the prefab, use that:
        Matter existing = GetComponent<Matter>();
        if (existing != null)
        {
            matterData = existing;
            return;
        }

        // Otherwise create a DarkMatter instance (or change to suit)
        matterData = new DarkMatter();
        matterData.SetValue(1); // default value (adjust in DarkMatter if needed)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Find a collector in the scene (make sure CoinCollector exists somewhere)
        MatterCollector collector = Object.FindFirstObjectByType<MatterCollector>();
        if (collector != null)
        {
            collector.AddMatter(new List<Matter> { matterData });
        }
        else
        {
            Debug.LogWarning("MatterPickup: No CoinCollector found in scene.");
        }

        // Update static UI counter if you use ScoreCounter.matterAmount
        ScoreCounter.matterAmount += matterData.GetValue();

        Debug.Log($"Collected {matterData.GetType().Name} (value {matterData.GetValue()}). Total: {ScoreCounter.matterAmount}");

        // Destroy pickup gameobject
        Destroy(gameObject);
    }
}
