using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanguardAnimController : MonoBehaviour
{
    static Animator anim;
    public float speed = 2.0f;
    public float rotationSpeed = 75.0f;
    private bool isRunning = false;
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();//Prendo il vanguardAnim con tutte le animazioni
        _speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
        float translation = Input.GetAxis("Vertical")* _speed;
        float rotation = Input.GetAxis("Horizontal")*rotationSpeed;

        translation *= Time.deltaTime;//?
        rotation *= Time.deltaTime;//?

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);



        //Vedo se ha premuto il tasto "W"
        checkMove(translation);

        checkJump();

    }

    private void checkMove(float translation)
    {
        if (translation != 0)
        {

            //Controllo se corre
            if (Input.GetKey(KeyCode.LeftShift) == true)
            {
                checkJump();
                if (isRunning == false)
                {
                    //E' la prima volta che inizio a correre, smetto quindi di camminare
                    anim.SetBool("isWalking", false);
                }
                _speed = speed*10;
                anim.SetBool("isRunning", true);
                Debug.Log("Corre! " + translation);
                isRunning = true;
            }
            else if (Input.GetKey(KeyCode.LeftShift) == false)
            {
                checkJump();
                if (isRunning == true)
                {
                    //Sto correndo e ora voglio camminare, dico che smetto di correre
                    anim.SetBool("isRunning", false);
                }
                //Non corre, allora cammina
                speed = 2.0f;
                anim.SetBool("isWalking", true);
                Debug.Log("Cammina! " + translation);
                isRunning = false;
            }

        }
        else
        {
            //E' fermo, dico che non corre
            speed = 2.0f;
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
            isRunning = false;
        }
    }

    private void checkJump()
    {
        if(Input.GetKey(KeyCode.Space) == true)
        {
            anim.SetBool("wantJump", true);
        }
        else
        {
            anim.SetBool("wantJump", false);
        }
    }
}
