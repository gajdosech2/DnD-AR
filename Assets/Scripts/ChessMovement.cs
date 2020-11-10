using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMovement : MonoBehaviour
{
    public GUIButton move;
    public GUIButton left;
    public GUIButton right;

    const float MOVE_DISTANCE = 0.2f;
    float move_speed = 2.0f;
    float move_lerp = 0.0f;
    Vector3 start_position = Vector3.zero;

    const int ROTATION_AMOUNT = 90;
    int rotation_dir = 1;
    float rotation_speed = 1.5f;
    float rotation_lerp = 1.0f;
    float start_rotation = 0;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (move_lerp < 1.0f)
        {
            move_lerp += move_speed * Time.deltaTime;
            transform.position = Vector3.Lerp(start_position, start_position + transform.forward * MOVE_DISTANCE, move_lerp);
        }
        else if (rotation_lerp < 1.0f)
        {
            rotation_lerp += rotation_speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, start_rotation, 0), Quaternion.Euler(0, start_rotation + rotation_dir * ROTATION_AMOUNT, 0), rotation_lerp);
        }
        else 
        {
            animator.SetInteger("State", 0);
            if (Input.GetAxisRaw("Vertical") > 0 || move.GetDown())
            {
                animator.SetInteger("State", 1);
                move_lerp = 0.0f;
                start_position = transform.position;

            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || right.GetDown() || left.GetDown())
            {
                rotation_dir = (Input.GetAxisRaw("Horizontal") > 0 || right.GetDown()) ? 1 : -1;
                rotation_lerp = 0.0f;
                start_rotation = transform.rotation.eulerAngles.y;
            }
        }
    }
}
