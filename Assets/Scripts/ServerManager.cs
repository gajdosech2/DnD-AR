using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ServerManager : NetworkBehaviour
{
    int player_on_turn = 0;
    List<int> initiatives = new List<int>();
    PlayerTurn current;

    [Server]
    private void FixedUpdate()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (current == null)
        {
            current = players[player_on_turn].GetComponent<PlayerTurn>();
        }

        UpdateInitiatives(players);
        ChangeTurn(players);
        CheckQuestion(players);
    }

    [Server]
    private void ChangeTurn(GameObject[] players)
    {
        player_on_turn = player_on_turn % players.Length;
        
        if (current.movement < 0)
        {
            player_on_turn = (player_on_turn + 1) % players.Length;
            int desired_initiative = initiatives[player_on_turn];

            PlayerTurn next = null;
            foreach (GameObject player in players)
            {
                int player_initiative = player.GetComponent<Initiative>().initiative;
                if (player_initiative == desired_initiative)
                {
                    next = player.GetComponent<PlayerTurn>();
                }
            }

            next.movement = 25;
            current = next;
        }
    }

    [Server]
    private void CheckQuestion(GameObject[] players)
    {
        if (current.triggerQuestion)
        {
            foreach (GameObject player in players)
            {
                PlayerTurn turn = player.GetComponent<PlayerTurn>();
                turn.askQuestion = true;
                turn.questionType = current.questionType;
            }
            current.triggerQuestion = false;
        }
    }

    [Server]
    private void UpdateInitiatives(GameObject[] players)
    {
        initiatives.Clear();
        foreach (GameObject player in players)
        {
            initiatives.Add(player.GetComponent<Initiative>().initiative);
        }
        initiatives.Sort();
        initiatives.Reverse();
    }
}
