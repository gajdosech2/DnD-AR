﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager: NetworkManager
{

    public void Host()
    {
        Common();
        NetworkManager.singleton.StartHost();
        GameObject.Find("MapGenerator").GetComponent<MapGenerator>().SpawnMap();
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
    }

}
