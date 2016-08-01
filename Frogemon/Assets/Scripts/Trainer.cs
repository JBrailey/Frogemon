using UnityEngine;
using System.Collections;

public class Trainer : MonoBehaviour {


    public GridController gridController;
    public LevelController levelController;
    public GameObject pokeBall;

    int timerRun = 999; // The timer that runs
    int timerSet;  // The set value of the timer
    int level;
    float speed;
//    Vector3 position;
    public Vector3 endPosition;

	// Use this for initialization
	void Start ()
    {
//        endPosition = gridController.ReturnEndPos(transform.position);
        level = levelController.ReturnLevel();
        timerSet = 120 - 2*level;
        timerRun = timerSet;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timerRun <= 0 )
        {
            speed = 1f + ((level * 0.1f) - 0.1f);
            GameObject go = (GameObject)Instantiate(pokeBall,transform.position,Quaternion.identity);
            go.transform.parent = transform;
            Pokeball pb = go.GetComponent<Pokeball>();
            pb.speed = speed;
            pb.endPosition = endPosition;
            timerRun = timerSet;
        }
        else
        {
            timerRun--;
        }
	}
}
