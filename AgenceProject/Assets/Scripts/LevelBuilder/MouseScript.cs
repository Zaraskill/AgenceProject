using UnityEngine;
using UnityEngine.EventSystems;

public class MouseScript : MonoBehaviour
{
    public enum LevelManipulation { Create, Rotate, Destroy }; // the possible level manipulation types
    public enum ItemList { Platform_1, Platform_2, Platform_3, Platform_4 }; // the list of items

    [HideInInspector] // we hide these to make them known to the rest of the project without them appearing in the Unity editor.
    public ItemList itemOption = ItemList.Platform_1; // setting the cylinder object as the default object
    [HideInInspector]
    public int itemOptionID = 0;
    [HideInInspector]
    public LevelManipulation manipulateOption = LevelManipulation.Create; // create is the default manipulation type.
    [HideInInspector]
    public SpriteRenderer mr;
    [HideInInspector]
    public GameObject rotObject;

    [Header("Material")]
    public Sprite goodPlace;
    public Sprite badPlace;

    [Header("Manager")]
    public ManagerScript ms;

    private Vector2 mousePos;
    private bool colliding;
    private Ray ray;
    private RaycastHit hit;

    private Grid grid;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    private void Start()
    {
        mr = GetComponent<SpriteRenderer>(); // get the mesh renderer component and store it in mr.
    }

    void Update()
    {
        // Have the object follow the mouse cursor by getting mouse coordinates and converting them to world point.
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector2(Mathf.Clamp(mousePos.x, -20, 20), Mathf.Clamp(mousePos.y, -12, 12));
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

        if (hit.collider != null) 
        {
            colliding = true;
            mr.sprite = badPlace;
        }
        else
        {
            colliding = false;
            mr.sprite = goodPlace;
        }
        
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (colliding == false && manipulateOption == LevelManipulation.Create)
                {
                    if (Input.GetMouseButton(0))
                    {
                        RaycastHit hitInfo;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            CreateObject(hitInfo.point);
                        }
                    }
                }
                else if (colliding == true && manipulateOption == LevelManipulation.Rotate)
                {
                    SetRotateObject();
                }
                else if (colliding == true && manipulateOption == LevelManipulation.Destroy)
                {
                    Destroy(hit.collider.gameObject);
                }

            }
        }
    }

    /// <summary>
    /// Object creation
    /// </summary>
    public void CreateObject(Vector2 clickPoint)
    {
        GameObject newObj;
        Vector2 InstPosition = grid.GetComponent<GridScript>().GetNearestPointOnGrid(clickPoint);

        newObj = Instantiate(ms.Objects[itemOptionID], InstPosition, Quaternion.identity);
        newObj.transform.parent = ms.level_Platforms.transform;
        newObj.layer = 9;
        newObj.name = "Platform_" + itemOptionID;

        //Add editor object component and feed it data.
        EditorObject eo = newObj.AddComponent<EditorObject>();
        eo.data.pos = newObj.transform.position;
        eo.data.rot = newObj.transform.rotation;
        eo.data.objectType = (EditorObject.ObjectType)itemOptionID;

    }

    /// <summary>
    /// Object rotation
    /// </summary>
    void SetRotateObject()
    {
        rotObject = hit.collider.gameObject;
        ms.rotSlider.value = rotObject.transform.rotation.z;
    }
}
