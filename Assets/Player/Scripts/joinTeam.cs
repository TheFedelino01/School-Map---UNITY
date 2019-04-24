using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class joinTeam : MonoBehaviour
{
    private Player giocatore;

    
    void Start()
    {
        Debug.Log("START JOIN");
    }

    
    public void Setup()
    {
        ManagerTeam.instance.setUp(this.GetComponent<Player>().name);//Faccio partire il setup del manager team
    }

    

}
