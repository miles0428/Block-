using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Reset_event : MonoBehaviour
{
    // Declare variables here
    public static event System.Action ResetEvent;
    
    
    void Start()
    {
        // Code to run on start
        Debug.Log("reset event start");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetEvent?.Invoke();
        }
    }

    public void ResetEventMsg()
    {
        ResetEvent?.Invoke();
    }

    // Declare functions here
}
