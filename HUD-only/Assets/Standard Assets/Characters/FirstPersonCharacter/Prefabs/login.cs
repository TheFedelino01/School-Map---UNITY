using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login : MonoBehaviour {

    public Texture2D menuImage;
    public Texture2D mirino;
    static public int punti = 0;
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(10, 10, 100, 20), menuImage); //new Rect(10, 10, menuImage.width, menuImage.height)
        GUI.Window(1, new Rect(10, 50, 100, 150), menuWindow, "Menù");
        GUI.DrawTexture(new Rect(Screen.width / 2f, Screen.height / 2f,
                        10f, 10f), mirino);
    }
    void menuWindow(int id)
    {
        if (GUILayout.Button("TEST"))
            Debug.Log("Hai premuto il pulsante TEST!");
        GUILayout.Label("Punti: " + punti);
        GUILayout.Label("Vite: infinite");

        if (GUILayout.Button("ESCI"))
            Application.Quit();
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
