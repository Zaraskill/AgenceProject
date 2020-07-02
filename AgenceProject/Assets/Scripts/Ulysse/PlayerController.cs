using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public static bool throwAllowed;
    [SerializeField] public static PlayerState playerState;

    public GameObject DebugPosCollide;

    [Header("Player Values")]
    public float shotForce;
    public float speedMax;
    public float magnitudeMin, magnitudeMax;
    private Rigidbody2D rb;
    [SerializeField] private bool isPcControl = true;

    private bool jump;
    public bool firstShot = true;
    private Vector2 startPosition, currentPosition, inputDir, lastCollidePosition, direction;
    private float magnitude;
    private float colliderRadius;

    [Header("Graphic")]
    public GameObject graphes;
    [HideInInspector] public Animator animator;
    private bool isGoingRight = true;
    private bool needRotate = false;
    GUIStyle style = new GUIStyle();

    [Header("Trajectory")]
    public GameObject trajectoryDot;
    public GameObject dotStorage;
    public int numberOfDot;
    public float maxExtendAngle;
    private GameObject[] TrajectoryDots;
    private GameObject lastBrickStuckOn;
    private int colliderSide, extendDir; //0=Top, 1=Right, 2=Bot, 3=Left
    private bool isValuableShot, isAnExtendShot, lastBrickImmaterial;
    private Coroutine CheckingSlideCoroutine;
    private int slidingStrike; //increased each Update it's potentially sliding
    private Vector2[] dirArray = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private float extendAngle;

    [Header("Walls Values")]
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
        style.fontSize = 40;
        style.fontStyle = FontStyle.Bold;
        style.richText = true;
        animator = graphes.GetComponent<Animator>();
        checkGm = GetComponentInParent<CheckListVelocity>();
        checkGm.playerRB = rb;
        colliderRadius = GetComponent<CircleCollider2D>().radius;

        CreateDots();
        throwAllowed = true;
    }

    void FixedUpdate()
    {
        if ( GameManager.gameManager.gameState == GameManager.STATE_PLAY.inMenu || GameManager.gameManager.isInMenu)
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
        {
            if(lastBrickStuckOn != null && isAnExtendShot)
                lastBrickStuckOn.layer = 11; //ImmaterialForPlayer layer
            rb.AddForce(inputDir * magnitude * shotForce, ForceMode2D.Impulse);
            Debug.Log(inputDir * magnitude * shotForce);
        }
        jump = false;
    }

    void ClampSpeed()
    {
        if (playerState == PlayerState.moving)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedMax);
        }
    }

    void ResetLastBrickLayer()
    {
        if (lastBrickStuckOn == null || lastBrickStuckOn.layer != 11)
            return;
        if (Vector2.Distance(transform.position,lastBrickStuckOn.GetComponent<BoxCollider2D>().ClosestPoint(transform.position)) > colliderRadius)
        {
            lastBrickStuckOn.layer = 0;
            lastBrickStuckOn = null;
        }
    }

    #endregion

    void Update()
    {
        ResetValues();
        if ( GameManager.gameManager.gameState == GameManager.STATE_PLAY.inMenu || GameManager.gameManager.isInMenu)
        {
            if (playerState == PlayerState.charging)
            {
                UpdatePlayerState(PlayerState.idle);
            }
        }
        else
        {
            CheckSliding();
            ReadingInput();
            RotateWithTrajectory();
            ResetLastBrickLayer();
        }
        if (needRotate)
        {
            RotateOnColliderSide();
            needRotate = false;
        }
    }

    #region Update Calls

    void ResetValues()
    {
        graphes.transform.localScale = new Vector3(1, 1, 1);
    }

    void IsValuableShot()
    {
        isValuableShot = false;
        isAnExtendShot = false;
        float diff = Vector2.Distance(inputDir, dirArray[colliderSide]);

        if (playerState == PlayerState.charging)
        {
            if (diff < 1.39f)
                isValuableShot = true;
            else if (diff < (1.39f + extendAngle) && Vector2.Dot(inputDir, dirArray[extendDir]) > 0)
            {
                isValuableShot = true;
                isAnExtendShot = true;
            }
        }
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
                graphes.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                graphes.transform.localScale = new Vector3(1,-1,1);
            }
        }
    }

        #region Controls

    private void ReadingInput()
    {
        if (throwAllowed && (GameManager.gameManager.gameState != GameManager.STATE_PLAY.levelResult) && CheckTouch())
        {
            if (isPcControl)
                PcControls();
            else if (!isPcControl)
                MobileControls();
        }
    }

    public void LaunchPlayerDebug()
    {
        UpdatePlayerState(PlayerState.moving);
        inputDir = new Vector2(-8.2f,5f);
        jump = true;
        GameManager.gameManager.Shoot();
        firstShot = false;
    }

    private void PcControls()
    {
        currentPosition = Input.mousePosition;
        inputDir = (startPosition - currentPosition).normalized;
        GetCurrentMagnitude();

        if (Input.GetMouseButtonDown(0))
        {
            startPosition = currentPosition;
            UpdatePlayerState(PlayerState.charging);
            VFXManager.instance.PlayOnScreenPositon("Circle_OnScreen", currentPosition);
            GameManager.gameManager.DeactivateTuto();
        }
        if (Input.GetKeyDown(KeyCode.Space))
            LaunchPlayerDebug();

        if (Input.GetMouseButton(0) && !GameManager.gameManager.isInMenu)
        {
            IsValuableShot();
            
            Trajectory();
        }

        else if (Input.GetMouseButtonUp(0) && playerState == PlayerState.charging)
        {
            Debug.Log(magnitude);
            Debug.Log("isVS " + isValuableShot);
            VFXManager.instance.Stop("Circle_OnScreen");
            animator.SetBool("Charging", false);
            if (magnitude > 0 && isValuableShot && !LevelManager.levelManager.level.needCancelSlingshot)
            {
                UpdatePlayerState(PlayerState.moving);
                direction = inputDir;
                jump = true;
                GameManager.gameManager.Shoot();
                firstShot = false;
            }
            else if (!isValuableShot && LevelManager.levelManager.level.needCancelSlingshot)
            {
                LevelManager.levelManager.level.needCancelSlingshot = false;
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

    private bool CheckTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector2 position = Input.touches[0].position;
            if (!GameManager.gameManager.isInPause)
            {
                if (Vector2.Distance(currentPosition, UIManager.uiManager.pauseButton.GetComponent<RectTransform>().position) < UIManager.uiManager.pauseButton.GetComponent<RectTransform>().sizeDelta.magnitude && Vector2.Distance(currentPosition, UIManager.uiManager.retryButton.GetComponent<RectTransform>().position) < UIManager.uiManager.retryButton.GetComponent<RectTransform>().sizeDelta.magnitude)
                {
                    return false;
                }
            }
            else
            {
                if (Vector2.Distance(currentPosition, UIManager.uiManager.resumeButton.GetComponent<RectTransform>().position) < UIManager.uiManager.resumeButton.GetComponent<RectTransform>().sizeDelta.magnitude)
                {
                    return false;
                }
            }
        }            
        return true;
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
            AudioManager.instance.Play("SFX_Player_Charging");
        }

        else if (playerState == PlayerState.moving)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            dotStorage.SetActive(false);
            //animator.SetBool("Charging", false);
            animator.SetBool("Fly", true);
            AudioManager.instance.Stop("SFX_Player_Charging");
            AudioManager.instance.RandomPlay("SFX_Slingshot_Launcher_", 1, 4);
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
        if (colliderTag != "StaticWall" && colliderTag != "StickyWall" && colliderTag != "DangerousWall")
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

    void MovePlayerBesideBrick()
    {
        Vector2 offset = Vector2.zero;

        if (colliderSide == 0)
            offset.y = colliderRadius;
        else if (colliderSide == 2)
            offset.y = -colliderRadius;
        else if (colliderSide == 1)
            offset.x = colliderRadius;
        else if (colliderSide == 3)
            offset.x = -colliderRadius;

        transform.position = lastCollidePosition + offset;
        if(DebugPosCollide != null)
            DebugPosCollide.transform.position = lastCollidePosition;
    }

    void SetExtendDir()
    {
        if (colliderSide == 0 || colliderSide == 2)
            extendDir = lastCollidePosition.x > lastBrickStuckOn.transform.position.x ? 1 : 3;

        else
            extendDir = lastCollidePosition.y > lastBrickStuckOn.transform.position.y ? 0 : 2;
    }

    void SetExtendAngle(Collider2D brickCollider)
    {
        float distanceToEdge;
        if(colliderSide == 0 || colliderSide == 2)
        {
            if (extendDir == 1)
                distanceToEdge = lastBrickStuckOn.transform.position.x + brickCollider.bounds.extents.x - lastCollidePosition.x;
            else
                distanceToEdge = lastBrickStuckOn.transform.position.x - brickCollider.bounds.extents.x - lastCollidePosition.x;
        }
        else
        {
            if (extendDir == 0)
                distanceToEdge = lastBrickStuckOn.transform.position.y + brickCollider.bounds.extents.y - lastCollidePosition.y;
            else
                distanceToEdge = lastBrickStuckOn.transform.position.y - brickCollider.bounds.extents.y - lastCollidePosition.y;
        }

        if (distanceToEdge < 0)
            distanceToEdge *= -1;
        Debug.Log("distanceToEdge " + distanceToEdge);
        float interpolationDistance = Mathf.Clamp(distanceToEdge / 0.35f, 0,1f);
        extendAngle = maxExtendAngle - (maxExtendAngle * interpolationDistance);
    }





    void OnCollisionEnter2D(Collision2D other)
    {
        string otherTag = other.gameObject.tag;
        //Debug.Log("collide with : " + otherTag + " / state " + playerState + " / velo.norm " + rb.velocity.normalized + " frame " + Time.frameCount);
        lastCollidePosition = other.contacts[0].point;
        GetColliderSide(otherTag);
        AudioManager.instance.RandomPlay("player_", 1, 13);
        VFXManager.instance.PlayOnPositon("Blob_Contact", transform.position);
        if (otherTag == "StickyWall" && (playerState == PlayerState.moving && ItShouldStick()) || firstShot)
        {
            lastBrickStuckOn = other.gameObject;
            UpdatePlayerState(PlayerState.idle);
            MovePlayerBesideBrick();
            SetExtendDir();
            SetExtendAngle(other.collider);
            VFXManager.instance.PlayOnPositon("Blob_Sticky", transform.position);
            AudioManager.instance.Play("SFX_Sticky_Collision");
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
                extendAngle = 0;
            }
            animator.Play("Bounce");
            AudioManager.instance.Play("SFX_Unbreakable_Collision");
        }
        else if (otherTag == "PushableWall")
        {
            if (rb.velocity.magnitude > 3)
            {
                rb.AddForce(rb.velocity.normalized * (pushableWBounciness * Bounciness));
            }                        
            else if (rb.velocity.magnitude < 1)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                StopCoroutine(StartCheckSliding());
            }                        
            animator.Play("Bounce");
        }
        else if (otherTag == "DangerousWall")
        {
            UpdatePlayerState(PlayerState.idle);
            animator.SetBool("isDying", true);
            GameManager.gameManager.EndLevel(false);
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
            AudioManager.instance.RandomPlay("enemy_", 1, 5);
            if (collision.GetComponent<Ennemy>().IsDying())
            {
                return;
            }
            collision.GetComponent<Ennemy>().Die();
            collision.GetComponent<Ennemy>().DissolveEnemy();
            //Material mat = collision.gameObject.GetComponent<SpriteRenderer>().material;
            //StartCoroutine(VFXManager.instance.DestroyingDissolve(collision.gameObject, mat, 0.7f));
            animator.Play("Eat");
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
    
    private void Trajectory()
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

    #region Debug

    void OnGUI()
    {
        //GUILayout.Label(SceneManager.GetActiveScene().name, style);
        //GUILayout.Label(Time.fixedDeltaTime.ToString() + "  ", style);
        //GUILayout.Label(((int)(1.0f / Time.smoothDeltaTime)).ToString(), style);
    }
    #endregion
}

public enum PlayerState
{
    idle,
    charging,
    moving,
}