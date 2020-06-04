using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public static bool throwAllowed;
    [SerializeField] public static PlayerState playerState;

    [Header("Player Values")]
    public float shotForce;
    public float speedMax;
    public float magnitudeMin;
    public float magnitudeMax;
    private Rigidbody2D rb;
    [SerializeField] private bool isPcControl = true;

    private bool jump;
    private bool firstShot = true;
    private Vector2 startPosition, currentPosition, inputDir, lastCollidePosition, direction;
    private float magnitude;

    [Header("Graphic")]
    public GameObject graphes;
    private bool isGoingRight = true;
    private bool needRotate = false;
    private Animator animator;
    GUIStyle style = new GUIStyle();

    [Header("Trajectory")]
    public GameObject trajectoryDot;
    public GameObject dotStorage;
    public int numberOfDot;
    private GameObject[] TrajectoryDots;
    private int colliderSide; //0=Top, 1=Right, 2=Bot, 3=Left
    private bool isValuableShot;
    private Coroutine CheckingSlideCoroutine;
    private int slidingStrike; //increased each Update it's potentially sliding
    private Vector2[] dirArray = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

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
        style.alignment = TextAnchor.MiddleLeft;
        style.fontSize = 16;
        style.richText = true;
        animator = graphes.GetComponent<Animator>();
        checkGm = GetComponentInParent<CheckListVelocity>();
        checkGm.playerRB = rb;

        CreateDots();
        throwAllowed = true;
    }

    void FixedUpdate()
    {
        if (GameManager.gameManager.gameState == GameManager.STATE_PLAY.inTutorial || GameManager.gameManager.gameState == GameManager.STATE_PLAY.inMenu || GameManager.gameManager.isInMenu)
        {

        }
        else
        {
            Shot();
            ClampSpeed();
        }
        
    }

    #region FixedUpdate Calls

    void Shot()
    {
        if (jump)
            rb.AddForce(inputDir * magnitude * shotForce, ForceMode2D.Impulse);
        jump = false;
    }

    void ClampSpeed()
    {
        if (playerState == PlayerState.moving)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedMax);
        }
    }
    #endregion

    void Update()
    {
        ResetValues();
        if (GameManager.gameManager.gameState == GameManager.STATE_PLAY.inTutorial || GameManager.gameManager.gameState == GameManager.STATE_PLAY.inMenu || GameManager.gameManager.isInMenu)
        {
            if (playerState == PlayerState.charging)
            {
                UpdatePlayerState(PlayerState.idle);
            }
        }
        else
        {
            ReadingInput();
            CheckSliding();
            RotateWithTrajectory();
        }
        if (needRotate)
        {
            RotateOnColliderSide();
            needRotate = false;
        }
    }

    #region Update Calls

    private void ResetValues()
    {
        graphes.transform.localScale = new Vector3(1, 1, 1);
    }

    void IsValuableShot()
    {
        float diff = Vector2.Distance(inputDir, dirArray[colliderSide]);

        if (diff < 1.4f && playerState == PlayerState.charging)
            isValuableShot = true;
        else
            isValuableShot = false;
    }

    void RotateWithTrajectory()
    {
        if (playerState == PlayerState.moving)
        {
            direction = rb.velocity.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (direction.x >= 0)
            {
                graphes.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                //graphes.transform.Rotate(0, graphes.transform.rotation.y * -1, Vector2.SignedAngle(graphes.transform.right, direction));
            }

            else
            {
                //graphes.transform.rotation = Quaternion.Euler(0, 180, graphes.transform.rotation.z);
                //angle -= 90;
                graphes.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                graphes.transform.localScale = new Vector3(1,-1,1);
                //graphes.transform.Rotate(graphes.transform.rotation.x * (180 / graphes.transform.rotation.x), 0, 0);
            }
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
        inputDir = (startPosition - currentPosition).normalized;
        GetCurrentMagnitude();

        if (Input.GetMouseButtonDown(0) && throwAllowed)
        {
            startPosition = currentPosition;
            UpdatePlayerState(PlayerState.charging);
            VFXManager.instance.PlayOnScreenPositon("Circle_OnScreen", currentPosition);
        }

        if (Input.GetMouseButton(0) && throwAllowed)
        {
            IsValuableShot();
            
            Trajectory();
        }

        else if (Input.GetMouseButtonUp(0) && playerState == PlayerState.charging)
        {
            Debug.Log(magnitude);
            Debug.Log("isVS " + isValuableShot);
            if (magnitude > 0 && isValuableShot)
            {
                UpdatePlayerState(PlayerState.moving);
                direction = inputDir;
                jump = true;
                GameManager.gameManager.Shoot();
                firstShot = false;
            }
            else
            {
                dotStorage.SetActive(false);
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
                VFXManager.instance.PlayOnScreenPositon("Circle_OnScreen", currentPosition);
            }
            else if (t.phase == TouchPhase.Moved && playerState == PlayerState.idle)
            {
                inputDir = (startPosition - currentPosition).normalized;
                GetCurrentMagnitude();
                UpdatePlayerState(PlayerState.charging);
            }
            else if (t.phase == TouchPhase.Ended && playerState == PlayerState.charging)
            {
                if (magnitude > 0)
                {
                    UpdatePlayerState(PlayerState.moving);
                    rb.AddForce(inputDir * magnitude * shotForce, ForceMode2D.Impulse);
                    GameManager.gameManager.Shoot();
                }
                else
                {
                    dotStorage.SetActive(false);
                }
            }
        }
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

        #endregion

    #endregion

    public void UpdatePlayerState(PlayerState newState)
    {
        playerState = newState;
        if (playerState == PlayerState.idle)
        {
            rb.bodyType = RigidbodyType2D.Static;
            animator.SetBool("Fly", false);
            needRotate = true;
        }

        else if (playerState == PlayerState.charging)
        {
            rb.bodyType = RigidbodyType2D.Static;

            dotStorage.SetActive(true);
            animator.SetBool("Charging", true);
            AudioManager.instance.Play("charging");
        }

        else if (playerState == PlayerState.moving)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            dotStorage.SetActive(false);
            animator.SetBool("Charging", false);
            animator.SetBool("Fly", true);
            animator.SetBool("Fly Up", true);
            AudioManager.instance.Stop("charging");
            AudioManager.instance.Play("shoot");
            VFXManager.instance.Stop("Circle_OnScreen");
        }
    }

    private void RotateOnColliderSide()
    {
        graphes.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Debug.Log("side " + colliderSide);
        if (colliderSide == 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);

        else if (colliderSide == 1)
            transform.rotation = Quaternion.Euler(0, 0, -90);

        else if (colliderSide == 2)
            transform.rotation = Quaternion.Euler(0,0,180);

        else if (colliderSide == 3)
            transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    #region Collision

    void GetColliderSide(string colliderTag)
    {
        if (colliderTag != "StaticWall" && colliderTag != "StickyWall")
            return;

        Vector2  dirCollideToPlayer = (new Vector2(transform.position.x, transform.position.y) - lastCollidePosition).normalized;
        float[] diff = new float[4];
        for (int i = 0; i < 4; i++)
        {
            diff[i] = Vector2.Distance(dirCollideToPlayer, dirArray[i]);
        }
        for (int i = 0; i < 4; i++)
        {
            int x = i + 1;
            while (x < 4 && diff[i] < diff[x])
                x++;

            if (x > 3)
            {
                colliderSide = i;
                break;
            }
        }
    }

    bool ItShouldStick()
    {
        if (Vector2.Dot(direction, dirArray[colliderSide]) >= 0)
            return false;
        return true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        int rdm = Random.Range(1, 13);
        string otherTag = other.gameObject.tag;
        //Debug.Log("collide with : " + otherTag + " / state " + playerState + " / velo.norm " + rb.velocity.normalized + " frame " + Time.frameCount);
        lastCollidePosition = other.contacts[0].point;
        GetColliderSide(otherTag);
        AudioManager.instance.Play("player_" + rdm);
        VFXManager.instance.PlayOnPositon("Blob_Contact", transform.position);
            if (otherTag == "StickyWall" && (playerState == PlayerState.moving && ItShouldStick()) || firstShot)
            {
                UpdatePlayerState(PlayerState.idle);
            }

            else if (otherTag == "StaticWall")
            {
                if (rb.velocity.magnitude > 3)
                {
                    rb.AddForce(rb.velocity.normalized * (staticWBounciness * Bounciness));
                }
                else if (rb.velocity.magnitude < 1 && colliderSide == 0)
                {
                    UpdatePlayerState(PlayerState.idle);
                }
            }

            else if (otherTag == "PushableWall")
            {
                if (rb.velocity.magnitude > 3)
                    rb.AddForce(rb.velocity.normalized * (pushableWBounciness * Bounciness));
                else
                    StopCoroutine(StartCheckSliding());
            }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (CheckingSlideCoroutine != null)
            StopCoroutine(CheckingSlideCoroutine);

        //Debug.Log("exit with : " + other.gameObject.tag + "/ state " + playerState + " frame " + Time.frameCount);
        if (other.gameObject.tag == "StaticWall")
        {
            CheckingSlideCoroutine = StartCoroutine(StartCheckSliding());
        }
        rb.angularVelocity = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) //Kill enemy
    {
        if (collision.tag == "Ennemy")
        {
            int rdm = Random.Range(1, 5);
            AudioManager.instance.Play("enemy_" + rdm);
            if (collision.GetComponent<Ennemy>().IsDying())
            {
                return;
            }
            collision.GetComponent<Ennemy>().Die();
            LevelManager.levelManager.EnemyDeath();

            animator.Play("Eat");
            Destroy(collision.gameObject);
        }
    }
    #endregion

    #region RenderSlingshot & Trajectory

    void CreateDots()
    {
        TrajectoryDots = new GameObject[numberOfDot];
        for (int i = 0; i < numberOfDot; i++)
        {
            TrajectoryDots[i] = Instantiate(trajectoryDot, dotStorage.transform);
        }
        dotStorage.SetActive(false);
    }
    
    private void Trajectory() //PEUX �TRE OPTI
    {
        for (int i = 0; i < numberOfDot; i++)
        {
            TrajectoryDots[i].transform.position = CalculatePosition(i * 0.1f);

            /* COLOR *
            float distance = Vector3.Distance(transform.position, TrajectoryDots[i].transform.position);
            if(distance >= 3 && distance < 5 )
            {
                TrajectoryDots[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 255, 255); //blue
            }
            else if (distance >= 5)
            {
                TrajectoryDots[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 0, 255); //purple
            }
            else
            {
                TrajectoryDots[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 255, 0); //green
            }
            */

            if (isValuableShot)
            {
                TrajectoryDots[i].transform.GetChild(0).gameObject.SetActive(true);
                TrajectoryDots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                TrajectoryDots[i].transform.GetChild(0).gameObject.SetActive(false);
                TrajectoryDots[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        return rb.position + //X0
                new Vector2(inputDir.x * (shotForce * magnitude), inputDir.y * (shotForce * magnitude)) * elapsedTime + //ut
                0.5f * Physics2D.gravity * elapsedTime * elapsedTime;
    }

    #endregion

    #region Check

    private void CheckSliding()
    {
        if (playerState == PlayerState.moving && CheckingSlideCoroutine != null)
        {
            if (Mathf.Approximately(rb.velocity.y, 0))
                slidingStrike++;
            else
                slidingStrike = 0;

            if (slidingStrike > 5)
            {
                UpdatePlayerState(PlayerState.idle);
                StopCoroutine(CheckingSlideCoroutine);
                Debug.Log("anti slide !!!" + " nbr slide : " + slidingStrike);
            }
        }
    }

    public IEnumerator StartCheckSliding()
    {
        slidingStrike = 0;
        yield return new WaitForSeconds(1);
    }
    #endregion

    /*
    #region Debug

    void OnGUI()
    {
        //GUILayout.Box("Force : " + (magnitude * 100) + "%\n" +
        //              "ThrowAllowed : " + throwAllowed + "%\n" +
        //              SceneManager.GetActiveScene().name);
        GUI.Box(new Rect(30, 50, 50, 50),
            SceneManager.GetActiveScene().name + "\n" +
            "ThrowAllowed : " + throwAllowed + "\n" +
            "Force : " + ((int)(magnitude * 100)) + "%", style);
        //GUILayout.BeginArea(new Rect(20, 100, 100, 100));
        //GUILayout.Box("Force : " + (magnitude * 100), style);
        //GUILayout.Label("PlayerState : " + playerState, style);
        //GUILayout.Label(SceneManager.GetActiveScene().name,style);
        //GUILayout.EndArea();
    }
    #endregion
    */

}

public enum PlayerState
{
    idle,
    charging,
    moving,
}