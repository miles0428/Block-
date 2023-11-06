
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.EventSystems;

public class PlayerMove1 : MonoBehaviour
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

    [SerializeField] EventSystem eventSystem;

    Rigidbody2D rigidbody2d = new Rigidbody2D();
    [SerializeField] float speed ;
    float vertical_speed ;
    bool falling ;
    bool move_right ;
    bool move_left ;
    int jump_count = 0;    
    bool dead = false;

    List<float> vertical_position_list = new();
 
    readonly List<Vector2> gravity_direction_list = new()
    {
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(-1, 0),
    };

    // choose initial gravity direction as down
    public Vector2 gravity_direction = new();
    // choose next gravity direction randomly
    Vector2 next_gravity_direction = new();
    Dictionary<string, List<GameObject>> dict_collision_normal_list = new();
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
        }

        if (Input.GetKey(KeyCode.D))
        {    
            MoveParallel("right");    
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            speed = math.max(math.abs(speed) - 3f*move_acceleration * Time.deltaTime,0) * math.sign(speed);
            if (dict_move_direction["right"] == Vector2.right || dict_move_direction["right"] == Vector2.left){
                rigidbody2d.velocity = new Vector2(0,rigidbody2d.velocity.y) + speed * dict_move_direction["right"];
                // Debug.Log("velocity_1"+rigidbody2d.velocity.ToString());
            }
            else
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x,0) + speed * dict_move_direction["right"];
                // Debug.Log("velocity_2"+rigidbody2d.velocity.ToString());
            }
        }

        void MoveParallel(string direction)
        {
            if (dict_move_bool[direction] == true)
            {
                if (direction == "left" && speed >= -2f)
                {                    
                    speed = - initial_speed;
                }
                else if (direction == "right" && speed <= 2f)
                {
                    speed = initial_speed;
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
                    // Debug.Log("velocity"+velocity.ToString());
                }
                else
                {   
                    Vector2 velocity = speed * dict_move_direction["right"] + new Vector2(RigidBodyVelocity.x,0);
                    rigidbody2d.velocity = velocity;
                    // Debug.Log("velocity"+velocity.ToString());
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
                rigidbody2d.velocity = -vertical_speed * gravity_direction + new Vector2(0,RigidBodyVelocity.y);
            }
            else
            {
                rigidbody2d.velocity = -vertical_speed * gravity_direction + new Vector2(RigidBodyVelocity.x,0);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {   
        // Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("floor"))
        {   
            Vector2 normal = collision.GetContact(0).normal;
            if (normal == dict_collision_normal["floor"])
            {
                falling = false;
            }
            if (normal == dict_collision_normal["top"])
            {
                Debug.Log("top");
                vertical_speed = - vertical_speed;
            }

            dict_collision_normal_list[Vector2ToString(normal)].Add(collision.gameObject);
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        foreach (KeyValuePair<string, List<GameObject>> kvp in dict_collision_normal_list)
        {   
            if (kvp.Value.Contains(other.gameObject))
            {
                kvp.Value.Remove(other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("floor"))
        {
            //if still have collision with other floor, and the normal is the same as the previous one, then do not change falling
            if (dict_collision_normal_list[Vector2ToString(dict_collision_normal["floor"])].Count == 0)
            {   
                falling = true;
                Debug.Log("falling");
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
            // Debug.Log("gravity_direction"+gravity_direction.ToString());
            ChangeMoveDirection(old_gravity_direction,gravity_direction);
        }
        // Debug.Log("Check point 0");
        eventSystem.GetComponent<Gravity_event>().GravityMsg(new(){gravity_direction,next_gravity_direction});
    }

    void ChangeMoveDirection(Vector2 old_gravity_direction,Vector2 gravity_direction)
    {   
        float rotate_angle = Vector2.SignedAngle(old_gravity_direction, gravity_direction);
        Dictionary<string, Vector2> dict_temp_direction = new();
        Dictionary<string, Vector2> dict_temp_collision_normal = new();
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
        while (dict_collision_normal_list[Vector2ToString(dict_collision_normal["top"])].Count != 0)
        {
            dict_collision_normal_list[Vector2ToString(dict_collision_normal["top"])].Remove(dict_collision_normal_list[Vector2ToString(dict_collision_normal["top"])][0]);
        }

        speed = 0f;
        vertical_speed = 0f;
        falling = false;

        if (dict_collision_normal_list[Vector2ToString(dict_collision_normal["floor"])].Count == 0)
        {
            falling = true;
        }
    }

    string Vector2ToString(Vector2 vector2)
    {
        return vector2.ToString();
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
            dict_collision_normal_list.Add(Vector2ToString(kvp.Value), new List<GameObject>());
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
        vertical_position_list.Clear();
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

    public void StopFalling()
    {
        Time.timeScale = 0;
    }

    public void ResumeFalling()
    {
        Time.timeScale = 1;
    }

    void OnApplicationQuit()
    {
        timer_event.TimeUpEvent -= ChangeGravityDirection;
        Reset_event.ResetEvent -= Reset;
        dead_event.DeadEvent -= Dead;
    }
}
