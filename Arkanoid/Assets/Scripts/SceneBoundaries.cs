using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBoundaries : MonoBehaviour
{
    [SerializeField] private GameObject hBoundary = null;

    [SerializeField] private Camera cam = null;

    public static SceneBoundaries self = null;
    private SceneBoundaries()
    {
    }

    public Vector3 bottomLeft { get; set; }
    public Vector3 bottomRight { get; set; }
    public Vector3 topLeft { get; set; }
    public Vector3 topRight { get; set; }


    void Awake()
    {
        if (self == null)
        {
            self = this; 
        }
        else if (self == this)
        { 
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        if (cam==null)
            cam = FindObjectOfType<Camera>();
        UpdateBoundaries();
    }

    public bool IsInsideVerticalBoundaries(Vector3 pos, float offset)
    {
        if (pos.y >= bottomLeft.y + offset && pos.y <= topLeft.y - offset) 
            return true; 
        else
            return false;
    }
    public bool IsInsideVerticalBoundaries(GameObject go, float offset)
    {
        return IsInsideVerticalBoundaries(go.transform.position, offset);
    }

    public bool IsInsideHorizontalBoundaries(Vector3 pos, float offset)
    {
        if (pos.x >= topLeft.x + offset && pos.x <= topRight.x - offset)
            return true;
        else
            return false;
    }
    public bool IsInsideHorizontalBoundaries(GameObject go, float offset)
    {
        return IsInsideHorizontalBoundaries(go.transform.position, offset);
    }

    public bool IsInsideAllBoundaries(GameObject go, float offset)
    {
        return IsInsideHorizontalBoundaries(go, offset) &&
               IsInsideVerticalBoundaries(go, offset);
    }

    public bool IsInsideAllBoundaries(Vector3 pos, float offset)
    {
        return IsInsideHorizontalBoundaries(pos, offset) &&
               IsInsideVerticalBoundaries(pos, offset);
    }

    public bool EscapedBottom(Vector3 pos)
    {
        return pos.y <= bottomLeft.y;
    }

    public Vector3 LimitHorizontal(Vector3 vec, float offset)
    {
        if (vec.x < topLeft.x + offset)
            vec.x = topLeft.x + offset;
        if (vec.x > bottomRight.x - offset)
            vec.x = bottomRight.x  - offset;

        return vec;
    }
    
    public Vector3 LimitVertical(Vector3 vec, float offset)
    {
        if (vec.y > topLeft.y - offset)
            vec.y = topLeft.y - offset;
        if (vec.y < bottomRight.y + offset)
            vec.y = bottomRight.y + offset;

        return vec;
    }

    public Vector3 LimitAll (Vector3 vec, float offset)
    {
        return LimitVertical(LimitHorizontal(vec, offset), offset);
    }

    public Vector3 Reflect(Vector3 pos, Vector3 direction, float offset)
    {
        if (!IsInsideHorizontalBoundaries(pos, offset))
        {
            return Vector3.Reflect(direction, new Vector3(1, 0, 0));
        } else if (!IsInsideVerticalBoundaries(pos, offset))
        {
            return Vector3.Reflect(direction, new Vector3(0, 1, 0));
        }
        return direction;
    }

    public void UpdateBoundaries()
    {
        bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        bottomRight = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));

        topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));

        DisplayBoundaries();
    }

    private void DisplayBoundaries()
    {
        Vector3 middle = new Vector3((topLeft.x + topRight.x) / 2.0f, (topLeft.y + bottomLeft.y) / 2.0f, 0.0f);

        Instantiate(hBoundary, new Vector3(middle.x, topLeft.y, cam.nearClipPlane), Quaternion.identity);

        Instantiate(hBoundary, new Vector3(bottomLeft.x, middle.y, cam.nearClipPlane), Quaternion.Euler(0, 0, 90));
        Instantiate(hBoundary, new Vector3(bottomRight.x, middle.y, cam.nearClipPlane), Quaternion.Euler(0, 0, 90));
    }
}
