using UnityEngine;
using Sonity; 

public class FKeyActivator : MonoBehaviour
{
    
    void Update()
    
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateFunction();
        }
    }
    
    public SoundEvent soundEventShot;
    private void SoundF()
    {
        soundEventShot.Play(transform);
    }

    void ActivateFunction()
    {
        Debug.Log("F key pressed!");

        // Put your activation code here
    }
}