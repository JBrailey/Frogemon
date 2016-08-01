using UnityEngine;
using System.Collections;

public class Trainer : MonoBehaviour {


    public GridController gridController;
    public LevelController levelController;
    public Pokeball pokeBall;

    int timerRun; // The timer that runs
    int timerSet;  // The set value of the timer
    int level;
    float speed;
    Vector3 position;
    Vector3 endPosition;

	// Use this for initialization
	void Start ()
    {
        endPosition = gridController.ReturnEndPos(position);
        level = levelController.ReturnLevel();
        timerSet = 60 *  (1/level);
        timerRun = timerSet;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timerRun <= 0 )
        {
            speed = 1f + ((level * 0.1f) - 0.1f);
            Instantiate(pokeBall,position,Quaternion.identity);
            pokeBall.speed = speed;
            pokeBall.endPosition = endPosition;
            timerRun = timerSet;
        }
        else
        {
            timerRun--;
        }
	}
}
