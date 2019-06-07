using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class matchManager : NetworkBehaviour //Controlla tutto ciò che riguarda il match
{
    [SyncVar]
    private int flagNumber;//Numero di bandiere da conquistare
    public int FlagNumber { get => flagNumber; }

    [SyncVar]
    private int flagConquered; //Numero di bandiere conquistate
    public int FlagConquered { get => flagConquered; }

    [SyncVar]
    private int attackTeamScore;  //Punteggio team d'attacco
    [SyncVar]
    private int defenseTeamScore;//Punteggio team difesa
    public int AttackTeamScore { get => attackTeamScore; }
    public int DefenseTeamScore { get => defenseTeamScore; }

    public statistichePartita statistichePartita;

    public int RedTeamScore
    {
        get
        {
            if (attackTeam == "RED")
                return attackTeamScore;
            else
                return defenseTeamScore;
        }
    }
    public int BlueTeamScore
    {
        get
        {
            if (attackTeam == "BLUE")
                return attackTeamScore;
            else
                return defenseTeamScore;
        }
    }

    public bool isAttackTeamWinner { get => AttackTeamScore > defenseTeamScore; }
    public bool isDefenseTeamWinner { get => !isAttackTeamWinner; }

    public int durataMatch;

    public Timer timer;
    public System.TimeSpan tRimanente { get; set; }

    [SyncVar]
    private string attackTeam;
    [SyncVar]
    private string defenceTeam;
    public string DefenceTeam { get => defenceTeam; }
    public string AttackTeam { get => attackTeam; }


    private static matchManager instance;
    public static matchManager Instance { get => instance; }

    GameObject canvasTime;
    void Awake()
    {
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasTime = GameObject.Find("Canvas").transform.Find("Tempo").gameObject;

        tRimanente = System.TimeSpan.FromSeconds(durataMatch);
        flagNumber = 5;
        flagConquered = 0;

        attackTeamScore = 0;
        defenseTeamScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(tRimanente);
        canvasTime.GetComponent<Text>().text = tRimanente.Minutes + " : " + tRimanente.Seconds;

        if (flagConquered == flagNumber || tRimanente.TotalSeconds <= 0)
        {
            Debug.Log("Partita finita!!!");
            statistichePartita.partitaFinita = true;

            this.EseguiAspettando(1, () =>
            {
                GameManager.Instance.partitaAvviata = false;
            });

            //TODO disconnessione
        }
    }


    [Server]
    public void StartMatch()
    {
        Debug.Log("Server match started");

        timer.startTime = Time.realtimeSinceStartup;
        timer.durataMatch = durataMatch;
        timer.StartTimer();
        attackTeam = Random.value < 0.5f ? "RED" : "BLUE";
        Debug.Log("Random attack team: " + attackTeam.ToString());
        defenceTeam = attackTeam == "RED" ? "BLUE" : "RED";

        flagNumber = 5;
        flagConquered = 0;

        attackTeamScore = 0;
        defenseTeamScore = 0;
    }

    [Command]
    public void CmdIncTeamScore(string nome, int pt)
    {
        if (nome == attackTeam)
            attackTeamScore += pt;
        else
            defenseTeamScore += pt;
    }

    [Command]
    public void CmdIncBandiereConquistate()
    {
        flagConquered++;
    }

    [ClientRpc]
    public void RpcUpdateTimers(int secRimanenti)
    {
        Debug.Log("Aggiornamento timer...");
        tRimanente = System.TimeSpan.FromSeconds(secRimanenti);
    }
}
