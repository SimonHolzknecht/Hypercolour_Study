using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;//Needed for writing log files.
using System;
using static UnityEngine.Input;
using UnityEngine.SceneManagement;
// using static UnityEngine.MeshRenderer;

public class TestDriver_Hypercolour : MonoBehaviour
{   
    // public LinkedList<GameObject> fireflies = new LinkedList<GameObject>();
    // public Queue<GameObject> fireflies = new Queue<GameObject>();
    private int HORIZONTAL_BORDER = 20;
    private int VERTICAL_BORDER = 10;
    private static int FIREFLY_COUNT = 10;
    private static float UPDATE_INTERVAL = 1f/60;
    private static float MOVEMENT_SPEED = 10f/60;
    private float lastUpdatedTime = -1;
    private Firefly[] fireflies = new Firefly[FIREFLY_COUNT];
    private Color[] colours = new Color[9];
    public static int testInstance = 0; 
    private float testStartTime = -3f;
    public static bool[] spawnIndicators = new bool[30];
    private bool waitingForReturn = true;
    public static bool lastInstanceWasCorrect = false;
    public static float testEndTime = 0f;
    private GameObject[] gameObjects = new GameObject[10];
    // public static GameObject hypercolourLeft, hypercolourRight;
    private int hypercolourFlyIndex = 0;



    // Start is called before the first frame update
    void Start() {
        UnityEngine.Random.InitState(4);
        print("TestDriver_p2 loaded...");
        DontDestroyOnLoad(GameObject.Find("Permanent_Scripts"));
        // hypercolourTemplate = GameObject.Find("Hypercolour_Template");
        // DontDestroyOnLoad(hypercolourTemplate);
        // hypercolourLeft = GameObject.Find("left");
        // hypercolourRight = GameObject.Find("right");
        // DontDestroyOnLoad(GameObject.Find("Firefly_Template"));
        // DontDestroyOnLoad(hypercolourLeft);
        // DontDestroyOnLoad(hypercolourRight);
        print("SETTINGS: " + "FIREFLY_COUNT:" + FIREFLY_COUNT + " UPDATE_INTERVAL:" + UPDATE_INTERVAL + " MOVEMENT_SPEED:" + MOVEMENT_SPEED);
        // try {
        //     spawnIndicators[0];
        // }
        // catch (Exception e) {
            fillSpawnIndicators();
        // }

        colours[0] = new Color(0,0,255,1);//blue
        colours[1] = new Color(255,128,0,1);//orange
        colours[2] = new Color(255,255,0,1);//yellow
        colours[3] = new Color(128,255,0,1);//lime
        colours[4] = new Color(0,255,0,1);//green
        colours[5] = new Color(0,255,255,1);//cyan
        colours[6] = new Color(255,0,0,1);//red
        colours[7] = new Color(128,0,255,1);//purple
        colours[8] = new Color(255,0,255,1);//pink

        // hypercolourTemplate = GameObject.Find("Hypercolour_Template");
        GameObject s;

        for(int i = 0; i < FIREFLY_COUNT; i++) {            
            s = GameObject.Instantiate(GameObject.Find("Firefly_Template"));
            s.transform.position = new Vector3 (UnityEngine.Random.Range(-HORIZONTAL_BORDER+2,HORIZONTAL_BORDER-2), UnityEngine.Random.Range(-VERTICAL_BORDER+2,VERTICAL_BORDER-2), 0F);
            
            fireflies[i] = new Firefly(s);
            gameObjects[i] = s;

            //APPLY MOVEMENT SPEED
            fireflies[i].direction.Item1 *= MOVEMENT_SPEED;
            fireflies[i].direction.Item2 *= MOVEMENT_SPEED;

            //disable fly despawn
            DontDestroyOnLoad(s);
        }

        GameObject.Find("Firefly_Template").SetActive(false);
        // GameObject.Find("Hypercolour_Template").SetActive(false);

        //hide flies
        hideFireflies();
    }

    // Update is called once per frame
    void Update() {
        String s = "";
        for(int i = 0; i < 30; i++) {
            s += i + "\t" + spawnIndicators[i] + "\n";
        }

        //if we are in an active test
        if(SceneManager.GetActiveScene().name.Equals("FireflyTest")) {

            //check if input detected
            if(OVRInput.Get(OVRInput.RawButton.A)) {
                if(spawnIndicators[testInstance]) {
                    lastInstanceWasCorrect = true;
                }
                else {
                    lastInstanceWasCorrect = false;
                }
                
                testInstance++;
                waitingForReturn = true;


                //load buffer
                hideFireflies();
                testEndTime = Time.time;
                SceneManager.LoadScene("Buffer");
            }
            
            //If we just returned from buffer
            if(waitingForReturn) {
                GameObject.Find("Firefly_Template").SetActive(false);
                // GameObject.Find("Hypercolour_Template").SetActive(false);
                waitingForReturn = false;//toggle flag
                Debug.Log("STAMP: INITIATING TEST");
                initiateTest();
            }
            else if(Time.time - testStartTime > 3f) {//if test instance is over

                //set "correct" flag
                if(spawnIndicators[testInstance])
                    lastInstanceWasCorrect = false;
                else
                    lastInstanceWasCorrect = true;


                testInstance++;
                waitingForReturn = true;


                //load buffer
                hideFireflies();
                testEndTime = Time.time;
                SceneManager.LoadScene("Buffer");
                
            }

            //move fireflies
            if(Time.time - lastUpdatedTime > UPDATE_INTERVAL) {
                lastUpdatedTime = Time.time;

                foreach (Firefly fly in fireflies) {
                    checkBorderCollision(fly);

                    fly.move();
                }
            }
        }
    }

    private void fillSpawnIndicators() {
        for(int i = 0; i < 30; i++) {
            spawnIndicators[i] = true;
        }

        for (int i = 0; i < 10; i++) {
            int rand = 0;

            do {
                rand = UnityEngine.Random.Range(0,30);
            } while(spawnIndicators[rand] == false);

            spawnIndicators[rand] = false;
        }

        for (int i = 0; i < 30; i++) {
            Debug.Log("INSTANCE " + i + " IS " + spawnIndicators[i]);
        }
    }

    private void initiateTest() {
        for(int i = 0; i < FIREFLY_COUNT; i++) {
            // fireflies[i].obj = gameObjects[i];
            fireflies[i].isHypercolour = false;
        }
        Debug.Log("STAMP: REFILLED GAMEOBJECTS");


        foreach(Firefly fly in fireflies) {

            //Randomize position
            fly.obj.transform.position = new Vector3 (UnityEngine.Random.Range(-HORIZONTAL_BORDER+2,HORIZONTAL_BORDER-2), UnityEngine.Random.Range(-VERTICAL_BORDER+2,VERTICAL_BORDER-2), 0F);
            // s.transform.localScale += new Vector3(-0.5f, -0.5f, -0.5f)

            //Randomize scaleDirection
            fly.randomizeDirection();

            //Apply movement speed
            fly.direction.Item1 *= MOVEMENT_SPEED;
            fly.direction.Item2 *= MOVEMENT_SPEED;

            //Apply colour
            fly.obj.GetComponent<Renderer>().material.color = colours[UnityEngine.Random.Range(0,9)];
        }

        Debug.Log("STAMP: SPAWNED FIREFLIES");

        //APPLYING COLOUR IF PRESENT
        if(spawnIndicators[testInstance]) {
            int hypercolourFlyIndex = UnityEngine.Random.Range(0,9);
            fireflies[hypercolourFlyIndex].isHypercolour= true;
            Debug.Log("STAMP: CHOSE FLY");

            GameObject.Find("left").transform.position = new Vector3 (UnityEngine.Random.Range(-HORIZONTAL_BORDER+2,HORIZONTAL_BORDER-2), UnityEngine.Random.Range(-VERTICAL_BORDER+2,VERTICAL_BORDER-2), 0F);
            GameObject.Find("right").transform.position = GameObject.Find("left").transform.position;

            //APPLY COLOUR TO HYPERCOLOUR
            if(TestDriver_Hypercolour.testInstance < 15) {
                GameObject.Find("left").GetComponent<Renderer>().material.color = new Color(0,0,255,1);
                GameObject.Find("right").GetComponent<Renderer>().material.color = new Color(255,255,0,1);
            }
            else {
                GameObject.Find("right").GetComponent<Renderer>().material.color = new Color(0,0,255,1);
                GameObject.Find("left").GetComponent<Renderer>().material.color = new Color(255,255,0,1);
            }
        }

        Debug.Log("STAMP: SPAWNED HYPERCOLOUR");


        //Make flies visible
        showFireflies();

        testStartTime = Time.time;
    }

    private void checkBorderCollision(Firefly fly) {
        if(fly.getNextPosition().Item1 > HORIZONTAL_BORDER) {
            fly.direction.Item1 *= -1;
        }
        else if(fly.getNextPosition().Item1 < -HORIZONTAL_BORDER) {
            fly.direction.Item1 *= -1;
        }
        else if(fly.getNextPosition().Item2 > VERTICAL_BORDER) {
            fly.direction.Item2 *= -1;
        }
        else if(fly.getNextPosition().Item2 < -VERTICAL_BORDER) {
            fly.direction.Item2 *= -1;
        }   
    }

    private void hideFireflies() {
        foreach(Firefly fly in fireflies) {
            fly.obj.SetActive(false);
        }
        Debug.Log("STAMP: NORMAL FLIES NOW HIDDEN");

        // hypercolourLeft.SetActive(false);
        // hypercolourRight.SetActive(false);
        // GameObject.Find("left").SetActive(false);
        // GameObject.Find("right").SetActive(false);

        // Debug.Log("STAMP: HYPERCOLOUR FLIES NOW HIDDEN");
    }

    private void showFireflies() {
        if(spawnIndicators[testInstance]) {
            for(int i = 0; i < FIREFLY_COUNT; i++) {
                if(i != hypercolourFlyIndex) {
                    fireflies[i].obj.SetActive(true);
                }
                else {
                    // fireflies[i].obj.SetActive(true);

                    GameObject.Find("left").SetActive(true);
                    GameObject.Find("right").SetActive(true);
                }
            }
        }
        else {
            foreach(Firefly fly in fireflies) {
                fly.obj.SetActive(true);
            }

            GameObject.Find("left").SetActive(false);
            GameObject.Find("right").SetActive(false);
        }
    }

    public bool[] getSpawnIndicators() {
        return spawnIndicators;
    }

    public int getInstanceCount() {
        return testInstance;
    }
}

public class Firefly {
    public GameObject obj;
    public bool isHypercolour = false;

    public (float,float) direction = (UnityEngine.Random.Range(-1f,1f),UnityEngine.Random.Range(-1f,1f));

    public Firefly(GameObject obj) {
        this.obj = obj;
        scaleDirection();
    }

    public void randomizeDirection() {
        direction = (UnityEngine.Random.Range(-1f,1f),UnityEngine.Random.Range(-1f,1f));
        scaleDirection();
    }

    public void move() {
        obj.transform.Translate(new Vector3(direction.Item1,direction.Item2,0));

        if(isHypercolour) {
            Debug.Log("STAMP: IS HYPERCOLOUR");

            GameObject.Find("left").transform.Translate(new Vector3(direction.Item1,direction.Item2,0));
            GameObject.Find("right").transform.Translate(new Vector3(direction.Item1,direction.Item2,0));

            Debug.Log("POSITION OF NORMAL COLOUR: " + obj.transform.position);
            Debug.Log("POSITION OF HYPERCOLOUR: " + GameObject.Find("right").transform.position);

            Debug.Log("STAMP: MOVED HYPERCOLOURS");
        }

        Debug.Log("STAMP: MOVED NORMAL OBJECT");
    }

    public (float,float) getNextPosition() {
        if(isHypercolour) {
            GameObject left = GameObject.Find("left");

            return (left.transform.position.x + direction.Item1, left.transform.position.y + direction.Item2);
        }
        else
            return (obj.transform.position.x + direction.Item1, obj.transform.position.y + direction.Item2);
    }

    private void scaleDirection() {
        float scale =(float) Math.Sqrt(Math.Pow(direction.Item1,2) + Math.Pow(direction.Item2,2));
        direction.Item1 = direction.Item1 / scale;
        direction.Item2 = direction.Item2 / scale;
    }

    //Returns 0 if no collision is detected, 1 if a collision on horizontal axis and 2 if on vertical axis
    // public bool collidesWithMe(Firefly fly) {
    //     (float,float) myPos = getNextPosition();
    //     (float,float) coords = fly.getNextPosition();

    //     // Debug.Log("COLLISON CHECK OF " + coords.Item1 + "|" + coords.Item2 + " and " + myPos.Item1 + "|" + myPos.Item2);

    //     // if((((float)Math.Sqrt(Math.Pow(myPos.Item1 - coords.Item1,2))) < 1f)  &&  (((float)Math.Sqrt(Math.Pow(myPos.Item2 - coords.Item2,2))) < 1f)) {
    //     //     Debug.Log("COLLIDING!");
    //     //     return 1;
    //     // }

    //     if(((float)Math.Sqrt(Math.Pow(myPos.Item1-coords.Item1,2) + Math.Pow(myPos.Item2-coords.Item2,2))) < 1f) {
    //         // Debug.Log("COLLIDING!");
    //         if(((float)Math.Sqrt(Math.Pow(myPos.Item1-coords.Item1,2))) < ((float)Math.Sqrt(Math.Pow(myPos.Item2-coords.Item2,2)))) {
    //             direction.Item2 *= -1;//invert horizontal direction
    //             fly.direction.Item2 *= -1;
    //         }
    //         else {
    //             direction.Item1 *= -1;//invert vertical direction
    //             fly.direction.Item1 *= -1;
    //         }
    //         return true;
    //     }
    //     else {
    //         return false;
    //     }
    // }
}
