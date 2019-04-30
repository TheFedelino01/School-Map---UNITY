using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.LogError("CREO NUOVO - SINGLETON TAROCCO:/");
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    private GameManager()
    {
        partitaAvviata = false;
    }

    //setto qui l'istanza in modo che poi nel get dovrebbe essere gia pronta
    void Awake()
    {
        if (_instance == null)
            //if not, set instance to this
            _instance = this;

        //If instance already exists and it's not this:
        else if (_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(this.gameObject);
    }

    void Update()
    {
    }


    public GameSettings gameSettings = new GameSettings();
    public bool partitaAvviata { get; set; }

    
    private const string PrefixPlayer = "Player.";
    public Dictionary<NetworkInstanceId, Player> giocatori = new Dictionary<NetworkInstanceId, Player>();

    //public override void OnStartClient()
    //{
    //    syncPlayerInfo.Callback = list_changed;
    //}

    //public void list_changed(NetworkPlayerInfoList.Operation op, int index)
    //{
    //    Debug.Log("Something has changed in list");
    //}

    public void RegisterPlayer(NetworkInstanceId netId, Player player)
    {
        //NetworkServer.Spawn(player.gameObject);
        //string plId = PrefixPlayer + netId;

        PlayerInfo playerInfo = player.PlayerInfo;
        playerInfo.id = netId;
        playerInfo.nome = PrefixPlayer + netId.ToString();
        player.PlayerInfo = playerInfo;

        giocatori.Add(netId, player);//Lo aggiungo alla lista
        //syncPlayerInfo.Add(player.PlayerInfo);
        syncManager.instance.CmdAddToList(player.PlayerInfo);//lo aggiungo alla lista sincronizzata
        //Debug.Log(giocatori[netId] + "---" + syncPlayerInfo[0]);
        player.transform.name = PrefixPlayer + netId.ToString();//Gli imposto il nome
    }



    public void unRegisterPlayer(NetworkInstanceId nome)
    {
        giocatori.Remove(nome);

        //foreach (PlayerInfo p in syncPlayerInfo)
        //    if (p.id == nome)
        //    {
        //        syncPlayerInfo.Remove(p);
        //        break;
        //    }
    }

    public Player getPlayer(NetworkInstanceId nomePlayer)
    {
        Debug.Log(giocatori[nomePlayer].PlayerInfo.ToString());
        return giocatori[nomePlayer];
    }

    public Dictionary<NetworkInstanceId, Player> getAllPlayers()
    {
        return giocatori;
    }
    public void addUccisione(NetworkInstanceId nome)
    {
        Player tmp = giocatori[nome];

        giocatori[nome].addUccisione();
        Debug.Log("Nuova uccisione: " + tmp.name + " kills updated: " + tmp.Kill);
    }

    public string toStringAll()
    {
        string ris = "";
        foreach (NetworkInstanceId _playerID in giocatori.Keys)
        {
            ris += _playerID + " TEAM: " + giocatori[_playerID].Squadra + "\n";
        }
        return ris;
    }



    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200,200,200,500));
    //    GUILayout.BeginVertical();

    //    //Aggiungo ogni elemento del dizionario giocatori su schermo
    //    foreach(string _playerID in giocatori.Keys)
    //    {
    //        GUILayout.Label(_playerID + " " + giocatori[_playerID].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}
}
