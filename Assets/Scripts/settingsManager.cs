﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsManager : MonoBehaviour
{
    public GameObject sliderSensibility;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateSensibility()
    {
        Debug.Log("LETTA: " + sliderSensibility.GetComponent<Slider>().value);
        GameManager.instance.gameSettings.sensibilità = (int)sliderSensibility.GetComponent<Slider>().value;
    }
}