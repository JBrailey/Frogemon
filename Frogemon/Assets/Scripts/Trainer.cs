using UnityEngine;
using System.Collections;

public class Trainer : MonoBehaviour {

    int timerRun; // The timer that runs
    int timerSet;  // The set value of the timer
    Vector3 position;

    public GridController gridController;
    public LevelController levelController;
    public Pokeball pokeBall;

	// Use this for initialization
	void Start ()
    {
        timerSet = 60 *  (1/levelController.level);
        timerRun = timerSet;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timerRun <= 0 )
        {
            Instantiate(pokeBall,position,Quaternion.identity);
            timerRun = timerSet;
        }
        else
        {
            timerRun--;
        }
	}
}
