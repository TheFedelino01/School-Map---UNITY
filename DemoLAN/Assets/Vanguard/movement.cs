using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //Inizializzo l'animazione
        //anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Debug.Log("Schiacciato salta");
            anim.CrossFade("Jump", 0.2f);
        }
        else
        {
            //anim.SetBool("jump", false);
        }
    }
}
