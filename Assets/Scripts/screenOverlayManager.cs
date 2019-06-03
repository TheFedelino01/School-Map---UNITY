using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenOverlayManager : MonoBehaviour
{
    public bool ForzaDisattivazione { get; set; }
    public GameObject[] childsToDisable;
    // Start is called before the first frame update
    void Start()
    {
        ForzaDisattivazione = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ForzaDisattivazione)
        {
            bool partitaAvviata = GameManager.Instance.partitaAvviata;
            foreach (GameObject g in childsToDisable)
                if (g.active != partitaAvviata)
                    g.SetActive(partitaAvviata);
        }
        else foreach (GameObject g in childsToDisable)
                if (g.active)
                    g.SetActive(false);
    }
}
