using UnityEngine;

public class LegacyFuelStore : MonoBehaviour, IPlayerFuelReceiver
{
    [Header("Fuel")]
    [SerializeField] [Tooltip("Current fuel amount for legacy setups.")]
    [Min(0f)]
    private float currentFuel = 100f;

    [SerializeField] [Tooltip("Maximum fuel amount for legacy setups.")]
    [Min(0f)]
    private float maxFuel = 100f;

    public void RestoreFullFuel()
    {
        currentFuel = maxFuel;
    }

    public void SetFuelToMax()
    {
        currentFuel = maxFuel;
    }
}
