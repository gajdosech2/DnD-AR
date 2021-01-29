using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Initiative : NetworkBehaviour
{
    [SyncVar]
    public int initiative = 0;

    void Start()
    {
        if (isLocalPlayer)
        {
            CmdSetInitiative();
        }
    }

    [Command]
    public void CmdSetInitiative()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<int> initiatives = new List<int>();
        foreach (GameObject player in players)
        {
            initiatives.Add(player.GetComponent<Initiative>().initiative);
        }
    
        initiative = Random.Range(1, 20);
        while (initiatives.Contains(initiative))
        {
            initiative = Random.Range(1, 20);
        }
    }
}
