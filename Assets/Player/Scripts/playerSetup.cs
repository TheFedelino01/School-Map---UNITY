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
        CmdAssegnaAutorita(GameManager.Instance.gameObject.GetComponentInChildren<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());
        //Imposto l'identia' del player
        //ogni player ha un ID unico
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        StartCoroutine(add(netId, _player));

    }


    //aspetto finche il server assegna l'autorità al gameManager per registrare il player
    private IEnumerator add(string id, Player p)
    {
        yield return new WaitForEndOfFrame();
        if (GameManager.Instance.gameObject.GetComponentInChildren<NetworkIdentity>().hasAuthority)
        {
            Debug.Log("add");
            GameManager.Instance.RegisterLocalPlayer(id, p);//Aggiungo il giocatore alla lista dei players

            SceltaNome.Instance.avvia(p);
            aspettaSceltaNome();

            //motodo che aspetta che il giocatore abbia un nome prima di fargli scegliere la squadra
            void aspettaSceltaNome()
            {
                this.EseguiAspettando(new WaitForEndOfFrame(), () =>
                {
                    if (p.Nome != null && p.Nome != "")
                    {
                        GetComponent<joinTeam>().Setup();
                        p.Setup();//Faccio partire il setUp
                    }
                    else
                        aspettaSceltaNome();
                });
            }

        }
        else
            StartCoroutine(add(id, p));
    }

    void setAsRemotePlayer()
    {
        //Dico che questo player ha questo tag (remoteLayerName)
        gameObject.tag = TAGremote;

        Debug.Log("Start altro client");
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.Instance.RegisterRemotePlayer(netId, _player);
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

        GameManager.Instance.unRegisterPlayer(transform.name);//Lo tolgo dalla lista dei players


    }
}
