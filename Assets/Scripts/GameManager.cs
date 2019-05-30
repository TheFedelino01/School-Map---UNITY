using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    private syncManager syncManager;
    public syncManager SyncManager { get => syncManager; }
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ERRORE: GAME MANAGER NULLO");
                //_instance = new GameManager();
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

    void Start()
    {
    }



    void Update()
    {
        if (syncManager != null)
        {
            Debug.Log("N giocatori: " + syncManager.Instance.SyncPlayerInfo.Count + " - " + giocatori.Count);
            Sync();
        }
    }

    private void Sync()
    {
        for (int i = 0; i < syncManager.Instance.SyncPlayerInfo.Count; i++)
        {
            string id = syncManager.Instance.SyncPlayerInfo[i].id;
            Debug.Log(id);
            try
            {
                if (giocatori.ContainsKey(id))
                    //giocatori[id].PlayerInfo.copiaDa(syncManager.Instance.SyncPlayerInfo[i]);
                    giocatori[id].PlayerInfo = syncManager.Instance.SyncPlayerInfo[i];
                else
                    Debug.LogError("PLAYER NON TROVATO: " + id);
            }
            catch (System.NullReferenceException e)
            {
                Debug.LogError("PORCO PORCO: " + e.Message + "\n____________\n" + e.Source);
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    //private Player findPlayer(string id)
    //{
    //    Player[] players = FindObjectsOfType<Player>();
    //    Debug.Log(players.ToString());
    //    foreach (Player p in players)
    //        if (p.PlayerInfo.id == id)
    //            return p;
    //    throw new System.ArgumentException("Player inesistente");
    //}

    public GameSettings gameSettings = new GameSettings();
    public bool partitaAvviata { get; set; }



    private const string PrefixPlayer = "Player.";
    private Dictionary<string, Player> giocatori = new Dictionary<string, Player>();
    private Player localPlayer; //lo salvo per la disconnessione
    public bool isServer { get; set; }

    public void RegisterLocalPlayer(string netId, Player player)
    {
        if (syncManager == null)
            syncManager = GetComponentInChildren<syncManager>();
        Debug.Log("REGISTRAZIONE PLAYER LOCALE");
        string plId = PrefixPlayer + netId;

        //PlayerInfo playerInfo = player.PlayerInfo;
        //playerInfo.id = plId;
        //player.PlayerInfo = playerInfo;
        player.createPlayerInfo(plId);

        giocatori.Add(plId, player);//Lo aggiungo alla lista

        syncManager.Instance.CmdAddToList(player.PlayerInfo);   //lo aggiungo anche alla lista sincronizzata
        Debug.Log(giocatori[plId] + "---" + syncManager.Instance.SyncPlayerInfo[0]);

        player.transform.name = plId;//Gli imposto il nome
        localPlayer = player;
    }

    public void RegisterRemotePlayer(string netId, Player player)
    {
        if (syncManager == null)
            syncManager = GetComponentInChildren<syncManager>();
        Debug.Log("REGISTRAZIONE PLAYER REMOTO");
        string plId = PrefixPlayer + netId;

        //PlayerInfo playerInfo = player.PlayerInfo;
        //playerInfo.id = plId;
        //player.PlayerInfo = playerInfo;
        player.createPlayerInfo(plId);

        giocatori.Add(plId, player);//Lo aggiungo alla lista

        Debug.Log(giocatori[plId] + "---" + syncManager.Instance.SyncPlayerInfo[0]);

        player.transform.name = plId;//Gli imposto il nome
    }


    public void unRegisterLocalPlayer()
    {
        string id = localPlayer.PlayerInfo.id;
        syncManager.Instance.CmdRemoveFromList(id);
        giocatori.Remove(id);
        Debug.LogError("Player: " + id + " disconnesso!");
    }

    public void unRegisterRemotePlayer(string id)
    {
        giocatori.Remove(id);
        Debug.LogError("Player: " + id + " disconnesso!");
    }

    public Player getPlayer(string nomePlayer)
    {
        Debug.Log(giocatori[nomePlayer].PlayerInfo.ToString());
        return giocatori[nomePlayer];
    }

    public Dictionary<string, Player> getAllPlayers()
    {
        return giocatori;
    }
    public void addUccisione(string nome, string morto)
    {
        Player tmp = giocatori[nome];
        tmp.RpcHoKillato(morto);
        giocatori[nome].addUccisione();
        Debug.Log("Nuova uccisione: " + tmp.name + " kills updated: " + tmp.Kill);
    }

    public string toStringAll()
    {
        string ris = "";
        foreach (string _playerID in giocatori.Keys)
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
