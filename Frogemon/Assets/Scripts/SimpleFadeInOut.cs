using UnityEngine;

public class SimpleFadeInOut : MonoBehaviour
{

    float fadeSpeed = 2f; // full fades per second

    void Start()
    {
        // start out fully transparent
        Color c = GetComponent<Renderer>().material.color;
        c.a = 0f;
        GetComponent<Renderer>().material.color = c;
    }

    void Update ()
    {
        // get the object color
        Color c = GetComponent<Renderer>().material.color;

        // adjust the alpha
        c.a += fadeSpeed * Time.deltaTime;

        // sanitize the value and manage fade direction
        if (c.a > 1f)
        {
            // clamp value
            c.a = 1f;

            // switch directions
            fadeSpeed = -fadeSpeed;
        }
        else if (c.a < 0f)
        {
            // clamp value
            c.a = 0f;

            // switch directions
            fadeSpeed = -fadeSpeed;
        }

        // set the new color
        GetComponent<Renderer>().material.color = c;
    }
}
