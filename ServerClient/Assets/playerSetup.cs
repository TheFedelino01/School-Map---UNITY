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
            for (int i=0; i < componentiDaDisabilitare.Length; i++)
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

        GetComponent<Player>().Setup();//Faccio partire il setUp

    }

    //Quando entra il player
    public override void OnStartClient()
    {
        //Imposto l'identia' del player
        //ogni player ha un ID unico
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, _player);//Aggiungo il giocatore alla lista dei players
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

        GameManager.unRegisterPlayer(transform.name);//Lo tolgo dalla lista dei players
    }
}
