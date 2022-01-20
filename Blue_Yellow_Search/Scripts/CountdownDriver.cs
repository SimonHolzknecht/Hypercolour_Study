using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownDriver : MonoBehaviour
{
    private TextMeshPro countDown;
    private float bufferStart = 0f;
    private bool correctTest = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // TestDriver_p2 driver = GameObject.Find("Permanent_Scripts").GetComponent<TestDriver_p2>();
        correctTest = TestDriver_Hypercolour.lastInstanceWasCorrect;

        if(TestDriver_Hypercolour.testInstance >= 30)
            SceneManager.LoadScene("FireflyEnd");

        GameObject obj = new GameObject("Counter_Object");
        obj.transform.position = new Vector3 (0f,15f, 100f);
        countDown = obj.AddComponent<TextMeshPro>();

        countDown.text = "";

        // GameObject example = GameObject.Instantiate(GameObject.Find("Firefly_Template"));
        // example.GetComponent<Renderer>().material.color = new Color(255,0,0,1);
        // example.SetActive(true);

        //APPLY COLOUR TO HYPERCOLOUR
        if(TestDriver_Hypercolour.testInstance < 15) {
            GameObject.Find("left").GetComponent<Renderer>().material.color = new Color(0,0,255,1);
            GameObject.Find("right").GetComponent<Renderer>().material.color = new Color(255,255,0,1);
        }
        else {
            GameObject.Find("right").GetComponent<Renderer>().material.color = new Color(0,0,255,1);
            GameObject.Find("left").GetComponent<Renderer>().material.color = new Color(255,255,0,1);
        }
        

        bufferStart = Time.time;

        if(TestDriver_Hypercolour.testInstance > 0)
            correctTest = TestDriver_Hypercolour.spawnIndicators[TestDriver_Hypercolour.testInstance-1];
        else
            correctTest = true;
    }

    // Update is called once per frame
    void Update()
    {
        //display correct or incorrect
        if(correctTest)
            countDown.text = "Target was present \n\n" + getTimer() + "\n\nLook for the following colour:";
        else
            countDown.text = "Target was NOT present \n\n" + getTimer() + "\n\nLook for the following colour:";



        if(Time.time - bufferStart > 3f) {
            SceneManager.LoadScene("FireflyTest");
        }
    }

    private string getTimer() {
        return "" + ((bufferStart + 3f - Time.time) - ((bufferStart+3f-Time.time) % 0.1));
    }
}
