using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BlockUI : MonoBehaviour
{   
    [SerializeField] EventSystem event_system;
    // Start is called before the first frame update
    void Start()
    {   
        GetComponent<SpriteRenderer>().enabled = true;
        Reset_event.ResetEvent += Reset;
        dead_event.DeadEvent += Dead;
    }

    // Update is called once per frame

    void Reset()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void Dead()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnDestroy()
    {
        Reset_event.ResetEvent -= Reset;
        dead_event.DeadEvent -= Dead;
    }
}
