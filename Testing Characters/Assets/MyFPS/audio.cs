using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    public AudioClip step1;
    public AudioClip step2;
    public AudioClip run;
    public AudioClip jump;

    public AudioSource player;

    private bool isStep1 = true;
    // Start is called before the first frame update
    void Start()
    {
        player.clip = step1;//Inizializzo
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playRun()
    {
        player.clip = run;
        emetti();
    }

    public void playWalk()
    {
        setForSteps();

        Debug.Log("SUONO walk");

        emetti();
    }

    private void setForSteps()
    {
        //Quando cammina faccio riprodurre
        //una volta step1 e la seconda volta step2
        if (isStep1 == true)
        {
            player.clip = step1;
            isStep1 = false;
        }
        else
        {
            player.clip = step2;
            isStep1 = true;
        }
    }

    public void playJump()
    {
        Debug.Log("SUONO jump");
        player.clip = jump;

        emetti();
    }

    private void emetti()
    {
        player.volume = Random.Range(0.8f, 1f);
        //player.pitch = Random.Range(0.85f, 1.1f); ;
        player.Play();
    }

    public void playMoveNoAvanti()
    {
        //Mi muovo in laterale Dx o Sx oppure in dietro
        //la differenza con playWalk() é che in questo metodo il volume é piú basso
        setForSteps();
        Debug.Log("SUONO laterale o indietro");

        player.volume = 0.4f;
        player.Play();
    }
}
