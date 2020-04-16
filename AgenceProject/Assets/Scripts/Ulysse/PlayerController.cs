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
        if (throwAllowed)
        {
            if (isPcControl)
                PcControls();
            if (!isPcControl)
                MobileControls();
        }
    }

    public void UpdatePlayerState(PlayerState newState)
    {
        playerState = newState;
        if (playerState == PlayerState.idle)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        else if (playerState == PlayerState.charging)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        else if (playerState == PlayerState.moving)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void PcControls()
    {
        currentPosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mousedown");
            startPosition = currentPosition;
            UpdatePlayerState(PlayerState.charging);
            ResetValues();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (magnitude > 0)
            {
                UpdatePlayerState(PlayerState.moving);
                rb.AddForce(direction * magnitude * throwForce, ForceMode2D.Impulse);
            }
            Debug.Log("mouse Up");
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
                UpdatePlayerState(PlayerState.charging);
                GetCurrentMagnitude();
                Debug.Log("moved");
            }
            else if (t.phase == TouchPhase.Ended)
            {
                if (magnitude > 0)
                {
                    UpdatePlayerState(PlayerState.moving);
                    rb.AddForce(direction * magnitude * throwForce, ForceMode2D.Impulse);
                }

                throwAllowed = false;
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