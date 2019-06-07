using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//@author Peduzzi Samuele

public class Flag : MonoBehaviour
{
    public bool isFlagCaptured { get; set; }
    public bool isFlagConquered { get; set; }

    private string collisionPlayerName;
    public int flagId;
    public GameObject[] effetti;

    private GameObject spawnPoint1, spawnPoint2;

    public AudioSource cassa;
    public AudioClip suonoCattura;

    private bool aspettaPlayerVaVia = false;

    private messageFlag messageFlag;

    private void showFlagId()
    {
        GameObject.Find(collisionPlayerName).transform.GetChild(6).gameObject.SetActive(true); //L'icona della bandiera sopra al player viene attivata
    }
    private void hideFlagId()
    {
        GameObject.Find(collisionPlayerName).transform.GetChild(6).gameObject.SetActive(false); //L'icona della bandiera sopra al player viene disattivata
    }


    private Transform getPlayerTransform()
    {
        return (GameManager.Instance.getPlayer(collisionPlayerName)).transform;
    }


    private void followThePlayer()
    {
        Debug.Log("La bandiera " + flagId + " è nelle mani di: " + collisionPlayerName + " Posizione -->" + getPlayerTransform().position.ToString());
        //la bandiera per motivi di performance non segue il player
    }

    public void conqueredFlag()
    {
        Player p = GameObject.Find(collisionPlayerName).GetComponent<Player>();
        p.capturedFlag = null;
        hideFlagId();

        isFlagCaptured = false; //La bandiera non è più nelle mani del player attaccante
        isFlagConquered = true; //La bandiera è stata conquistata
        //GetComponent<matchManager>().flagConquered++; //Viene incrementato il numero di bandiere conquistato dalla squadra d'attacco <--match manager da istanziare
        Debug.Log("La bandiera " + flagId + "è stata conquistata da " + collisionPlayerName);

        if (p.isLocalPlayer)
        {
            messageFlag.showMessageConquistata();//Visualizzo il messaggio
            //TO DO
            //Incrementa punteggio player
            //Incrementa punteggio squadra attaccante...
            p.incNumBandiere();
        }
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
        Player p = other.collider.GetComponent<Player>();

        if (!p.Attacco)
            return;

        p.capturedFlag = this.gameObject;
        isFlagCaptured = true;

        if (p.isLocalPlayer)
        {
            messageFlag.showMessagePresa();//Visualizzo il messaggio
            cassa.Play();
        }

        this.gameObject.SetActive(false); //La bandiera viene nascosta
        collisionPlayerName = other.collider.name;

        showFlagId();

        Debug.Log(flagId + ": Flag captured !!");
        attivaEffetti();

    }


    private void attivaEffetti()
    {
        for (int i = 0; i < effetti.Length; i++)
        {
            effetti[i].SetActive(true);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //Se non sta morendo e non ha gia preso una bandiera
        if (aspettaPlayerVaVia == false && GameObject.Find(other.collider.name).GetComponent<Player>().capturedFlag == null)
        {
            flagCaptured(other);
        }
    }

    void Start()
    {
        messageFlag = GameObject.Find("FlagsManager").GetComponent<messageFlag>();
        isFlagCaptured = false;
        //flagId = "FlagMIA";
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
            //chkPlayerPosition();
            followThePlayer();
        }

    }


    public void dropTheFlag() //Da richiamare nel momento in cui il player viene ucciso
    {
        Player p = GameObject.Find(collisionPlayerName).GetComponent<Player>();
        p.capturedFlag = null;
        hideFlagId();
        isFlagCaptured = false;
        this.gameObject.transform.position = getPlayerTransform().position;
        this.gameObject.SetActive(true);
        aspettaPlayerVaVia = true;

        if (p.isLocalPlayer)
            messageFlag.showMessagePersa();//Visualizzo il messaggio

        StartCoroutine(respawnBandiera());

    }

    private IEnumerator respawnBandiera()
    {
        //Aspetto il tempo di respawn
        yield return new WaitForSeconds(5);

        aspettaPlayerVaVia = false;

        Debug.Log("Bandiera rilasciata!");
    }

}
