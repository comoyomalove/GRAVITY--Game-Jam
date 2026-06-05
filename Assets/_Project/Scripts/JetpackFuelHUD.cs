using UnityEngine;
using UnityEngine.UI;

public class JetpackFuelHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] [Tooltip("Player movement component that stores jetpack fuel values.")]
    private NoraPlayerWallMovement2D playerMovement;

    [SerializeField] [Tooltip("Slider used to display current fuel visually.")]
    private Slider fuelSlider;

    [SerializeField] [Tooltip("Optional UI text used to display numeric fuel values.")]
    private Text fuelText;

    private void Update()
    {
        if (playerMovement == null)
        {
            return;
        }

        float currentFuel = playerMovement.CurrentFuel;
        float maxFuel = playerMovement.MaxFuel;

        if (fuelSlider != null)
        {
            fuelSlider.maxValue = maxFuel;
            fuelSlider.value = currentFuel;
        }

        if (fuelText != null)
        {
            fuelText.text = $"Fuel: {currentFuel:0}/{maxFuel:0}";
        }
    }
}
