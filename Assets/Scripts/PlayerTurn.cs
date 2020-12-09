using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerTurn : NetworkBehaviour
{
    Animator animator;
    GUIButton move, left, right, attack;
    Text movementText, experienceText;

    private int movement = 25;
    private int exp = 0;

    const float ATTACK_SPEED = 0.6f;
    float attack_lerp = 1.0f;

    const float MOVE_DISTANCE = 0.2f;
    const float MOVE_SPEED = 2.0f;
    float move_lerp = 1.0f;
    Vector3 start_position = Vector3.zero;

    const int ROTATION_AMOUNT = 90;
    const float ROTATION_SPEED = 1.5f;
    int rotation_dir = 1;
    float rotation_lerp = 1.0f;
    float start_rotation = 0;

    void Start()
    {
        if (isLocalPlayer)
        {
            animator = GetComponent<Animator>();
            move = GameObject.Find("MoveButton").GetComponent<GUIButton>();
            left = GameObject.Find("RotateLeftButton").GetComponent<GUIButton>();
            right = GameObject.Find("RotateRightButton").GetComponent<GUIButton>();
            attack = GameObject.Find("DiceButton").GetComponent<GUIButton>();
            movementText = GameObject.Find("MovementText").GetComponent<Text>();
            experienceText = GameObject.Find("ExperienceText").GetComponent<Text>();

            List<float> offsets = new List<float> { -0.4f, -0.2f, 0.0f, 0.2f, 0.4f };
            Vector3 position = new Vector3(offsets[Random.Range(0, 4)], 0, offsets[Random.Range(0, 4)]);
            transform.position = position;
        }
    }
    
    void Update()
    {
        if (isLocalPlayer)
        {
            Move();
        }
    }

    void Move()
    {
        if (move_lerp < 1.0f)
        {
            move_lerp += MOVE_SPEED * Time.deltaTime;
            transform.position = Vector3.Lerp(start_position, start_position + transform.forward * MOVE_DISTANCE, move_lerp);
        }
        else if (rotation_lerp < 1.0f)
        {
            rotation_lerp += ROTATION_SPEED * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, start_rotation, 0), Quaternion.Euler(0, start_rotation + rotation_dir * ROTATION_AMOUNT, 0), rotation_lerp);
        }
        else if (attack_lerp < 1.0f)
        {
            attack_lerp += ATTACK_SPEED * Time.deltaTime;
        }
        else 
        {
            animator.SetInteger("State", 0);
            if (Input.GetAxisRaw("Vertical") > 0 || move.GetDown())
            {
                animator.SetInteger("State", 1);
                move_lerp = 0.0f;
                start_position = transform.position;
                movement = (movement - 5 >= 0) ? movement - 5 : 25;
                movementText.text = movement + "\nft";
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || right.GetDown() || left.GetDown())
            {
                rotation_dir = (Input.GetAxisRaw("Horizontal") > 0 || right.GetDown()) ? 1 : -1;
                rotation_lerp = 0.0f;
                start_rotation = transform.rotation.eulerAngles.y;
            }
            else if (Input.GetKeyDown("space") || attack.GetDown())
            {
                animator.SetInteger("State", 2);
                attack_lerp = 0.0f;
                float min_distance = 0.4f;
                GameObject closest_enemy = null;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    var dist = Vector3.Distance(transform.position, enemy.transform.position);
                    if (dist < min_distance)
                    {
                        min_distance = dist;
                        closest_enemy = enemy;
                    }
                }
                if (closest_enemy)
                {
                    StartCoroutine(DestroyEnemyAfterSecond(closest_enemy));
                }
            }
        }
    }

    IEnumerator DestroyEnemyAfterSecond(GameObject enemy)
    {
        yield return new WaitForSeconds(1.0f);
        NetworkServer.Destroy(enemy);
        exp += 10;
        experienceText.text = exp + "\nXP";
    }
}
