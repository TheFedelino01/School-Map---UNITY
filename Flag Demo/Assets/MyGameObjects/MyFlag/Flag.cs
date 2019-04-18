using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private bool isFlagCaptured { get; set; }

    private void conqueredFlag()
    {
        //TO DO
        //Incrementa punteggio squadra attaccante...
    }


    private void chkPlayerPosition()
    {
        //Controlla la posizione del player che si è impossessato della bandiera 
        //Se quest'ultimo giunge allo spawn d'attacco la bandiera è conquistata
        //if(player.Position == spawn)
            //conqueredFlag();
    }


    private void flagCaptured()
    {
        isFlagCaptured = true;
        Debug.Log("Flag captured !!");
        while (isFlagCaptured)
        {
            chkPlayerPosition();
        }
    }

    void Start()
    {
        isFlagCaptured = false;
        Debug.Log("Start");
    }

    void OnCollisionEnter(Collision other)
    {
        flagCaptured();
    }


    
}
