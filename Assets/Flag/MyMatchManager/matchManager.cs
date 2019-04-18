using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matchManager : MonoBehaviour
{
    public int flagNumber { get; set; } //Numero di bandiere da conquistare
    public int flagConquered { get; set; } //Numero di bandiere conquistate
    
    public int attackTeamScore { get; set; } //Punteggio team d'attacco
    public int defenseTeamScore { get; set; } //Punteggio team difesa

    public bool isAttackTeamWinner { get; set; }
    public bool isDefenseTeamWinner { get; set; }

    //TO DO
    //Timer di gioco da introdurre

    // Start is called before the first frame update
    void Start()
    {
        flagNumber = 1;
        flagConquered = 0;

        attackTeamScore = 0;
        defenseTeamScore = 0;

        isAttackTeamWinner = false;
        isDefenseTeamWinner = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flagConquered == flagNumber && !isDefenseTeamWinner) //TO DO da controllare anche il tempo residuo
            isAttackTeamWinner = true;
        //else if(tempoTerminato && !isAttackTeamWinner)
            //isDefenseTeamWinner = true;

            
    }
}
