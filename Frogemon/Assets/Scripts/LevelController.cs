using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{

    public GameObject gridController;

    public int level;


    // called when the level has been completed
    public void NextLevel()
    {
    }

    // called when pikachu died
    public void GameOver()
    {
    }


    // Use this for initialization
    void Start ()
    {
        // spawn the first level
        Instantiate(gridController, new Vector3(0f, 0f, 0f), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
