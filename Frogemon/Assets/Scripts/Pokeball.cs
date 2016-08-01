using UnityEngine;
using System.Collections;

public class Pokeball : MonoBehaviour
{
    public float speed;
//    bool movingRight = true;
//    Vector3 position;
    public Vector3 endPosition;
    Animator anim;

    void Die()
    {
        anim.Play("Explode");
        StartCoroutine(Wait("Die"));
    }


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        // Check for movement here
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position != endPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
        }
        else
        {
            Die();
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Pikachu"))
        {
            Die();
        }

    }
    

    // Makes the program Wait
    IEnumerator Wait(string action)
    {
        if (action.Equals("Die"))
        {
            yield return new WaitForSeconds(1);
            DestroyObject(gameObject);
        }
    }
}
