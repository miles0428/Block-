using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Score_event : MonoBehaviour
{   
    public class IntEvent : UnityEngine.Events.UnityEvent<int> {};
    public static IntEvent AddScoreEvent = new IntEvent();
    int score;
    // Start is called before the first frame update
    void Start()
    {
        BlockCount_event.BlockCountEventMsg.AddListener(AddScoreMsg);
        Reset_event.ResetEvent += Reset;
        timer_event.TimeUpEvent += AddScoreMsg;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("score is " + score);
    }

    void AddScoreMsg(int count)
    {   
        score += 100;
        AddScoreEvent?.Invoke(score);
    }
    void AddScoreMsg()
    {   
        score += 50;
        AddScoreEvent?.Invoke(score);
    }

    void Reset()
    {   
        score = 0;
        AddScoreEvent?.Invoke(score);
    }
}
