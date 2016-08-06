using UnityEngine;
using System.Collections;

public class Trainer : MonoBehaviour {


    public GameObject pokeBall;
    public float speedMod;
    public float intervalMod;
    public bool doesBurst;
    Animator anim;

    float timerRun = 999; // The timer that runs
    float timerBurst = 999;
    float timerSet;  // The set value of the timer
    float burstSet;
    float speed;
    bool ballThrown; // Checks whether the first ball has been thrown when Burst is activated
//    Vector3 position;
    public Vector3 endPosition;
    public int level;

    bool hasBursted = false;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        //timerSet = (180*intervalMod) - 2*level; //Old Math
        timerSet = (180 - 2 * level) * intervalMod; //Alternate Math
        if (doesBurst == true)
        {
            ballThrown = false;
            burstSet = ((180 * intervalMod) - 2 * level)+30;
            timerBurst = burstSet;
        }

        //  Randomise the initial throw
        Random.seed = Random.Range(0, 99999);
        timerRun = 120 - Random.Range(0,120);
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (timerRun <= 0 )
        {
            ThrowPokeball();   
        }
        else
        {
            timerRun--;
        }
	}

    //Throw Pokeball
    void ThrowPokeball()
    {
        anim.Play("Throw");
        //speed = (1f * speedMod) + ((level * 0.1f) - 0.1f); // Old Math
        speed = (1f + ((level * 0.1f) - 0.1f)) * speedMod;
        GameObject go = (GameObject)Instantiate(pokeBall, transform.position, Quaternion.identity);
        go.transform.parent = transform; // make sure the pokeballs are parented to the trainer
                                         // this will be important for continuous level looping

        Pokeball pb = go.GetComponent<Pokeball>();
        pb.speed = speed;
        pb.endPosition = endPosition;
        pb.spin = GetComponent<SpriteRenderer>().flipX ? 1f : -1f;
        timerRun = timerSet;
        if (doesBurst && !hasBursted)
        {
            StartCoroutine(Wait("Burst"));
        }else
        {
            StartCoroutine(Wait("Idle"));
        }        
    }

    IEnumerator Wait(string action)
    {
        if (action.Equals("Idle"))
        {
            yield return new WaitForSeconds(0.5f);
            hasBursted = false;
            anim.Play("Idle");
        }else if (action.Equals("Burst"))
        {            
            yield return new WaitForSeconds(0.5f);
            hasBursted = true;
            ThrowPokeball();
        }
    }
}
