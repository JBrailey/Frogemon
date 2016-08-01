using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject gridObject; // the grid controller object
    public GameObject titleObject; // the game title
    public GameObject keyToStartObject; // start game instructions
    public int level; // the current level number

    CameraController camControl; // the camera controller
    GameObject currentLevelGrid; // the current level grid controller
    bool waitToStart = true;

    GameObject titleInstance; // ui element instances
    GameObject keyToStartInstance;

    const float HUD_Z = -2f; // the z depth of HUD elements

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

        // get the camera controller and position at the top
        camControl = Camera.main.GetComponent<CameraController>();
        camControl.SnapToTop();

        // spawn the first level
        BuildLevel();
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

                // TODO: implement camera pan to start

                // start the level
                currentLevelGrid.GetComponent<GridController>().StartLevel();
                camControl.SnapToBottom();
            }
        }
    }

    // show or hide the title
    void TitleVisible(bool a_visible)
    {
        titleInstance.GetComponent<Renderer>().enabled = a_visible;
        keyToStartInstance.GetComponent<Renderer>().enabled = a_visible;
    }

    // spawns a level
    void BuildLevel()
    {
        // destroy the current level, if it exists
        if (currentLevelGrid != null)
        {
            DestroyObject(currentLevelGrid);
        }

        // spawn the level
        currentLevelGrid = (GameObject)Instantiate(gridObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        currentLevelGrid.GetComponent<GridController>().levelController = gameObject;
    }

    // called when the current level has been completed
    public void NextLevel()
    {
        // increment the level
        ++level;

        // spawn the new level
        BuildLevel();

        // start the level immediately
        currentLevelGrid.GetComponent<GridController>().autoLevelStart = true;
        camControl.SnapToBottom();
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

        // spawn a new level
        BuildLevel();

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
