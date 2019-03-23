using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsManager : MonoBehaviour
{
    public GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GameObject.Find("M4_Carbine");
        //POSIZIONE ARMA CAMMINATA FRONTALE: 22.9 -11.1 22.3
    }

    // Update is called once per frame
    void Update()
    {
        
        //weapon.transform.rotation = new Quaternion(-76.40601f, -340.81f, 540.438f,-3);
    }

    public void setWeaponToWalk()
    {
        //TODO DA CAPIRE COME POSIZIONARE CORRETTAMENTE L'ARMA
        weapon.transform.position = new Vector3(22.9f, -11.1f, 22.3f);
        Debug.Log("Imposto arma su Walk");
    }
}
