using UnityEngine;
using UnityEngine.UI;


public class CameraMove : MonoBehaviour
{
    public ManagerScript ms;

    private float xAxis;
    private float yAxis;
    private float zoom;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>(); // get the camera component for later use
    }

    // Update is called once per frame
    void Update()
    {
        if (ms.saveLoadMenuOpen == false) // if no save or load menus are open.
        {
            xAxis = Input.GetAxis("Horizontal"); // get user input
            yAxis = Input.GetAxis("Vertical");

            //zoom = Input.GetAxis("Mouse ScrollWheel") * 10;

            // move camera based on info from xAxis and yAxis
            // transform.Translate(new Vector3(xAxis * 1 * -1, 0.0f));
            

            //change camera's orthographic size to create zooming in and out. Can only be between -25 and -5.
            //if (zoom < 0 && cam.orthographicSize >= -25)
            //    cam.orthographicSize -= zoom * -1;

            //if (zoom > 0 && cam.orthographicSize <= -5)
            //    cam.orthographicSize += zoom * 1;
        }
    }
}
