using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armsController : MonoBehaviour
{

    public Transform mirinoTesta, mirinoFucile;
    public Transform manoDestra, manoSinistra;

    public Transform chest;
    public Transform spine;

    class Coord
    {
        public Vector3 position { get; set; }
        public Vector3 foward { get; set; }
        public Vector3 up { get; set; }

        public Coord(Transform obj)
        {
            position = obj.position;
            foward = obj.forward;
            up = obj.up;
        }

        public Coord() { }
    }

    // Start is called before the first frame update
    void Start()
    {
        //chest = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Chest);
        //spine = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine);
    }
    void LateUpdate()
    {
        chest.rotation *= Quaternion.Slerp(Quaternion.identity, GetComponentInChildren<Camera>().transform.localRotation, 0.25f);
        spine.rotation *= Quaternion.Slerp(Quaternion.identity, GetComponentInChildren<Camera>().transform.localRotation, 0.25f);

        Coord coordRelativeManoDestra = new Coord
        {
            position = mirinoFucile.InverseTransformPoint(manoDestra.position),
            foward = mirinoFucile.InverseTransformDirection(manoDestra.forward),
            up = mirinoFucile.InverseTransformDirection(manoDestra.up)
        };

        Coord coordRelativeManoSinistra = new Coord
        {
            position = mirinoFucile.InverseTransformPoint(manoSinistra.position),
            foward = mirinoFucile.InverseTransformDirection(manoSinistra.forward),
            up = mirinoFucile.InverseTransformDirection(manoSinistra.up)
        };

        Coord coordTargetManoSinistra = new Coord
        {
            position = mirinoTesta.TransformPoint(coordRelativeManoSinistra.position),
            foward = mirinoTesta.TransformDirection(coordRelativeManoSinistra.foward),
            up = mirinoTesta.TransformDirection(coordRelativeManoSinistra.up)
        };

        Coord coordTargetManoDestra = new Coord
        {
            position = mirinoTesta.TransformPoint(coordRelativeManoDestra.position),
            foward = mirinoTesta.TransformDirection(coordRelativeManoDestra.foward),
            up = mirinoTesta.TransformDirection(coordRelativeManoDestra.up)
        };
        IK.ik(manoDestra, coordTargetManoDestra.position, Quaternion.LookRotation(coordTargetManoDestra.foward, coordTargetManoDestra.up), 1, 1);
        IK.ik(manoSinistra, coordTargetManoSinistra.position, Quaternion.LookRotation(coordTargetManoSinistra.foward, coordTargetManoSinistra.up), 1, 1);

        //manoDestra.position = coordTargetManoDestra.position;
        //manoDestra.forward = coordTargetManoDestra.foward;
        //manoDestra.up = coordTargetManoDestra.up;
        //manoSinistra.position = coordTargetManoSinistra.position;
        //manoSinistra.forward = coordTargetManoSinistra.foward;
        //manoSinistra.up = coordTargetManoSinistra.up;
    }
}