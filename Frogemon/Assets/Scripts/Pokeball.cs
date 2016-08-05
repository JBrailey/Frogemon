using UnityEngine;
using System.Collections;

public class Pokeball : MonoBehaviour
{
    public float speed; // speed of the ball in units per second
    public Vector3 endPosition; // target
    public float spin; // spin direction (1 or -1)

    Animator anim; // animation controller

    // called to destroy the pokeball
    void Die()
    {
        StartCoroutine(Wait("Die"));
    }

    // delay an action
    IEnumerator Wait(string action)
    {
        if (action.Equals("Die"))
        {
            // delay to allow the animation to play
            yield return new WaitForSeconds(.25f);

            // destroy the ball
            DestroyObject(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        // update spin to revolve in coordination with speed
        spin = spin * 360f * speed;

        // load the animation controller
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // move until the target is reached
        if (transform.position != endPosition)
        {
            // move and spin the ball
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            transform.Rotate(Vector3.forward * spin * Time.deltaTime);
        }
        else
        {
            // explode the ball
            anim.Play("Explode");
            Die();
        }        
    }

    void OnCollisionEnter(Collision collision)
    {
        // did the ball hit pikachu?
        if (collision.gameObject.tag.Equals("Pikachu"))
        {
            // explode the ball
            anim.Play("Explode");
            Die();
        }

    }
}
