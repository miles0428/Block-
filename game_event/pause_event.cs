using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause_event : MonoBehaviour
{   
    public static event System.Action PauseEvent;
    public static event System.Action ResumeEvent;
    bool dead ;
    bool pause;
    // Start is called before the first frame update
    void Start()
    {
        dead_event.DeadEvent += Dead;
        Reset_event.ResetEvent += Reset;
        dead = false;
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !dead)
        {
            if (!pause)
            {
                PauseEvent?.Invoke();
                pause = true;
                Debug.Log("pause");
            }
            else
            {
                ResumeEvent?.Invoke();
                pause = false;
                Debug.Log("resume");
            }
        }
    }

    public void ResumeEventMsg()
    {
        ResumeEvent?.Invoke();
        pause = false;
    }

    void Dead()
    {
    dead = true;
    }

    void Reset()
    {
    dead = false;
    pause = false;
    }

    void OnDestroy() 
    {
        dead_event.DeadEvent -= Dead;
        Reset_event.ResetEvent -= Reset;
    }


}
