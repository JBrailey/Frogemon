using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public GameObject gridObject; // the grid controller object
    public int level; // the current level number

    CameraController camControl; // the camera controller
    GameObject currentLevelGrid; // the current level grid controller
    bool waitToStart = true;

    // Use this for initialization
    void Start ()
    {
        // get the camera controller and position at the top
        camControl = Camera.main.GetComponent<CameraController>();
        camControl.SnapToTop();

        // spawn the first level
        currentLevelGrid = (GameObject)Instantiate(gridObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        currentLevelGrid.GetComponent<GridController>().levelController = gameObject;
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

                // TODO: implement camera pan to start

                // start the level
                currentLevelGrid.GetComponent<GridController>().StartLevel();
                camControl.SnapToBottom();
            }
        }
    }

    // called when the current level has been completed
    public void NextLevel()
    {
        // destroy the current level
        DestroyObject(currentLevelGrid);

        // increment the level
        ++level;

        // spawn the next level
        currentLevelGrid = (GameObject)Instantiate(gridObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        currentLevelGrid.GetComponent<GridController>().levelController = gameObject;

        // start the level immediately
        currentLevelGrid.GetComponent<GridController>().autoLevelStart = true;
        camControl.SnapToBottom();
    }

    // called when pikachu died
    public void GameOver()
    {
        Debug.Log("game over");
    }
}
