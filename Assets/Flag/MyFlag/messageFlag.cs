using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messageFlag : MonoBehaviour
{
    public GameObject bandieraPersaTxt;
    public GameObject bandieraCatturataTxt;
    public GameObject bandieraConquistataTxt;
    public GameObject bandieraRecuparata;
    public int tempoVisualizzazioneMsg = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showMessagePresa()
    {
        StartCoroutine(showBandieraTxtPresa());
    }


    public IEnumerator showBandieraTxtPresa()
    {
        bandieraCatturataTxt.SetActive(true);
        yield return new WaitForSeconds(tempoVisualizzazioneMsg);
        bandieraCatturataTxt.SetActive(false);

    }

    public void showMessagePersa()
    {
        StartCoroutine(showMessageTxtPersa());
    }

    public IEnumerator showMessageTxtPersa()
    {
        bandieraPersaTxt.SetActive(true);
        yield return new WaitForSeconds(tempoVisualizzazioneMsg);
        bandieraPersaTxt.SetActive(false);
    }

    public void showMessageConquistata()
    {
        bandieraConquistataTxt.SetActive(true);
        this.EseguiAspettando(tempoVisualizzazioneMsg, () =>
        {
            bandieraConquistataTxt.SetActive(false);
        });
    }

    public void showMessageRecuperata()
    {
        bandieraRecuparata.SetActive(true);
        this.EseguiAspettando(tempoVisualizzazioneMsg, () =>
        {
            bandieraRecuparata.SetActive(false);
        });
    }
}
