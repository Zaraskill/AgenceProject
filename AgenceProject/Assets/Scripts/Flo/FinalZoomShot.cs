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
    float yVelocity = 0.0f;
    Vector3 Velocity;
    public float minSizeCamera = 2f;
    public float maxSizeCamera = 4.5f;
    [Header("Controls Slow")]
    public bool isSlowmo;
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    public float timeForReset = 0.7f;


    Camera mc;
    
    private GameObject lastEnemy;

    void Start()
    {
        mc = Camera.main;
        maxSizeCamera = mc.orthographicSize;
        Reset();
    }

    public void LateUpdate()
    {
        if (PlayerController.playerState != PlayerState.idle && ls.enemiTest <= 1)
        {
            transform.position = player.transform.position;

            if (isSlowmo && lastEnemy != null && !GameManager.gameManager.isInMenu)
            {
                Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            }

            Zoom();
        }
        else if (isSlowmo && PlayerController.playerState == PlayerState.idle)
        {
            Reset();
        }
        
    }

    void Zoom() // /!\ PAS OPTI
    {
        if (ZoomActive)
        {
            mc.orthographicSize = Mathf.SmoothDamp(mc.orthographicSize, minSizeCamera, ref yVelocity, zoomSpeed);
            mc.transform.position = Vector3.SmoothDamp(mc.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10f), ref Velocity, zoomSpeed);
        }
        else
        {
            mc.orthographicSize = Mathf.SmoothDamp(mc.orthographicSize, maxSizeCamera, ref yVelocity, zoomSpeed);
            mc.transform.position = Vector3.SmoothDamp(mc.transform.position, new Vector3(0, -0.1f, -10f), ref Velocity, zoomSpeed);
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        ZoomActive = true;
        AudioManager.instance.Play("slow");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ennemy" && ls.enemiTest <= 1)
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
            StartCoroutine(RetardReset());
        }
    }

    IEnumerator RetardReset()
    {
        yield return new WaitForSeconds(timeForReset);
        Reset();
    }

    void Reset()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isSlowmo = false;
        ZoomActive = false;
        AudioManager.instance.Stop("slow");
    }

}
