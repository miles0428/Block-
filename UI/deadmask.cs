using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadmask : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        dead_event.DeadEvent += Dead;
        Reset_event.ResetEvent += Reset;
        pause_event.PauseEvent += Dead;
        pause_event.ResumeEvent += Reset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Dead()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void Reset()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnDestroy()
    {
        dead_event.DeadEvent -= Dead;
        Reset_event.ResetEvent -= Reset;
        pause_event.PauseEvent -= Dead;
        pause_event.ResumeEvent -= Reset;
    }
}
