using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public bool isFlagCaptured { get; set; }
    public bool isFlagConquered { get; set; }

    private Collision collisionPlayer;


    private void followThePlayer()
    {
        //TO DO posizione player da aggiornare !!
        this.transform.position = GameManager.getPlayer(collisionPlayer.collider.name).transform.position;
        Debug.Log("Sto seguendo il player ...");
    }

    private void conqueredFlag()
    {
        isFlagCaptured = false; //La bandiera non è più nelle mani del player attaccante
        isFlagConquered = true; //La bandiera è stata conquistata
        GetComponent<matchManager>().flagConquered++; //Viene incrementato il numero di bandiere conquistato dalla squadra d'attacco


        //TO DO
        //Incrementa punteggio player
        //Incrementa punteggio squadra attaccante...
    }


    private void chkPlayerPosition()
    {
        Debug.Log("Sto controllando la posizione...");
        //Controlla la posizione del player che si è impossessato della bandiera 
        //Se quest'ultimo giunge allo spawn d'attacco la bandiera è conquistata
        //if(player.Position == spawn)
            //conqueredFlag();
    }


    private void flagCaptured(Collision other)
    {
        isFlagCaptured = true;
        collisionPlayer = other;
        Debug.Log("Flag captured !!");
    }


    void OnCollisionEnter(Collision other)
    {
        flagCaptured(other);
    }

    void Start()
    {
        isFlagCaptured = false;
        Debug.Log("Start");
    }

    void Update()
    {
        if (isFlagCaptured)
        {
            chkPlayerPosition();
            followThePlayer();
        }
            
    }
    
}
