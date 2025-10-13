/* 
CameraMove.cs
By: Jake Schott
*/

using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //CLASS CONSTANTS
    private float MOUSE_SENSITIVITY = 1.0f;

    private Vector2 mouseMove = new Vector2();
    private Vector2 prevPos = new Vector2(0f, 0); //x represents angle of camera, y represents angle of player capsule

    private Camera my_camera;
    private Transform camera_transform;
    private float zoomFOV = 40f;
    public Rigidbody rb;

    private void Start()
    {
        camera_transform = transform.GetChild(0);

        my_camera = camera_transform.GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //handle pause/unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        //if not paused
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            MouseMove();
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.Mouse1))
            {
                my_camera.fieldOfView = Mathf.Max(zoomFOV, my_camera.fieldOfView -= 100.0f * Time.deltaTime);
                return;
            }
            my_camera.fieldOfView = Mathf.Min(60.0f, my_camera.fieldOfView += 100.0f * Time.deltaTime);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
    void MouseMove()
    {
        Cursor.visible = false;
        //Gets mouse input
        mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //Increases the sensitivity to movement
        mouseMove *= MOUSE_SENSITIVITY * Mathf.Min(1.0f, (1.1f - ((60.0f - my_camera.fieldOfView) / 20.0f)));

        prevPos.y = Mathf.Clamp(prevPos.y, -90.0f, 90.0f);

        prevPos.y -= mouseMove.y;
        prevPos.x += mouseMove.x;

        transform.localRotation = Quaternion.AngleAxis(prevPos.x, Vector3.up);
        camera_transform.localRotation = Quaternion.AngleAxis(prevPos.y, Vector3.right);
    }
}