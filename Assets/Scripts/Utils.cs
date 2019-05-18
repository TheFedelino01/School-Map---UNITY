using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    /**
     * Esegue una funzione dopo un tempo prestabilito
     * Per non fare ogni volta lo StartCoroutine che è un bordello
     * 
     * @param secondi Secondi da aspettare
     * @param azione Azione da fare dopo aver aspettato
     * @return Coroutine avviata (può servire per annullarla)
     */
    public static Coroutine EseguiAspettando(this MonoBehaviour a, float secondi, Action azione)
    {
        return a.StartCoroutine(Esegui());
        IEnumerator Esegui()
        {
            yield return new WaitForSeconds(secondi);
            azione();
        }
    }

    /**
     * Esegue una funzione dopo un tempo prestabilito
     * Per non fare ogni volta lo StartCoroutine che è un bordello
     * 
     * @param attesa YieldInstruction di attesa
     * @param azione Azione da fare dopo aver aspettato
     * @return Coroutine avviata (può servire per annullarla)
     */
    public static Coroutine EseguiAspettando(this MonoBehaviour a, YieldInstruction attesa, Action azione)
    {
        return a.StartCoroutine(Esegui());
        IEnumerator Esegui()
        {
            yield return attesa;
            azione();
        }
    }

}
