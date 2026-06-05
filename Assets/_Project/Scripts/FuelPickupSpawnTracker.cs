using UnityEngine;

public class FuelPickupSpawnTracker : MonoBehaviour
{
    private FuelSpawner2D owner;

    public void Initialize(FuelSpawner2D spawner)
    {
        owner = spawner;
    }

    private void OnDestroy()
    {
        if (owner != null)
        {
            owner.NotifyPickupDestroyed();
        }
    }
}
