﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class vanguardAnimController : MonoBehaviour
{
    static Animator anim;
    public float speed = 2.0f;
    public float jumpSpeed = 500;
    public float rotationSpeed = 75.0f;
    private bool isRunning = false;
    private bool _isJumping = false;
    private float _speed;

    private int _actualCollision;
    private float _mouseY; //la salvo per non farlo muovere troppo in verticale

    //Gestione visuale (DA TESTARE)
    //private Camera cam;
    //private MouseLook mouseLook;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();//Prendo il vanguardAnim con tutte le animazioni
        _speed = speed;
        //cam = Camera.current;
        //mouseLook.Init(transform, cam.transform);

        //cam = Camera.current;
        //mouseLook.Init(transform, cam.transform);
        Cursor.lockState = CursorLockMode.Locked;
        _actualCollision = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * _speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;//?
        rotation *= Time.deltaTime;//?

        if (translation > 0)//Solo se vuole andare in avanti
        {
            transform.Translate(0, 0, translation);
            //Debug.Log(translation);
        }
        //transform.Rotate(0, rotation, 0);

        //Verifica se viene mossa la visuale
        checkMouseMovement();

        checkJump();
        _isJumping = isJumping();
        if (!anim.GetBool("wantJump"))
        {
            //Vedo se ha premuto il tasto "W"
            checkMoveForward();
            checkMoveRight(rotation);
            checkMoveLeft(rotation);
            checkMoveBack(translation);
        }
    }

    private void checkMoveForward()
    {
        if (Input.GetKey(KeyCode.W) == true)
        {
            //Controllo se corre    (se non sta saltando)
            if (Input.GetKey(KeyCode.LeftShift) == true && !_isJumping)
            {
                //checkJump();
                if (isRunning == false)
                {
                    //E' la prima volta che inizio a correre, smetto quindi di camminare
                    anim.SetBool("isWalking", false);
                }
                //_speed = speed*5;
                _speed = speed * 10;
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
                _speed = 2.0f;
                if (!_isJumping)
                    anim.SetBool("isWalking", true);
                //Debug.Log("Cammina! ");
                isRunning = false;

                audio sn = gameObject.GetComponent<audio>();
                if (sn.player.isPlaying == false)
                {
                    sn.playWalk();
                }
            }
        }
        else
        {
            //E' fermo, dico che non corre
            _speed = 2.0f;
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
            isRunning = false;
        }
    }

    private void checkJump()
    {
        if (Input.GetKey(KeyCode.Space) == true && /*GetComponent<Rigidbody>().velocity.y == 0 &&*/ isColliding() && !_isJumping && !anim.GetBool("wantJump"))
        //salta solose non sta già saltanto, se non si sta gia muovendo in verticale e se è appoggiato a qualcosa
        {
            Debug.Log(anim.GetBool("isWalking"));
            Debug.Log("SALTO");
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("wantJump", true);
            GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
        }
    }


    private void checkMoveRight(float orizzontale)
    {
        if (Input.GetKey(KeyCode.D) == true)
        {
            if (!_isJumping)
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
            if (!_isJumping)
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
            if (!_isJumping)
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


    private void checkMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        this.transform.Rotate(0, mouseX, 0);    //routo il GIOCATORE
        if (_mouseY + mouseY > -30 && _mouseY + mouseY < 70)
        {
            GetComponentInChildren<Camera>().transform.Rotate(mouseY, 0, 0);//se non è troppo alto o basso ruoto la CAMERA
            _mouseY += mouseY;
        }
        //mouseLook.LookRotation(transform, cam.transform);
    }

    void OnCollisionEnter()
    {
        _actualCollision++;
    }

    void OnCollisionExit()
    {
        _actualCollision--;
    }

    private bool isColliding()  //true se sta collidendo con qualcosa
    {
        return _actualCollision > 0;
    }

    private bool isJumping()    //controlla se sta eseguendo l'animazione salto
    {
        if (anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime
               && anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            anim.SetBool("wantJump", false);
            return true;
        }
        else
            return false;

    }

}
