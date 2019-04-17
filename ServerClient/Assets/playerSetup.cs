using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
            setAsLocalPlayer();
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }

        //Imposto l'identia' del player
        //ogni player ha un ID unico
        setIdentityPlayer();
    }

    void setIdentityPlayer()
    {
        string ID = "Giocatore."+GetComponent<NetworkIdentity>().netId;
        transform.name = ID;
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

    void onDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
