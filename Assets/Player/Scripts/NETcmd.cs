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
        Debug.Log("STARTING SERVCER...");
        NetworkManager.singleton.networkPort = int.Parse("7777");
        NetworkManager.singleton.StartHost();

        //imposto che sta giocando
        ///TODO Impostarlo anche quando un client si connete ad un server
        GameManager.instance.partitaAvviata = true;
        Debug.Log("Partita avviata " + GameManager.instance.partitaAvviata+ GameManager.instance.partitaAvviata);
    }
}
