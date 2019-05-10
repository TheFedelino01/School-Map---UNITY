using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatServer : NetworkBehaviour
{
    private static int idCount;


    // Start is called before the first frame update
    void Start()
    {
        idCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void RegisterMessage(ref Message m)
    {
        m.SetId(idCount++);
    }


}
