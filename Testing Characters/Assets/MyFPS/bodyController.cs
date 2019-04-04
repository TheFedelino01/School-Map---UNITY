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

    private vanguardAnimController animController;
    private Vector2 mousePosition;

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
        if (mousePosition.y + mouseY > -30 && mousePosition.y + mouseY < 70)
        {
            mousePosition.y += mouseY;
            GetComponentInChildren<Camera>().transform.Rotate(mouseY, 0, 0);    //se non è troppo alto o basso ruoto la CAMERA
        }
        chest.Rotate(mousePosition.y / 2f, 0, 0);
        spine.Rotate(mousePosition.y / 2f, 0, 0);
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
        if (!animController.IsJumping && !animController.IsRunning) //se non sta saltando o correndo sposto il fucile vicino alla testa
        {
            Coord coordManoDestra = new Coord(manoDestra);
            Coord coordManoSinistra = new Coord(manoSinistra);

            coordManoDestra.toLocal(mirinoFucile);
            coordManoSinistra.toLocal(mirinoFucile);

            coordManoDestra.toGlobal(mirinoTesta);
            coordManoSinistra.toGlobal(mirinoTesta);

            IK.ik(manoDestra, coordManoDestra.position, Quaternion.LookRotation(coordManoDestra.forward, coordManoDestra.up), 1, 1);
            IK.ik(manoSinistra, coordManoSinistra.position, Quaternion.LookRotation(coordManoSinistra.forward, coordManoSinistra.up), 1, 1);
            //manoDestra.position = coordTargetManoDestra.position;
            //manoDestra.forward = coordTargetManoDestra.foward;
            //manoDestra.up = coordTargetManoDestra.up;
            //manoSinistra.position = coordTargetManoSinistra.position;
            //manoSinistra.forward = coordTargetManoSinistra.foward;
            //manoSinistra.up = coordTargetManoSinistra.up;
        }
    }
}