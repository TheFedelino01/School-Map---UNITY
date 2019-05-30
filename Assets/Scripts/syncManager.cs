﻿using UnityEngine;
using UnityEngine.Networking;

public class syncManager : NetworkBehaviour
{
    private static syncManager instance;
    public syncManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("[Server] ERRORE: SYNC MANAGER NULLO");
            return instance;
        }
    }
    public class NetworkPlayerInfoList : SyncListStruct<PlayerInfo> { } //classe che rappresenta una lista di playerInfo sincronizzata in rete

    private NetworkPlayerInfoList syncPlayerInfo;   //lista di playerInfo sincronizzata in rete

    public NetworkPlayerInfoList SyncPlayerInfo { get => syncPlayerInfo; }
    //NetworkPlayerInfoList test = new NetworkPlayerInfoList();

    void Awake()
    {
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(this.gameObject);
        syncPlayerInfo = new NetworkPlayerInfoList();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.A))
        //    CmdAdd();
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    PlayerInfo p = test[0];
        //    p.nome = "pippo";
        //    test[0] = p;
        //}
        foreach (PlayerInfo PI in syncPlayerInfo)
            Debug.Log(PI.ToString());
        //foreach (PlayerInfo PI in test)
        //    Debug.Log(PI.ToString());
        //Debug.Log(base.isServer);
    }


    //[Command]
    //public void CmdAdd()
    //{
    //    Debug.LogError("CMD");
    //    if (isServer)
    //    {
    //        Debug.LogError("AGGIUNGO");
    //        PlayerInfo p = new PlayerInfo();
    //        p.nome = "ciao";
    //        test.Add(p);
    //    }
    //    else Debug.LogError("Non va una sega");
    //}


    [Command]
    public void CmdAddToList(PlayerInfo p)
    {
        Debug.Log("[Server] CMD AGGIUNGO ALLA LISTA: " + p.id);
        //if (isServer)
        //{
        syncPlayerInfo.Add(p);  //lo aggiungo alla lista sincronizzata    
        //p.RpcSetName(p.PlayerInfo.id);
        //}
        //else Debug.LogError("Non va una sega");
    }

    [Command]
    public void CmdRemoveFromList(string id)
    {
        for (int i = 0; i < syncPlayerInfo.Count; i++)
            if (syncPlayerInfo[i].id == id)
            {
                syncPlayerInfo.RemoveAt(i);
                Debug.LogError("[Server] Player: " + id + " disconnesso!");
                RpcUnRegisterRemotePlayer(id); //tolgo il player da tutti i client
                return;
            }
        Debug.LogError("[Server] Giocatore Non trovato nella lista sincronizzata: " + id);
    }

    [ClientRpc]
    private void RpcUnRegisterRemotePlayer(string id)
    {
        GameManager.instance.unRegisterRemotePlayer(id);
    }

    [Command]
    public void CmdEditInList(PlayerInfo nuove)
    {
        Debug.Log("[Server] EDIT LISTA");
        for (int i = 0; i < syncPlayerInfo.Count; i++)
            if (syncPlayerInfo[i].id == nuove.id)
            {
                //syncPlayerInfo[i].copiaDa(nuove);
                syncPlayerInfo[i] = nuove;
                return;
            }
        Debug.LogError("[Server] Giocatore Non trovato nella lista sincronizzata: " + nuove.id + "\n" + nuove.ToString());
    }
}
