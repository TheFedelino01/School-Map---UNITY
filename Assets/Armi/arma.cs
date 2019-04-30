using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arma : MonoBehaviour
{
    public float danno;
    public float tempoRicarica;
    public int colpiMax;
    public float tempo;//tra uno sparo e l'altro
    public bool mitraglietta;
    private int colpiRimanenti;
    private GameObject colpiRimanentiLabel;
    private GameObject colpiMaxLabel;
    private System.DateTime ultimoSparo;
    private bool ricaricando;

    // Start is called before the first frame update
    void Start()
    {
        colpiRimanenti = colpiMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (colpiMaxLabel == null || colpiMaxLabel == null)
            getLabelReference();

        if (colpiRimanentiLabel.GetComponent<Text>().text != colpiRimanenti.ToString())
            colpiRimanentiLabel.GetComponent<Text>().text = colpiRimanenti.ToString();

        if (Input.GetKeyDown(KeyCode.R))
            ricarica();
    }

    public bool spara()
    {
        if (colpiRimanenti > 0 && (System.DateTime.Now - ultimoSparo).TotalSeconds >= tempo && !ricaricando)
        {
            colpiRimanenti--;
            ultimoSparo = System.DateTime.Now;
            return true;
        }
        if (colpiRimanenti <= 0 && GameManager.instance.gameSettings.ricaricaAuto)
            ricarica();
        return false;
    }

    public void ricarica()
    {
        if (colpiRimanenti < colpiMax)
        {
            StartCoroutine(aspettaERicarica());
            ricaricando = true;
        }
        else
            Debug.Log("Caricatore pieno");
    }

    private IEnumerator aspettaERicarica()
    {
        yield return new WaitForSeconds(tempoRicarica);
        colpiRimanenti = colpiMax;
        ricaricando = false;
    }

    //non lo faccio nello start perchè all'inizio sono disattivati
    private void getLabelReference()
    {
        colpiRimanentiLabel = GameObject.Find("colpiLabel");
        colpiMaxLabel = GameObject.Find("maxColpiLabel");
        colpiMaxLabel.GetComponent<Text>().text = colpiMax.ToString();
    }
}
