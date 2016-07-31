using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Pikachu"))
        {
            DestroyObject(this.gameObject);
        }
    }
}
