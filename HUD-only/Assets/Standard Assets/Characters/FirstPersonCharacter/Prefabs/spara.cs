using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spara : MonoBehaviour
{

    public GameObject prefabProiettile;
    public float speed = 6000f;
    public GameObject explosion;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            //Lo sparò verrà originato nello SpawnPoint             
            // che va messo dentro la Cam, altrimenti non rileva le inclinazioni su/giu 

            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            //new GameObject(...)
            //Il proiettile generato
            GameObject nuovoPr = (GameObject)Instantiate(prefabProiettile,
                spawnPoint.transform.position, this.transform.rotation);

            //Aggiungo alcune caratteristiche al proiettile
            nuovoPr.GetComponent<Rigidbody>().AddForce(transform.forward * speed,
                ForceMode.Impulse);

            Debug.Log("Sparato");
            shoot();
        }

    }


    private void shoot()
    {
        Camera cam = Camera.main;//Prendo la camera FPS
        //Prendo la linea in mezzo allo schermo
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        //Controllo se colpisco qualcosa
        if(Physics.Raycast(ray,out hit))
        {
            //Vedo se è un nemico
            if (hit.collider.tag.ToUpper() == "PlayerREMOTE")
            {
                Destroy(hit.collider.gameObject);
                login.punti++;

                //Posiziono l'esplosione sulle cordinate dell'oggetto colpito
                GameObject esplosione = (GameObject)Instantiate(explosion,
                hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);

            }
        }
    }


}
