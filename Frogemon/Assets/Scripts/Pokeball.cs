using UnityEngine;
using System.Collections;

public class Pokeball : MonoBehaviour
{
    float speed = 1f;
    bool movingRight = true;
    Vector3 position;

    public GridController gridController;

    // Use this for initialization
    void Start()
    {
        speed = 1f + ((gridController.level * 0.1f) - 0.1f);

        // Check for movement here
    }
    // Update is called once per frame
    void Update()
    {
        if (movingRight == true)
        {

        }
    }
}
