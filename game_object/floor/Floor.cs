using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Floor : MonoBehaviour
{
    // Start is called before the first frame update
    int survial_time;
    int time;
    void Start()
    {
        time = 0;
        survial_time = Random.Range(3,6);
        timer_event.TimeUpEvent += TimeUp;
        Reset_event.ResetEvent += Reset;
    }

    // Update is called once per frame

    void TimeUp()
    {
        time += 1;
        if (time == survial_time)
        {
            transform.parent.GetComponent<FloorManager>().RemoveFloor(gameObject);
            Destroy(gameObject);
        }
        else if (time >= survial_time-1)
        {
            //change transparent to 0.5 and red
            GetComponent<SpriteRenderer>().color = new Color(1,0,0,0.5f);
        }

    }

    void Update() 
    {
     //check if have parent
    if (transform.parent == null)
    {
        transform.SetParent(GameObject.Find("FloorManager").transform);
    }

    }

    void Reset()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        timer_event.TimeUpEvent -= TimeUp;
        Reset_event.ResetEvent -= Reset;
    }

}
