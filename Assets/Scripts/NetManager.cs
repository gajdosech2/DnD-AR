using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager : NetworkManager
{
    public GameObject menu;
    public MapGenerator map_generator;

    public void Host()
    {
        NetworkManager.singleton.networkAddress = "192.168.100.77";
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.StartHost();
        menu.SetActive(false);
    }

    public void Join()
    {
        NetworkManager.singleton.networkAddress = "192.168.100.77";
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.StartClient();
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Host();
            map_generator.SpawnMap();
        }
    }
}
