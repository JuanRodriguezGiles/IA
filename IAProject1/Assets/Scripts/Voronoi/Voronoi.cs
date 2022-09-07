using System.Collections.Generic;

using UnityEngine;

public class Voronoi : MonoBehaviour
{
    public List<Vector2> points;
    
    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                Gizmos.color = Color.black;
                Vector3 start = points[i];
                Vector3 end = points[j];
                Gizmos.DrawLine(start, end);

                Gizmos.color = Color.green;
                Vector3 middle = (start + end) * 0.5f;
                Vector3 perp = Vector2.Perpendicular(new Vector2(middle.x, middle.y));
                Gizmos.DrawLine(middle, perp);
            }
        }
    }
}