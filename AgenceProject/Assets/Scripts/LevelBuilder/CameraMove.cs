using UnityEngine;
using UnityEngine.UI;


public class CameraMove : MonoBehaviour
{
    public ManagerScript ms;

    private float xAxis;
    private float yAxis;
    private float zoom;
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    
    void Update()
    {
        if (ms.saveLoadMenuOpen == false)
        {
            // get user input
            xAxis = Input.GetAxis("Horizontal"); 
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
