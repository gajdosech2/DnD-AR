using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayersManager : MonoBehaviour
{
    public GameObject[] portraits;
    public Material[] mats;

    int lastLength = 0;
    int myId = -1;

    public int GetId()
    {
        return myId;
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0 && myId == -1)
        {
            myId = players.Length - 1;
        }

        if (lastLength != players.Length)
        {
            lastLength = players.Length;
            UpdatePortraits(players);
        }
    }

    void UpdatePortraits(GameObject[] players)
    {
        for (int i = 0; i < lastLength && i < portraits.Length; i++)
        {
            GameObject omni = players[i].transform.Find("omniknight").gameObject;
            omni.GetComponent<Renderer>().material = mats[i % mats.Length];
            portraits[i].SetActive(true);
            if (i == myId)
            {
                portraits[i].transform.Find("Self").gameObject.SetActive(true);
            }
        }
    }
}
