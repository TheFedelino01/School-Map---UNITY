using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyController : MonoBehaviour
{
    public Transform mirinoTesta;
    public Transform manoDestra, manoSinistra;
    public Transform mirinoPosCorsa;
    public Transform mirinoPosMirando;

    public Transform chest;
    public Transform spine;

    public float mouseSensitivity = 1;

    public Transform spallaSinistra, spallaDestra, collo;
    public Transform testa;
    //public GameObject fucile;
    public Transform posizioneIdleManoSinistra;
    //public GameObject posizioneManoDestra;

    private weaponsManager weaponsManager;
    private vanguardAnimController animController;
    private Vector2 mousePosition;
    //private Transform polsoPosizioneIniziale;

    public Transform cam;
    public Transform ancoraggio;

    private bool sparando;
    private int _count = 0;
    private Transform mirinoAttuale;

    private bool toUpdate;//booleana a caso per sistemare le braccia dopo ogni sparo. NON SO PERCHE' SE RICHIAMO DIRETTAMENTE correggiPosBraccia DAL METODO SPARA NON VA:/

    class Coord
    {
        public Vector3 position { get; set; }
        public Vector3 forward { get; set; }
        public Vector3 up { get; set; }

        public Vector3 right
        {
            get
            {
                return Vector3.Cross(forward.normalized, up.normalized);
            }
        }

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

        public void toLocalUnscaled(Transform point)
        {
            position = TransformExtensions.InverseTransformPointUnscaled(point, position);
            forward = point.InverseTransformDirection(forward);
            up = point.InverseTransformDirection(up);
        }

        public void toGlobal(Transform point)
        {
            position = point.TransformPoint(position);
            forward = point.TransformDirection(forward);
            up = point.TransformDirection(up);
        }
        public void toGlobalUnscaled(Transform point)
        {
            position = TransformExtensions.TransformPointUnscaled(point, position);
            forward = point.TransformDirection(forward);
            up = point.TransformDirection(up);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //chest = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Chest);
        //spine = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine);
        weaponsManager = GetComponent<weaponsManager>();
        animController = GetComponent<vanguardAnimController>();
        manoSinistra.Translate(0, -1, 1);
        mirinoAttuale = Instantiate(mirinoTesta.gameObject).transform;
        //manoDestra.transform.SetParent(fucile.transform);
        //mettiFucileInPosizione();

        //manoDestra.transform.localPosition = posizioneManoDestra.transform.localPosition;
        //manoDestra.transform.localRotation = posizioneManoDestra.transform.localRotation;
        //manoDestra.transform.localScale = posizioneManoDestra.transform.localScale;
    }

    void Update()
    {
        //Debug.Log(mouseSensitivity);
        mouseSensitivity = GameManager.instance.gameSettings.sensibilità;
        if (!weaponsManager.Mirando)
            cam.position = ancoraggio.position;
        else
            cam.position = mirinoTesta.position;
    }


    void LateUpdate()
    {
        correggi();
        checkMouseMovement();
    }

    //sposta la camera e le braccia in base alla rotazione del mouse
    private void checkMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity / 100;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity / 100;
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

    public void movimentoSparo()
    {
        //Debug.Log("movimentoFucile");
        if (_count < 5)
        {
            mirinoAttuale.Translate(0.2f, 0.1f, 0);
            //testa.Rotate(-10, 0, 0);
            //Debug.Log("TRASLO");
        }
        else if (_count < 10)
        {
            mirinoAttuale.Translate(-0.2f, -0.1f, 0);
            //testa.Rotate(10, 0, 0);
            //Debug.Log("-TRASLO");
        }
        else
        {
            sparando = false;
            _count = 0;
        }
        _count++;
        spostaFucile(mirinoAttuale);
    }

    private void correggi()
    {
        if (toUpdate)
            correggiPosBraccia();
        if (sparando)
            movimentoSparo();
        else
            correggiPosBraccia();


        //sistemo le altre parti del corpo
        spallaDestra.Rotate(0, 15, 0);
        spallaSinistra.Rotate(0, 15, 0);
        collo.Rotate(0, 10, 0);
    }

    private void correggiPosBraccia()
    {
        //Debug.Log("correggiPosBraccia");
        if (!animController.IsRunning) //se non sta saltando o correndo sposto il fucile vicino alla testa
        {
            //fucile.transform.localRotation = new Quaternion(mouseX, mouseY, fucile.transform.rotation.z, fucile.transform.rotation.w);
            //Debug.Log("CAMERA ROTATION: " + GetComponentInChildren<Camera>().transform.localRotation.ToString());
            //Debug.Log("fucile ROTATION: " + fucile.transform.localRotation.ToString());
            //Debug.Log("Right Arm: " + manoDestra.transform.localRotation.ToString());

            if (!weaponsManager.Mirando)
            {
                //Debug.Log("1");
                spostaFucile(mirinoTesta);
                mirinoAttuale.position = mirinoTesta.position;
                mirinoAttuale.rotation = mirinoTesta.rotation;
            }
            else
            {
                //Debug.Log("2");
                spostaFucile(mirinoPosMirando);
                mirinoAttuale.position = mirinoPosMirando.position;
                mirinoAttuale.rotation = mirinoPosMirando.rotation;
            }

        }
        else //se sta correndo sposto il fucile nella posizione corsa
        {
            //Debug.Log(manoSinistra.position.ToString());
            //Debug.Log("3");
            spostaFucile(mirinoPosCorsa);
            mirinoAttuale.position = mirinoPosCorsa.position;
            mirinoAttuale.rotation = mirinoPosCorsa.rotation;
            //Debug.Log(manoSinistra.position.ToString());
        }
        toUpdate = false;
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
        try
        {
            Transform mirinoFucile = weaponsManager.getMirino();

            //posizioneStandard.transform.localRotation = new Quaternion(0.1f, 0.8f, 0.8f, -0.2f);
            //Debug.Log("PRIMA" + mirinoFucile.position);
            //Debug.Log(mirinoFucile.parent.localScale);
            //Vector3 posm = mirinoFucile.position;
            //posm.Scale(mirinoFucile.parent.localScale);
            //mirinoFucile.position = posm;
            //Debug.Log("DOPO" + posm);
            //Debug.Log("DOPO" + mirinoFucile.position);
            //manoSinistra.position = mio.position;
            Coord coordManoDestra = new Coord(manoDestra);
            Coord coordManoSinistra = new Coord(manoSinistra);
            coordManoDestra.toLocalUnscaled(mirinoFucile);
            coordManoSinistra.toLocalUnscaled(mirinoFucile);
            //Debug.Log("LOCAL: " + coordManoDestra.position);

            coordManoDestra.toGlobalUnscaled(newPosition);
            coordManoSinistra.toGlobalUnscaled(newPosition);
            //Debug.Log("GLOBAL: " + coordManoDestra.position);


            if (weaponsManager.GetWeaponType() == WeaponType.PISTOLA)
            {
                coordManoSinistra = (Coord)coordManoDestra.Clone();
                coordManoSinistra.up = -coordManoDestra.up;
                Vector3 pos = coordManoSinistra.position;
                //Debug.Log(manoSinistra.rotation.x+";"+ manoSinistra.rotation.y);
                pos.z -= 1f;
                coordManoSinistra.position = pos;
            }
            IK.ik(manoSinistra, coordManoSinistra.position, Quaternion.LookRotation(coordManoSinistra.forward, coordManoSinistra.up), 1, 1);
            IK.ik(manoDestra, coordManoDestra.position, Quaternion.LookRotation(coordManoDestra.forward, coordManoDestra.up), 1, 1);
            //manoDestra.position = coordManoDestra.position;
            //manoDestra.forward = coordManoDestra.forward;
            //manoDestra.up = coordManoDestra.up;
            //manoSinistra.position = coordManoSinistra.position;
            //manoSinistra.forward = coordManoSinistra.forward;
            //manoSinistra.up = coordManoSinistra.up;
        }
        catch (System.NullReferenceException e)
        {
            //ignored
            //arma non ancora attivata
        }
    }


    public void spara()
    {
        Debug.Log("SPARO");
        toUpdate = true;
        _count = 0;
        sparando = true;
    }
}