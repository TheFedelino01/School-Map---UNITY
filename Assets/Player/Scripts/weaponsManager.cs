using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsManager : MonoBehaviour
{
    public GameObject[] weapons;
    public Transform weaponPosition;
    private GameObject activeWeapon;
    private int activeIndex;
    public GameObject proiettile;
    public float shootForce;


    // Start is called before the first frame update
    void Start()
    {
        activeWeapon = GameObject.Instantiate(weapons[0], weaponPosition);
        //POSIZIONE ARMA CAMMINATA FRONTALE: 22.9 -11.1 22.3
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            cambiaArma(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            cambiaArma(1);
        //weapon.transform.rotation = new Quaternion(-76.40601f, -340.81f, 540.438f,-3);
    }

    //public void setWeaponToWalk()
    //{
    //    //TODO DA CAPIRE COME POSIZIONARE CORRETTAMENTE L'ARMA
    //    activeWeapon.transform.position = new Vector3(22.9f, -11.1f, 22.3f);
    //    //Debug.Log("Imposto arma su Walk");
    //}

    public Transform getMirino()
    {
        return activeWeapon.transform.Find("mirino");
    }

    private void cambiaArma(int index)
    {
        //DISTRUGGE QUELLA VECCHIA E NE CREA UNA NUOVA
        //TODO ANIMAZIONI CAMBIO ARMA
        if (activeIndex != index)
        {
            GetComponent<bodyController>().resetZoom();
            Destroy(activeWeapon);
            activeWeapon = GameObject.Instantiate(weapons[index], weaponPosition);
            activeIndex = index;
        }
    }

    public int getZoom()
    {
        switch (GetWeaponType())
        {
            case WeaponType.FUCILE: return 30;
            case WeaponType.PISTOLA: return 10;
        }
        return 0;
    }

    public WeaponType GetWeaponType()
    {
        switch (activeIndex)
        {
            case 0:
                return WeaponType.FUCILE;
            case 1:
                return WeaponType.PISTOLA;
            default:
                return WeaponType.FUCILE;
        }
    }

    public void spara()
    {
        Transform shootPoint = activeWeapon.transform.Find("shootPoint");
        var proiet = Instantiate(proiettile, shootPoint.position, shootPoint.rotation);
        proiet.GetComponent<Rigidbody>().AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        Destroy(proiet.gameObject, 3);
    }
}


public enum WeaponType
{
    FUCILE, PISTOLA
}