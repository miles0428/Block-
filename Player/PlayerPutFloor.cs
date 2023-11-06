using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;
using Vector2 = UnityEngine.Vector2;

public class PlayerPutFloor : MonoBehaviour
{   
    Dictionary<string, Vector2> dict_move_direction ;
    [SerializeField]GameObject floor_manager;
    [SerializeField]GameObject preview_floor ;
    [SerializeField]EventSystem floor_event_system;
    Vector2 player_position;
    Vector2 preview_floor_position;

    string facing = "right";
    Vector2 facing_vector = new(1,0);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        dict_move_direction = PlayerMove1.dict_move_direction;

        if(Input.GetKey(KeyCode.W))
        {
            facing = "up";
        }
        else if (Input.GetKey(KeyCode.S))
        {
            facing = "down";
        }
        else if (Input.GetKey(KeyCode.A))
        {
            facing = "left";
        }
        else if (Input.GetKey(KeyCode.D))
        {
            facing = "right";
        }

        facing_vector = dict_move_direction[facing];
        player_position = transform.position;
        show_preview_floor(player_position,facing);

    }

    void show_preview_floor(Vector2 player_position,string facing)
    {
        preview_floor_position = player_position + facing_vector;
        Vector2 preview_floor_position_1 = new Vector2(Mathf.Round(preview_floor_position.x),Mathf.Round(preview_floor_position.y));
        Vector2 preview_floor_position_2 = new Vector2(Mathf.Round(preview_floor_position.x),Mathf.Round(preview_floor_position.y))+dict_move_direction["down"];
        Vector2 preview_floor_position_3 = new Vector2(Mathf.Round(preview_floor_position.x),Mathf.Round(preview_floor_position.y))+dict_move_direction["up"];

        if ( HaveNearby(preview_floor_position_1) 
          && (!floor_manager.GetComponent<FloorManager>().FloorMap.ContainsValue(preview_floor_position_1)))
        {
            floor_event_system.GetComponent<floor_event>().CanPutMsg();
            preview_floor.transform.position = preview_floor_position_1;
        }
        else if ((facing == "left" ||facing == "right") 
                && HaveNearby(preview_floor_position_2) 
                && (!floor_manager.GetComponent<FloorManager>().FloorMap.ContainsValue(preview_floor_position_2)))
        {
            floor_event_system.GetComponent<floor_event>().CanPutMsg();
            preview_floor.transform.position = preview_floor_position_2;
        }
        else if (( facing == "left" ||facing == "right") 
                && HaveNearby(preview_floor_position_3) 
                && (!floor_manager.GetComponent<FloorManager>().FloorMap.ContainsValue(preview_floor_position_3)))
        {
            floor_event_system.GetComponent<floor_event>().CanPutMsg();
            preview_floor.transform.position = preview_floor_position_3;
        }
        else
        {
            floor_event_system.GetComponent<floor_event>().CanNotPutMsg();
            preview_floor.transform.position = preview_floor_position_1;
        }

    }
    bool HaveNearby(Vector2 preview_position)
    {
        List<Vector2> nearby_position = new List<Vector2>
        {
            preview_position + dict_move_direction["up"],
            preview_position + dict_move_direction["down"],
            preview_position + dict_move_direction["left"],
            preview_position + dict_move_direction["right"]
        };
        foreach (Vector2 position in nearby_position)
        {
            if (floor_manager.GetComponent<FloorManager>().FloorMap.ContainsValue(position))
            {
                return true;
            }
        }
        return false;
    }
}
