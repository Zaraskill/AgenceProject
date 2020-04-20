﻿using System.Collections;
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

    [Header("Trajectory")]
    public GameObject trajectoryDot;
    public GameObject dotStorage;
    private GameObject[] TrajectoryDots;
    public int numberOfDot;

    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        TrajectoryDots = new GameObject[numberOfDot];
        for (int i = 0; i < numberOfDot; i++)
        {
            TrajectoryDots[i] = Instantiate(trajectoryDot, dotStorage.transform);
        }
    }

    void Update()
    {
        ResetCollisionSettings();
        if (throwAllowed)
        {
            if (isPcControl)
                PcControls();
            if (!isPcControl)
                MobileControls();
        }

        if(playerState == PlayerState.charging)
            Trajectory();

    }

    void ResetCollisionSettings()
    {
        if(playerState == PlayerState.moving)
            Physics2D.IgnoreLayerCollision(8, 9, false);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ennemy")
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "LeftCollider")
        {
            collision.GetComponentInParent<BouncingWall>().sideJump = BouncingWall.SIDE_JUMP.LEFT;
        }
        else if (collision.tag == "RightCollider")
        {
            collision.GetComponentInParent<BouncingWall>().sideJump = BouncingWall.SIDE_JUMP.RIGHT;
        }
        else if (collision.tag == "TopCollider")
        {
            collision.GetComponentInParent<BouncingWall>().sideJump = BouncingWall.SIDE_JUMP.UP;
        }
        else if (collision.tag == "BotCollider")
        {
            collision.GetComponentInParent<BouncingWall>().sideJump = BouncingWall.SIDE_JUMP.DOWN;
        }
    }

    //Trajectoire 
    private void Trajectory()
    {
        for (int i = 0; i < numberOfDot; i++)
        {
            TrajectoryDots[i].transform.position = CalculatePosition(i * 0.1f);
        }
    }
    
    private Vector2 CalculatePosition(float elapsedTime)
    {
        return rb.position + //X0
                new Vector2(direction.x * (throwForce * magnitude), direction.y * (throwForce * magnitude)) * elapsedTime + //ut
                0.5f * Physics2D.gravity * elapsedTime * elapsedTime;
    }

}

public enum PlayerState
{
    idle,
    charging,
    moving,
}