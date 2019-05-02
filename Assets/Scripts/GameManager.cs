using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
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
        syncPlayerInfo = new NetworkPlayerInfoList();
    }


    [Command]
    private void CmdAdd()
    {
        Debug.LogError("CMD");
        if (isServer)
        {
            Debug.LogError("AGGIUNGO");
            PlayerInfo p = new PlayerInfo();
            p.nome = "ciao";
            test.Add(p);
        }
        else Debug.LogError("Non va una sega");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            CmdAdd();
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerInfo p = test[0];
            p.nome = "pippo";
            test[0] = p;
        }
        foreach (PlayerInfo PI in syncPlayerInfo)
            Debug.Log(PI.ToString());
        Debug.Log("N giocatori: " + syncPlayerInfo.Count + " - " + giocatori.Count);
        Sync();
        foreach (PlayerInfo PI in test)
            Debug.Log(PI.ToString());
        Debug.Log(base.isServer);
    }

    private void Sync()
    {
        for (int i = 0; i < syncPlayerInfo.Count; i++)
        {
            try
            {
                if (giocatori.ContainsKey(syncPlayerInfo[i].id))
                    giocatori[syncPlayerInfo[i].id].PlayerInfo = syncPlayerInfo[i];
                else
                    giocatori.Add(syncPlayerInfo[i].id, new Player(syncPlayerInfo[i]));
            }
            catch (System.NullReferenceException e)
            {
                Debug.LogError("PORCO PORCO: " + e.Message + "\n____________\n" + e.Source);
            }
        }

    }

    public GameSettings gameSettings = new GameSettings();
    public bool partitaAvviata { get; set; }

    public class NetworkPlayerInfoList : SyncListStruct<PlayerInfo> { } //classe che rappresenta una lista di playerInfo sincronizzata in rete

    private NetworkPlayerInfoList syncPlayerInfo;   //lista di playerInfo sincronizzata in rete

    NetworkPlayerInfoList test = new NetworkPlayerInfoList();

    private const string PrefixPlayer = "Player.";
    private Dictionary<string, Player> giocatori = new Dictionary<string, Player>();

    //public override void OnStartClient()
    //{
    //    syncPlayerInfo.Callback = list_changed;
    //}

    //public void list_changed(NetworkPlayerInfoList.Operation op, int index)
    //{
    //    Debug.Log("Something has changed in list");
    //}

    public void RegisterPlayer(string netId, Player player)
    {
        Debug.LogError("REGISTRAZIONE");
        string plId = PrefixPlayer + netId;

        PlayerInfo playerInfo = player.PlayerInfo;
        playerInfo.id = plId;
        player.PlayerInfo = playerInfo;

        giocatori.Add(plId, player);//Lo aggiungo alla lista

        CmdAddToList(player.PlayerInfo);//lo aggiungo alla lista sincronizzata
        Debug.Log(giocatori[plId] + "---" + syncPlayerInfo[0]);

        player.transform.name = plId;//Gli imposto il nome
    }

    [Command]
    private void CmdAddToList(PlayerInfo i)
    {
        Debug.LogError("CMD AGGIUNGO ALLA LISTA");
        //if (isServer)
        //{
            Debug.LogError("AGGIUNGO");
            syncPlayerInfo.Add(i);  //lo aggiungo alla lista sincronizzata       
        //}
        //else Debug.LogError("Non va una sega");
    }

    [Command]
    private void CmdRemoveFromList(string id)
    {
        for (int i = 0; i < syncPlayerInfo.Count; i++)
            if (syncPlayerInfo[i].id == id)
            {
                syncPlayerInfo.RemoveAt(i);
                return;
            }
        Debug.LogError("Giocatore Non trovato nella lista sincronizzata: " + id);
    }

    [Command]
    public void CmdEditInList(PlayerInfo nuove)
    {
        Debug.LogError("EDIT");
        for(int i = 0; i < syncPlayerInfo.Count; i++)
            if (syncPlayerInfo[i].id == nuove.id)
            {
                syncPlayerInfo[i] = nuove;
                return;
            }
        Debug.LogError("Giocatore Non trovato nella lista sincronizzata: " + nuove.id + "\n" + nuove.ToString());
    }

    public void unRegisterPlayer(string nome)
    {
        CmdRemoveFromList(nome);
        giocatori.Remove(nome);
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
    public void addUccisione(string nome)
    {
        Player tmp = giocatori[nome];

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
