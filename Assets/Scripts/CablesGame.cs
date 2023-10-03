using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CablesGame : MonoBehaviour, IEventSystemHandler
{
    private InputMaster Controls;
    private Vector2 mousePosition;
    private Vector2 originPosition;
    private Vector2 actualPosition;
    private Vector2 direction;
    private Vector2 originalPosition;

    private Vector2 originalSize;

    public RectTransform finalCable;
    public Image finalCableCol;
    public GameObject lightCable;

    void Awake()
    {
        Controls = new InputMaster();
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        // originalSizeY = (finalCable.anchorMax.x - finalCable.anchorMin.x)*Screen.width;
        // originalSizeX = (finalCable.anchorMax.y - finalCable.anchorMin.y)*Screen.height;
        originalSize = finalCable.sizeDelta;
        // finalCable.sizeDelta = new Vector2 (10000, finalCable.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
        bool IsClickPressed = Controls.Player.Click.ReadValue<float>() > 0.1f;
        if(IsClickPressed)
        {
            //Reset();
        }
    }

    private void OnDrag(PointerEventData eventData)
    {
        ChangePosition();
        ChangeRotation();
        ChangeSize();
        Debug.Log("And");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Black");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        ConnectionCheck();
        Debug.Log("Yellow");
    }

    private void ChangePosition()
    {
        bool IsClickPressed = Controls.Player.Click.ReadValue<float>() > 0.1f;
        if(IsClickPressed)
        {
            mousePosition = Controls.Player.Look.ReadValue<Vector2>();
            transform.position = new Vector3(mousePosition.x, mousePosition.y);
            Debug.Log("Move");
        }
    }

    private void ChangeRotation()
    {
        actualPosition = transform.position;
        originPosition = transform.parent.position;

        direction = actualPosition - originPosition;
        float angle = Vector2.SignedAngle(Vector2.right * transform.lossyScale, direction);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Debug.Log("Rotate");
    }

    private void ChangeSize()
    {
        actualPosition = transform.position;
        originPosition = transform.parent.position;

        float distance = Vector2.Distance(actualPosition, originPosition);
        finalCable.sizeDelta = new Vector2 (distance, finalCable.sizeDelta.y);
        Debug.Log("Size");
        //finalCable.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        //finalCable.RectTransform.SetSizeWithCurrentAnchors = new Vector2(distance, finalCable.RectTransform.sizeDelta.y);
    }

    private void Reset()
    {
        transform.position = originalPosition;
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