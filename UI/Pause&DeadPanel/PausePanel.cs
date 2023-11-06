using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PausePanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        pause_event.PauseEvent += Pause;
        pause_event.ResumeEvent += Resume;
        Reset_event.ResetEvent += Reset;
        transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Pause()
    {
        transform.gameObject.SetActive(true);
    }

    void Resume()
    {
        transform.gameObject.SetActive(false);
    }

    void Reset()
    {
        transform.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        pause_event.PauseEvent -= Pause;
        pause_event.ResumeEvent -= Resume;
        Reset_event.ResetEvent -= Reset;
    }
}

