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

    // move the camera to the given position, constraining it to the extents
    // above
    public void MoveTo(Vector3 a_pos)
    {
        // track final position
        float x = Mathf.Max(MIN_CAMERA_X, a_pos.x);
        x = Mathf.Min(MAX_CAMERA_X, x);
        float y = Mathf.Max(MIN_CAMERA_Y, a_pos.y);
        y = Mathf.Min(MAX_CAMERA_Y, y);
        float z = Mathf.Max(MIN_CAMERA_Z, a_pos.z);
        z = Mathf.Min(MAX_CAMERA_Z, z);

        // move the camera
        transform.position = new Vector3(x, y, z);
    }

    // snap the camera to the top of the level (max position)
    public void SnapToTop()
    {
        transform.position = new Vector3(MAX_CAMERA_X, MAX_CAMERA_Y, MAX_CAMERA_Z);
    }

    // snap the camera to the bottom of the level (min position)
    public void SnapToBottom()
    {
        transform.position = new Vector3(MIN_CAMERA_X, MIN_CAMERA_Y, MIN_CAMERA_Z);
    }
}
