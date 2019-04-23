using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joinTeam : MonoBehaviour
{
    private Player giocatore;

    
    void Start()
    {
        Debug.Log("START JOIN");
    }

    public void Setup()
    {
        giocatore = this.GetComponent<Player>();
        Debug.Log("Selezionato il player: " + giocatore.name);

        ManagerTeam.instance.setUp(giocatore.name);//Faccio partire il setup del manager team
    }

    

}
