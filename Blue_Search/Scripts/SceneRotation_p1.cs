using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;//Needed for writing log files.
// using System;
using static UnityEngine.Input;
// using static UnityEngine.Input.Button;
using UnityEngine.SceneManagement;
// using static OVRManager;
// using OVRPlugin;

public class SceneRotation_p1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        print("Loaded...");

        // if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown("space")) {
        //     GameObject.Find("test").GetComponent<PrimitiveType.Cube>().levelSelection = hitObject.transform.name;
        //     SceneManager.LoadSceneAsync("Loading");
        // }
    }

    // Update is called once per frame
    void Update() {
        OVRInput.Update();

        if(OVRInput.GetDown(OVRInput.RawButton.A)) {
            print("Button press detected on scene " + SceneManager.GetActiveScene().name);
            if(SceneManager.GetActiveScene().name.Equals("StartingScreen")) {
                SceneManager.LoadScene("Red-Green");
            }
            else if(SceneManager.GetActiveScene().name.Equals("Red-Green")) {
                SceneManager.LoadScene("Blue-Yellow");
            }
            else if(SceneManager.GetActiveScene().name.Equals("Blue-Yellow")) {
                SceneManager.LoadScene("StartingScreen");
            }           
        }

        // print("CANT FIND BUTTON PRESS:" + OVRInput.RawButton.A);
        // System.Threading.Thread.Sleep(10000);
        // SceneManager.LoadScene("Red-Green");
    }
}
