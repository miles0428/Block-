using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Home);
    }

    // Update is called once per frame
    void Home()
    {
        Debug.Log("HomeButton");
        //change scene
        SceneManager.LoadScene("HomePage");
    }

}
