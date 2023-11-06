using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using math = System.Math;

public class FloorManager : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 generate_range_x;
    Vector2 generate_range_y;

    int maxiter = 100;
    public Dictionary<GameObject,Vector2> FloorMap = new();
    Vector2 gravity_direction;

    int round = 0;

    void Start()
    {   
        gravity_direction = new Vector2(0,-1);
        foreach (Transform child in transform)
        {
            RemoveFloor(child.gameObject);
            Destroy(child.gameObject);
        }
        transform.position = transform.parent.position;
        GameObject gamezone = GameObject.Find("GameZone");
        generate_range_y = new Vector2(gamezone.transform.position.y - gamezone.transform.localScale.y * .45f,
                                       gamezone.transform.position.y + gamezone.transform.localScale.y * .45f);
        generate_range_x = new Vector2(gamezone.transform.position.x - gamezone.transform.localScale.x * .45f,
                                        gamezone.transform.position.x + gamezone.transform.localScale.x * .45f);
        GenerateNewFloor((Vector2)new(MathF.Round(GameObject.Find("GameZone").transform.position.x),
                                      MathF.Round(GameObject.Find("GameZone").transform.position.y-2)));        
        GenerateNewFloor(5);
        Reset_event.ResetEvent += Reset;
        floor_event.PutFloorEvent.AddListener(GenerateNewFloor);
        Gravity_event.GravityEvent.AddListener(change_gravity_direction);
        timer_event.TimeUpEvent += GenerateNewFloor_Timer;
        round = 0;
    }

    void AddFloor(GameObject floor)
    {
        FloorMap.Add(floor,floor.transform.position);
        if (gravity_direction == new Vector2(0,1) || gravity_direction == new Vector2(0,-1))
        {
            CombineCollider_x();
        }
        else
        {
            CombineCollider_y();
        }
    }

    internal void RemoveFloor(GameObject floor)
    {
        FloorMap.Remove(floor);
        if (gravity_direction == new Vector2(0,1) || gravity_direction == new Vector2(0,-1))
        {
            CombineCollider_x();
        }
        else
        {
            CombineCollider_y();
        }
    }

    void GenerateNewFloor()
    {
        Vector2 generate_position = new Vector2(MathF.Round(Random.Range(generate_range_x.x,generate_range_x.y)),
                                                MathF.Round(Random.Range(generate_range_y.x,generate_range_y.y)));
        int iter = 0;
        while (FloorMap.ContainsValue(generate_position))
        {   
            iter += 1;
            if (iter > maxiter)
            {
                break;
            }
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(generate_position.x, generate_position.y), 0.5f);
            if (colliders.Length == 1 && colliders[0].gameObject.name == "GameZone")
            {
                break;
            }
            generate_position = new Vector2(MathF.Round(Random.Range(generate_range_x.x,generate_range_x.y)),
                                            MathF.Round(Random.Range(generate_range_y.x,generate_range_y.y)));
        }
        GenerateNewFloor(generate_position);
    }

    void GenerateNewFloor(Vector2 position)
    {
        GameObject new_floor = Instantiate(Resources.Load("floor")) as GameObject;
        new_floor.transform.position = position;
        new_floor.transform.SetParent(transform);
        AddFloor(new_floor);
    }

    void GenerateNewFloor(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GenerateNewFloor();
        }
    }

    void Reset()
    {
        foreach (Transform child in transform)
        {
            RemoveFloor(child.gameObject);
            Destroy(child.gameObject);
        }
        GenerateNewFloor((Vector2)new(MathF.Round(GameObject.Find("GameZone").transform.position.x),
                                      MathF.Round(GameObject.Find("GameZone").transform.position.y-2)));
        GenerateNewFloor(5);
        gravity_direction = new Vector2(0,-1);
        round=0;
    }

    void OnDestroy()
    {
        Reset_event.ResetEvent -= Reset;
        floor_event.PutFloorEvent.RemoveListener(GenerateNewFloor);
        Gravity_event.GravityEvent.RemoveListener(change_gravity_direction);
        timer_event.TimeUpEvent -= GenerateNewFloor_Timer;
    }

    void GenerateNewFloor_Timer()
    {   
        round += 1;
        if (round % 5 == 0)
        {
            GenerateNewFloor(3);
        }
        else
        {
            GenerateNewFloor(Random.Range(0,2));
        }
    }

    void CombineCollider_x()
    {   
        foreach(KeyValuePair<GameObject,Vector2> kvpair in FloorMap)
        {   
            kvpair.Key.GetComponent<BoxCollider2D>().enabled = true;
            kvpair.Key.GetComponent<BoxCollider2D>().size = new Vector2(1,1);
            kvpair.Key.GetComponent<BoxCollider2D>().offset = new Vector2(0,0);
        }
        List<float> y_list_already_combined = new();
        foreach(KeyValuePair<GameObject,Vector2> kvpair in FloorMap)
        {   
            if (y_list_already_combined.Contains(kvpair.Value.y))
            {
                continue;
            }
            y_list_already_combined.Add(kvpair.Value.y);
            List<KeyValuePair<GameObject,Vector2>> same_y_list = new();
            // check if there are any collider with the same y value
            foreach(KeyValuePair<GameObject,Vector2> kvpair2 in FloorMap)
            {   
                if (kvpair.Value.y == kvpair2.Value.y )
                {
                    same_y_list.Add(kvpair2);
                }                
            }
            // sort the list by x value
            same_y_list.Sort((x,y) => x.Value.x.CompareTo(y.Value.x));
            // combine the collider
            KeyValuePair<GameObject,Vector2> combined_start_kvpair = same_y_list[0];
            KeyValuePair<GameObject,Vector2> combined_end_kvpair = same_y_list[0];
            foreach(KeyValuePair<GameObject,Vector2> kvpair2 in same_y_list){
                if (kvpair2.Key == combined_start_kvpair.Key)
                {
                    continue;
                }
                //if has collider on the right side, disable the right collider and make the left collider bigger to the right
                if (kvpair2.Value.x == combined_end_kvpair.Value.x + 1)
                {   
                    //stop player falling during combine collider
                    GameObject.Find("player").GetComponent<PlayerMove1>().StopFalling();
                    combined_end_kvpair = kvpair2;
                    combined_start_kvpair.Key.GetComponent<BoxCollider2D>().size = new Vector2(math.Abs(combined_end_kvpair.Value.x - combined_start_kvpair.Value.x) + 1,1);
                    combined_start_kvpair.Key.GetComponent<BoxCollider2D>().offset = new Vector2((combined_end_kvpair.Value.x - combined_start_kvpair.Value.x)/2,0);
                    kvpair2.Key.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject.Find("player").GetComponent<PlayerMove1>().ResumeFalling();
                }

                if (kvpair2.Value.x > combined_end_kvpair.Value.x + 1)
                {
                    combined_start_kvpair = kvpair2;
                    combined_end_kvpair = kvpair2;
                }
            }
            
        }


    }
    void CombineCollider_y(){
        foreach(KeyValuePair<GameObject,Vector2> kvpair in FloorMap)
        {   
            kvpair.Key.GetComponent<BoxCollider2D>().enabled = true;
            kvpair.Key.GetComponent<BoxCollider2D>().size = new Vector2(1,1);
            kvpair.Key.GetComponent<BoxCollider2D>().offset = new Vector2(0,0);
        }
        List<float> x_list_already_combined = new();
        foreach(KeyValuePair<GameObject,Vector2> kvpair in FloorMap)
        {   
            if (x_list_already_combined.Contains(kvpair.Value.x))
            {
                continue;
            }
            x_list_already_combined.Add(kvpair.Value.x);
            List<KeyValuePair<GameObject,Vector2>> same_x_list = new();
            // check if there are any collider with the same x value
            foreach(KeyValuePair<GameObject,Vector2> kvpair2 in FloorMap)
            {   
                if (kvpair.Value.x == kvpair2.Value.x )
                {
                    same_x_list.Add(kvpair2);
                }                
            }
            // sort the list by y value
            same_x_list.Sort((x,y) => x.Value.y.CompareTo(y.Value.y));
            // combine the collider
            KeyValuePair<GameObject,Vector2> combined_start_kvpair = same_x_list[0];
            KeyValuePair<GameObject,Vector2> combined_end_kvpair = same_x_list[0];
            foreach(KeyValuePair<GameObject,Vector2> kvpair2 in same_x_list){
                if (kvpair2.Key == combined_start_kvpair.Key)
                {
                    continue;
                }
                //if has collider on the right side, disable the right collider and make the left collider bigger to the right
                if (kvpair2.Value.y == combined_end_kvpair.Value.y + 1)
                {   
                    //stop player falling during combine collider
                    combined_end_kvpair = kvpair2;
                    combined_start_kvpair.Key.GetComponent<BoxCollider2D>().size = new Vector2(1,math.Abs(combined_end_kvpair.Value.y - combined_start_kvpair.Value.y) + 1);
                    combined_start_kvpair.Key.GetComponent<BoxCollider2D>().offset = new Vector2(0,(combined_end_kvpair.Value.y - combined_start_kvpair.Value.y)/2);
                    kvpair2.Key.GetComponent<BoxCollider2D>().enabled = false;
                }
                if (kvpair2.Value.y > combined_end_kvpair.Value.y + 1)
                {
                    combined_start_kvpair = kvpair2;
                    combined_end_kvpair = kvpair2;
                }
            }
        }

    }
    void change_gravity_direction(List<Vector2> gravity_direction_list)
    {
        gravity_direction = gravity_direction_list[0];
    }

}
