using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KillWindow : NetworkBehaviour
{
    public ScrollRect Scroll;
    public Transform killWindow;
    public GameObject prefab;
    private int killCount;

    private static KillWindow instance;
    public static KillWindow Instance { get => instance; }

    void Awake()
    {
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        killCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Command]
    public void CmdSetKill(string assassino, string morto)
    {
        RpcRegisterKill(assassino, morto);
    }

    [ClientRpc]
    public void RpcRegisterKill(string assassino, string morto)
    {
        killCount++;
        Debug.Log("kill: " + killCount);
        GameObject o = Instantiate(prefab, killWindow);
        o.GetComponent<Text>().text = assassino + " kill " + morto;
        //Scroll.verticalScrollbar.size = 1 / killCount;
        //Canvas.ForceUpdateCanvases();
        //if (killCount <= 5)
        //    killWindow.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1;
        //else
        //    killWindow.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}
