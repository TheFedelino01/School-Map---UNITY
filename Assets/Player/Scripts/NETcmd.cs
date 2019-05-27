using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NETcmd : NetworkBehaviour
{
    private int port = 7777;
    private string ip = "";
    public InputField inputIP;
    private bool connecting = false;

    public GameObject stopConnecting;
    public GameObject connect;
    public GameObject labelConnecting;

    public GameObject menuWindows;

   

    // Start is called before the first frame update
    void Start()
    {
        stopConnecting.SetActive(false);
        connect.SetActive(true);

        labelConnecting.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        //Sta provando a connettersi
        if (connecting == true)
        {
            //Mostro il bottone cancella connessione se non si e' connesso
            if (NetworkManager.singleton.IsClientConnected() == false)
            {
                labelConnecting.SetActive(true);
                stopConnecting.SetActive(true);//visualizzo il bottone cancella

                connect.SetActive(false);//Nascondo il bottone connetti
            }
            else
            {
                connecting = false;
                stopConnecting.SetActive(false);//Nascondo il bottone cancella
                connect.SetActive(true);//visualizzo il bottone connetti
            }
        }
    }



    public void stopTryingToJoin()
    {
        connecting = false;//Dico che non sto piu cercando di connettermi

        labelConnecting.SetActive(false);
        stopConnecting.SetActive(false);//Nascondo il bottone cancella

        connect.SetActive(true);//visualizzo il bottone connetti

        NetworkManager.singleton.client.Disconnect();//Disconnetto il client
    }



    public void serverStartMIO()
    {
        Debug.Log("STARTING SERVER...");
        NetworkManager.singleton.networkPort = port;
        NetworkManager.singleton.StartHost();

        menuWindows.SetActive(false);

        //soleDaAttivareQuandoSpawno.SetActive(true);
    }

    public void connectTo()
    {
        IPAddress x;
        //Controllo se e' stato inserito un IP esistente
        if (IPAddress.TryParse(ip, out x))
        {
            connettitiAspettando(0.3f);//Aspetto cosí riproduce il suono del bottone
        }
        else
        {
            Debug.Log("IP inesistente!: " + ip);
        }
    }

    public void IPCHANGED()
    {
        ip = inputIP.text;//Prendo il testo inserito
        Debug.Log("Nuovo IP: " + ip);
    }

    private void connettitiAspettando(float sec) {
        StartCoroutine(cnt(sec));
        //menuWindows.SetActive(false);
    }

    private IEnumerator cnt(float sec)
    {
        //Aspetto il tempo indicato
        yield return new WaitForSeconds(sec);

        Debug.Log("IP attendibile: " + ip);
        Debug.Log("Connessione in corso...");
        //Imposto l'ip e la porta dove collegarsi
        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.networkPort = port;

        //Lo faccio collegare
        NetworkManager.singleton.StartClient();
        connecting = true;
    }

    


}




