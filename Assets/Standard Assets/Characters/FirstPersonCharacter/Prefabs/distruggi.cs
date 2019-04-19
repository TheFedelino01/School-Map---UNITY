using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distruggi : MonoBehaviour {

    public float tempo = 2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(this.gameObject, tempo);
	}
}
