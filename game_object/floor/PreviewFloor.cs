using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewFloor : MonoBehaviour
{
    bool canput = false;
    [SerializeField] EventSystem floor_event_system;
    // Start is called before the first frame update
    void Start()
    {
        floor_event.CanPutEvent += CanPut;
        floor_event.CanNotPutEvent += CanNotPut;

        canput = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, GameObject.Find("player").transform.position);
        // Debug.Log("distance"+distance.ToString());
        if (distance < 0.7f)
        {
            canput = false;
        }
        GetComponent<Renderer>().enabled = canput;

        if (Input.GetKey(KeyCode.E)){
            if (canput)
            {
                floor_event_system.GetComponent<floor_event>().PutFloorMsg(transform.position);
            }
        }
    }




    void CanPut()
    {
        canput = true;
        // GetComponent<Renderer>().enabled = true;
    }

    void CanNotPut()
    {
        canput = false;
        // GetComponent<Renderer>().enabled = false;
        // Debug.Log("can not put");
    }

    void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.name == "player")
        {
            canput= false;
            // Debug.Log("trigger stay");
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "player")
        {
            canput = false;
            // Debug.Log("trigger enter");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "player")
        {
            canput = true;
            Debug.Log("trigger exit");
        }
    }

    void OnDestroy() 
    {
        floor_event.CanPutEvent -= CanPut;
        floor_event.CanNotPutEvent -= CanNotPut;
    }
}
