using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float throwForce, magnitudeMin, magnitudeMax;
    public PlayerState playerState;
    public static bool throwAllowed;
    public GameObject graphes;
    private bool isGoingRight = true;

    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private bool isPcControl = true;
    private Vector2 startPosition, currentPosition, direction, lastCollidePosition;
    private float magnitude;
    private bool isGrounded = true;

    [Header("Trajectory")]
    public GameObject trajectoryDot;
    public GameObject dotStorage;
    private GameObject[] TrajectoryDots;
    public int numberOfDot;

    [Header("Walls")]
    public float timerPushableDestruction = 3f;
    [Range(0, 10f)]
    public float bouncyPushableWall = 3f;
    [Range(0, 10f)]
    public float bouncyStaticWall = 5f;
    [Range(0, 124f)]
    public float forceBouncyWall = 100f;


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

        throwAllowed = true;
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

    #region Controls

    private void PcControls()
    {
        currentPosition = Input.mousePosition;
        direction = (startPosition - currentPosition).normalized;
        GetCurrentMagnitude();

        if (Input.GetMouseButtonDown(0) && playerState == PlayerState.idle)
        {
            startPosition = currentPosition;
            UpdatePlayerState(PlayerState.charging);
            ResetValues();
        }

        else if (Input.GetMouseButtonUp(0) && playerState == PlayerState.charging)
        {
            if (magnitude > 0)
            {
                GetColliderDirection();
                UpdatePlayerState(PlayerState.moving);
                rb.AddForce(direction * magnitude * throwForce, ForceMode2D.Impulse);
                GameManager.gameManager.Shoot();
            }
            StartChecking(); //
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
                    rb.AddForce(direction * magnitude * throwForce, ForceMode2D.Impulse);
                    GameManager.gameManager.Shoot();
                }
                StartChecking(); //
                
                print("Dir " + direction);
                print("Magnitude " + magnitude);
            }
        }
    }

    #endregion

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

    void GetColliderDirection()//return false if the dir is into the actual wall
    {
        Vector2 dirCollideFromPlayer = (transform.position * lastCollidePosition).normalized;
        Debug.Log("DirCollideFromPLayer " + dirCollideFromPlayer);
        Debug.Log("Direction " + direction);
        Debug.Log("Diff Bot " + Vector2.Distance(dirCollideFromPlayer, Vector2.down));
        Debug.Log("Diff Top " + Vector2.Distance(dirCollideFromPlayer, Vector2.up));
        Debug.Log("Diff Right " + Vector2.Distance(dirCollideFromPlayer,Vector2.right));
        Debug.Log("Diff Left " + Vector2.Distance(dirCollideFromPlayer,Vector2.left));
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "StickyWall" && playerState == PlayerState.moving)
        {
            UpdatePlayerState(PlayerState.idle);

            Debug.Log("collide with : " + other.gameObject.tag);
            lastCollidePosition = other.contacts[0].point;
            isGrounded = true;
        }
        if (other.gameObject.tag == "PushableWall")
        {
            var force = transform.position - other.transform.position;
            force.Normalize();
            rb.AddForce(force * (bouncyPushableWall * forceBouncyWall));
            Destroy(other.gameObject, timerPushableDestruction);
        }
        if (other.gameObject.tag == "StaticWall")
        {
            var force = transform.position - other.transform.position;
            force.Normalize();
            rb.AddForce(force * (bouncyStaticWall * forceBouncyWall));
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (playerState == PlayerState.moving && isGrounded)
        {
            isGrounded = false;
            Debug.Log("exit with : " + other.gameObject.tag);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ennemy")
        {
            if (collision.GetComponent<Ennemy>().IsDying())
            {
                return;
            }
            collision.GetComponent<Ennemy>().Die();
            LevelManager.levelManager.EnnemyDeath();
            Destroy(collision.gameObject);
        }
        /* ***************   A Delete   *************** *
        else if (collision.tag == "LeftCollider")
        {
            collision.GetComponentInParent<BouncingWall>().LeftHit();
        }
        else if (collision.tag == "RightCollider")
        {
            collision.GetComponentInParent<BouncingWall>().RightHit();
        }
        else if (collision.tag == "TopCollider")
        {
            collision.GetComponentInParent<BouncingWall>().UpHit();
        }
        else if (collision.tag == "BotCollider")
        {
            collision.GetComponentInParent<BouncingWall>().DownHit();
        }
        */
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
        throwAllowed = false;
        checkGm.CheckMoving();
    }

}

public enum PlayerState
{
    idle,
    charging,
    moving,
}