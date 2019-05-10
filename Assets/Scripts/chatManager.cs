using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chatManager : MonoBehaviour
{
    public GameObject chatPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.partitaAvviata)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                chatPanel.SetActive(!chatPanel.active);
        }
        else if (chatPanel.active)
            chatPanel.SetActive(false);
    }
}
