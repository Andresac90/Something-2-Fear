using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveCable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private InputMaster Controls;
    private Vector2 lastMousePosition;
    private Vector2 mousePosition;
    private Vector2 oldPos;
    private Vector2 originalSize;
    private Transform pos;
    private Image finalCableCol;
    private RectTransform finalCable;
    
    public GameObject lightCable;
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
        finalCable = GetComponentInChildren<RectTransform>();
        originalSize = finalCable.sizeDelta;
        finalCableCol = GetComponentInChildren<Image>();
        // _canvas = GameObject.Find("CablesBox(Clone)");
        // child = _canvas.transform.GetChild(0).gameObject;
    }

    void Update()
    {
    
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
        if(oldPos.x >= currentMousePosition.x || 55 >= currentMousePosition.y || 270 <= currentMousePosition.y) return;
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
        ConnectionCheck();
        bool IsClickPressed = Controls.Player.Click.ReadValue<float>() > 0.1f;
        if(IsClickPressed == false)
        {
            Reset();
        }
        Debug.Log("End Drag");
        //Implement your funtionlity here
    }

    private void ChangeRotation(Vector2 currentMousePosition)
    {
        currentMousePosition = transform.position;
        Vector2 originalPos = transform.parent.position;

        Vector2 direction = currentMousePosition - originalPos;
        float angle = Vector2.SignedAngle(Vector2.left * transform.lossyScale, direction);
        transform.rotation = Quaternion.Euler(0, 0, angle);
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

    private void ConnectionCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

        foreach (Collider2D col in colliders)
        {
            //Don't check the collider of the cable we are using
            if (col.gameObject != gameObject)
            {
                transform.position = col.transform.position;
                
                CablesGame connectorCable = col.gameObject.GetComponent<CablesGame>();

                if (finalCableCol.color == connectorCable.finalCableCol.color)
                {
                    Connect();
                    connectorCable.Connect();
                }
            }
        }
        Debug.Log("Check");
    }

    public void Connect()
    {
        lightCable.SetActive(true);
        //To not move the cable again
        Destroy(this);
        Debug.Log("Connect");
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
