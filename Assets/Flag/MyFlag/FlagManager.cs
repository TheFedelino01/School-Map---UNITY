using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PENSAVO DI USARLO MA HO CAMBIATO IDEA E HO FATTO IN UN ALTRO MODO
//LO LASCIO PERCHE' MAGARI SERVIRA'
public class FlagManager : MonoBehaviour
{
    private static FlagManager instance;
    public static FlagManager Instance { get => instance; }


    public Flag[] flags;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Flag getFlag(int id)
    {
        foreach (Flag f in flags)
            if (f.flagId == id)
                return f;
        throw new System.ArgumentException("Bandiera inesistente");
    }

}
