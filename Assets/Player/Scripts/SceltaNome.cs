using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SceltaNome : MonoBehaviour
{
    public GameObject camera;
    public GameObject nomeWindow;
    public InputField inputField;

    private Player player;

    private static SceltaNome _instance;
    public static SceltaNome Instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.LogError("CREO NUOVO - SINGLETON TAROCCO:/");
                _instance = new SceltaNome();
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start scelta nome");
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

    private void mostraFinestra()
    {
        Debug.Log("apro");
        camera.SetActive(true);
        nomeWindow.SetActive(true);
        nomeWindow.GetComponent<Animator>().Play("Settings In");
        Cursor.lockState = CursorLockMode.None;
    }

    private void nascondiFinestraTeam()
    {
        nomeWindow.GetComponent<Animator>().Play("Settings Out");
        nomeWindow.SetActive(false);
        camera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        //imgMirino.GetComponent<mirinoManager>().ForzaDisattivazione = false;

        //Debug.Log("LISTA RIASSUNTIVA PLAYERS:\n"+GameManager.instance.toStringAll());
    }

    public void setName()
    {
        player.setNome(inputField.text);
        nascondiFinestraTeam();
    }

    public void avvia(Player p)
    {
        this.player = p;
        mostraFinestra();
    }
}
