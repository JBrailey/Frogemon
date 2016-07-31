using UnityEngine;
using System.Collections;

public class Trainer : MonoBehaviour {

    int timerRun; // The timer that runs
    int timerSet;  // The set value of the timer

    public GridController gridController;
    public Pokeball pokeBall;

	// Use this for initialization
	void Start ()
    {
        timerSet = 60 *  (1/gridController.level);
        timerRun = timerSet;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timerRun <= 0 )
        {
            Instantiate(pokeBall);
            timerRun = timerSet;
        }
        else
        {
            timerRun--;
        }
	}
}
