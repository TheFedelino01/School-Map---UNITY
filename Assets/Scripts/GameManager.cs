using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("CREO NUOVO - SINGLETON TAROCCO:/");
                _instance = new GameManager();
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
    }



    public GameSettings gameSettings = new GameSettings();
    public bool partitaAvviata { get; set; }

    private static string PrefixPlayer = "Player.";
    private static Dictionary<string, Player> giocatori = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netId, Player player)
    {
        string plId = PrefixPlayer + netId;
        giocatori.Add(plId, player);//Lo aggiungo alla lista

        player.transform.name = plId;//Gli imposto il nome
    }

    public static void unRegisterPlayer(string nome)
    {
        giocatori.Remove(nome);
    }

    public static Player getPlayer(string nomePlayer)
    {
        return giocatori[nomePlayer];
    }

    public static Dictionary<string, Player> getAllPlayers()
    {
        return giocatori;
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
