using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyMenuManager : MonoBehaviour
{
    public GameObject imgMirino;
    public GameObject settingsWindow;
    public GameObject exitWindow;
    public GameObject finestraStatistiche;
    public GameObject menuWindow;
    public GameObject camera;
    public GameObject inGameSettings;
    private bool inGameSettingsOpened;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.partitaAvviata && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC");
            if (!inGameSettingsOpened)
                apriInGameSettings();
            else
                chiudiInGameSettings();
            inGameSettingsOpened = !inGameSettingsOpened;
        }
    }

    public void apriImpostazioni()
    {
        Debug.Log("apro impostazioni");
        if (GameManager.instance.partitaAvviata)
        {
            finestraStatistiche.GetComponent<Animator>().Play("Settings Out");
        }

        settingsWindow.GetComponent<Animator>().Play("Settings In");
        inGameSettings.SetActive(false);
    }
    public void chiudiImpostazioni()
    {
        inGameSettings.SetActive(true);
        settingsWindow.GetComponent<Animator>().Play("Settings Out");
        if (GameManager.instance.partitaAvviata)
        {
            finestraStatistiche.GetComponent<Animator>().Play("Settings In");
        }
    }
    public void apriExitWindow()
    {
        if (GameManager.instance.partitaAvviata)
        {
            finestraStatistiche.GetComponent<Animator>().Play("Settings Out");
        }
        exitWindow.GetComponent<Animator>().Play("Exit Panel In");
        inGameSettings.SetActive(false);
    }
    public void chiudiExitWindow()
    {
        inGameSettings.SetActive(true);
        exitWindow.GetComponent<Animator>().Play("Settings Out");
        if (GameManager.instance.partitaAvviata)
        {
            finestraStatistiche.GetComponent<Animator>().Play("Settings In");
        }
        exitWindow.GetComponent<Animator>().Play("Exit Panel Out");
    }

    public void esci()
    {
        inGameSettings.SetActive(true);
        inGameSettings.GetComponent<Animator>().Play("Settings Out");
        imgMirino.GetComponent<mirinoManager>().ForzaDisattivazione = false;
        inGameSettingsOpened = false;
        if (GameManager.instance.partitaAvviata)
        {
            GameManager.instance.partitaAvviata = false;
            NetworkManager.singleton.StopHost();
            NetworkManager.singleton.StopClient();
            exitWindow.GetComponent<Animator>().Play("Exit Panel Out");
            finestraStatistiche.GetComponent<Animator>().Play("Settings Out");
            menuWindow.SetActive(true);
        }
        else
            GetComponent<ExitToSystem>().ExitGame();
    }
    public void apriInGameSettings()
    {
        imgMirino.GetComponent<mirinoManager>().ForzaDisattivazione = true;
        camera.SetActive(true);
        inGameSettings.GetComponent<Animator>().Play("Settings In");
        Cursor.lockState = CursorLockMode.None;
    }

    public void chiudiInGameSettings()
    {
        inGameSettings.GetComponent<Animator>().Play("Settings Out");
        camera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        imgMirino.GetComponent<mirinoManager>().ForzaDisattivazione = false;
    }
}