using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ServerManager : NetworkBehaviour
{
    int player_on_turn = 0;
    List<int> initiatives = new List<int>();

   [Server]
    private void FixedUpdate()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        PlayerTurn current = players[player_on_turn].GetComponent<PlayerTurn>();

        UpdateInitiatives(players);
        ChangeTurn(players, current);
        CheckQuestion(players, current);
    }

    [Server]
    private void ChangeTurn(GameObject[] players, PlayerTurn current)
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
        }
    }

    [Server]
    private void CheckQuestion(GameObject[] players, PlayerTurn current)
    {
        if (current.triggerQuestion == true)
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerTurn>().askQuestion = true;
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
    }
}
