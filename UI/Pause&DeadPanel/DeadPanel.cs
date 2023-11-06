using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeadPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        Reset_event.ResetEvent += Reset;
        dead_event.DeadEvent += Dead;
        transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Dead()
    {
        transform.gameObject.SetActive(true);
    }

    void Reset()
    {
        transform.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        Reset_event.ResetEvent -= Reset;
        dead_event.DeadEvent -= Dead;
    }
}

