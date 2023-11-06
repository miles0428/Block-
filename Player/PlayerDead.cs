using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDead : MonoBehaviour
{   
    // Declare variables here
    [SerializeField] EventSystem event_system;

    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        //get camera bounds
        Reset_event.ResetEvent += Reset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "GameZone")
        {
            if (!dead)
            {
                Debug.Log("player dead");
                //check if event system is destroyed
                if (event_system != null)
                {
                    event_system.GetComponent<dead_event>().DeadEventMsg();
                }
                else
                {
                    Debug.Log("event system is null");
                }
                dead = true;
            }
        }
    }

    void Reset()
    {
        dead = false;
    }

    void OnDestroy() 
    {
        Reset_event.ResetEvent -= Reset;
        
    }
}
