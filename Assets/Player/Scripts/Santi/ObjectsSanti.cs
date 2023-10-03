using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectsSanti : MonoBehaviour
{
    private InputMaster Controls;

    [SerializeField]
    private Transform ObjectRightCamera;
    [SerializeField]
    private Transform ObjectLeftCamera;
    [SerializeField]
    private Transform PlayerCamera;
    [SerializeField]
    private float RayLine;
    [SerializeField]
    private float ThrowForce;

    private RaycastHit Hit;
    private float ObjectScaleData;
    private float ObjectOriginalScale;
    private Rigidbody ObjectRightRb;
    private Rigidbody ObjectLeftRb;
    private Transform ObjectRightT;
    private Transform ObjectLeftT;
    private bool GrabbedObjR = false;
    private bool GrabbedObjL = false;

    public void Awake()
    {
        Controls = new InputMaster();
    }
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        Grab();
        Drop();
    }

    public void Grab()
    {
        Physics.Raycast(PlayerCamera.position, PlayerCamera.TransformDirection(Vector3.forward), out Hit, RayLine);
        if(Hit.transform != null && Hit.transform.tag == "Object")
        {
            bool IsRightPressed = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
            bool IsLeftPressed = Controls.Player.LeftItem.ReadValue<float>() > 0.1f;
            if(IsRightPressed && GrabbedObjR == false)
            {
                StartCoroutine(RightGrab());
            }
            else if(IsLeftPressed && GrabbedObjL == false)
            {
                StartCoroutine(LeftGrab());
            }
        }
    }

    public void Drop()
    {
        bool IsRightPressed = Controls.Player.RightThrow.ReadValue<float>() > 0.1f;
        bool IsLeftPressed = Controls.Player.LeftThrow.ReadValue<float>() > 0.1f;
        if(IsRightPressed && GrabbedObjR == true)
        {
            StartCoroutine(RightDrop());
        }
        else if(IsLeftPressed && GrabbedObjL == true)
        {
            StartCoroutine(LeftDrop());
        }
    }

    public IEnumerator LeftGrab()
    {
        Hit.rigidbody.isKinematic = true;
        Hit.transform.parent = ObjectLeftCamera;
        ObjectScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
        Hit.transform.localScale = new Vector3(ObjectScaleData, ObjectScaleData, ObjectScaleData);
        Hit.transform.localPosition = new Vector3(0, 0, 0);
        Hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        ObjectOriginalScale = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        ObjectLeftRb = Hit.rigidbody;
        ObjectLeftT = Hit.transform;
        yield return new WaitForSeconds(0.5f);
        GrabbedObjL = true;
    }

    public IEnumerator RightGrab()
    {
        Hit.rigidbody.isKinematic = true;
        Hit.transform.parent = ObjectRightCamera;
        ObjectScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
        Hit.transform.localScale = new Vector3(ObjectScaleData, ObjectScaleData, ObjectScaleData);
        Hit.transform.localPosition = new Vector3(0, 0, 0);
        Hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        ObjectOriginalScale = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        ObjectRightRb = Hit.rigidbody;
        ObjectRightT = Hit.transform;
        yield return new WaitForSeconds(0.5f);
        GrabbedObjR = true;
    }

    public IEnumerator LeftDrop()
    {
        ObjectLeftT.transform.localScale = new Vector3(ObjectOriginalScale, ObjectOriginalScale, ObjectOriginalScale);
        Vector3 camerDirection = PlayerCamera.transform.forward;
        ObjectLeftT.transform.parent = null;
        ObjectLeftRb.isKinematic = false;
        ObjectLeftRb.AddForce(camerDirection * ThrowForce);
        yield return new WaitForSeconds(0.5f);
        GrabbedObjL = false;
    }

    public IEnumerator RightDrop()
    {
        ObjectRightT.transform.localScale = new Vector3(ObjectOriginalScale, ObjectOriginalScale, ObjectOriginalScale);
        Vector3 camerDirection = PlayerCamera.transform.forward;
        ObjectRightT.transform.parent = null;
        ObjectRightRb.isKinematic = false;
        ObjectRightRb.AddForce(camerDirection * ThrowForce);
        yield return new WaitForSeconds(0.5f);
        GrabbedObjR = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(PlayerCamera.position, PlayerCamera.TransformDirection(Vector3.forward) * RayLine);
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