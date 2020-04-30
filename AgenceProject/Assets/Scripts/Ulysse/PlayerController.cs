using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool throwAllowed;
    public static float timerPushableDestroy;

    [Header("Player Values")]
    public float shotForce;
    public float speedMax;
    public float magnitudeMin;
    public float magnitudeMax;
    public PlayerState playerState;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private bool isPcControl = true;
    [SerializeField] private bool isStuck;
    [SerializeField] private float SetTimerPushableDestroy;

    private bool jump;
    private Vector2 startPosition, currentPosition, direction, lastCollidePosition;
    private float magnitude;

    [Header("Graphic")]
    public GameObject graphes;
    private bool isGoingRight = true;
    private Animator animator;

    [Header("Trajectory")]
    public GameObject trajectoryDot;
    public GameObject dotStorage;
    public int numberOfDot;
    private GameObject[] TrajectoryDots;

    [Header("Walls")]
    [Range(0, 5f)]
    public float pushableWBounciness = 3f;
    [Range(0, 5f)]
    public float staticWBounciness = 5f;
    [Range(0, 30f)]
    public float Bounciness = 15f;

    

    //checking script
    private CheckListVelocity checkGm;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        checkGm = GetComponentInParent<CheckListVelocity>();
        timerPushableDestroy = SetTimerPushableDestroy;

        CreateDots();
        throwAllowed = true;
    }

    void FixedUpdate()
    {
        Shot();
        if (playerState != PlayerState.idle)
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedMax);
    }

    void Update()
    {
        ReadingInput();

        if (playerState == PlayerState.charging)
            Trajectory(); //
        else if (playerState == PlayerState.moving)
        {
            //if (isGoingRight)
            //{
            //    if (Vector2.Dot(graphes.transform.right, rb.velocity) < 0)
            //    {
            //        graphes.GetComponent<SpriteRenderer>().flipX = true;
            //        isGoingRight = !isGoingRight;
            //    }
            //    else
            //    {
            graphes.transform.Rotate(0, 0, Vector2.SignedAngle(graphes.transform.right, rb.velocity));
            //    }                
            //}
            //if (!isGoingRight)
            //{
            //    if (Vector2.Dot(graphes.transform.right, rb.velocity) > 0)
            //    {
            //        graphes.GetComponent<SpriteRenderer>().flipX = false;
            //        isGoingRight = !isGoingRight;
            //    }
            //    else
            //    {
            //        graphes.transform.Rotate(0, 0, Vector2.SignedAngle(-graphes.transform.right, rb.velocity));
            //    }
            //}

        }

    }

    public void UpdatePlayerState(PlayerState newState)
    {
        playerState = newState;
        if (playerState == PlayerState.idle)
        {
            rb.bodyType = RigidbodyType2D.Static;
            animator.SetBool("Fly", false);
        }

        else if (playerState == PlayerState.charging)
        {
            rb.bodyType = RigidbodyType2D.Static;

            dotStorage.SetActive(true);
            animator.SetBool("Charging", true);
        }

        else if (playerState == PlayerState.moving)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            dotStorage.SetActive(false);
            animator.SetBool("Charging", false);
            animator.SetBool("Fly", true);
            animator.SetBool("Fly Up", true);
        }
    }

    #region Controls

    private void ReadingInput()
    {
        if (throwAllowed)
        {
            if (isPcControl)
                PcControls();
            else if (!isPcControl)
                MobileControls();
        }
        else
        {
            animator.SetFloat("Up", rb.velocity.y);
        }
    }

    private void PcControls()
    {
        currentPosition = Input.mousePosition;
        direction = (startPosition - currentPosition).normalized;
        GetCurrentMagnitude();

        if (Input.GetMouseButtonDown(0) && throwAllowed)
        {
            startPosition = currentPosition;
            UpdatePlayerState(PlayerState.charging);
            ResetValues();
        }

        else if (Input.GetMouseButtonUp(0) && playerState == PlayerState.charging)
        {
            Debug.Log(magnitude);
            if (magnitude > 0.4f)
            {
                GetColliderSide();
                UpdatePlayerState(PlayerState.moving);
                jump = true;
                StartCoroutine(SetIsStuckToFalseLate());
                GameManager.gameManager.Shoot();
                StartChecking();
            }
        }
    }

    private void MobileControls()
    {
        if (Input.touchCount > 0 && playerState == PlayerState.idle)
        {
            Touch t = Input.GetTouch(0);
            currentPosition = Input.touches[0].position;

            if (t.phase == TouchPhase.Began)
            {
                startPosition = Input.touches[0].position;
                ResetValues();
            }
            else if (t.phase == TouchPhase.Moved && playerState == PlayerState.idle)
            {
                direction = (startPosition - currentPosition).normalized;
                GetCurrentMagnitude();
                UpdatePlayerState(PlayerState.charging);
                Debug.Log("moved");
            }
            else if (t.phase == TouchPhase.Ended && playerState == PlayerState.charging)
            {
                if (magnitude > 0)
                {
                    UpdatePlayerState(PlayerState.moving);
                    rb.AddForce(direction * magnitude * shotForce, ForceMode2D.Impulse);
                    GameManager.gameManager.Shoot();
                }
                StartChecking(); //

                print("Dir " + direction);
                print("Magnitude " + magnitude);
            }
        }
    }

    #endregion

    void Shot()
    {
        if(jump)
            rb.AddForce(direction * magnitude * shotForce, ForceMode2D.Impulse);
        jump = false;
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

    void GetColliderSide() // Get the Collider Side
    {
        Vector2 dirCollideFromPlayer = (lastCollidePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2[] dirArray = {Vector2.up, Vector2.right, Vector2.down, Vector2.left};
        float[] diff = new float[4];
        int finalDirection = 6;
        for (int i = 0; i < 4; i++)
        {
            diff[i] = Vector2.Distance(dirCollideFromPlayer, dirArray[i]);
        }
        for (int i = 0; i < 4; i++)
        {
            int x = i + 1;
            while (x < 4 && diff[i] < diff[x])
                x++;

            if (x > 3)
            {
                finalDirection = i;
                break;
            }
        }
        Debug.Log("Final Dir " + finalDirection); // 0 = Up, 1 = Right, etc...
    }

    #region Debug

    void OnGUI()
    {
        GUILayout.Label("Force : " + (magnitude * 100) + "%");
        GUILayout.Label("ThrowAllowed : " + throwAllowed);
        GUILayout.Label("playerState : " + playerState);
        //GUILayout.Label("shoot : " + GameManager.gameManager.shoot);
        //GUILayout.Label("Ennemy " + LevelManager.levelManager.level.ennemiTest);
    }
    #endregion

    #region RenderSlingshot

    void CreateDots()
    {
        TrajectoryDots = new GameObject[numberOfDot];
        for (int i = 0; i < numberOfDot; i++)
        {
            TrajectoryDots[i] = Instantiate(trajectoryDot, dotStorage.transform);
        }
        dotStorage.SetActive(false);
    }
    #endregion

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("collide with : " + other.gameObject.tag + " / as " + isStuck + "/ state " + playerState + " frame " + Time.frameCount);
        if (other.gameObject.tag == "StickyWall" && playerState == PlayerState.moving && !isStuck) //&& playerState == PlayerState.moving && !isGrounded
        {
            UpdatePlayerState(PlayerState.idle);
            lastCollidePosition = other.contacts[0].point;
            isStuck = true;
        }

        else if (other.gameObject.tag == "StaticWall")
        {
            if (rb.velocity.magnitude > 3)
            {
                rb.AddForce(rb.velocity.normalized * (staticWBounciness * Bounciness));
            }
        }

        else if (other.gameObject.tag == "PushableWall")
        {
            if (rb.velocity.magnitude > 3)
            {
                rb.AddForce(rb.velocity.normalized * (pushableWBounciness * Bounciness));
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //Debug.Log("exit with : " + other.gameObject.tag + " / as " + isStuck + "/ state " + playerState + " frame " + Time.frameCount);
        if (playerState == PlayerState.moving && isStuck && other.gameObject.tag == "StickyWall")
        {
            isStuck = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //Kill enemy
    {
        if (collision.tag == "Ennemy")
        {
            Debug.Log(collision.tag);
            if (collision.GetComponent<Ennemy>().IsDying())
            {
                return;
            }
            collision.GetComponent<Ennemy>().Die();
            LevelManager.levelManager.EnnemyDeath();

            animator.Play("Eat");
            Destroy(collision.gameObject);
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
                new Vector2(direction.x * (shotForce * magnitude), direction.y * (shotForce * magnitude)) * elapsedTime + //ut
                0.5f * Physics2D.gravity * elapsedTime * elapsedTime;
    }

    //Check Movement
    public void StartChecking()
    {
        throwAllowed = false;
        checkGm.CheckMoving();
    }


    public IEnumerator SetIsStuckToFalseLate() //to avoid the sticky bugs
    {
        yield return StartCoroutine(WaitFor.Frames(5));
        isStuck = false;
        Debug.Log("!IsStuck " + Time.frameCount);
    }

}

public enum PlayerState
{
    idle,
    charging,
    moving,
}