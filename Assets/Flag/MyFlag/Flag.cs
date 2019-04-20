using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public bool isFlagCaptured { get; set; }
    public bool isFlagConquered { get; set; }

    private string collisionPlayerName;

    private GameObject spawnPoint1, spawnPoint2;


    private Transform getPlayerTransform()
    {
        return (GameManager.getPlayer(collisionPlayerName)).transform; 
    }


    private void followThePlayer()
    {
        Debug.Log("La bandiera " + this.name + " è nelle mani di: " + collisionPlayerName + " Posizione -->" + getPlayerTransform().position.ToString());
        //la bandiera per motivi di performance non segue il player
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
        //Debug.Log(this.name + ": Sto controllando la posizione...");
        //Controlla la posizione del player che si è impossessato della bandiera 
        //Se quest'ultimo giunge allo spawn d'attacco la bandiera è conquistata
        //if(player.Position == spawn)
        //conqueredFlag();
        //if (getPlayerTransform().position == spawnPoint1.transform.position
        //    || getPlayerTransform().position == spawnPoint2.transform.position)
        //{
        //    isFlagCaptured = false;
        //    isFlagConquered = true;
        //    Debug.Log("La bandiera " + this.name + "è stata conquistata da " + collisionPlayerName);
        //}


    }


    private void flagCaptured(Collision other)
    {
        isFlagCaptured = true;
        collisionPlayerName = other.collider.name;

        Debug.Log(this.name + ": Flag captured !!");
    }


    void OnCollisionEnter(Collision other)
    {
        flagCaptured(other);
    }

    void Start()
    {
        isFlagCaptured = false;
        spawnPoint1 = GameObject.Find("spawn1");
        spawnPoint1 = GameObject.Find("spawn2");
        Debug.Log(this.name + ": Start");
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
