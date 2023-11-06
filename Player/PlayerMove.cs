
using System.Collections.Generic;

using UnityEngine;

using Unity.Mathematics;


public class PlayerMove : MonoBehaviour
{
    // Initialize variables
    [SerializeField] float max_move_speed        = 9.0f;
    [SerializeField] float move_acceleration     = 23.0f;
    [SerializeField] float gravity_acceleration  = 20.0f;
    [SerializeField] float jump_speed            = 5.0f;
    [SerializeField] float max_vertical_speed    = 6.0f;

    [SerializeField] float initial_speed = 2.0f;

    [SerializeField] int max_jump_count = 2;

    [SerializeField] bool change_gravity = true;

    Rigidbody2D rigidbody2d = new Rigidbody2D();
    float speed ;
    float vertical_speed ;
    bool falling ;
    bool move_right ;
    bool move_left ;
    int jump_count = 0;    
    bool dead = false;
 
    readonly List<Vector2> gravity_direction_list = new()
    {
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(-1, 0),
    };

    // choose initial gravity direction as down
    Vector2 gravity_direction = new();
    // choose next gravity direction randomly
    Vector2 next_gravity_direction = new();
    Dictionary<string, List<string>> dict_collision_normal_list = new();
    Dictionary<string, Vector2> dict_collision_normal ;
    internal static Dictionary<string, Vector2> dict_move_direction ;
    // dict_move_bool is used to check if player is touching the wall
    // and should or should not move
    Dictionary<string, bool> dict_move_bool = new();
    
    // Start is called before the first frame update
    void Start()
    {   
        rigidbody2d = GetComponent<Rigidbody2D>();
        Debug.Log("player start");
        InitailParameters();
        // subscribe to event
        Reset_event.ResetEvent += Reset;
        timer_event.TimeUpEvent += ChangeGravityDirection;
        dead_event.DeadEvent += Dead;
        pause_event.PauseEvent += Pause;
        pause_event.ResumeEvent += Resume;

    }

    // Update is called once per frame
    void Update()
    {   
        if (dead)
        {
            return;
        }
        Player_Move();
        falling = PlayerFall(falling);
        Player_Gravity();
        CheckTouching("floor");
        CheckTouching("right");
        CheckTouching("left");
        dict_move_bool["right"] = move_right;
        dict_move_bool["left"] = move_left;

    }

    void Player_Move()
    {   
        // Move player if key is pressed A or D
        if (falling && Input.GetKey(KeyCode.LeftShift))
        {
            vertical_speed = -max_vertical_speed;   
        }

        if (Input.GetKey(KeyCode.A))
        {   
            MoveParallel("left");
            // Debug.Log("move left");

        }

        if (Input.GetKey(KeyCode.D))
        {    
            MoveParallel("right");         
            // Debug.Log("move right");  
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
        }

        void MoveParallel(string direction)
        {
            if (dict_move_bool[direction] == true)
            {
                if (direction == "left" && speed >= -2f)
                {                    
                    speed = - initial_speed;
                    Debug.Log("move left");
                    Debug.Log(speed.ToString());
                }
                else if (direction == "right" && speed <= 2f)
                {
                    speed = initial_speed;
                    Debug.Log("move right");
                    Debug.Log(speed.ToString());
                }

                speed += move_acceleration * Time.deltaTime * math.sign(speed);
            }
            else if (dict_move_bool[direction] == false)
            {
                Debug.Log("cannot move");
            }
            else
            {
                Debug.Log("dict_move_bool[direction] is not true or false");
            }

            if (math.abs(speed) > max_move_speed)
            {
                speed = max_move_speed * math.sign(speed);
            }
            
            if (dict_move_bool[direction])
            {   
                Vector2 RigidBodyVelocity = GetComponent<Rigidbody2D>().velocity;

                if (dict_move_direction["right"] == Vector2.right || dict_move_direction["right"] == Vector2.left)
                {
                    Vector2 velocity = speed * dict_move_direction["right"] + new Vector2(0,RigidBodyVelocity.y);
                    rigidbody2d.velocity = velocity;
                    // Debug.Log(GetComponent<Rigidbody2D>().velocity);
                }
                else
                {
                    rigidbody2d.velocity = speed * dict_move_direction["right"] + new Vector2(RigidBodyVelocity.x,0);
                }
            }
            
        }
    }

    bool PlayerFall(bool falling)
    {
        // Jump if key is pressed Space
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            if (! falling) 
            {
                vertical_speed = jump_speed;
                falling = true;
            }
            else if (jump_count < max_jump_count-1)
            {
                if (falling && vertical_speed >= 0)
                {   
                    vertical_speed += jump_speed;
                    jump_count += 1;
                }

                else if (falling && vertical_speed < 0)
                {
                    vertical_speed = jump_speed;
                    jump_count += 1;
                }
            }
        }
        if (! falling)
        {
            jump_count = 0;
        }
        return falling;
    }

    void Player_Gravity()
    {
        // Gravity
        if (falling)
        {   
            vertical_speed -= gravity_acceleration * Time.deltaTime;
            if (math.abs(vertical_speed) > max_vertical_speed)
            {
                vertical_speed = max_vertical_speed * math.sign(vertical_speed);
            }
            Vector2 RigidBodyVelocity = GetComponent<Rigidbody2D>().velocity;
            if (gravity_direction == Vector2.right || gravity_direction == Vector2.left)
            {
                // GetComponent<Rigidbody2D>().velocity = -vertical_speed * gravity_direction + new Vector2(0,RigidBodyVelocity.y);
                rigidbody2d.velocity = -vertical_speed * gravity_direction + new Vector2(0,RigidBodyVelocity.y);
            }
            else
            {
                // GetComponent<Rigidbody2D>().velocity = -vertical_speed * gravity_direction + new Vector2(RigidBodyVelocity.x,0);
                rigidbody2d.velocity = -vertical_speed * gravity_direction + new Vector2(RigidBodyVelocity.x,0);
            }
            // GetComponent<Rigidbody2D>().velocity = -vertical_speed * gravity_direction + new Vector2(RigidBodyVelocity.x,0);
            // transform.Translate(-Time.deltaTime * vertical_speed * gravity_direction);


        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {   
        // Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("floor"))
        {   
            Vector2 normal = collision.GetContact(0).normal;
            PlayTouchFloor(normal,"floor",collision);
            PlayTouchFloor(normal,"right",collision);
            PlayTouchFloor(normal,"left",collision);
            PlayTouchFloor(normal,"top",collision);
        }
    }
    void PlayTouchFloor(Vector2 normal, string direction,Collision2D collision)
    {
        if (normal == dict_collision_normal[direction])
        {
            if (direction == "top")
            {
                vertical_speed = - vertical_speed;
            }
            else{
            if (direction == "floor")
            {
                falling = false;
            }
            if (direction == "right")
            {
                move_right = false;
            }
            if (direction == "left")
            {
                move_left = false;
            }
            dict_collision_normal_list[Vector2ToString(dict_collision_normal[direction])].Add(collision.gameObject.name);
            }
        }
    }


    void ChangeGravityDirection()
    {   
        
        if (change_gravity)
        {
            Vector2 old_gravity_direction = gravity_direction;
            gravity_direction = next_gravity_direction ;
            while (gravity_direction == next_gravity_direction)
            {
                next_gravity_direction = gravity_direction_list[UnityEngine.Random.Range(0, gravity_direction_list.Count)];
            }
            ChangeMoveDirection(old_gravity_direction,gravity_direction);
        }

    }

    void ChangeMoveDirection(Vector2 old_gravity_direction,Vector2 gravity_direction)
    {   
        float rotate_angle = Vector2.SignedAngle(old_gravity_direction, gravity_direction);
        Dictionary<string, Vector2> dict_temp_direction = new();
        Dictionary<string, Vector2> dict_temp_collision_normal = new();
        // for each element in dict_move_direction, rotate the value by rotate_angle
        foreach (KeyValuePair<string, Vector2> kvp in dict_move_direction)
        {   
            dict_temp_direction[kvp.Key] = (Vector2)(Quaternion.Euler(0, 0, rotate_angle) * kvp.Value);
        }
        dict_move_direction = dict_temp_direction;

        foreach (KeyValuePair<string, Vector2> kvp in dict_collision_normal)
        {   
            dict_temp_collision_normal[kvp.Key] = (Vector2)(Quaternion.Euler(0, 0, rotate_angle) * kvp.Value);
        }
        dict_collision_normal = dict_temp_collision_normal;
        speed =0f;
        vertical_speed = 0f;
        falling = false;
        while (dict_collision_normal_list[Vector2ToString(dict_collision_normal["top"])].Count != 0)
        {
            dict_collision_normal_list[Vector2ToString(dict_collision_normal["top"])].Remove(dict_collision_normal_list[Vector2ToString(dict_collision_normal["top"])][0]);
        }
        CheckTouching("floor");
        CheckTouching("right");
        CheckTouching("left");

    }

    string Vector2ToString(Vector2 vector2)
    {
        return vector2.ToString();
    }

    void CheckTouching(string direction)
    {   
        List<string> Object_remove_list = new();
        Collider2D this_collider = GetComponent<Collider2D>();
        if (dict_collision_normal_list[Vector2ToString(dict_collision_normal[direction])].Count != 0)
        {
            foreach (string game_object_name in dict_collision_normal_list[Vector2ToString(dict_collision_normal[direction])])
            {
                Collider2D other_collider = GameObject.Find(game_object_name).GetComponent<Collider2D>();
                if (this_collider.IsTouching(other_collider))
                {
                }
                else
                {
                    Object_remove_list.Add(game_object_name);
                }
            }
        }
        if (Object_remove_list.Count != 0)
        {
            foreach (string game_object_name in Object_remove_list)
            {
                dict_collision_normal_list[Vector2ToString(dict_collision_normal[direction])].Remove(game_object_name);
            }
        }
        if (dict_collision_normal_list[Vector2ToString(dict_collision_normal[direction])].Count == 0)
        {
            if (direction == "floor")
            {
                falling = true;
            }
            if (direction == "left")
            {
                move_left = true;
            }
            if (direction == "right")
            {
                move_right = true;
            }
        }
        else 
        {
            if (direction == "floor")
            {
                falling = false;
            }
            if (direction == "left")
            {
                move_left = false;
            }
            if (direction == "right")
            {
                move_right = false;
            }

        }
    }

    void Reset()
    {   
        Debug.Log("player reset");
        dict_collision_normal.Clear();
        dict_collision_normal_list.Clear();
        dict_move_bool.Clear();
        InitailParameters();
    }
    void InitailParameters()
    {
        // initialize variables
        transform.position = GameObject.Find("GameZone").transform.position+new Vector3(0,0.5f,0);
        speed = 2f;
        vertical_speed = 0f;
        falling = true;
        move_right = true;
        move_left = true;
        jump_count = 0;

        dict_collision_normal = new Dictionary<string, Vector2>()
        {
            {"floor", new Vector2(0, 1)},
            {"left", new Vector2(1, 0)},
            {"right", new Vector2(-1, 0)},
            {"top", new Vector2(0, -1)},
        };

        foreach (KeyValuePair<string, Vector2> kvp in dict_collision_normal)
        {
            dict_collision_normal_list.Add(Vector2ToString(kvp.Value), new List<string>());
        }

        dict_move_direction = new Dictionary<string, Vector2>()
        {
            {"right", new Vector2(1, 0)},
            {"left", new Vector2(-1, 0)},
            {"up", new Vector2(0, 1)},
            {"down", new Vector2(0, -1)},
        };

        gravity_direction = new Vector2(0, -1);
        next_gravity_direction = gravity_direction_list[UnityEngine.Random.Range(1, gravity_direction_list.Count)];

        dict_move_bool.Add("right",true);
        dict_move_bool.Add("left",true);
        Time.timeScale = 1;
        dead = false;
        GetComponent<SpriteRenderer>().enabled = true;

 
    }

    void OnDestroy() 
    {   
        // unsubscribe to event
        timer_event.TimeUpEvent -= ChangeGravityDirection;
        Reset_event.ResetEvent -= Reset;
        dead_event.DeadEvent -= Dead;
    }

    void Dead()
    {
        dead = true;
        GetComponent<SpriteRenderer>().enabled = false;
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
