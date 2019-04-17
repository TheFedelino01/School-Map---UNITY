using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyController : MonoBehaviour
{
    public Transform mirinoTesta, mirinoFucile;
    public Transform manoDestra, manoSinistra;

    public Transform chest;
    public Transform spine;

    public float mouseSensitivity = 1;

    public Transform spallaSinistra, spallaDestra, collo;
    public GameObject fucile;
    public GameObject posizioneStandard;
    public GameObject posizioneManoDestra;


    private vanguardAnimController animController;
    private Vector2 mousePosition;
    private Transform polsoPosizioneIniziale;

    public Transform cam;
    public Transform ancoraggio;

    private bool mirando = false;

    class Coord
    {
        public Vector3 position { get; set; }
        public Vector3 forward { get; set; }
        public Vector3 up { get; set; }

        public Coord(Transform obj)
        {
            position = obj.position;
            forward = obj.forward;
            up = obj.up;
        }

        public Coord() { }

        public void toLocal(Transform point)
        {
            position = point.InverseTransformPoint(position);
            forward = point.InverseTransformDirection(forward);
            up = point.InverseTransformDirection(up);
        }

        public void toGlobal(Transform point)
        {
            position = point.TransformPoint(position);
            forward = point.TransformDirection(forward);
            up = point.TransformDirection(up);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //chest = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Chest);
        //spine = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine);
        animController = GetComponent<vanguardAnimController>();
        manoSinistra.Translate(0, -1, 1);
        //manoDestra.transform.SetParent(fucile.transform);
        //mettiFucileInPosizione();

        //manoDestra.transform.localPosition = posizioneManoDestra.transform.localPosition;
        //manoDestra.transform.localRotation = posizioneManoDestra.transform.localRotation;
        //manoDestra.transform.localScale = posizioneManoDestra.transform.localScale;
    }

    void Update()
    {
        //se preme il pulsante destro sposta la camera sul mirino
        if (Input.GetButtonDown("Fire2"))
            mirando = !mirando;
        if (mirando)
            cam.position = mirinoTesta.position;
        else
            cam.position = ancoraggio.position;
    }

    void LateUpdate()
    {
        correggiPosBraccia();
        checkMouseMovement();
    }

    //sposta la camera e le braccia in base alla rotazione del mouse
    private void checkMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        this.transform.Rotate(0, mouseX, 0);    //routo il GIOCATORE
        if (mousePosition.y + mouseY > -60 && mousePosition.y + mouseY < 60)
        {
            mousePosition.y += mouseY;
            cam.transform.Rotate(mouseY, 0, 0);    //se non è troppo alto o basso ruoto la CAMERA
        }
        chest.Rotate(mousePosition.y / 2f, 0, 0);
        spine.Rotate(mousePosition.y / 2f, 0, 0);
        spine.Rotate(0, 10, 0);
        mousePosition.x += mouseX;

        //spallaDestra.transform.Rotate(mousePosition.y, 0, 0);
        //spallaSinistra.transform.Rotate(mousePosition.y, 0, 0);
        //collo.transform.Rotate(mousePosition.y - 25, 0, 0);

        //chest.rotation *= Quaternion.Slerp(Quaternion.identity, GetComponentInChildren<Camera>().transform.rotation, 0.25f);
        //spine.rotation *= Quaternion.Slerp(Quaternion.identity, GetComponentInChildren<Camera>().transform.rotation, 0.25f);
        //mouseLook.LookRotation(transform, cam.transform);
    }

    private void correggiPosBraccia()
    {
        //mettiFucileInPosizione();

        if (!animController.IsJumping && !animController.IsRunning) //se non sta saltando o correndo sposto il fucile vicino alla testa
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
            //fucile.transform.localRotation = new Quaternion(mouseX, mouseY, fucile.transform.rotation.z, fucile.transform.rotation.w);
            Debug.Log("CAMERA ROTATION: " + GetComponentInChildren<Camera>().transform.localRotation.ToString());
            Debug.Log("fucile ROTATION: " + fucile.transform.localRotation.ToString());
            Debug.Log("Right Arm: " + manoDestra.transform.localRotation.ToString());

            spostaFucile(mirinoTesta);

            //sistemo le altre parti del corpo
            spallaDestra.Rotate(0, 15, 0);
            spallaSinistra.Rotate(0, 15, 0);
            collo.Rotate(0, 10, 0);
        }
    }

    //private void mettiFucileInPosizione()
    //{
    //    //Dico che il fucile deve avere le stesse posizioni del mio posizioneStandard 
    //    fucile.transform.localPosition = posizioneStandard.transform.localPosition;
    //    fucile.transform.localRotation = posizioneStandard.transform.localRotation;
    //    fucile.transform.localScale = posizioneStandard.transform.localScale;
    //}

    private void spostaFucile(Transform newPosition)
    {
        //posizioneStandard.transform.localRotation = new Quaternion(0.1f, 0.8f, 0.8f, -0.2f);

        //manoSinistra.position = mio.position;
        Coord coordManoDestra = new Coord(manoDestra);
        Coord coordManoSinistra = new Coord(manoSinistra);
        coordManoDestra.toLocal(mirinoFucile);
        coordManoSinistra.toLocal(mirinoFucile);

        coordManoDestra.toGlobal(newPosition);
        coordManoSinistra.toGlobal(newPosition);

        IK.ik(manoDestra, coordManoDestra.position, Quaternion.LookRotation(coordManoDestra.forward, coordManoDestra.up), 1, 1);
        IK.ik(manoSinistra, coordManoSinistra.position, Quaternion.LookRotation(coordManoSinistra.forward, coordManoSinistra.up), 1, 1);
        //manoDestra.position = coordManoDestra.position;
        //manoDestra.forward = coordManoDestra.forward;
        //manoDestra.up = coordManoDestra.up;
        //manoSinistra.position = coordManoSinistra.position;
        //manoSinistra.forward = coordManoSinistra.forward;
        //manoSinistra.up = coordManoSinistra.up;

    }
}