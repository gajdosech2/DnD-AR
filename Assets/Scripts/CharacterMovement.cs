using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Vector3 move_direction = Vector3.zero;
    Vector3 rotation_vector = Vector3.zero;
    float move_speed = 0.5f;
    float rotation_speed = 100.0f;

    CharacterController controller;
    Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Animation()
    {
        if (Mathf.Abs(move_direction.x) < 0.1f && Mathf.Abs(move_direction.z) < 0.1f)
        {
            animator.SetInteger("State", 0);
        }
        else
        {
            animator.SetInteger("State", 1);
        }
    }

    void Move()
    {
        move_direction.x = 0;
        move_direction.z = 0;
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            move_direction.x = (controller.transform.forward * move_speed).x;
            move_direction.z = (controller.transform.forward * move_speed).z;
        }

        controller.Move(move_direction * Time.deltaTime);

        rotation_vector.y = Input.GetAxis("Horizontal");
        transform.Rotate(rotation_vector * rotation_speed * Time.deltaTime);
    }

    void Update()
    {
        Move();
        Animation();
    }
}
