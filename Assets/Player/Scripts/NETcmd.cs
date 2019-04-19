using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NETcmd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void serverStartMIO()
    {
        NetworkManager.singleton.networkPort = int.Parse("7777");
        NetworkManager.singleton.StartHost();
    }
}
