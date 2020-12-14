using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager: NetworkManager
{
    public GameObject menu;

    public void Host()
    {
        Common();
        NetworkManager.singleton.StartHost();
    }

    public void Join()
    {
        Common();
        NetworkManager.singleton.StartClient();
    }

    void Common()
    {
        NetworkManager.singleton.networkAddress = "90.64.193.109";
        NetworkManager.singleton.networkPort = 7777;
        menu.SetActive(false);
    }

    void Start()
    {
        menu.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Host();
            GameObject.Find("MapGenerator").GetComponent<MapGenerator>().SpawnMap();
        }
    }
}
