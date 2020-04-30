using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoomShot : MonoBehaviour
{
    [Header("Scripts")]
    public LevelState ls;
    public PlayerController player;

    [Header("Controls")]
    public AnimationCurve control;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    bool isSlowmo = false;

    Camera mc;
    private float sizeCamera;

    void Start()
    {
        mc = Camera.main;
        sizeCamera = mc.orthographicSize;
    }

    void Update()
    {
        transform.position = player.transform.position;

        if (isSlowmo)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            
            if (mc.orthographicSize > 2f)
            {
                mc.orthographicSize = sizeCamera * (1f - control.Evaluate(Time.timeScale));
            }
            mc.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            if (Time.timeScale == 1f)
            {
                Reset();
            }
        }

        if (ls.ennemiTest <= 0)
        {
            FindObjectOfType<AudioManager>().Play("victory");
        }
        
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        FindObjectOfType<AudioManager>().Play("slow");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ennemy" && ls.ennemiTest <= 1)
        {
            isSlowmo = true;
            DoSlowMotion();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ennemy")
        {
            Time.timeScale = 1f;
        }
    }

    void Reset()
    {
        isSlowmo = false;
        mc.transform.position = new Vector3(0, 0, -10f);
        mc.orthographicSize = sizeCamera;
        FindObjectOfType<AudioManager>().Stop("slow");
    }

}
