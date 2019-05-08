using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statistichePartita : MonoBehaviour
{
    public GameObject imgMirino;
    public GameObject menuWindowsCamera;
    public GameObject finestraStatistiche;
    public GameObject lista;
    public GameObject lista1;
    public GameObject lista2;
    public GameObject prefabPlayer;
    private bool toUpdate = false;

    private static readonly string[] CAMPI_STATISTICHE = { "Content/Name", "Content/Kill", "Content/Morti", "Content/Bandiere", "Content/Punti" };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.partitaAvviata)
            return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("TAB");
            mostraFinestra();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
            nascondiFinestra();

        if (toUpdate)
        {
            var giocatori = GameManager.instance.getAllPlayers();

            for (int i = 0; i < lista1.transform.childCount; i++)
                Destroy(lista1.transform.GetChild(i).gameObject);
            for (int i = 0; i < lista2.transform.childCount; i++)
                Destroy(lista2.transform.GetChild(i).gameObject);
            //for (int i = 2; i < lista.transform.childCount; i++)
            //   Destroy(lista.transform.GetChild(i).gameObject);
            foreach (var player in giocatori)
            {
                Debug.Log(">>>>>>>>>>GIOCATORE: " + player.Value.ToString() + ":" + player.Value.Squadra);
                GameObject stat;

                if (player.Value.Squadra == "RED")
                {
                    stat = Instantiate(prefabPlayer, lista1.transform);
                }
                else
                    stat = Instantiate(prefabPlayer, lista2.transform);

                stat.transform.Find(CAMPI_STATISTICHE[0]).GetComponent<Text>().text = player.Key + ": " + player.Value.Squadra;
                stat.transform.Find(CAMPI_STATISTICHE[1]).GetComponent<Text>().text = player.Value.Kill.ToString();
                stat.transform.Find(CAMPI_STATISTICHE[2]).GetComponent<Text>().text = player.Value.Morti.ToString();
                stat.transform.Find(CAMPI_STATISTICHE[3]).GetComponent<Text>().text = player.Value.Bandiere.ToString();
                stat.transform.Find(CAMPI_STATISTICHE[4]).GetComponent<Text>().text = player.Value.Punti.ToString();
            }
            toUpdate = false;
        }
    }

    private void mostraFinestra()
    {
        imgMirino.GetComponent<mirinoManager>().ForzaDisattivazione = true;
        menuWindowsCamera.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        finestraStatistiche.GetComponent<Animator>().Play("Settings In");
        toUpdate = true;
    }

    private void nascondiFinestra()
    {
        finestraStatistiche.GetComponent<Animator>().Play("Settings Out");
        Cursor.lockState = CursorLockMode.Locked;
        menuWindowsCamera.SetActive(false);
        imgMirino.GetComponent<mirinoManager>().ForzaDisattivazione = false;
    }


}
