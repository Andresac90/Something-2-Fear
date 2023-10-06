using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private InputMaster Controls;
    private Vector2 lastMousePosition;
    private Vector2 mousePosition;
    private Vector2 oldPos;
    private Vector2 originalSize;
    private Transform pos;

    public RectTransform finalCable;
    // private GameObject _canvas;
    // private GameObject child;
 
    /// <summary>
    /// This method will be called on the start of the mouse drag
    /// </summary>
    /// <param name="eventData">mouse pointer event data</param>
    public void Awake()
    {
        pos = GetComponent<Transform>();
        Controls = new InputMaster();
        oldPos = pos.position;
        originalSize = finalCable.sizeDelta;
        // _canvas = GameObject.Find("CablesBox(Clone)");
        // child = _canvas.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        bool IsClickPressed = Controls.Player.Click.ReadValue<float>() > 0.1f;
        if(IsClickPressed == false)
        {
            Reset();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        lastMousePosition = eventData.position;
    }
 
    /// <summary>
    /// This method will be called during the mouse drag
    /// </summary>
    /// <param name="eventData">mouse pointer event data</param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        // RectTransform rect = child.GetComponent<RectTransform>();
 
        Vector3 newPosition = pos.position +  new Vector3(diff.x, diff.y, transform.position.z);
        pos.position = newPosition;
        lastMousePosition = currentMousePosition;
        ChangeRotation(currentMousePosition);
        ChangeSize(currentMousePosition);
    }
 
    /// <summary>
    /// This method will be called at the end of mouse drag
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        //Implement your funtionlity here
    }

    private void ChangeRotation(Vector2 currentMousePosition)
    {
        currentMousePosition = transform.position;
        Vector2 originalPos = transform.parent.position;

        Vector2 direction = currentMousePosition - originalPos;
        float angle = Vector2.SignedAngle(Vector2.right * transform.lossyScale, direction);
        transform.rotation = Quaternion.Euler(angle, 0, 0);
        Debug.Log("Rotate");
    }

    private void ChangeSize(Vector2 currentMousePosition)
    {
        currentMousePosition = transform.position;
        Vector2 originalPos = transform.parent.position;

        float distance = Vector2.Distance(currentMousePosition, originalPos);
        finalCable.sizeDelta = new Vector2 (distance, finalCable.sizeDelta.y);
        Debug.Log("Size");
        //finalCable.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        //finalCable.RectTransform.SetSizeWithCurrentAnchors = new Vector2(distance, finalCable.RectTransform.sizeDelta.y);
    }

    private void Reset()
    {
        transform.position = oldPos;
        transform.rotation = Quaternion.identity;
        finalCable.sizeDelta = originalSize;
        Debug.Log("Reset");
        //finalCable.SetNativeSize = new Vector2(originalSizeX, originalSizeY);
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
