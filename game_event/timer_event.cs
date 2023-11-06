using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class timer_event : MonoBehaviour
{   
    public static event System.Action TimeUpEvent;
    AudioSource audioSource ;
    // Start is called before the first frame update
    // this is a timer event
    // if timer is 0, broadcast event
    void Start()
    {
        Debug.Log("timer start");
        audioSource = GetComponent<AudioSource>();
    }

    public void TimeMsg(float time)
    {
        // Debug.Log("time is " + time);
        if (time <= 1.504f)
        {   
            Debug.Log("time is 0");
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            if (time<=0)
            {
                TimeUpEvent?.Invoke();
                audioSource.Stop();
            }
        }
        

    }

}
