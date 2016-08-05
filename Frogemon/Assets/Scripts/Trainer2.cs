﻿using UnityEngine;
using System.Collections;

public class Trainer2 : MonoBehaviour
{


    public GameObject pokeBall;
    Animator anim;

    int timerRun = 999; // The timer that runs
    int timerSet;  // The set value of the timer
    float speed;
    //    Vector3 position;
    public Vector3 endPosition;
    public int level;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        timerSet = 240 - 2 * level;

        //  Randomise the initial throw
        Random.seed = Random.Range(0, 99999);
        timerRun = 120 - Random.Range(0, 120);

    }

    // Update is called once per frame
    void Update()
    {
        if (timerRun <= 0)
        {
            speed = 2f + ((level * 0.1f) - 0.1f);
            GameObject go = (GameObject)Instantiate(pokeBall, transform.position, Quaternion.identity);
            go.transform.parent = transform; // make sure the pokeballs are parented to the trainer
                                             // this will be important for continuous level looping
            Pokeball pb = go.GetComponent<Pokeball>();
            pb.speed = speed;
            pb.endPosition = endPosition;
            timerRun = timerSet;
            anim.Play("Throw");
            StartCoroutine(Wait());
        }
        else
        {
            timerRun--;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        anim.Play("Idle");
    }
}
