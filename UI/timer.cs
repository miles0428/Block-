using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//textmeshpro
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] EventSystem event_system;
    [SerializeField] float time_change_gravity = 5.0f;
    float time ;
    bool dead = false;
    
    // Start is called before the first frame update
    void Start()
    {   

        time = time_change_gravity;
        //set text to time
        GetComponent<TextMeshProUGUI>().text = TimeToString(time);
        timer_event.TimeUpEvent += Reset;
        Reset_event.ResetEvent += Reset;
        dead_event.DeadEvent += Dead;
        pause_event.PauseEvent += Pause;
        pause_event.ResumeEvent += Resume;
        GetComponent<TextMeshProUGUI>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {   
        if (dead)
        {
            return;
        }
        //reduce time
        time -= Time.deltaTime;
        //set text to time
        event_system.GetComponent<timer_event>().TimeMsg(time);
        GetComponent<TextMeshProUGUI>().text = TimeToString(time);
    }

    void Reset()
    {
        time = time_change_gravity;
        GetComponent<TextMeshProUGUI>().text = TimeToString(time);
        gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
        Time.timeScale = 1;
        dead = false;
    }

    string TimeToString(float time)
    {   
        time +=1;
        string time_str = time.ToString();
        if (time_str.IndexOf(".") != -1)
        {
            time_str = time_str.Substring(0, time_str.IndexOf("."));
        }
        return time_str;
    }

    void OnDestroy() 
    {
        timer_event.TimeUpEvent -= Reset;
        Reset_event.ResetEvent -= Reset;
        dead_event.DeadEvent -= Dead;
        pause_event.PauseEvent -= Pause;
        pause_event.ResumeEvent -= Resume;
        
    }

    void Dead()
    {
        GetComponent<TextMeshProUGUI>().enabled = false;
        dead = true;
    }

    void Pause()
    {
    Time.timeScale = 0;
    }

    void Resume()
    {
    Time.timeScale = 1;
    }


}
