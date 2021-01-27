using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ServerManager : NetworkBehaviour
{
    int player_on_turn = 0;

    [Server]
    private void FixedUpdate()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        player_on_turn = player_on_turn % players.Length;
        PlayerTurn current = players[player_on_turn].GetComponent<PlayerTurn>();
        if (current.movement < 0)
        {
            player_on_turn = (player_on_turn + 1) % players.Length;
            PlayerTurn next = players[player_on_turn].GetComponent<PlayerTurn>();
            next.movement = 25;
        }

        if (current.triggerQuestion == true)
        {
            foreach(GameObject player in players)
            {
                player.GetComponent<PlayerTurn>().askQuestion = true;
            }
            current.triggerQuestion = false;
        }
    }
}
