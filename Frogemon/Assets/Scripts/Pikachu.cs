using UnityEngine;
using System.Collections;

public class Pikachu : MonoBehaviour
{

    public GridController gridController;
    AudioSource pikaWalk, pikaDeath, pikaBlock;
    Animator anim;
    Vector3 position, newPosition;
    float speed = 1f;
    bool moving = false, eating = false;
    string lastDirection = "Up";
    bool isDead; // added to prevent pikachu from moving once dead

    //// CAMERA TRACKING
    CameraController camControl;
    //// CAMERA TRACKING

    // Use this for initialization
    void Start()
    {
        //gridController = GetComponent<GridController>();
        anim = GetComponent<Animator>();

        AudioSource[] audio = GetComponents<AudioSource>();
        pikaWalk = audio[0];
        pikaBlock = audio[1];
        pikaDeath = audio[2];

        position = transform.position;

        //// CAMERA TRACKING
        camControl = Camera.main.GetComponent<CameraController>();
        //// CAMERA TRACKING
    }

    void SetGridController(GridController gc)
    {
        gridController = gc;
    }

    // Update is called once per frame
    void Update()
    {
        // ignore updates once dead
        if (isDead)
        {
            return;
        }

        // Kill Test
        if (Input.GetKeyUp(KeyCode.K))
        {
            Die();
        }

        // If not currently moving, Check for Key Presses
        if (!moving)
        {
            CheckForKeyPress();
        }
        //If still moving, Kep moving
        else
        {
            Move();
        }
    }

    void CheckForKeyPress()
    {
        // Check for WASD being pressed. Checking for Key Release.
        if (Input.GetKeyUp(KeyCode.W))
        {
            newPosition = GetNewPosition("Up");
            moving = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            newPosition = GetNewPosition("Left");
            moving = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            newPosition = GetNewPosition("Down");
            moving = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            newPosition = GetNewPosition("Right");
            moving = true;
        }
    }

    // Moves Pikachu toward New Location
    void Move()
    {
        //Check if done moving
        if (transform.position.Equals(newPosition))
        {
            moving = false;
            PlayIdleAnimation();
        }
        //If not done moving, Move
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

            //// CAMERA TRACKING
            camControl.MoveTo(transform.position);
            //// CAMERA TRACKING
        }
    }

    void PlayIdleAnimation()
    {
        if (!eating)
        {
            if (lastDirection.Equals("Up"))
            {
                anim.Play("IdleForward");
            }
            else if (lastDirection.Equals("Left"))
            {
                anim.Play("IdleLeft");
            }
            else if (lastDirection.Equals("Down"))
            {
                anim.Play("IdleBackward");
            }
            else if (lastDirection.Equals("Right"))
            {
                anim.Play("IdleRight");
            }
        }
    }

    // Gets Pikachu's new Positon & Plays Walking animation
    Vector3 GetNewPosition(string direction)
    {
        Vector3 newPos = gridController.ReturnNewPikaPos(direction);
        position = transform.position;
        newPos = newPos - new Vector3(0, 0, .1f);

        // Set direction for animation purposes
        lastDirection = direction;

        // Check if Pikachu can Move
        if (newPos.Equals(position))
        {
            pikaBlock.Play();
            moving = false;
        }
        else
        {
            // Play Walking Animation
            if (direction.Equals("Up"))
            {
                anim.Play("WalkForward");
            }
            else if (direction.Equals("Left"))
            {
                anim.Play("WalkLeft");
            }
            else if (direction.Equals("Down"))
            {
                anim.Play("WalkBackward");
            }
            else
            {
                anim.Play("WalkRight");
            }

            // Play Walking SoundFX
            pikaWalk.Play();

        }
        return newPos;
    }

    void EatFood()
    {
        // Play Eating Animation
        anim.Play("Eat");

        // Tell Grid Controller Food is Eaten
        gridController.FoodEaten();

        // Pikachu is eating, wait for it to Eat before going playing backward idle animation.
        eating = true;
        lastDirection = "Down";
        StartCoroutine(Wait("Eat"));
    }

    void Die()
    {
        // Play Death Animation & Sound
        pikaDeath.Play();
        anim.Play("DeadRight");

        // flag as dead
        isDead = true;

        // Wait 1 second then tell GridController Pikachu is Dead
        StartCoroutine(Wait("Die"));
    }

    // Makes the program Wait
    IEnumerator Wait(string action)
    {
        if (action.Equals("Die"))
        {
            yield return new WaitForSeconds(3);
            //Tell GridController Pikachu Died
            gridController.PikachuDead();
        }
        else if (action.Equals("Eat"))
        {
            yield return new WaitForSeconds(2);
            eating = false;
            PlayIdleAnimation();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Pokeball"))
        {
            Die();
        }
        else if (collision.gameObject.tag.Equals("Food"))
        {
            EatFood();
        }
    }
}
