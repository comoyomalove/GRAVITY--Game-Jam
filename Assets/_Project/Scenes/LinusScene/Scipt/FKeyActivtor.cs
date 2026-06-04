using UnityEngine;

public class FKeyActivator : MonoBehaviour
{
    
    void Update()
    
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateFunction();
        }
    }

    void ActivateFunction()
    {
        Debug.Log("F key pressed!");

        // Put your activation code here
    }
}