using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using static UnityEngine.Input;
using UnityEngine.SceneManagement;

public class SceneRotation_p2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        print("SceneRotation_p2 loaded...");
    }

    // Update is called once per frame
    void Update() {
        OVRInput.Update();

        if(OVRInput.GetDown(OVRInput.RawButton.X)) {
            if(SceneManager.GetActiveScene().name.Equals("FireflyStart")) {
                SceneManager.LoadScene("FireflyTest");
            }
            // else if(SceneManager.GetActiveScene().name.Equals("FireflyEnd")) {
            //     SceneManager.LoadScene("FireflyStart");
            // }
            else {
                print("COULD NOT FIND A SCENE REGISTERED FOR A SCENE SWAP");
            }
        }
    }
}
