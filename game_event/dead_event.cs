using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dead_event : MonoBehaviour
{   
    public static event System.Action DeadEvent;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("dead event start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DeadEventMsg()
    {
        DeadEvent?.Invoke();
    }
}
