using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class LAN_PlayerSpawn : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            //disattivo tutti i componenti per controllare il giocatore
            this.gameObject.GetComponent<FirstPersonController>().enabled = false;
            this.gameObject.GetComponent<CharacterController>().enabled = false;
            this.gameObject.GetComponent<AudioBehaviour>().enabled = false;
            this.gameObject.GetComponent<movement>().enabled = false;
            this.gameObject.GetComponentInChildren<Camera>().enabled = false;
            this.gameObject.GetComponentInChildren<AudioBehaviour>().enabled = false;
            this.gameObject.GetComponentInChildren<FlareLayer>().enabled = false;
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
