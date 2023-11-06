using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity_event : MonoBehaviour
{
    public class Event : UnityEngine.Events.UnityEvent<List<Vector2>> {};
    public static Event GravityEvent = new Event();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GravityMsg(List<Vector2> direction)
    {   
        Debug.Log("direction"+direction.ToString());
        GravityEvent?.Invoke(direction);
        Debug.Log("gravity msg");
    }
}
