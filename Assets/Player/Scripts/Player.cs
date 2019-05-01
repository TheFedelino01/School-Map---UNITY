﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour
{

    [SerializeField]
    private Behaviour[] disabilitareQuandoMorto;
    [SerializeField]
    private bool[] statusIniziale;
    private PlayerInfo playerInfo;

    public PlayerInfo PlayerInfo { get => playerInfo; set => playerInfo = value; }
    

    public int Kill { get => playerInfo.kill; }
    public int Morti { get => playerInfo.morti; }
    public int Bandiere { get => playerInfo.bandiere; }
    public int Punti { get => playerInfo.punti; }
    public string Nome { get => playerInfo.nome; }
    public string Squadra { get => playerInfo.squadra; }


    public Player() { playerInfo = new PlayerInfo(); }
    public Player(PlayerInfo info) { this.PlayerInfo = playerInfo; }
    public void Setup()//All'inizio, parte quando la classe PlayerSetup e' partita completamente 
    {
        //Imposto la salute massima in base a quella presente nel game settings
        playerInfo.maxSalute = GameManager.instance.gameSettings.saluteMax;
        playerInfo.currentSalute = playerInfo.maxSalute;

        salvaSituaIniziale();//Mi salvo gli elementi che sono attivi e disattivati all'inizio del player
        

    }

    void Update()
    {
        //Se il giocatore preme K, questo si suicida
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcPrendiDanno(999999, "");
        }
        Debug.Log(playerInfo.squadra);
        if (playerInfo.squadra == null)
        {
            Debug.Log(playerInfo.nome + " non ha team quindi glilo faccio scegliere");
            GetComponent<joinTeam>().Setup();
        }
    }


    public void SetTeam(string nome)
    {
        playerInfo.squadra = nome;

        //imposto che sta giocando
        GameManager.instance.partitaAvviata = true;
        Debug.Log("Partita avviata: " + GameManager.instance.partitaAvviata + "Player: " + nome);
    }



    [ClientRpc]
    public void RpcPrendiDanno(float danno, string pistolero)
    {
        float futureSalute = playerInfo.currentSalute - danno;

        if (futureSalute <= 0)
            playerInfo.isDead = true;

        if (playerInfo.isDead == false)
        {
            playerInfo.currentSalute = futureSalute;
            Debug.Log("TEAM: " + playerInfo.squadra + " - " + transform.name + " salute aggiornata: " + playerInfo.currentSalute);
        }
        else
        {
            //E' morto
            muori(pistolero);
        }

    }

    private void muori(string assassino)
    {
        //DISABILITIAMO ALCUNI COMPONENTI COSI' NON SI PUO' MUOVERE
        GetComponent<vanguardAnimController>().muori();
        disabilitaElementiDaMorto();

        playerInfo.morti++;//Aumento il numero di morti
        GameManager.instance.addUccisione(assassino);//Aumento il numero di uccisioni di chi mi ha ucciso

        Debug.Log(transform.name + " is now dead!");

        //Respawn
        StartCoroutine(respawn());
    }

    private IEnumerator respawn()
    {
        //Aspetto il tempo di respawn
        yield return new WaitForSeconds(GameManager.instance.gameSettings.respawnTime);

        riportaSituaIniziale();//Riabilito i componenti

        //Reimposto la vita
        playerInfo.currentSalute = playerInfo.maxSalute;

        //Riporto il player allo spawn
        //Prendo la pos dello spawnPoint
        Transform _startPoint = NetworkManager.singleton.GetStartPosition();

        //Muovo il giocatore
        this.transform.position = _startPoint.position;
        this.transform.rotation = _startPoint.rotation;

        Debug.Log(transform.name + " has just respawned!");

        playerInfo.isDead = false;
    }

    private void disabilitaElementiDaMorto()
    {
        for (int i = 0; i < disabilitareQuandoMorto.Length; i++)
        {
            disabilitareQuandoMorto[i].enabled = false;
        }

        //Se il player ha un collider, disabilitalo
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;
    }

    private void salvaSituaIniziale()
    {
        for (int i = 0; i < disabilitareQuandoMorto.Length; i++)
        {
            statusIniziale[i] = disabilitareQuandoMorto[i].enabled;
        }
    }

    private void riportaSituaIniziale()
    {
        for (int i = 0; i < disabilitareQuandoMorto.Length; i++)
        {
            disabilitareQuandoMorto[i].enabled = statusIniziale[i];
        }

        //Se il player ha un collider, riabilitalo
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }

    public void addUccisione()
    {
        playerInfo.kill++;
    }
}


public struct PlayerInfo
{
    public string id;

    public bool isDead;
    public float maxSalute;

    public float currentSalute;

    public int kill;
    public int morti;
    public int bandiere;
    public int punti;
    public string nome;
    public string squadra;

    public override string ToString()
    {
        return id + "- Squadra: " + squadra;
    }
}