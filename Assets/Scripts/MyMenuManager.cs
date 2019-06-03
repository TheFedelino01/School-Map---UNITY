using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyMenuManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject settingsWindow;
    public GameObject exitWindow;
    public GameObject finestraStatistiche;
    public GameObject menuWindow;
    public GameObject camera;
    public GameObject inGameSettings;
    private bool inGameSettingsOpened;

    private bool hightexture = false;
    public GameObject[] hightextureComponents;

    public GameObject debugGui;
    // Start is called before the first frame update
    void Start()
    {
        debugGui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.partitaAvviata && Input.GetKeyDown(KeyCode.Escape))
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
        if (GameManager.Instance.partitaAvviata)
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
        if (GameManager.Instance.partitaAvviata)
        {
            finestraStatistiche.GetComponent<Animator>().Play("Settings In");
        }
    }
    public void apriExitWindow()
    {
        if (GameManager.Instance.partitaAvviata)
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
        if (GameManager.Instance.partitaAvviata)
        {
            finestraStatistiche.GetComponent<Animator>().Play("Settings In");
        }
        exitWindow.GetComponent<Animator>().Play("Exit Panel Out");
    }

    public void esci()
    {
        inGameSettings.SetActive(true);
        inGameSettings.GetComponent<Animator>().Play("Settings Out");
        canvas.GetComponent<screenOverlayManager>().ForzaDisattivazione = false;
        inGameSettingsOpened = false;
        if (GameManager.Instance.partitaAvviata)
        {
            GameManager.Instance.partitaAvviata = false;
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
        canvas.GetComponent<screenOverlayManager>().ForzaDisattivazione = true;
        camera.SetActive(true);
        inGameSettings.GetComponent<Animator>().Play("Settings In");
        Cursor.lockState = CursorLockMode.None;
    }

    public void chiudiInGameSettings()
    {
        inGameSettings.GetComponent<Animator>().Play("Settings Out");
        camera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        canvas.GetComponent<screenOverlayManager>().ForzaDisattivazione = false;
    }


    public void changeHighTextures()
    {
        hightexture = !hightexture;
        for(int i=0; i< hightextureComponents.Length; i++)
        {
            //Se vuole attivare le high textures, abilito i componenti, altrimenti li disabilito
            if (hightexture == true)
            {
                hightextureComponents[i].SetActive(true);
            }else hightextureComponents[i].SetActive(false);
        }
        
    }

    public GameObject getDebugGUI() { return debugGui; }
}