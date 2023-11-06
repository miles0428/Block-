using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class floor_event : MonoBehaviour
{
    // Start is called before the first frame update
    public static event System.Action CanPutEvent;
    public static event System.Action CanNotPutEvent;
    public class Vector2Event : UnityEngine.Events.UnityEvent<Vector2> {};

    public static Vector2Event PutFloorEvent = new Vector2Event();

    [SerializeField] float CDPutFloor = 0.15f;
    float CDPutFloorCount = 0.1f;
    void Start()
    {
        Reset_event.ResetEvent += Reset;
        pause_event.PauseEvent += Pause;
        pause_event.ResumeEvent += Resume;
        CDPutFloorCount = CDPutFloor;
    }

    // Update is called once per frame
    void Update()
    {
        CDPutFloorCount += Time.deltaTime;
    }

    public void CanPutMsg()
    {
        CanPutEvent?.Invoke();
    }

    public void CanNotPutMsg()
    {
        CanNotPutEvent?.Invoke();
    }

    public void PutFloorMsg(Vector2 position)
    {   
        if (CDPutFloorCount < CDPutFloor)
        {
            return;
        }
        if (GetComponent<BlockCount_event>().GetBlockCount() <= 0)
        {
            return;
        }
        Debug.Log("put floor msg");
        PutFloorEvent?.Invoke(position);
        GetComponent<BlockCount_event>().MinusBlockCountMsg();
        CDPutFloorCount = 0.0f;
    }

    void Reset()
    {
        CDPutFloorCount = CDPutFloor;
    }

    void Pause()
    {
        Time.timeScale = 0;
    }

    void Resume()
    {
        Time.timeScale = 1;
    }
}
