﻿using System.Collections;
using UnityEngine;

public class vanguardAnimController : MonoBehaviour
{
    private Animator anim;
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed; //velocità movimenti destra/sinistra
    public bool IsRunning { get; set; }
    public bool IsJumping { get; set; }
    public bool Ricaricando { get; set; }
    private float _speed;

    private int _actualCollision;
    private audio sound;

    private static readonly Vector3 ForwardDirection = new Vector3(0.3f, 0, 0.7f);
    private static readonly Vector3 RightDirection = new Vector3(0.7f, 0, -0.3f);

    //private Transform head;
    //private Transform chest;
    //private Transform spine;
    //private Transform rightHand;
    //private Transform leftHand;

    // Use this for initialization
    void Start()
    {
        //Gestione visuale (DA TESTARE)
        //private Camera cam;
        //private MouseLook mouseLook;

        // Start is called before the first frame update

        anim = GetComponent<Animator>();//Prendo il vanguardAnim con tutte le animazioni
        _speed = speed;

        anim = GetComponent<Animator>();
        //head = anim.GetBoneTransform(HumanBodyBones.Head);
        //chest = anim.GetBoneTransform(HumanBodyBones.Chest);
        //spine = anim.GetBoneTransform(HumanBodyBones.Spine);
        //rightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);
        //leftHand = anim.GetBoneTransform(HumanBodyBones.LeftHand);

        //cam = Camera.current;
        //mouseLook.Init(transform, cam.transform);

        //cam = Camera.current;
        //mouseLook.Init(transform, cam.transform);
        Cursor.lockState = CursorLockMode.Locked;
        _actualCollision = 0;

        sound = gameObject.GetComponent<audio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ChatManager.Instance.ChatAperta)
        {
            float translation = Input.GetAxis("Vertical") * _speed;
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

            translation *= Time.deltaTime;//?
            rotation *= Time.deltaTime;//?

            if (translation > 0)//Solo se vuole andare in avanti
            {
                //transform.Translate(0, 0, translation);
                transform.Translate(ForwardDirection * translation);
            }
            //transform.Rotate(0, rotation, 0);



            checkJump();
            IsJumping = checkIsJumping();   //salvo se sta saltando o no per non doverlo richiamare per ogni direzione

            if (!anim.GetBool("wantJump"))  //controllo i movimenti solo se non vuole saltare
            {
                //Vedo se ha premuto il tasto "W"
                checkMoveForward();
                checkMoveRight(rotation);
                checkMoveLeft(rotation);
                checkMoveBack(translation);
            }
        }
    }

    private void checkMoveForward()
    {
        if (Input.GetKey(KeyCode.W) == true)
        {
            //Controllo se corre    (se non sta saltando)
            if (Input.GetKey(KeyCode.LeftShift) == true && !IsJumping)
            {
                //checkJump();
                if (IsRunning == false)
                {
                    //E' la prima volta che inizio a correre, smetto quindi di camminare
                    anim.SetBool("isWalking", false);

                }
                //_speed = speed*5;
                _speed = speed * 3;
                anim.SetBool("isRunning", true);
                //Debug.Log("Corre! ");
                IsRunning = true;

                if (sound.player.isPlaying == false && IsJumping == false)
                {
                    sound.playRun();
                }


            }
            else if (Input.GetKey(KeyCode.LeftShift) == false)
            {
                //checkJump();
                if (IsRunning == true)
                {
                    //Sto correndo e ora voglio camminare, dico che smetto di correre
                    anim.SetBool("isRunning", false);
                }
                //Non corre, allora cammina
                _speed = speed;
                if (!IsJumping)
                    anim.SetBool("isWalking", true);
                //Debug.Log("Cammina! ");
                IsRunning = false;

                emettiSuonoWalk(false);

                //TODO DA SISTEMARE QUESTA CLASSE:
                //weaponsManager wm = gameObject.GetComponent<weaponsManager>();
                //wm.setWeaponToWalk();
            }
        }
        else
        {
            //E' fermo, dico che non corre
            _speed = speed;
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
            IsRunning = false;
        }
    }

    private void checkJump()
    {
        if (Input.GetKey(KeyCode.Space) == true && /*GetComponent<Rigidbody>().velocity.y == 0 &&*/ isColliding() && !IsJumping && !anim.GetBool("wantJump"))
        //salta solose non sta già saltanto, se non si sta gia muovendo in verticale e se è appoggiato a qualcosa
        {
            //Debug.Log("SALTO");

            //disattivo tutte le altre animazioni
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("walkRight", false);
            anim.SetBool("walkLeft", false);
            anim.SetBool("walkBack", false);

            anim.SetBool("wantJump", true);
            GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);

            sound.playJump();
        }
    }


    private void checkMoveRight(float orizzontale)
    {
        if (Input.GetKey(KeyCode.D) == true)
        {
            if (!IsJumping)
                anim.SetBool("walkRight", true);
            Debug.Log(RightDirection.ToString());
            transform.Translate(RightDirection * orizzontale);
            //Debug.Log("Vado a Destra!");

            emettiSuonoWalk(true);
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
            if (!IsJumping)
                anim.SetBool("walkLeft", true);
            transform.Translate(RightDirection * orizzontale);
            //Debug.Log("Vado a Sinistra");

            emettiSuonoWalk(true);
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
            if (!IsJumping)
                anim.SetBool("walkBack", true);

            transform.Translate(ForwardDirection * translation);
            //Debug.Log("Indietro");
            //Debug.Log(translation);

            emettiSuonoWalk(true);
        }
        else
        {
            anim.SetBool("walkBack", false);
        }
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

    public bool checkIsJumping()    //controlla se sta eseguendo l'animazione salto
    {
        if (anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime
               && anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))   //(cose copiate)
        {
            anim.SetBool("wantJump", false);    //imposto che non vuole più saltare perchè sta gia saltando
            return true;
        }
        else
            return false;

    }

    private void emettiSuonoWalk(bool silezioso)
    {
        //Se non salta e non sta riproducendo nessun suono
        if (sound.player.isPlaying == false && IsJumping == false)
        {
            if (silezioso == true)//Se il suono deve avere volume basso
            {
                //Utilizzato per movimenti laterali Dx e Sx e per movimento all'indietro
                sound.playMoveNoAvanti();
            }
            else
            {
                sound.playWalk();
            }

        }
    }

    public void muori(float respawTime)
    {
        respawTime -= 0.1f;
        float velocita = 4.4f / respawTime;
        anim.speed = velocita;
        anim.SetBool("isDead", true);

        StartCoroutine(muoriFerma(respawTime));
    }

    public void ricarica(float tempo)
    {
        float velocita = 3.3f / tempo;
        anim.speed = velocita;
        Ricaricando = true;
        anim.SetBool("reload", true);

        StartCoroutine(resetVelocitaAnimazione(tempo));
    }

    private IEnumerator resetVelocitaAnimazione(float ritardo)
    {
        yield return new WaitForSeconds(ritardo);
        anim.SetBool("reload", false);
        Ricaricando = false;
        anim.speed = 1;
    }

    private IEnumerator muoriFerma(float ritardo)
    {
        yield return new WaitForSeconds(ritardo);
        anim.SetBool("isDead", false);
    }
}
