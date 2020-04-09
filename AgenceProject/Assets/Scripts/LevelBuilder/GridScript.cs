using UnityEngine;

public class GridScript : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;
    public int Y_axi = 25;
    public int X_axi = 25;

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float y = -Y_axi; y < Y_axi; y += size)
        {
            for (float x = -X_axi; x <= X_axi; x += size)
            {
                var point = GetNearestPointOnGrid(new Vector2(x, y));
                Gizmos.DrawSphere(point, 0.1f);
            }

        }
    }
    
}