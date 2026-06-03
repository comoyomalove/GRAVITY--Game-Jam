using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [Header("Respawn Point")]
    [SerializeField] [Tooltip("If enabled, this point can be used as a valid player respawn location.")]
    private bool isActive = true;

    public bool IsActive => isActive;
}
