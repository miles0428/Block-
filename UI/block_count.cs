using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BlockCount : MonoBehaviour
{   
    [SerializeField] EventSystem event_system;
    [SerializeField] int count = 0;
    // Start is called before the first frame update
    void Start()
    {   
        GetComponent<TextMeshProUGUI>().text = count.ToString();
        BlockCount_event.BlockCountEventMsg.AddListener(UpdateCount);
        GetComponent<TextMeshProUGUI>().enabled = true;
        Reset_event.ResetEvent += Reset;
        dead_event.DeadEvent += Dead;
    }

    // Update is called once per frame
    void Update()
    {   
        this.count = event_system.GetComponent<BlockCount_event>().GetBlockCount();
        GetComponent<TextMeshProUGUI>().text = this.count.ToString();
    }

    void UpdateCount(int count)
    {
        this.count = count;
        GetComponent<TextMeshProUGUI>().text = count.ToString();
    }

    void Reset()
    {
        GetComponent<TextMeshProUGUI>().text = count.ToString();
        GetComponent<TextMeshProUGUI>().enabled = true;
    }

    void Dead()
    {
        GetComponent<TextMeshProUGUI>().enabled = false;
    }

    void OnDestroy()
    {
        BlockCount_event.BlockCountEventMsg.RemoveListener(UpdateCount);
        Reset_event.ResetEvent -= Reset;
        dead_event.DeadEvent -= Dead;
    }
}
