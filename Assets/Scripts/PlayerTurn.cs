using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerTurn : NetworkBehaviour
{
    [SyncVar]
    public int movement = -5;

    [SyncVar]
    public bool askQuestion = false;

    [SyncVar]
    public bool triggerQuestion = false;

    QuestionManager questionManager;
    DiceRollManager diceRollManager;
    Animator animator;
    GUIButton move, left, right, attack, dice;

    Text movementText, experienceText;

    private int exp = 0;

    const float ATTACK_SPEED = 0.75f;
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
            animator.SetInteger("State", 0);
            move = GameObject.Find("MoveButton").GetComponent<GUIButton>();
            left = GameObject.Find("RotateLeftButton").GetComponent<GUIButton>();
            right = GameObject.Find("RotateRightButton").GetComponent<GUIButton>();
            attack = GameObject.Find("AttackButton").GetComponent<GUIButton>();
            dice = GameObject.Find("DiceButton").GetComponent<GUIButton>();
            diceRollManager = GameObject.Find("RollManager").GetComponent<DiceRollManager>();
            questionManager = GameObject.Find("QuestionManager").GetComponent<QuestionManager>();
            movementText = GameObject.Find("MovementText").GetComponent<Text>();
            experienceText = GameObject.Find("ExperienceText").GetComponent<Text>();

            List<float> offsets = new List<float> { -0.4f, -0.2f, 0.0f, 0.2f, 0.4f };
            Vector3 position = new Vector3(offsets[Random.Range(0, 4)], 0, offsets[Random.Range(0, 4)]);
            transform.position = position;

            GameObject.Find("Menu").SetActive(false);
        }
    }
    
    void Update()
    {
        if (isLocalPlayer)
        {
            movementText.text = ((movement >= 0) ? movement : 0) + "\nft";
            if (askQuestion)
            {
                questionManager.activateSelf();
                CmdQuestionDone();
            }
            if (movement >= 0)
            {
                Turn();
            }
        }
    }

    bool CanMove(Vector3 new_pos)
    {
        new_pos.y = 0;
        return (new_pos.x > -0.41 && new_pos.z > -0.41 && new_pos.x < 0.41 && new_pos.z < 0.41) ||
            (Physics.OverlapSphere(new_pos, 0.01f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide).Length > 0);
    }

    void Turn()
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
            Action();
        }
    }

    void Action()
    {
        if (movement == 0)
        {
            CmdReduceMovement();
        }
        else if ((Input.GetAxisRaw("Vertical") > 0 || move.GetDown()) && CanMove(transform.position + transform.forward * MOVE_DISTANCE))
        {
            animator.SetInteger("State", 1);
            move_lerp = 0.0f;
            start_position = transform.position;
            CmdReduceMovement();
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || right.GetDown() || left.GetDown())
        {
            rotation_dir = (Input.GetAxisRaw("Horizontal") > 0 || right.GetDown()) ? 1 : -1;
            rotation_lerp = 0.0f;
            start_rotation = transform.rotation.eulerAngles.y;
        }
        else if (Input.GetKeyDown("space") || attack.GetDown())
        {
            Attack();
        }
        else if (dice.GetDown())
        {
            diceRollManager.activateSelf(this);
        }
    }

    void Attack()
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
            StartCoroutine(DestroyEnemyAfterSecond(closest_enemy.GetComponent<NetworkIdentity>().netId));
        }
    }

    [Command]
    public void CmdTriggerQuestion()
    {
        triggerQuestion = true;
    }

    [Command]
    public void CmdQuestionDone()
    {
        askQuestion = false;
    }

    [Command]
    void CmdReduceMovement()
    {
        movement -= 5;
    }

    [Command]
    void CmdDestroyEnemy(NetworkInstanceId netId)
    {
        GameObject enemy = NetworkServer.FindLocalObject(netId);
        NetworkServer.Destroy(enemy);
    }

    IEnumerator DestroyEnemyAfterSecond(NetworkInstanceId netId)
    {
        yield return new WaitForSeconds(1.0f);
        exp += 10;
        experienceText.text = exp + "\nXP";
        CmdDestroyEnemy(netId);
    }
}
