using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockCount_event : MonoBehaviour
{   
    public class BlockCountEvent : UnityEngine.Events.UnityEvent<int> {};
    public static BlockCountEvent BlockCountEventMsg = new BlockCountEvent();
    // Start is called before the first frame update
    // public static event System.Action AddScoreEvent;
    int count ;
    void Start()
    {
        Debug.Log("block count event start");
        count = 5;
        Reset_event.ResetEvent += Reset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddBlockCountMsg()
    {   
        count += 1;
        Debug.Log("block count is " + count);
        BlockCountEventMsg?.Invoke(count);
    }

    public void MinusBlockCountMsg()
    {   
        count -= 1;
    }

    public int GetBlockCount()
    {
        return count;
    }

    void Reset()
    {
        count = 5;
        BlockCountEventMsg?.Invoke(count);
    }
}
