using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] disabilitareQuandoMorto;
    [SerializeField]
    private bool[] statusIniziale; 

    [SyncVar]
    public bool isDead = false;

    //Imposto la salute massima in base a quella presente nel game settings
    [SerializeField]
    private float maxSalute = GameManager.instance.gameSettings.saluteMax;

    [SyncVar]//Ogni volta che cambia, innoltra il valore a tutti i clients
    private float currentSalute;

    [SyncVar]//Ogni volta che cambia, innoltra il valore a tutti i clients
    private int kill;
    [SyncVar]//Ogni volta che cambia, innoltra il valore a tutti i clients
    private int morti;
    [SyncVar]//Ogni volta che cambia, innoltra il valore a tutti i clients
    private int bandiere;
    [SyncVar]//Ogni volta che cambia, innoltra il valore a tutti i clients
    private int punti;
    [SyncVar]//Ogni volta che cambia, innoltra il valore a tutti i clients
    private string nome;
    [SyncVar]//Ogni volta che cambia, innoltra il valore a tutti i clients
    private string squadra;

    public int Kill { get => kill;}
    public int Morti { get => morti;}
    public int Bandiere { get => bandiere;}
    public int Punti { get => punti;}
    public string Nome { get => nome;}
    public string Squadra { get => squadra;}

    public void Setup()//All'inizio, parte quando la classe PlayerSetup e' partita completamente 
    {
        
        currentSalute = maxSalute;

        salvaSituaIniziale();//Mi salvo gli elementi che sono attivi e disattivati all'inizio del player

        GetComponent<joinTeam>().Setup();//Faccio partire il setUp
    }

    void Update()
    {
        //Se il giocatore preme K, questo si suicida
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K)){
            RpcPrendiDanno(999999,"");
        }
    }

    public void setTeam(string nome)
    {
        squadra = nome;
    }

    

    [ClientRpc]
    public void RpcPrendiDanno(float danno, string pistolero)
    {
        float futureSalute = currentSalute - danno;

        if (futureSalute <= 0)
            isDead = true;

        if (isDead == false)
        {
            currentSalute = futureSalute;
            Debug.Log(transform.name + " salute aggiornata: " + currentSalute);
        }
        else
        {
            //E' morto
            muori(pistolero);
        }
        
    }

    private void muori(string assassino)
    {
        //DISABILITIAMO ALCUNI COMPONENTI COSI' NON SI PUO' MUOVERE
        disabilitaElementiDaMorto();

        morti++;//Aumento il numero di morti
        GameManager.instance.addUccisione(assassino);//Aumento il numero di uccisioni di chi mi ha ucciso

        Debug.Log(transform.name + " is now dead!");

        //Respawn
        StartCoroutine(respawn());
    }

    private IEnumerator respawn()
    {
        //Aspetto il tempo di respawn
        yield return new WaitForSeconds(GameManager.instance.gameSettings.respawnTime);

        riportaSituaIniziale();//Riabilito i componenti

        //Reimposto la vita
        currentSalute = maxSalute;

        //Riporto il player allo spawn
        //Prendo la pos dello spawnPoint
        Transform _startPoint = NetworkManager.singleton.GetStartPosition();

        //Muovo il giocatore
        this.transform.position = _startPoint.position;
        this.transform.rotation = _startPoint.rotation;

        Debug.Log(transform.name + " has just respawned!");

        isDead = false;
    }

    private void disabilitaElementiDaMorto()
    {
        for (int i = 0; i < disabilitareQuandoMorto.Length; i++)
        {
            disabilitareQuandoMorto[i].enabled =false;
        }

        //Se il player ha un collider, disabilitalo
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;
    }

    private void salvaSituaIniziale()
    {
        for (int i = 0; i < disabilitareQuandoMorto.Length; i++)
        {
            statusIniziale[i] = disabilitareQuandoMorto[i].enabled;
        }
    }

    private void riportaSituaIniziale()
    {
        for (int i = 0; i < disabilitareQuandoMorto.Length; i++)
        {
            disabilitareQuandoMorto[i].enabled = statusIniziale[i];
        }

        //Se il player ha un collider, riabilitalo
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }

    public void addUccisione()
    {
        kill++;
    }
}
