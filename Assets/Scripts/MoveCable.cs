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
    private bool conectado = false;
    
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
        if(!conectado)
        {
            Vector2 currentMousePosition = eventData.position;
            ChangeRotation(currentMousePosition);
            ChangeSize(currentMousePosition);
            ConnectionCheck();
        }
        
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!conectado)
        {
            bool IsClickPressed = Controls.Player.Click.ReadValue<float>() > 0.1f;
            if(IsClickPressed == false)
            {
                Reset();
            }
            Debug.Log("End Drag");
        }
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

        // BoxCollider2D boxCollider = this.GetComponent<BoxCollider2D>();
        // boxCollider.offset = this.transform.position - connector.transform.position;
        // Vector2 currOffSet = boxCollider.offset;
        // currOffSet.y = -currOffSet.y;
        // boxCollider.offset = currOffSet;
    }

    private void Reset()
    {
        connector.transform.position = oldPosCon;
        transform.position = oldPos;
        connector.transform.rotation = Quaternion.Euler(0, 0, 180);
        finalCable.transform.localRotation = Quaternion.Euler(0, 0, 180);
        finalCable.sizeDelta = originalSize;
        Debug.Log("Reset");
    }

    private void ConnectionCheck()
    {
        Collider2D colliders = Physics2D.OverlapBox(connector.transform.position, new Vector2(1f,1f), 0);
        
        //Don't check the collider of the cable we are using
        if(colliders != null)
        {
            

            if (colliders.gameObject != gameObject)
            {

               
                Debug.Log("Check3");
                
                
                CablesGame connectorCable = colliders.gameObject.GetComponent<CablesGame>();
                
                conectado = true;
                Debug.Log("Check4");
                Connect();
                connectorCable.Connect();
                // if (finalCableCol.color == connectorCable.finalCableCol.color)
                // {
                    
                // }

            }
        
        }
        
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.name == "Move")
    //     {
    //         this.transform.position = other.transform.position;
    //         Debug.Log("Conectado");
    //     }
        
    // }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if(other.transform.name == "Move")
    //     {
    //         this.transform.position = other.transform.position;
            
    //     }
    //     Debug.Log("Conectado");
    // }


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
