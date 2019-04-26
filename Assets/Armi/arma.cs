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

    // Start is called before the first frame update
    void Start()
    {
        colpiRimanenti = colpiMax;
        colpiRimanentiLabel = GameObject.Find("colpiLabel");
        colpiMaxLabel = GameObject.Find("maxColpiLabel");
        colpiMaxLabel.GetComponent<Text>().text = colpiMax.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (colpiRimanentiLabel.GetComponent<Text>().text != colpiRimanenti.ToString())
            colpiRimanentiLabel.GetComponent<Text>().text = colpiRimanenti.ToString();
    }

    public bool spara()
    {
        if (colpiRimanenti > 0 && (System.DateTime.Now - ultimoSparo).TotalSeconds >= tempo)
        {
            colpiRimanenti--;
            ultimoSparo = System.DateTime.Now;
            return true;
        }
        if (colpiRimanenti <= 0)
            ricarica();
        return false;
    }

    public void ricarica()
    {
        StartCoroutine(aspettaERicarica());
    }

    private IEnumerator aspettaERicarica()
    {
        yield return new WaitForSeconds(tempoRicarica);
        colpiRimanenti = colpiMax;
    }
}
