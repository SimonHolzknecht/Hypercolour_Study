using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.IO; //Needed for writing log files.
using UnityEngine.SceneManagement;
using System.Diagnostics;


public class Logging : MonoBehaviour
{

    public GameObject Scripts;
    public bool loggingEnabled = false;

    private string logMsg;
    private string logMsg2;
    private string path;

    private List<string> sceneHistory = new List<string>();

    private bool isDone = false;
    private bool isPressed = false;
    private string prev = "";
    private string current = "";
    private string dateTime = "";
    private string elapsedTime = "";
    private Stopwatch stopWatch;
    private TimeSpan ts;
    private TestDriver_Hypercolour driver = new TestDriver_Hypercolour();
    private bool[] spawnIndicators;
    private int instanceCount = 0;


    //Use this for initialization
    void Start()
    {
        //Create logs
        System.DateTime theTime = System.DateTime.Now;
        dateTime = theTime.ToString("dd-MM-yyyy\\ HH:mm:ss");
        string colon = "(:\\.?)";
        string theDate = Regex.Replace(dateTime, colon, string.Empty);
        // spawnIndicators =  driver.getSpawnIndicators();
        //Path of the files
        // path = "Recorded Metrics/" + System.DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss") + ".txt";
        path = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss") + ".txt");

        //Save starting time 
        if(SceneManager.GetActiveScene().name.Equals("FireflyStart")){
            UnityEngine.Debug.Log("Start Date and Time: " + System.DateTime.Now.ToString("dd-MM-yyyy\\ HH:mm:ss"));
            File.WriteAllText(path, "Search Test Start Date and Time: " + System.DateTime.Now.ToString("dd-MM-yyyy\\ HH:mm:ss") + ",\n");
        }




        //Create log files with headers
        // File.WriteAllText(path, "Record; Time Stamp; TimeDelta(ms); Create Link; Delete Link; Grabbing; Rotation; Selected; action; pointer; hitObject name; hitObject texture; hitObject position\n");
        // Instantiate(pointerCursor);
        // pointerCursor.GetComponent<Renderer>().enabled = false;

     }

    
    // Update is called once per frame
    void Update()
    {
        spawnIndicators =  TestDriver_Hypercolour.spawnIndicators;
        instanceCount = TestDriver_Hypercolour.testInstance-1;

        current = SceneManager.GetActiveScene().name;
        isPressed = false;
        //Detecting a new scene and saving times 
        if(!current.Equals(prev)){
            if(!prev.Equals("") && !prev.Equals("Buffer") && !prev.Equals("FireflyStart")){
                UnityEngine.Debug.Log(prev + " ended.");
                File.AppendAllText(path, prev + " Target present: " + spawnIndicators[instanceCount] + ",\n");
                File.AppendAllText(path, prev + " End Time: " + DateTime.Now.ToString("HH:mm:ss.ff tt") + ",\n");
                stopWatch.Stop();
                ts = stopWatch.Elapsed;
                elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
                UnityEngine.Debug.Log("RunTime " + elapsedTime);
                if(ts.Seconds > (float) 3 && isPressed == true){
                    File.AppendAllText(path, "Timedout: True" + ",\n");
                }

                File.AppendAllText(path, prev+ ": "+ instanceCount + " Run time: " + elapsedTime + ",\n");
                stopWatch.Reset();

            }
            stopWatch = new Stopwatch();
            stopWatch.Start();
            

            UnityEngine.Debug.Log(current + " is active.");

            //Saving current scene start time.
            if(!current.Equals("Buffer")){
                if(current.Equals("FireflyTest")){
                    File.AppendAllText(path, current + ": " + (instanceCount+1) + " Start Time: " + DateTime.Now.ToString("HH:mm:ss.ff tt") + ",\n");
                } else{ 
                    File.AppendAllText(path, current + " Start Time: " + DateTime.Now.ToString("HH:mm:ss.ff tt") + ",\n");

                }
            }

            
        }


        // //Check if hypercolour is active and store search period time
        // if(GameObject.FindGameObjectWithTag("Ball").activeSelf == true && isActive == false){
        //     isActive = true;
        //     UnityEngine.Debug.Log("Search Period Start: " + DateTime.Now.ToString("HH:mm:ss.ff tt"));
        //     File.AppendAllText(path, "Search Period Start Time: " + DateTime.Now.ToString("HH:mm:ss.ff tt") + "\n");
        //     stopWatch2 = new Stopwatch();
        //     stopWatch.Start();

        // }
        
        // //log location of the hypercolour object if active.
        // if(GameObject.FindGameObjectWithTag("Ball").activeSelf == true && isActive==true){
        //     UnityEngine.Debug.Log("Position" + GameObject.FindGameObjectWithTag("Ball").transform.position);
        //     // File.AppendAllText(path, "Position X Y Z" + GameObject.FindGameObjectWithTag("Ball").transform.position + "\n");
        // }


        //After A button is pressed, Log time and if hypercolour is present
        if(!current.Equals("Buffer") && OVRInput.GetDown(OVRInput.RawButton.A)){
            File.AppendAllText(path, "Participant pressed Button A: " + DateTime.Now.ToString("HH:mm:ss.ff tt") + ",\n");
            isPressed = true;
            // if(stopWatch2.IsRunning){ 
            //     stopWatch2.Stop();
            //     ts = stopWatch2.Elapsed;
            //     elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //     ts.Hours, ts.Minutes, ts.Seconds,
            //     ts.Milliseconds / 10);
            //     UnityEngine.Debug.Log("Pressed vs Alive " + elapsedTime);

            //     File.AppendAllText(path, prev + "Pressed Vs Alive: " + elapsedTime + "\n");
            //     stopWatch.Reset();
            // }

            // instanceCount = driver.getInstanceCount();

            // if(instanceCount != -1 && spawnIndicators[instanceCount] == true){
            //     File.AppendAllText(path, "Hypercolour Active: True" + "\n;");
            // }else { 
            //     File.AppendAllText(path, "Hypercolour Active: False" + "\n;");
            //     // File.AppendAllText(path, "INFO: " + instanceCount + "\t" + spawnIndicators[instanceCount] + "\n");
            // }
        }


        // //Check if hypercolour isn't active and store search period time
        // if (GameObject.FindGameObjectWithTag("Ball").activeSelf == false && isActive == true){
        //     isActive = false;
        //     UnityEngine.Debug.Log("Search Period End: " + DateTime.Now.ToString("HH:mm:ss.ff tt"));
        //     File.AppendAllText(path, "Search Period End Time: " + DateTime.Now.ToString("HH:mm:ss.ff tt") + "\n");
        //     if(isCorrect){ 
        //         File.AppendAllText(path, "Participant Was Correct: True" + "\n");
        //     }else{
        //         File.AppendAllText(path, "Participant Was Correct: False" + "\n");
        //     }

        //     if(timedOut){ 
        //         File.AppendAllText(path, "Participant Timed Out: True" + "\n");
        //     } else { 
        //         File.AppendAllText(path, "Participant Time Out: False" + "\n");
        //     }
        // }


        // GameObject.FindGameObjectWithTag("Ball").SetActive(false);

        if(SceneManager.GetActiveScene().name.Equals("FireflyEnd") && isDone == false){
            isDone = true;
            UnityEngine.Debug.Log("End Date and Time: " + System.DateTime.Now.ToString("dd-MM-yyyy\\ HH:mm:ss"));
            File.AppendAllText(path, "Search Test End Date and Time: " + System.DateTime.Now.ToString("dd-MM-yyyy\\ HH:mm:ss") + ",\n");
        }

        // Debug.Log("Position" + GameObject.FindGameObjectWithTag("Ball").transform.position);
        prev = current;
    }

   
}