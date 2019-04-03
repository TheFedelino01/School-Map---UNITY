using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private bool isFlagCaptured;

    void Start()
    {
        isFlagCaptured = false;
        Debug.Log("Start");
    }

    void OnTriggerEnter(Collider other)
    {
        
        isFlagCaptured = true;
        Debug.Log("Bandiera catturata !!");

        //TO DO
    }
}
