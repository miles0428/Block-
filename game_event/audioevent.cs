using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioevent : MonoBehaviour
{
    // Start is called before the first frame update
    public class Event : UnityEngine.Events.UnityEvent<AudioClip> {};
    public static Event AudioEvent = new Event();
    [SerializeField] AudioClip getblock;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetBlockMsg()
    {   
        AudioEvent?.Invoke(getblock);
    }
        
}
