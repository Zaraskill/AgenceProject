using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float throwForce, magnitudeMin, magnitudeMax;
    public PlayerState playerState;
    public static bool throwAllowed = true;

    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private bool isPcControl = true;
    private Vector2 startPosition, currentPosition, direction, lastCollidePosition;
    private float magnitude;

    [Header("Trajectory")]
    public GameObject trajectoryDot;
    public GameObject dotStorage;
    private GameObject[] TrajectoryDots;
    public int numberOfDot;

    //checking script
    private CheckListVelocity checkGm;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        checkGm = GetComponentInParent<CheckListVelocity>();

        TrajectoryDots = new GameObject[numberOfDot];
        for (int i = 0; i < numberOfDot; i++)
        {
            TrajectoryDots[i] = Instantiate(trajectoryDot, dotStorage.transform);
        }
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

        if(playerState == PlayerState.charging)
            Trajectory(); //

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
        direction = (startPosition - currentPosition).normalized;
        GetCurrentMagnitude();

        if (Input.GetMouseButtonDown(0))
        {
            startPosition = currentPosition;
            UpdatePlayerState(PlayerState.charging);
            ResetValues();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (magnitude > 0)
            {
                CheckShotEfficiency();
                UpdatePlayerState(PlayerState.moving);
                rb.AddForce(direction * magnitude * throwForce, ForceMode2D.Impulse);
            }

            StartChecking(); //
        }
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
                UpdatePlayerState(PlayerState.charging);
                Debug.Log("moved");
            }
            else if (t.phase == TouchPhase.Ended)
            {
                if (magnitude > 0)
                {
                    UpdatePlayerState(PlayerState.moving);
                    rb.AddForce(direction * magnitude * throwForce, ForceMode2D.Impulse);
                }

                StartChecking(); //
                
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

    void CheckShotEfficiency()//return false if the dir is into the actual wall
    {
        Debug.Log((transform.position * lastCollidePosition).normalized);
        Debug.Log(direction);
        //Miss some code, it's WIP
    }

    #region Debug

    void OnGUI()
    {
        GUILayout.Label("Force : " + (magnitude * 100) + "%");
    }

    #endregion

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "StickyWall" && playerState == PlayerState.moving && !throwAllowed)
        {
            UpdatePlayerState(PlayerState.idle);
            Debug.Log("collide with : " + other.gameObject.tag);
            //throwAllowed = true;
            lastCollidePosition = other.contacts[0].point;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "StickyWall" && playerState == PlayerState.moving && throwAllowed)
        {
            Debug.Log("exit with : " + other.gameObject.tag);
            throwAllowed = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ennemy")
        {
            LevelManager.levelManager.EnnemyDeath();
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

    //Check Movement
    public void StartChecking()
    {
        //throwAllowed = false;
        checkGm.CheckMoving();
    }

}

public enum PlayerState
{
    idle,
    charging,
    moving,
}