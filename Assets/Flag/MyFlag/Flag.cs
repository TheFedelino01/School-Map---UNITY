using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//@author Peduzzi Samuele

public class Flag : MonoBehaviour
{
    public bool isFlagCaptured { get; set; }
    public bool isFlagConquered { get; set; }

    private string collisionPlayerName;
    public string flagId; // <-- Da implementare a breve !!
    public GameObject[] effetti;

    private GameObject spawnPoint1, spawnPoint2;

    public AudioSource cassa;
    public AudioClip suonoCattura;


    private Transform getPlayerTransform()
    {
        return (GameManager.instance.getPlayer(collisionPlayerName)).transform;
    }


    private void followThePlayer()
    {
        Debug.Log("La bandiera " + flagId + " è nelle mani di: " + collisionPlayerName + " Posizione -->" + getPlayerTransform().position.ToString());
        //la bandiera per motivi di performance non segue il player
    }

    private void conqueredFlag()
    {
        GameObject.Find(collisionPlayerName).transform.GetChild(6).gameObject.SetActive(false); //L'icona della bandiera sopra al player viene disattivata

        isFlagCaptured = false; //La bandiera non è più nelle mani del player attaccante
        isFlagConquered = true; //La bandiera è stata conquistata
        //GetComponent<matchManager>().flagConquered++; //Viene incrementato il numero di bandiere conquistato dalla squadra d'attacco <--match manager da istanziare
        Debug.Log("La bandiera " + flagId + "è stata conquistata da " + collisionPlayerName);

        //TO DO
        //Incrementa punteggio player
        //Incrementa punteggio squadra attaccante...
    }


    private void chkPlayerPosition()
    {
        //Debug.Log(flagId + ": Sto controllando la posizione...");
        //Controlla la posizione del player che si è impossessato della bandiera 
        //Se quest'ultimo giunge allo spawn d'attacco la bandiera è conquistata
        //if(player.Position == spawn)
        //conqueredFlag();
        if ((int)getPlayerTransform().position.x == (int)spawnPoint1.transform.position.x
            && (int)getPlayerTransform().position.z == (int)spawnPoint1.transform.position.z)
        {
            conqueredFlag();
        }
        else if ((int)getPlayerTransform().position.x == (int)spawnPoint2.transform.position.x
            && (int)getPlayerTransform().position.z == (int)spawnPoint2.transform.position.z)
        {
            conqueredFlag();
        }


    }


    private void flagCaptured(Collision other)
    {
        other.collider.transform.GetChild(6).gameObject.SetActive(true); //L'icona della bandiera sopra al player viene attivata

        isFlagCaptured = true;
        this.gameObject.SetActive(false); //La bandiera viene nascosta
        collisionPlayerName = other.collider.name;

        Debug.Log(flagId + ": Flag captured !!");
        attivaEffetti();

        cassa.Play();
    }

    private void attivaEffetti()
    {
        for(int i=0; i< effetti.Length; i++)
        {
            effetti[i].SetActive(true);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        flagCaptured(other);
    }

    void Start()
    {
        isFlagCaptured = false;
        //System.Random rn = new System.Random();
       // flagId = "FlagID"+ rn.Next(0,9)+ rn.Next(0, 9)+ rn.Next(0, 9);
        spawnPoint1 = GameObject.Find("spawn1");
        spawnPoint2 = GameObject.Find("spawn2");
        Debug.Log(flagId + ": Start");
        Debug.Log(flagId + ": spawn1 : " + spawnPoint1.transform.position);
        Debug.Log(flagId + ": spawn2 : " + spawnPoint2.transform.position);

        cassa.clip = suonoCattura;//Inizializzo
    }

    void Update()
    {
        if (isFlagCaptured)
        {
            chkPlayerPosition();
            followThePlayer();
        }

    }


    public void dropTheFlag() //Da richiamare nel momento in cui il player viene ucciso
    {
        GameObject.Find(collisionPlayerName).transform.GetChild(6).gameObject.SetActive(false); //L'icona della bandiera sopra al player viene disattivata
        isFlagCaptured = false;
        (GameObject.Find(flagId)).transform.position = getPlayerTransform().position;
    }

}
