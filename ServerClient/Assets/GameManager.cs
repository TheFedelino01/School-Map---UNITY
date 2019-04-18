using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get {
            if (_instance == null)
                _instance = new GameManager();


            return _instance;
        }
    }

    private GameManager()
    {

    }
    


    public GameSettings gameSettings = new GameSettings();


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
