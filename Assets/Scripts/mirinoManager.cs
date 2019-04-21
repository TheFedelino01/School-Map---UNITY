using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mirinoManager : MonoBehaviour
{
    public bool ForzaDisattivazione { get; set; }
    private RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        ForzaDisattivazione = false;
        rawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ForzaDisattivazione)
        {
            bool partitaAvviata = GameManager.instance.partitaAvviata;
            if (rawImage.enabled != partitaAvviata)
                rawImage.enabled = partitaAvviata;
        }
        else if (rawImage.enabled)
            rawImage.enabled = false;
    }
}
