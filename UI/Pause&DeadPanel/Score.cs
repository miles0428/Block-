using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    int score;

    void Start()
    {
        score = 0;
        Reset_event.ResetEvent += Reset;
        Score_event.AddScoreEvent.AddListener(AddScore);
        dead_event.DeadEvent += Dead;
    }

    void Reset()
    {
        Debug.Log("Reset score");
        score = 0;
    }

    void AddScore(int score)
    {
        this.score = score;
        Debug.Log("AddScore");
    }

    void Dead()
    {
        GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    void OnDestroy()
    {
        Reset_event.ResetEvent -= Reset;
        Score_event.AddScoreEvent.RemoveListener(AddScore);
        dead_event.DeadEvent -= Dead;
    }

}
