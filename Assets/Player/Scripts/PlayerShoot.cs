using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerShoot : NetworkBehaviour
{
    //public float danno = 10f;
    public string tagNemico = "PlayerREMOTE";

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;//Per sapere cosa colpisco

    void Start()
    {
        if (cam != null)
        {

        }
        else
        {
            Debug.LogError("Camera principale non trovata");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (GetComponentInChildren<arma>().mitraglietta)
        {
            if (Input.GetButton("Fire1"))
                spara();
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
                spara();
        }

        //Se il giocatore preme K, questo si suicida
        if (Input.GetKeyDown(KeyCode.K) && !ChatManager.Instance.ChatAperta && GameManager.Instance.partitaAvviata)
        {
            CmdPlayerAsBeenShoot(GetComponent<Player>().Id, int.MaxValue);
        }
    }
    //In questo modo questo metodo puo' essere richiamato solo dal 
    //Client e MAI dal Server
    [Client]
    void spara()
    {
        if (GameManager.Instance.partitaAvviata && GetComponent<weaponsManager>().spara())
        {
            RaycastHit _hit;

            //Prendo la linea in mezzo allo schermo
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            //Controllo se colpisco qualcosa
            if (Physics.Raycast(ray, out _hit))
            {
                //Controllo se e' un nemico
                if (_hit.collider.tag == tagNemico)
                {
                    //Dico che ho colpito il player e gli passo il nome
                    //del player colpito
                    CmdPlayerAsBeenShoot(_hit.collider.name, GetComponentInChildren<arma>().danno);
                }
            }
        }
    }

    [Command]
    void CmdPlayerAsBeenShoot(string idDelPlayerColpito, float danno)
    {
        //Metodo fatto dal server

        Debug.Log("COLPITO> " + this.name + " HA COLPITO IL Giocatore: " + idDelPlayerColpito);

        Player giocatoreColpito = GameManager.Instance.getPlayer(idDelPlayerColpito);
        if (!giocatoreColpito.PlayerInfo.isDead)
            giocatoreColpito.RpcPrendiDanno(danno, this.name);


    }

}
