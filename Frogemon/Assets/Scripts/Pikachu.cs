using UnityEngine;
using System.Collections;

public class Pikachu : MonoBehaviour
{
    // References to Piakchu's components
    public GridController gridController;
    AudioSource pikaWalk, pikaDeath, pikaBlock, pikaEat;
    Animator anim;
    public Score score;

    Vector3 newPosition;
    float speed = 1f;
    bool isMoving = false, isEating = false, isDead = false, lastMoveGood = false;
    string lastDirection = "Up";

    int pikaY = 0, pikaHighestY = 0;

    //// CAMERA TRACKING
    CameraController camControl;
    //// CAMERA TRACKING

    // Use this for initialization
    void Start()
    {
        // Get reference to Animator
        anim = GetComponent<Animator>();

        // Get Audio
        AudioSource[] audio = GetComponents<AudioSource>();
        pikaWalk = audio[0];
        pikaBlock = audio[1];
        pikaDeath = audio[2];
        pikaEat = audio[3];

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

        // Kill Test
        if (Input.GetKeyUp(KeyCode.K))
        {
            Die();
        }

        // If not currently moving, Check for Key Presses
        if (!isMoving && !isDead && !isEating)
        {
            CheckForKeyPress();
        }

        // If still moving, Keep moving
        else
        {
            Move();
        }
    }

    // continuous level support
    public void SimulateMoveForward(Vector3 a_pos)
    {
        newPosition = a_pos; // GetNewPosition("Up");
        anim.Play("WalkForward");
        pikaWalk.Play();
        lastDirection = "Up";
        isMoving = true;
        Move();
        pikaY = 0;
        pikaHighestY = 0;
    }
    // continuous level support

    void CheckForKeyPress()
    {
        // Check for WASD being pressed.
        if (Input.GetKey(KeyCode.W))
        {
            isMoving = true;
            newPosition = GetNewPosition("Up");
            Move();
            if(newPosition != transform.position)
            {
                pikaY++;
                if(pikaY > pikaHighestY)
                {
                    pikaHighestY = pikaY;
                    score.NewPikachuY(pikaHighestY);
                }
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            isMoving = true;
            newPosition = GetNewPosition("Left");
            Move();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            isMoving = true;
            newPosition = GetNewPosition("Down");
            Move();
            if (newPosition != transform.position)
            {
                pikaY--;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            isMoving = true;
            newPosition = GetNewPosition("Right");
            Move();
        }
        // Check if key is no longer being held down
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            lastMoveGood = true;
        }
    }

    // Moves Pikachu toward New Location
    void Move()
    {
        //Check if done moving
        if (transform.position.Equals(newPosition))
        {
            isMoving = false;
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
        if (!isEating && !isDead)
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
        newPos = newPos - new Vector3(0, 0, .1f);

        // Set direction for animation purposes
        lastDirection = direction;

        // Check if Pikachu can Move
        if (newPos.Equals(transform.position))
        {
            // If Pikachu has just moved or changed direction, play Blocked Sound
            if(lastMoveGood)
            {
                pikaBlock.Play();
            }
            
            // Pikachu is not moving, its last attempt to move was not good
            lastMoveGood = false;
            isMoving = false;
            return newPos;
        }
        else
        {
            // Pikachu's last move was good
            lastMoveGood = true;

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
        // Play Eating Animation & sound
        anim.Play("Eat");
        pikaEat.Play();

        // Pikachu is eating, wait for it to Eat before going playing backward idle animation.
        isEating = true;
        lastDirection = "Down";
        StartCoroutine(Wait("Eat"));
    }

    void Die()
    {
        // Play Death Animation & Sound
        pikaDeath.Play();
        anim.Play("Dead");

        // Set isDead to true
        isDead = true;

        // Wait 1 second then tell GridController Pikachu is Dead
        StartCoroutine(Wait("Die"));
    }

    // Makes the program Wait
    IEnumerator Wait(string action)
    {
        if (action.Equals("Die"))
        {
            yield return new WaitForSeconds(0.5f);
            score.PikachuDead();
            pikaHighestY = 0;
            // Tell GridController Pikachu Died
            gridController.PikachuDead();
        }
        else if (action.Equals("Eat"))
        {
            yield return new WaitForSeconds(2);
            score.FoodEaten();
            // Tell Grid Controller Food is Eaten
            gridController.FoodEaten();

            isEating = false;

            // this was commented out as pikachu is now simulated to walk
            // immediately after eating and to fix the 2 space walk I need to
            // fully simulate, rather than just calling GetNewPosition()
            // PlayIdleAnimation();
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
