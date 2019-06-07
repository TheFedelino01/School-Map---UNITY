using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Timer : NetworkBehaviour
{
    public int durataMatch { get; set; }
    public float startTime { get; set; }
    private bool started = false;


    // Start is called before the first frame update
    void Start()
    {
        durataMatch = matchManager.Instance.durataMatch;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            int seconds = (int)(Time.realtimeSinceStartup - startTime);
            int secRimanenti = durataMatch - seconds;
            matchManager.Instance.RpcUpdateTimers (secRimanenti);
        }
    }



    [Server]
    public void StartTimer()
    {
        started = true;
    }
}
