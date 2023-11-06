using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is a block generator
// it will generate block in a random position when timer is 0
public class Block_generator : MonoBehaviour
{   
    Vector2 generate_range_x;
    Vector2 generate_range_y;

    int count_timer = 0;

    //free gamezone
    
    // Start is called before the first frame update
    void Start()
    {
        timer_event.TimeUpEvent += GenerateBlockwithotPosition;
        Reset_event.ResetEvent += Reset;
        GameObject gamezone = GameObject.Find("GameZone");
        generate_range_y = new Vector2(gamezone.transform.position.y - gamezone.transform.localScale.y * .45f,
                                       gamezone.transform.position.y + gamezone.transform.localScale.y * .45f);
        generate_range_x = new Vector2(gamezone.transform.position.x - gamezone.transform.localScale.x * .45f,
                                       gamezone.transform.position.x + gamezone.transform.localScale.x * .45f);
        count_timer = 0;
        GenerateBlockwithotPosition();
    }

    void GenerateBlockwithotPosition()
    {   
        count_timer += 1;
        //generate block every 5 seconds the generate number follow the function n=5 * exp(-x/40) + 2 cos(x/10) exp(-x/40)
        int n = (int) (5 * Mathf.Exp(-count_timer/40) + 2 * Mathf.Cos(count_timer/10) * Mathf.Exp(-count_timer/40));
        if (n < 1)
        {
            n = 1;
        }
        for (int i = 0; i < n; i++)
        {
            GenerateBlock(GetGeneratePosition());
        }
    }
    internal Vector2 GetGeneratePosition()
    {   
        float x ;
        float y ;
        int maxiter = 100;
        int iter = 0;
        while (true)
        {
            iter += 1;
            x = Random.Range(generate_range_x[0], generate_range_x[1]);
            y = Random.Range(generate_range_y[0], generate_range_y[1]);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), 0.5f);

            if (colliders.Length == 1 && colliders[0].gameObject.name == "GameZone")
            {
                break;
            }
            else
            {
                Debug.Log("block overlap");
                Debug.Log(colliders[0].gameObject.name);
            }
            if (iter > maxiter)
            {
                Debug.Log("generate block failed");
                break;
            }

        }
        return new Vector2(x, y);
    }
    
    void GenerateBlock(Vector2 position)
    {   
        float x = position[0];
        float y = position[1];
        GameObject block = Instantiate(Resources.Load("block", typeof(GameObject))) as GameObject;
        block.transform.position = new Vector2(x, y);
        block.transform.parent = GameObject.Find("block_generator").transform;
        block.GetComponent<block>().random_position = true;
        Debug.Log("generate block successful on "+ new Vector2(x, y).ToString());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy() 
    {
        timer_event.TimeUpEvent -= GenerateBlockwithotPosition;
        Reset_event.ResetEvent -= Reset;
    }

    void Reset()
    {
        count_timer = 0;
        GenerateBlockwithotPosition();

    }
}
