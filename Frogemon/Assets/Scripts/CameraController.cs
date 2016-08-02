using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    const float MIN_CAMERA_X = 3.25f; // camera extents, these are matched to the
    const float MAX_CAMERA_X = 3.25f; // values specified in the editor
    const float MIN_CAMERA_Y = 2.44f;
    const float MAX_CAMERA_Y = 6.06f;
    const float MIN_CAMERA_Z = -10f;
    const float MAX_CAMERA_Z = -10f;

    bool isAnimating = false; // flag to control camera animation
    Vector3 animationTarget; // end position of the animation
    float animationSpeed = 1f; // speed of the camera movement

    // move the camera to the given position, constraining it to the extents
    // above
    public void MoveTo(Vector3 a_pos)
    {
        // track final position
        float x = Mathf.Max(MIN_CAMERA_X, a_pos.x);
        x = Mathf.Min(MAX_CAMERA_X, x);
        float y = Mathf.Max(MIN_CAMERA_Y, a_pos.y);
        float z = Mathf.Max(MIN_CAMERA_Z, a_pos.z);
        z = Mathf.Min(MAX_CAMERA_Z, z);

        // move the camera
        transform.position = new Vector3(x, y, z);
    }

    // snap the camera to the top of the level (max position)
    public void SnapToTop()
    {
        transform.position = new Vector3(MAX_CAMERA_X, MAX_CAMERA_Y, MAX_CAMERA_Z);

        // ensure animation is disabled
        isAnimating = false;
    }

    // snap the camera to the bottom of the level (min position)
    public void SnapToBottom()
    {
        transform.position = new Vector3(MIN_CAMERA_X, MIN_CAMERA_Y, MIN_CAMERA_Z);

        // ensure animation is disabled
        isAnimating = false;
    }

    // animate the camera to the bottom of the level (min position)
    // speed must be greater than 0
    // returns the duration of the animation in seconds
    public float AnimateToBottom(float a_speed)
    {
        // set the target and speed
        animationTarget = new Vector3(MIN_CAMERA_X, MIN_CAMERA_Y, MIN_CAMERA_Z);
        animationSpeed = a_speed;

        // enable animation
        isAnimating = true;

        float distance = Vector3.Distance(transform.position, animationTarget);
        return distance / a_speed;
    }

    void Update()
    {
        // is animation enabled?
        if (isAnimating)
        {
            transform.position = Vector3.MoveTowards(transform.position, animationTarget, animationSpeed * Time.deltaTime);

            // is the animation finished?
            if (transform.position.Equals(animationTarget))
            {
                isAnimating = false;
            }
        }
    }
}
