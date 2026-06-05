using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FuelPickup : MonoBehaviour
{
    [Header("Fuel Pickup")]
    [SerializeField] [Tooltip("How much fuel this pickup restores to the player's jetpack.")]
    [Min(0f)]
    private float fuelRestoreAmount = 20f;

    [SerializeField] [Tooltip("Tag used to identify the player object.")]
    private string playerTag = "Player";

    [SerializeField] [Tooltip("If enabled, this pickup destroys itself after being collected.")]
    private bool destroyOnCollect = true;

    private void Reset()
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            collider2D.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        NoraPlayerWallMovement2D player = other.GetComponent<NoraPlayerWallMovement2D>();
        if (player != null)
        {
            player.AddFuel(fuelRestoreAmount);
        }

        if (destroyOnCollect)
        {
            Destroy(gameObject);
        }
    }
}
