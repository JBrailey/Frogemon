using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public GameObject gridObject; // the grid controller object
    public GameObject titleObject; // the game title
    public GameObject keyToStartObject; // start game instructions
    public GameObject instructionsObject; // 
    public int level; // the current level number
    public GameObject scoreObject;

    CameraController camControl; // the camera controller
    GameObject previousLevelGrid; // the previous level grid controller
    GameObject currentLevelGrid; // the current level grid controller
    GameObject nextLevelGrid; // the next level grid controller
    const float levelPosIncrement = 8.5f; // size in units of level grid
    bool waitToStart = true;

    GameObject titleInstance; // ui element instances
    GameObject keyToStartInstance;
    GameObject scoreInstance;
    GameObject instructionsInstance;

    const float HUD_Z = -2f; // the z depth of HUD elements

    public GameObject pikachuObject; //Pikachu Object
    GameObject pikachu; // Pikachu instance

    // Use this for initialization
    void Start ()
    {
        // create the title instance and parent it to the camera
        Vector3 titlePos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, HUD_Z);
        titleInstance = (GameObject)Instantiate(titleObject, titlePos, Quaternion.identity);
        titleInstance.transform.parent = Camera.main.transform;

        // create the start instruction instance and parent it to the camera
        Vector3 keyPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 2f, HUD_Z);
        keyToStartInstance = (GameObject)Instantiate(keyToStartObject, keyPos, Quaternion.identity);
        keyToStartInstance.transform.parent = Camera.main.transform;

        // create the move Instruction and make the camera it's parent.
        Vector3 instuctPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 1.5f, HUD_Z);
        instructionsInstance = (GameObject)Instantiate(instructionsObject, instuctPos, Quaternion.identity);
        instructionsInstance.transform.parent = Camera.main.transform;

        // get the camera controller and position at the top
        camControl = Camera.main.GetComponent<CameraController>();
        camControl.SnapToTop();

        // spawn the first levels
        currentLevelGrid = BuildLevel(0f);
        nextLevelGrid = BuildLevel(levelPosIncrement);

        // Create Score Instance and make the Camera it's parent
        Vector3 scorePos = new Vector3(Camera.main.transform.position.x - 3.05f, Camera.main.transform.position.y + 2f, HUD_Z);
        scoreInstance = (GameObject)Instantiate(scoreObject, scorePos, Quaternion.identity);
        scoreInstance.transform.parent = Camera.main.transform;
	}

	// Update is called once per frame
	void Update ()
    {
        // is the level waiting to start?
        if (waitToStart)
        {
            // start the level on any key
            if (Input.anyKey)
            {
                // stop waiting
                waitToStart = false;

                // hide the title
                TitleVisible(false);

                // pan the camera and start the game once the pan is completed
                StartCoroutine(WaitStart(camControl.AnimateToBottom(1.5f) - .01f));
            }
        }
    }

    // show or hide the title
    void TitleVisible(bool a_visible)
    {
        titleInstance.GetComponent<Renderer>().enabled = a_visible;
        keyToStartInstance.GetComponent<Renderer>().enabled = a_visible;
        instructionsInstance.GetComponent<Renderer>().enabled = a_visible;
    }

    // spawns a level
    GameObject BuildLevel(float a_verticalPos)
    {
        // spawn the level
        GameObject levelGrid = (GameObject)Instantiate(gridObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        levelGrid.GetComponent<GridController>().levelController = gameObject;
        levelGrid.GetComponent<GridController>().gridVerticalPosition = a_verticalPos;

        return levelGrid;
    }

    // coroutine to start the level once the camera animation is complete
    IEnumerator WaitStart(float a_wait)
    {
        yield return new WaitForSeconds(a_wait);
        StartLevel();
    }

    // called to create pikachu and start the level
    void StartLevel()
    {
        // create and setup pikachu, requesting to move down will fail but will
        // return the correct starting location
        pikachu = (GameObject)Instantiate(pikachuObject, currentLevelGrid.GetComponent<GridController>().ReturnNewPikaPos("down"), Quaternion.identity);
        pikachu.GetComponent<Pikachu>().gridController = currentLevelGrid.GetComponent<GridController>();
        pikachu.GetComponent<Pikachu>().score = scoreInstance.GetComponent<Score>();

        camControl.SnapToBottom();
    }

    // called when the current level has been completed
    public void NextLevel()
    {
        // destroy the previous level
        if (previousLevelGrid)
        {
            DestroyObject(previousLevelGrid);
        }

        // set the current level to be the previous
        previousLevelGrid = currentLevelGrid;

        // set the next level to be the current
        currentLevelGrid = nextLevelGrid;

        // create a new next level
        nextLevelGrid = BuildLevel(currentLevelGrid.GetComponent<GridController>().gridVerticalPosition + levelPosIncrement);

        // increment the level
        ++level;

        // tell pikachu about the new grid controller and move pikachu into the new level
        pikachu.GetComponent<Pikachu>().gridController = currentLevelGrid.GetComponent<GridController>();
        Vector3 startPos = pikachu.transform.position;
        startPos.y += .5f;
        pikachu.GetComponent<Pikachu>().SimulateMoveForward(startPos);
    }

    // Callerd when trainer needs level
    public int ReturnLevel()
    {
        return level;
    }

    // called when pikachu died
    public void GameOver()
    {
        // TODO: add code to show a game over notice and move this code to a coroutine
        //       to time-out the game over notice

        // destroy pikachu
        DestroyObject(pikachu);

        // destroy all current level grids
        if (previousLevelGrid)
        {
            DestroyObject(previousLevelGrid);
        }
        if (currentLevelGrid)
        {
            DestroyObject(currentLevelGrid);
        }
        if (nextLevelGrid)
        {
            DestroyObject(nextLevelGrid);
        }

        // spawn the new levels
        previousLevelGrid = null;
        currentLevelGrid = BuildLevel(0f);
        nextLevelGrid = BuildLevel(levelPosIncrement);

        // reset the level number
        level = 1;

        // show the title
        TitleVisible(true);

        // snap camera to the top
        camControl.SnapToTop();

        // wait to start the level
        waitToStart = true;
    }
}
