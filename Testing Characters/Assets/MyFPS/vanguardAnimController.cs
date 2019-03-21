using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanguardAnimController : MonoBehaviour
{
    static Animator anim;
    public float speed = 2.0f;
    public float jumpSpeed = 5;
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

        if (translation > 0)//Solo se vuole andare in avanti
        {
            transform.Translate(0, 0, translation);
            Debug.Log(translation);
        }
        //transform.Rotate(0, rotation, 0);



        //Vedo se ha premuto il tasto "W"
        checkMoveForward();

        checkJump();

        checkMoveRight(rotation);
        checkMoveLeft(rotation);


        checkMoveBack(translation);

    }

    private void checkMoveForward()
    {
        if (Input.GetKey(KeyCode.W) == true)
        {

            //Controllo se corre
            if (Input.GetKey(KeyCode.LeftShift) == true)
            {
                //checkJump();
                if (isRunning == false)
                {
                    //E' la prima volta che inizio a correre, smetto quindi di camminare
                    anim.SetBool("isWalking", false);
                }
                _speed = speed*10;
                anim.SetBool("isRunning", true);
                Debug.Log("Corre! ");
                isRunning = true;
            }
            else if (Input.GetKey(KeyCode.LeftShift) == false)
            {
                //checkJump();
                if (isRunning == true)
                {
                    //Sto correndo e ora voglio camminare, dico che smetto di correre
                    anim.SetBool("isRunning", false);
                }
                //Non corre, allora cammina
                speed = 2.0f;
                anim.SetBool("isWalking", true);
                Debug.Log("Cammina! ");
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
            //TODO DA SISTEMARE IL SALTO
           // transform.Translate(0, jumpSpeed, 0);
        }
        else
        {
            anim.SetBool("wantJump", false);
        }
    }


    private void checkMoveRight(float orizzontale)
    {
        if (Input.GetKey(KeyCode.D) == true)
        {
            anim.SetBool("walkRight", true);
            transform.Translate(orizzontale, 0, 0);
            Debug.Log("Vado a Destra!");
        }
        else
        {
            anim.SetBool("walkRight", false);
        }
    }

    private void checkMoveLeft(float orizzontale)
    {
        if (Input.GetKey(KeyCode.A) == true)
        {
            anim.SetBool("walkLeft", true);
            transform.Translate(orizzontale, 0, 0);
            Debug.Log("Vado a Sinistra");
        }
        else
        {
            anim.SetBool("walkLeft", false);
        }
    }

    private void checkMoveBack(float translation)
    {
        if (Input.GetKey(KeyCode.S) == true)
        {
            anim.SetBool("walkBack", true);
            
            transform.Translate(0, 0, translation);
            Debug.Log("Indietro");
            Debug.Log(translation);
        }
        else
        {
            anim.SetBool("walkBack", false);
        }
    }
}
