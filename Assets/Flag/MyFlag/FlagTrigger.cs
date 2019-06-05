using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Giocatore sullo spawn!!!");
        GameObject f = other.GetComponent<Player>().capturedFlag;
        if (f != null)
            f.GetComponent<Flag>().conqueredFlag();
    }
}
