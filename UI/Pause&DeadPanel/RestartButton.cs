using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    [SerializeField] EventSystem reset_event_system;
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Restart);
    }

    public void  Restart()
    {
        reset_event_system.GetComponent<Reset_event>().ResetEventMsg();
        Debug.Log("restart button clicked");
    }

}
