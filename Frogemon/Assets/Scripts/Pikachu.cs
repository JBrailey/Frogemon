﻿using UnityEngine;
using System.Collections;

public class Pikachu : MonoBehaviour
{

    public GridController gridController;
    AudioSource pikaWalk, pikaDeath, pikaBlock, pikaEat;
    Animator anim;
    Vector3 position, newPosition;
    float speed = 1f;
    bool isMoving = false, isEating = false, isDead = false, lastMoveGood = false, dirChanged = false;
    string lastDirection = "Up";

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
    public void SimulateMoveForward()
    {
        newPosition = GetNewPosition("Up");
        isMoving = true;
        Move();
    }
    // continuous level support

    void CheckForKeyPress()
    {
        // Check for WASD being pressed. Checking for Key Release.
        if (Input.GetKey(KeyCode.W))
        {
            newPosition = GetNewPosition("Up");
            isMoving = true;
            Move();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            newPosition = GetNewPosition("Left");
            isMoving = true;
            Move();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newPosition = GetNewPosition("Down");
            isMoving = true;
            Move();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            newPosition = GetNewPosition("Right");
            isMoving = true;
            Move();
        }
        // Testing speed increase (Remove Later)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 5;
        }
        else
        {
            speed = 1;
        }
        // Check if key is no longer being held down
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            isMoving = false;
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
        position = transform.position;
        newPos = newPos - new Vector3(0, 0, .1f);

        // Check if Direction has changed [Blocking Sound Spam Fix]
        if (!dirChanged)
        {
            dirChanged = false;
            
        }
        else
        {
            dirChanged = true;
        }

        // Set direction for animation purposes
        lastDirection = direction;

        // Check if Pikachu can Move
        if (newPos.Equals(position))
        {
            // If Pikachu has just moved or changed direction, play Blocked Sound
            if(lastMoveGood || dirChanged)
            {
                pikaBlock.Play();
            }
            
            // Pikachu is not moving, its last attempt to move was not good
            lastMoveGood = false;
            isMoving = false;
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
        anim.Play("DeadRight");

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
            // Tell GridController Pikachu Died
            gridController.PikachuDead();
        }
        else if (action.Equals("Eat"))
        {
            yield return new WaitForSeconds(2);
            // Tell Grid Controller Food is Eaten
            gridController.FoodEaten();

            isEating = false;
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
