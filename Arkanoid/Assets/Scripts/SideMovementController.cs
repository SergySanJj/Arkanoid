using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMovementController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float horizontalOffset = 0.0f;

    private bool _touchStarted = false;
    private Vector3 _startTouchPosition;

    void Start()
    {
        if (cam == null)
            cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
#if UNITY_ANDROID
        MobileTouchHandler();
#else
        MouseTouchHandler();
#endif
    }

    private void MobileTouchHandler()
    {
        RaycastHit hit;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (_touchStarted)
            {
                Vector3 touchPos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                touchPos.z = 0;
                touchPos.y = gameObject.transform.position.y;
                transform.position = SceneBoundaries.self.LimitHorizontal(touchPos, horizontalOffset);

            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        _touchStarted = true;
                        _startTouchPosition = hit.point;
                        _startTouchPosition.z = 0;
                    }
                }
            }
        } else {
            _touchStarted = false;
        }
    }


    private void MouseTouchHandler()
    {
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            if (_touchStarted)
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                mousePos.y = gameObject.transform.position.y;
                transform.position = SceneBoundaries.self.LimitHorizontal(mousePos, horizontalOffset) ;

            } else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        _touchStarted = true;
                        _startTouchPosition = hit.point;
                        _startTouchPosition.z = 0;
                    }
                }
            }
        } else
        {
            _touchStarted = false;
        }
    }
}
