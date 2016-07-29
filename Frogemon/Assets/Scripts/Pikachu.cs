using UnityEngine;
using System.Collections;

public class Pikachu : MonoBehaviour {

    public GridController gridController;
    AudioSource pikaWalk, pikaDeath, pikaBlock;
    Animator anim;
    Vector3 position;

	// Use this for initialization
	void Start () {
        //gridController = GetComponent<GridController>();
        anim = GetComponent<Animator>();

        AudioSource[] audio = GetComponents<AudioSource>();
        pikaWalk = audio[0];
        pikaBlock = audio[1];
        pikaDeath = audio[2];

        position = transform.position;
	}

    void SetGridController(GridController gc)
    {
        gridController = gc;
    }
	
	// Update is called once per frame
	void Update () {
        // Check for WASD being pressed. Checking for Key Release.
        if (Input.GetKeyUp(KeyCode.W))
        {
            Move("Up");
        }else if (Input.GetKeyUp(KeyCode.A))
        {
            Move("Left");
        }else if (Input.GetKeyUp(KeyCode.S))
        {
            Move("Down");
        }else if (Input.GetKeyUp(KeyCode.D))
        {
            Move("Right");
        }
	}

    private void SetAnimation(int animation)
    {
        anim.SetInteger("Animation", animation);
    }

    void Move(string direction)
    {
        // pikaWalk.Play();
        Vector3 newPosition = gridController.ReturnNewPikaPos(direction);
        position = transform.position;

        // Check if Pikachu can Move
        if (newPosition == position)
        {
            pikaBlock.Play();
            
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
            // Move Pikachu
            transform.Translate(newPosition * Time.deltaTime);
            //SetAnimation(0);            
        }

    }

    void Die()
    {
        //Play Death Animation & Sound
        pikaDeath.Play();

        
        //Tell GridController Pikachu Died
        gridController.PikachuDead();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Pokeball"))
        {
            
            Die();
        }
    }
}
