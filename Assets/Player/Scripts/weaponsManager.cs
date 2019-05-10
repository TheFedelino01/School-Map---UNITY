using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsManager : MonoBehaviour
{
    public GameObject[] weapons;
    public Transform weaponPosition;
    public GameObject activeWeapon { get; set; }
    public GameObject proiettile;
    public float shootForce;

    private vanguardAnimController animController;
    private bodyController bodyController;

    public bool Mirando { get; set; }
    public int defaultFieldOfView = 60;

    private Camera cam;
    private GameObject imgMirino;
    public bool MirinoAttivo { get; set; }

    private arma activeArma;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = GameObject.Instantiate(weapons[i], weaponPosition);
            weapons[i].SetActive(false);
        }
        activeWeapon = weapons[0];
        activeWeapon.SetActive(true);
        activeArma = activeWeapon.GetComponent<arma>();
        //POSIZIONE ARMA CAMMINATA FRONTALE: 22.9 -11.1 22.3
        animController = GetComponent<vanguardAnimController>();
        bodyController = GetComponent<bodyController>();
        cam = GetComponentInChildren<Camera>();
        imgMirino = GameObject.Find("Canvas").transform.Find("imgMirino").gameObject;
        Debug.Log(imgMirino);
        Mirando = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ChatManager.Instance.ChatAperta)
        {
            if (Input.GetKeyDown(KeyCode.R))
                ricarica();
            if (Input.GetKeyDown(KeyCode.Alpha1))
                cambiaArma(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                cambiaArma(1);
            //weapon.transform.rotation = new Quaternion(-76.40601f, -340.81f, 540.438f,-3);
        }

        //se preme il pulsante destro e non sta correndo sposta la camera sul mirino
        if (Input.GetButtonDown("Fire2") && !animController.IsRunning && !animController.IsJumping)
        {
            Mirando = !Mirando;
            if (Mirando)
                cam.fieldOfView -= getZoom();
            else
                cam.fieldOfView += getZoom();
        }
        chkMirino();

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
        if (activeWeapon != weapons[index])
        {
            resetZoom();
            //Destroy(activeWeapon);
            //activeWeapon = GameObject.Instantiate(weapons[index], weaponPosition);
            Debug.Log("Cambio arma");
            activeWeapon.SetActive(false);
            activeWeapon = weapons[index];
            activeWeapon.SetActive(true);
            activeArma = activeWeapon.GetComponent<arma>();
        }
    }

    public int getZoom()
    {
        switch (activeArma.type)
        {
            case WeaponType.FUCILE: return 30;
            case WeaponType.PISTOLA: return 10;
        }
        return 0;
    }


    public bool spara()
    {
        if (activeArma.spara())
        {
            Transform shootPoint = activeWeapon.transform.Find("shootPoint");
            var proiet = Instantiate(proiettile, shootPoint.position, shootPoint.rotation);
            proiet.GetComponent<Rigidbody>().AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
            bodyController.spara();
            Destroy(proiet.gameObject, 3);
            return true;
        }

        if (activeArma.ColpiRimanenti <= 0 && GameManager.instance.gameSettings.ricaricaAuto)
            ricarica();
        return false;
    }


    public void resetZoom()
    {
        if (Mirando)
        {
            //Debug.Log("RESETTO zoom");
            cam.fieldOfView = defaultFieldOfView;
        }
        Mirando = false;
    }


    private void chkMirino()
    {
        if (animController.IsJumping || animController.IsRunning)
            resetZoom();
    }

    private void ricarica()
    {
        animController.ricarica(activeArma.tempoRicarica);
        activeArma.ricarica();
    }
}


public enum WeaponType
{
    FUCILE, PISTOLA
}