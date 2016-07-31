using UnityEngine;
using System.Collections;

public class Pokeball : MonoBehaviour
{
    float speed = 1f;
    bool movingRight = true;
    Vector3 position;
    Animator anim;

    public GridController gridController;
    public LevelController levelController;

    // Use this for initialization
    void Start()
    {
        speed = 1f + ((levelController.level * 0.1f) - 0.1f);

        // Check for movement here
    }
    // Update is called once per frame
    void Update()
    {
        if (movingRight == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, position + Vector3.right , speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, position + Vector3.left, speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Pikachu"))
        {
            anim.Play("Explode");
            Destroy(this);
        }

    }
}
