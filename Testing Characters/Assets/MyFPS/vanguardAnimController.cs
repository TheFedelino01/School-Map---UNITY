using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class vanguardAnimController : MonoBehaviour
{
    static Animator anim;
    public float speed = 2.0f;
    public float jumpSpeed = 500;
    public float rotationSpeed = 75.0f; //velocità movimenti destra/sinistra
    public float mouseSensitivity = 1;
    private bool isRunning = false;
    private bool _isJumping = false;
    private float _speed;

    private int _actualCollision;
    private float _mouseY; //la salvo per non farlo muovere troppo in verticale
    private audio sound;

    public GameObject spallaSinistra, spallaDestra, collo;



    private Transform head;
    private Transform chest;
    private Transform spine;
    private Transform rightHand;
    private Transform leftHand;
    public Transform cam;
    public Transform ancoraggio;

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
        head = anim.GetBoneTransform(HumanBodyBones.Head);
        chest = anim.GetBoneTransform(HumanBodyBones.Chest);
        spine = anim.GetBoneTransform(HumanBodyBones.Spine);
        rightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);
        leftHand = anim.GetBoneTransform(HumanBodyBones.LeftHand);

        

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
        //head.rotation = cam.rotation;
        cam.position = ancoraggio.position;

        float translation = Input.GetAxis("Vertical") * _speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;//?
        rotation *= Time.deltaTime;//?

        if (translation > 0)//Solo se vuole andare in avanti
        {
            transform.Translate(0, 0, translation);
        }
        //transform.Rotate(0, rotation, 0);



        checkJump();
        _isJumping = isJumping();   //salvo se sta saltando o no per non doverlo richiamare per ogni direzione

        if (!anim.GetBool("wantJump"))  //controllo i movimenti solo se non vuole saltare
        {
            //Vedo se ha premuto il tasto "W"
            checkMoveForward();
            checkMoveRight(rotation);
            checkMoveLeft(rotation);
            checkMoveBack(translation);
        }
    }

    void LateUpdate()
    {
        //Verifica se viene mossa la visuale dopo che sono state applicate le animazioni per muovere le braccia
        checkMouseMovement();
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
                _speed = speed * 5;
                anim.SetBool("isRunning", true);
                Debug.Log("Corre! ");
                isRunning = true;

                if (sound.player.isPlaying == false && _isJumping == false)
                {
                    sound.playRun();
                }


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
                _speed = speed;
                if (!_isJumping)
                    anim.SetBool("isWalking", true);
                //Debug.Log("Cammina! ");
                isRunning = false;

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
            isRunning = false;
        }
    }

    private void checkJump()
    {
        if (Input.GetKey(KeyCode.Space) == true && /*GetComponent<Rigidbody>().velocity.y == 0 &&*/ isColliding() && !_isJumping && !anim.GetBool("wantJump"))
        //salta solose non sta già saltanto, se non si sta gia muovendo in verticale e se è appoggiato a qualcosa
        {
            Debug.Log("SALTO");

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
            if (!_isJumping)
                anim.SetBool("walkRight", true);
            transform.Translate(orizzontale, 0, 0);
            Debug.Log("Vado a Destra!");

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
            if (!_isJumping)
                anim.SetBool("walkLeft", true);
            transform.Translate(orizzontale, 0, 0);
            Debug.Log("Vado a Sinistra");

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
            if (!_isJumping)
                anim.SetBool("walkBack", true);

            transform.Translate(0, 0, translation);
            Debug.Log("Indietro");
            Debug.Log(translation);

            emettiSuonoWalk(true);
        }
        else
        {
            anim.SetBool("walkBack", false);
        }
    }


    private void checkMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        this.transform.Rotate(0, mouseX, 0);    //routo il GIOCATORE
        if (_mouseY + mouseY > -30 && _mouseY + mouseY < 70)
        {
            GetComponentInChildren<Camera>().transform.Rotate(mouseY, 0, 0);//se non è troppo alto o basso ruoto la CAMERA
            _mouseY += mouseY;
        }
        //GetComponent<Transform>().GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).Rotate(_mouseY, 0, 0);         //spalla sinistra
        //GetComponent<Transform>().GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).Rotate(_mouseY, 0, 0);         //spalla destra      
        spallaDestra.transform.Rotate(_mouseY, 0, 0);
        spallaSinistra.transform.Rotate(_mouseY, 0, 0);
        collo.transform.Rotate(_mouseY - 25, 0, 0);
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
        if (sound.player.isPlaying == false && _isJumping == false)
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

}
