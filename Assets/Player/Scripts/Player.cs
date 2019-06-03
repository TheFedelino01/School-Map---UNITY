using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] disabilitareQuandoMorto;
    [SerializeField]
    private bool[] statusIniziale;
    private PlayerInfo playerInfo;


    private GameObject redScreen;
    private Coroutine c;

    public GameObject debugGui;

    public PlayerInfo PlayerInfo
    {
        get => playerInfo;
        set
        {
            if (playerInfo.id == value.id)
                playerInfo = value;
            else
                Debug.LogError("ERRORE!! STAI CERCANDO DI ASSEGNARE A UN GIOCATORE INFORMAZIONI NON SUE!!!!");
        }
    }


    public int Kill { get => playerInfo.kill; }
    public int Morti { get => playerInfo.morti; }
    public int Bandiere { get => playerInfo.bandiere; }
    public int Punti { get => playerInfo.punti; }
    public string Nome { get => playerInfo.nome; }
    public string Squadra { get => playerInfo.squadra; }
    public string Id { get => PlayerInfo.id; }
    public GameObject capturedFlag { get; set; } //Puntatore alla bandiera catturata


    public Player() { }
    public Player(PlayerInfo info) { playerInfo = info; }

    public void createPlayerInfo(string id)
    {
        Debug.Log("CREO INFO: " + id);
        playerInfo = new PlayerInfo(id);
    }
    void Start()
    {
        redScreen = GameObject.Find("Canvas").transform.Find("rosso").gameObject;
        capturedFlag = null;
    }
    public void Setup()//All'inizio, parte quando la classe PlayerSetup e' partita completamente 
    {
        //Imposto la salute massima in base a quella presente nel game settings
        playerInfo.maxSalute = GameManager.Instance.gameSettings.saluteMax;
        playerInfo.currentSalute = playerInfo.maxSalute;

        salvaSituaIniziale();//Mi salvo gli elementi che sono attivi e disattivati all'inizio del player


        //ASSEGNO L'ID AL NOME MOMENTANEAMENTE
        //TODO SCELTA NOME
        //playerInfo.nome = playerInfo.id;

        syncManager.Instance.CmdEditInList(playerInfo);
        //imposto il nome con cui manderà i messaggi
        ChatManager.Instance.playerName = playerInfo.nome;

        debugGui = GameObject.Find("Menu Manager").GetComponent<MyMenuManager>().getDebugGUI();

    }

    void Update()
    {
        Debug.Log(playerInfo.id + ":::::" + gameObject.name);
        if (gameObject.name != playerInfo.id)
            gameObject.name = playerInfo.id;

        if (!isLocalPlayer)
            return;


        ////Debug.Log(playerInfo.squadra);
        //if (playerInfo.squadra == null)
        //{
        //    //Debug.Log(playerInfo.nome + " non ha team quindi glilo faccio scegliere");
        //    GetComponent<joinTeam>().Setup();
        //}

        //if (playerInfo.nome == null)
        //{
        //    //Debug.Log(playerInfo.nome + " non ha team quindi glilo faccio scegliere");
        //    GetComponent<SceltaNome>().mostraFinestra();
        //}

        //Se clicca 0 visualizzo o nascondo il debug visivo
        if (Input.GetKeyDown(KeyCode.Alpha0) == true)
        {
            debugGui.SetActive(!debugGui.active);
        }
    }

    public void setNome(string nome)
    {
        playerInfo.nome = nome;
        syncManager.Instance.CmdEditInList(playerInfo);

    }
    public void SetTeam(string nome)
    {
        playerInfo.squadra = nome;
        syncManager.Instance.CmdEditInList(playerInfo);
        //imposto che sta giocando
        GameManager.Instance.partitaAvviata = true;
        Debug.Log("Partita avviata: " + GameManager.Instance.partitaAvviata + "Player: " + nome);
    }

    [ClientRpc]
    public void RpcPrendiDanno(float danno, string pistolero)
    {
        float futureSalute = playerInfo.currentSalute - danno;
        Debug.LogError(danno);
        Debug.LogError(futureSalute);
        if (futureSalute <= 0)
            playerInfo.isDead = true;

        if (isLocalPlayer)
        {
            if (c != null)
                StopCoroutine(c);
            redScreen.SetActive(true);

            c = this.EseguiAspettando(3, () =>
            {
                redScreen.SetActive(false);

            });
        }


        if (playerInfo.isDead == false)
        {
            playerInfo.currentSalute = futureSalute;
            Debug.Log("TEAM: " + playerInfo.squadra + " - " + transform.name + " salute aggiornata: " + playerInfo.currentSalute);
        }
        else
        {
            //E' morto
            muori(pistolero);
        }

        syncManager.Instance.CmdEditInList(playerInfo);
    }

    [ClientRpc]
    public void RpcHoKillato(string chiHoUcciso)
    {
        if (isLocalPlayer)
        {
            GameObject.Find("GameMessage").GetComponent<Text>().text = "KILL " + chiHoUcciso;
            this.EseguiAspettando(3, () =>
            {
                if (GameObject.Find("GameMessage").GetComponent<Text>().text == "KILL " + chiHoUcciso)
                    GameObject.Find("GameMessage").GetComponent<Text>().text = "";
            });
        }
    }

    private void muori(string idAssassino)
    {
        if (isLocalPlayer)
        {
            string nomeAssassino = GameManager.Instance.getPlayerName(idAssassino);
            GameObject.Find("GameMessage").GetComponent<Text>().text = "Sei stato ucciso da: " + nomeAssassino;
            KillWindow.Instance.CmdSetKill(nomeAssassino, PlayerInfo.nome);
        }
        //DISABILITIAMO ALCUNI COMPONENTI COSI' NON SI PUO' MUOVERE
        GetComponent<vanguardAnimController>().muori(GameManager.Instance.gameSettings.respawnTime);
        disabilitaElementiDaMorto();

        playerInfo.morti++;//Aumento il numero di morti
        syncManager.Instance.CmdEditInList(playerInfo);
        GameManager.Instance.addUccisione(idAssassino, PlayerInfo.nome);//Aumento il numero di uccisioni di chi mi ha ucciso

        Debug.Log(transform.name + " is now dead!");

        //Respawn
        StartCoroutine(respawn());

        if (capturedFlag != null)
            capturedFlag.GetComponent<Flag>().dropTheFlag();
    }

    private IEnumerator respawn()
    {
        //Aspetto il tempo di respawn
        yield return new WaitForSeconds(GameManager.Instance.gameSettings.respawnTime);

        riportaSituaIniziale();//Riabilito i componenti
        if (isLocalPlayer)
            GameObject.Find("GameMessage").GetComponent<Text>().text = "";
        //Reimposto la vita
        playerInfo.currentSalute = playerInfo.maxSalute;

        //Riporto il player allo spawn
        //Prendo la pos dello spawnPoint
        Transform _startPoint = NetworkManager.singleton.GetStartPosition();

        //Muovo il giocatore
        this.transform.position = _startPoint.position;
        this.transform.rotation = _startPoint.rotation;

        Debug.Log(transform.name + " has just respawned!");

        playerInfo.isDead = false;
        syncManager.Instance.CmdEditInList(playerInfo);
    }

    private void disabilitaElementiDaMorto()
    {
        for (int i = 0; i < disabilitareQuandoMorto.Length; i++)
        {
            disabilitareQuandoMorto[i].enabled = false;
        }

        ////Se il player ha un collider, disabilitalo
        //Collider _col = GetComponent<Collider>();
        //if (_col != null)
        //    _col.enabled = false;
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

        ////Se il player ha un collider, riabilitalo
        //Collider _col = GetComponent<Collider>();
        //if (_col != null)
        //    _col.enabled = true;
    }

    public void addUccisione()
    {
        playerInfo.kill++;
        syncManager.Instance.CmdEditInList(playerInfo);
    }
}


public struct PlayerInfo
{
    public readonly string id;

    public bool isDead;
    public float maxSalute;

    public float currentSalute;

    public int kill;
    public int morti;
    public int bandiere;
    public int punti;
    public string nome;
    public string squadra;

    public PlayerInfo(string id) : this()
    {
        this.id = id;
    }

    public void copiaDa(PlayerInfo p)
    {
        if (id == p.id)
        {
            isDead = p.isDead;
            maxSalute = p.maxSalute;
            currentSalute = p.currentSalute;
            kill = p.kill;
            morti = p.morti;
            bandiere = p.bandiere;
            punti = p.punti;
            nome = p.nome;
            squadra = p.squadra;
        }
        else
            Debug.LogError("ERRORE AGGIORNAMENTO PLAYER REMOTO: " + id);
    }
    public override string ToString()
    {
        return id + " nome: " + nome + "- Squadra: " + squadra;
    }
}