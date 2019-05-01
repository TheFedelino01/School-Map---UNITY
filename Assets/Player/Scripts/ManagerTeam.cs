using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ManagerTeam : MonoBehaviour
{
    public GameObject camera;
    public GameObject teamWindow;

    private string playerCheStaScegliendo;

    private static ManagerTeam _instance;
    public static ManagerTeam instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.LogError("CREO NUOVO - SINGLETON TAROCCO:/");
                _instance = new ManagerTeam();
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start manager Team");
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

    public void setUp(string sceglitore)//Avviato quando il player entra in partita
    {
        Debug.Log("Start SETUP Team per: "+  sceglitore);
        playerCheStaScegliendo = sceglitore; //Prendo il player che e' entrato in partita e vuole scegliere il team

        mostraFinestraTeam();//gli mostro la finestra dei team
    }

    public void mostraFinestraTeam()
    {
        //imgMirino.GetComponent<mirinoManager>().ForzaDisattivazione = true;
        camera.SetActive(true);
        teamWindow.GetComponent<Animator>().Play("Settings In");
        Cursor.lockState = CursorLockMode.None;
    }

    public void nascondiFinestraTeam()
    {
        teamWindow.GetComponent<Animator>().Play("Settings Out");
        camera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        //imgMirino.GetComponent<mirinoManager>().ForzaDisattivazione = false;

        //Debug.Log("LISTA RIASSUNTIVA PLAYERS:\n"+GameManager.instance.toStringAll());
    }

    public void setTeamBLUE()//richiamato quando clicca sul pulsante "BLUE" presente nel "joinTeamMio"
    {
        GameManager.instance.getPlayer(playerCheStaScegliendo).SetTeam("BLUE");
        Debug.Log("Il giocatore: " + playerCheStaScegliendo + " ha scelto il team BLUE");

        nascondiFinestraTeam();
    }

    public void setTeamRED()//richiamato quando clicca sul pulsante "RED" presente nel "joinTeamMio"
    {
        GameManager.instance.getPlayer(playerCheStaScegliendo).SetTeam("RED");
        Debug.Log("Il giocatore: " + playerCheStaScegliendo + " ha scelto il team RED");

        nascondiFinestraTeam();
    }

}
