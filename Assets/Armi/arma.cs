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
    public WeaponType type;
    private int colpiRimanenti;
    private GameObject colpiRimanentiLabel;
    private GameObject colpiMaxLabel;
    private System.DateTime ultimoSparo;
    private bool ricaricando;
    public int ColpiRimanenti { get => colpiRimanenti; }

    // Start is called before the first frame update
    void Start()
    {
        colpiRimanenti = colpiMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (colpiMaxLabel == null || colpiMaxLabel == null)
            getLabelsReference();
        else if (colpiRimanentiLabel.GetComponent<Text>().text != colpiRimanenti.ToString())
            colpiRimanentiLabel.GetComponent<Text>().text = colpiRimanenti.ToString();
    }

    public bool spara()
    {
        if (colpiRimanenti > 0 && (System.DateTime.Now - ultimoSparo).TotalSeconds >= tempo && !ricaricando)
        {
            colpiRimanenti--;
            ultimoSparo = System.DateTime.Now;
            return true;
        }
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
    private void getLabelsReference()
    {
        colpiRimanentiLabel = GameObject.Find("colpiLabel");
        colpiMaxLabel = GameObject.Find("maxColpiLabel");
    }

    void OnEnable()
    {
        if (colpiMaxLabel != null)
            colpiMaxLabel.GetComponent<Text>().text = colpiMax.ToString();
        else
            this.EseguiAspettando(new WaitForEndOfFrame(), () =>
            {
                OnEnable();
            });
    }


}
