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
            GameObject.Find("Portraits").GetComponent<PortraitsManager>().Initialize(this);
            CmdSetInitiative();
        }
    }

    [Command]
    public void CmdSetInitiative()
    {
        initiative = Random.Range(1, 20);
    }
}
