using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] EventSystem pause_event_system;
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Resume);
    }

    // Update is called once per frame
    void Resume()
    {
        pause_event_system.GetComponent<pause_event>().ResumeEventMsg();
        Debug.Log("resume button clicked");
    }
}
