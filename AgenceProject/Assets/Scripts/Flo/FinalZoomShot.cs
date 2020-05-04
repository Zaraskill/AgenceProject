using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoomShot : MonoBehaviour
{
    [Header("Scripts")]
    public LevelState ls;
    public PlayerController player;

    [Header("Controls Zoom")]
    public bool ZoomActive;
    public float zoomSpeed;
    public float minSizeCamera = 2f;
    public float maxSizeCamera = 4.5f;
    [Header("Controls Slow")]
    public bool isSlowmo;
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    
    
    AudioManager am;
    Camera mc;
    
    private GameObject lastEnemy;

    void Start()
    {
        mc = Camera.main;
        am = FindObjectOfType<AudioManager>();
        maxSizeCamera = mc.orthographicSize;
    }

    public void LateUpdate()
    {
        transform.position = player.transform.position;

        if (isSlowmo && lastEnemy != null)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }

        if (ZoomActive)
        {
            mc.orthographicSize = Mathf.Lerp(mc.orthographicSize, minSizeCamera, zoomSpeed);
            mc.transform.position = Vector3.Lerp(mc.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10f), zoomSpeed);
        }
        else
        {
            mc.orthographicSize = Mathf.Lerp(mc.orthographicSize, maxSizeCamera, zoomSpeed);
            mc.transform.position = Vector3.Lerp(mc.transform.position, new Vector3(0, 0, -10f), zoomSpeed);
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        ZoomActive = true;
        am.Play("slow");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ennemy" && ls.ennemiTest <= 1)
        {
            lastEnemy = collision.gameObject;
            isSlowmo = true;
            DoSlowMotion();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ennemy")
        {
            Reset();
        }
    }

    void Reset()
    {
        Time.timeScale = 1f;
        isSlowmo = false;
        ZoomActive = false;
        am.Stop("slow");
    }

}
