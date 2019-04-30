using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class syncManager : NetworkBehaviour
{

    public class NetworkPlayerInfoList : SyncListStruct<PlayerInfo> { } //classe che rappresenta una lista di playerInfo sincronizzata in rete

    private NetworkPlayerInfoList syncPlayerInfo;   //lista di playerInfo sincronizzata in rete

    public static syncManager instance { get; set; }
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
        Debug.Log("N giocatori: " + syncPlayerInfo.Count + " - " + GameManager.instance.giocatori.Count);
        RpcSync();
    }


    [ClientRpc]
    private void RpcSync()
    {
        foreach (PlayerInfo PI in syncPlayerInfo)
            Debug.Log(PI.ToString());
        for (int i = 0; i < syncPlayerInfo.Count; i++)
        {
            try
            {
                if (GameManager.instance.giocatori.ContainsKey(syncPlayerInfo[i].id))
                    GameManager.instance.giocatori[syncPlayerInfo[i].id].PlayerInfo = syncPlayerInfo[i];
                else
                    GameManager.instance.giocatori.Add(syncPlayerInfo[i].id, new Player(syncPlayerInfo[i]));
            }
            catch (System.NullReferenceException e)
            {
                Debug.LogError("PORCO PORCO: " + e.Message + "\n____________\n" + e.Source);
            }
        }
    }

    [Command]
    public void CmdAddToList(PlayerInfo i)
    {
        Debug.LogError("AGGIUNGO ALLA LISTA");
        syncPlayerInfo.Add(i);//lo aggiungo alla lista sincronizzata
    }
    [Command]
    public void CmdEditPlayerInfo(NetworkInstanceId id, string sq)
    {
        PlayerInfo p = syncPlayerInfo[0];
        p.squadra = sq;
        syncPlayerInfo[0] = p;
    }
}
