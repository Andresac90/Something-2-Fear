using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveCable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private InputMaster Controls;
    private Vector3 oldPosition;
    private Vector3 oldPositionConnector;
    private Vector2 originalSize;
    private Transform pos;
    private RectTransform finalCable;
    private GameObject connector;
    private bool falseConnect;
    private bool connect = false;
    private GameObject puzzle;
    private Puzzle comprobations;
    private int value = 0;

    [SerializeField]
    private string identifier;

    public void Awake()
    {
        Controls = new InputMaster();
    }

    public void Start()
    {
        puzzle = GameObject.Find("Cables");
        comprobations = puzzle.GetComponent<Puzzle>();
        pos = GetComponent<Transform>();
        oldPosition = transform.position;
        finalCable = GetComponentInChildren<RectTransform>();
        originalSize = finalCable.sizeDelta;
        connector = this.transform.GetChild(1).gameObject;
        oldPositionConnector = connector.transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        falseConnect = true;
        connect = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!falseConnect)
        {
            return;
        }
        else if(falseConnect)
        {
            Vector2 currentMousePosition = eventData.position;
            ChangeRotation(currentMousePosition);
            ChangeSize(currentMousePosition);
            ConnectionCheck();
        }
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        if(falseConnect)
        {
            bool IsClickPressed = Controls.Player.Click.ReadValue<float>() > 0.1f;
            if(!IsClickPressed)
            {
                Reset();
            }
        }
    }

    private void ChangeRotation(Vector2 currentMousePosition)
    {
        Vector3 direction = (Vector2)pos.position - currentMousePosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        finalCable.rotation = Quaternion.Euler(0, 0, angle);
        connector.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ChangeSize(Vector2 currentMousePosition)
    {
        // float estirar;
        // if(finalCable.rect.width <= 800)
        // {
        //     estirar = 1f;
        // }
        // else
        // {
        //     estirar = 4f;
        // }
        float distance = Vector2.Distance(new Vector2(0f, 0.5f) * finalCable.rect.width, ((Vector2)pos.position - currentMousePosition)*2f);

        finalCable.sizeDelta = new Vector2(distance, finalCable.sizeDelta.y);
        connector.transform.position = finalCable.TransformPoint(new Vector3(finalCable.rect.width, 0f, 0f));
    }

    private void Reset()
    {
        connector.transform.position = oldPositionConnector;
        transform.position = oldPosition;
        connector.transform.rotation = Quaternion.Euler(0, 0, 180);
        finalCable.transform.localRotation = Quaternion.Euler(0, 0, 180);
        finalCable.sizeDelta = originalSize;
    }

    private void ConnectionCheck()
    {
        Collider2D colliders = Physics2D.OverlapBox(connector.transform.position, new Vector2(4f,4f), 0);
        
        if(colliders != null)
        {
            if (colliders.gameObject != gameObject)
            {
                MoveFigure connectorCable = colliders.gameObject.GetComponent<MoveFigure>();

                falseConnect = false;
                
                if (identifier == colliders.gameObject.name)
                {
                    connect = true;
                }
            }
        }
        Connect();
    }

    private void Connect()
    {
        if(connect)
        {
            value = 1;
            comprobations.comprobations++;
            comprobations.Completed();
        }
        else if(!connect && value == 1)
        {
            value = 0;
            comprobations.comprobations--;
        }
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