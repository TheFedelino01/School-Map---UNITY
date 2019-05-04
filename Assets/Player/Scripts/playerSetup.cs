using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class playerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentiDaDisabilitare;

    [SerializeField]
    string TAGlocal = "PlayerLOCAL";
    string TAGremote = "PlayerREMOTE";
    Camera sceneCamera;

    void Start()
    {
        //Controllo se questo player non e' comandato da me
        //(Non sono io questo player)
        if (isLocalPlayer == false)
        {
            setAsRemotePlayer();

            //Proibisco che il giocatore possa essere comandato da me
            //in quanto non sono io il "proprietario"
            for (int i = 0; i < componentiDaDisabilitare.Length; i++)
            {
                componentiDaDisabilitare[i].enabled = false;
            }
        }
        else
        {
            //Sono io questo player
            setAsLocalPlayer();
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }

    }
    //void Update()
    //{
    //    Debug.LogError(GameManager.instance.gameObject.GetComponent<NetworkIdentity>().hasAuthority);
    //}
    [Command]
    private void CmdAssegnaAutorita(NetworkIdentity toAssign, NetworkIdentity playerID)
    {
        //Debug.LogError(toAssign.hasAuthority);
        //Debug.LogError(toAssign);
        try
        {
            toAssign.RemoveClientAuthority(toAssign.clientAuthorityOwner);
        }
        catch (System.NullReferenceException e) { }
        Debug.Log("Autorità GameManager assegnata: " + toAssign.AssignClientAuthority(playerID.connectionToClient));    //assegno al gameManager l'autorità del 
        //Debug.LogError(toAssign.hasAuthority);
    }

    //Quando entra il player
    public override void OnStartLocalPlayer()
    {
        Debug.Log("OnStartLocalPlayer");
        CmdAssegnaAutorita(GameManager.instance.gameObject.GetComponentInChildren<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());
        //Imposto l'identia' del player
        //ogni player ha un ID unico
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        StartCoroutine(add(netId, _player));


        _player.Setup();//Faccio partire il setUp
    }

    //aspetto finche il server assegna l'autorità al gameManager per registrare il player
    private IEnumerator add(string id, Player p)
    {
        yield return new WaitForSeconds(0.1f);
        if (GameManager.instance.gameObject.GetComponentInChildren<NetworkIdentity>().hasAuthority)
            GameManager.instance.RegisterPlayer(id, p);//Aggiungo il giocatore alla lista dei players
        else
            StartCoroutine(add(id, p));
    }
    void setAsRemotePlayer()
    {
        //Dico che questo player ha questo tag (remoteLayerName)
        gameObject.tag = TAGremote;
    }

    void setAsLocalPlayer()
    {
        //Dico che questo player ha questo tag (remoteLayerName)
        gameObject.tag = TAGlocal;
    }

    //Quando il player esce
    void onDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.instance.unRegisterPlayer(transform.name);//Lo tolgo dalla lista dei players
    }
}
