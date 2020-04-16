using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float throwForce, magnitudeMin, magnitudeMax;
    public PlayerState playerState;

    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private bool isPcControl = true;
    private Vector2 startPosition, currentPosition, direction;
    private float magnitude;
    private bool throwAllowed = true;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isPcControl)
            PcControls();
        if(!isPcControl)
            MobileControls();
    }

    private void PcControls()
    {
        currentPosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = currentPosition;
            ResetValues();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (magnitude > 0)
            {
                rb.AddForce(direction * magnitude * throwForce, ForceMode2D.Impulse);
            }
        }
        direction = (startPosition - currentPosition).normalized;
        GetCurrentMagnitude();
    }

    private void MobileControls()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            currentPosition = Input.touches[0].position;

            if (t.phase == TouchPhase.Began)
            {
                startPosition = Input.touches[0].position;
                ResetValues();
            }
            else if (t.phase == TouchPhase.Moved)
            {
                direction = (startPosition - currentPosition).normalized;
                GetCurrentMagnitude();
            }
            else if (t.phase == TouchPhase.Ended && throwAllowed)
            {
                if (magnitude > 0)
                {
                    rb.AddForce(direction * magnitude * throwForce, ForceMode2D.Impulse);
                }

                //_throwAllowed = false;
                print("Dir " + direction);
                print("Magnitude " + magnitude);
            }
        }
    }

    private void ResetValues()
    {
        magnitude = 0;
        direction = Vector2.zero;
    }

    void GetCurrentMagnitude()
    {
        float rawMagnitude = (startPosition - currentPosition).magnitude / 10;
        if (rawMagnitude > magnitudeMin)
        {
            magnitude = (Mathf.Clamp(rawMagnitude, 0, magnitudeMax) / magnitudeMax);
            playerState = PlayerState.charging;
        }
        else
            magnitude = 0;
    }

    #region Debug

    void OnGUI()
    {
        GUILayout.Label("Force : " + (magnitude * 100) + "%");
    }

    #endregion
}

public enum PlayerState
{
    idle,
    charging,
    moving,
}