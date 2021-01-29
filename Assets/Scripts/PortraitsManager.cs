using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PortraitsManager : MonoBehaviour
{
    public GameObject[] portraits;
    public Material[] mats;

    int myId = -1;

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0 && myId == -1)
        {
            myId = players.Length - 1;
        }

        UpdatePortraits(players);
    }

    void UpdatePortraits(GameObject[] players)
    {
        for (int i = 0; i < players.Length && i < portraits.Length; i++)
        {
            if (!portraits[i].activeSelf)
            {
                GameObject omni = players[i].transform.Find("omniknight").gameObject;
                omni.GetComponent<Renderer>().material = mats[i % mats.Length];

                portraits[i].SetActive(true);

                portraits[i].transform.Find("Self").gameObject.SetActive(false);
                if (i == myId)
                {
                    portraits[i].transform.Find("Self").gameObject.SetActive(true);
                }
            }

            Text number = portraits[i].transform.Find("Number").gameObject.GetComponent<Text>();
            int player_initiative = players[i].GetComponent<Initiative>().initiative;
            number.text = player_initiative.ToString("00");
        }
    }
}
