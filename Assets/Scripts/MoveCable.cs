using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveCable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private InputMaster Controls;
    private Vector2 lastMousePosition;
    private Vector3 oldPos;
    private Vector3 oldPosCon;
    private Vector2 originalSize;
    private Transform pos;
    private Image finalCableCol;
    private RectTransform finalCable;
    private Vector2 fixedAnchor;
    private GameObject connector;
    
    public GameObject lightCable;

    public void Awake()
    {
        Controls = new InputMaster();
    }

    public void Start()
    {
        pos = GetComponent<Transform>();
        oldPos = transform.position;
        finalCable = GetComponentInChildren<RectTransform>();
        originalSize = finalCable.sizeDelta;
        finalCableCol = GetComponentInChildren<Image>();
        fixedAnchor = new Vector2(0f, 0.5f);
        connector = this.transform.GetChild(1).gameObject;
        oldPosCon = connector.transform.position;
    }

    void Update()
    {
    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        ChangeRotation(currentMousePosition);
        ChangeSize(currentMousePosition);
        ConnectionCheck();
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        
        bool IsClickPressed = Controls.Player.Click.ReadValue<float>() > 0.1f;
        if(IsClickPressed == false)
        {
            Reset();
        }
        Debug.Log("End Drag");
    }

    private void ChangeRotation(Vector2 currentMousePosition)
    {

        Vector3 direction = (Vector2)pos.position - currentMousePosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        finalCable.rotation = Quaternion.Euler(0, 0, angle);
        connector.transform.rotation = Quaternion.Euler(0, 0, angle);


        lastMousePosition = currentMousePosition;
    }

    private void ChangeSize(Vector2 currentMousePosition)
    {
        float estirar;
        if(finalCable.rect.width <= 800)
        {
            estirar = 10f;
        }
        else
        {
            estirar = 7f;
        }
        float distance = Vector2.Distance(fixedAnchor * finalCable.rect.width, ((Vector2)pos.position - currentMousePosition)*estirar);

        finalCable.sizeDelta = new Vector2(distance, finalCable.sizeDelta.y);
        connector.transform.position = finalCable.TransformPoint(new Vector3(finalCable.rect.width, 0f, 0f));
        BoxCollider2D boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.offset = this.transform.position - connector.transform.position;
        Vector2 currOffSet = boxCollider.offset;
        currOffSet.y = -currOffSet.y;
        boxCollider.offset = currOffSet;
    }

    private void Reset()
    {
        connector.transform.position = oldPosCon;
        transform.position = oldPos;
        connector.transform.rotation = Quaternion.Euler(0, 0, 180);
        transform.rotation = Quaternion.identity;
        finalCable.sizeDelta = originalSize;
        Debug.Log("Reset");
    }

    private void ConnectionCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 1f), 1);

        foreach (Collider2D col in colliders)
        {
            Debug.Log("Check2");
            Debug.Log(col.gameObject.name);
            Debug.Log(gameObject.name);
            //Don't check the collider of the cable we are using
            if (col.gameObject != gameObject)
            {
                Debug.Log("Check3");
                transform.position = col.transform.position;
                
                CablesGame connectorCable = col.gameObject.GetComponent<CablesGame>();

                if (finalCableCol.color == connectorCable.finalCableCol.color)
                {
                    Debug.Log("Check4");
                    Connect();
                    connectorCable.Connect();
                }
            }
        }
        
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
