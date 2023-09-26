using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CablesGame : MonoBehaviour
{
    private InputMaster Controls;
    private Vector2 mousePosition;
    private Vector2 originPosition;
    private Vector2 actualPosition;
    private Vector2 direction;
    private Vector2 originalPosition;
    private Vector2 originalSize;

    public SpriteRenderer finalCable;
    public GameObject light;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        originalSize = finalCable.size;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Reset();
        }
    }

    private void OnMouseDrag()
    {
        ChangePosition();
        ChangeRotation();
        ChangeSize();
        ConnectionCheck();
    }

    private void ChangePosition()
    {
        mousePosition = Controls.Player.Look.ReadValue<Vector2>();
        transform.position = new Vector3(mousePosition.x, mousePosition.y);
    }

    private void ChangeRotation()
    {
        actualPosition = transform.position;
        originPosition = transform.parent.position;

        direction = actualPosition - originPosition;
        float angle = Vector2.SignedAngle(Vector2.right * transform.lossyScale, direction);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ChangeSize()
    {
        actualPosition = transform.position;
        originPosition = transform.parent.position;

        float distance = Vector2.Distance(actualPosition, originPosition);
        finalCable.size = new Vector2(distance, finalCable.size.y);
    }

    private void Reset()
    {
        transform.position = originalPosition;
        transform.rotation = Quaternion.identity;
        finalCable.size = new Vector2(originalSize.x, originalSize.y);
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

                if (finalCable.color == connectorCable.finalCable.color)
                {
                    Connect();
                    connectorCable.Connect();
                }
            }
        }
    }

    public void Connect()
    {
        light.SetActive(true);
        //To not move the cable again
        Destroy(this);
    }
}