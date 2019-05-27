using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public float respawnTime = 4f;
    public float saluteMax = 100f;
    public bool ricaricaAuto { get; set; }

    public int sensibilità { get; set; }


    public GameSettings()
    {
        sensibilità = 60;
    }



}
