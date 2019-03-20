using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanguardAnimController : MonoBehaviour
{
    static Animator anim;
    public float speed = 2.0f;
    public float rotationSpeed = 75.0f; 
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();//Prendo il vanguardAnim con tutte le animazioni
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical")*speed;
        float rotation = Input.GetAxis("Horizontal")*rotationSpeed;

        translation *= Time.deltaTime;//?
        rotation *= Time.deltaTime;//?

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (translation != 0)//Non si sta muovendo
        {
            anim.SetBool("isWalking", true);
            Debug.Log("Cammina! "+ translation);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

    }
}
