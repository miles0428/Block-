using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
public class block : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] EventSystem event_system;
    [SerializeField] int deactive_time = 1;
    [SerializeField] Block_generator block_generator;
    int survival_time;

    internal bool random_position = false;

     

    void Start()
    {   
        event_system = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        survival_time = 0;
        timer_event.TimeUpEvent += UpdateSurvivalTime;
        Reset_event.ResetEvent += Reset;
    }

    void Update()
    {

    }

    void UpdateSurvivalTime()
    {
        survival_time += 1;
        if (survival_time >= deactive_time)
        {
            Destroy(gameObject);
        }
    }

    void Reset()
    {
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {   
        Debug.Log("block trigger");
        //play sound
        event_system.GetComponent<BlockCount_event>().AddBlockCountMsg();
        event_system.GetComponent<Audioevent>().GetBlockMsg();
        Destroy(gameObject);
    }

    void OnDestroy() 
    {
        timer_event.TimeUpEvent -= UpdateSurvivalTime;
        Reset_event.ResetEvent -= Reset;
    }
}
