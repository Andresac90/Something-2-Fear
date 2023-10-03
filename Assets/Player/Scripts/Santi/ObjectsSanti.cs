using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEditor;

public class ObjectsSanti : MonoBehaviour
{
    private InputMaster Controls;

    [SerializeField]
    private Transform ObjectRightCamera;
    [SerializeField]
    private Transform ObjectLeftCamera;
    [SerializeField]
    private Transform ObjectRightHand;
    [SerializeField]
    private Transform ObjectLeftHand;
    [SerializeField]
    private Transform PlayerCamera;
    [SerializeField]
    private float RayLine;
    [SerializeField]
    private float ThrowForce;

    private GameObject playerL;
    private GameObject playerR;
    private GameObject cloneL;
    private GameObject cloneR;
    private RaycastHit Hit;
    private float ObjectScaleData;
    private float ObjectOriginalScale;
    private Rigidbody ObjectRightRb;
    private Rigidbody ObjectLeftRb;
    private Transform ObjectRightT;
    private Transform ObjectLeftT;
    private bool GrabbedObjR = false;
    private bool GrabbedObjL = false;
    private bool ThrowCheck = false;

    public void Awake()
    {
        Controls = new InputMaster();
        playerL = GameObject.Find("LeftObject");
        playerR = GameObject.Find("RightObject");
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
        Hit.transform.position = ObjectLeftCamera.position;
        Hit.rigidbody.isKinematic = true;
        Hit.transform.parent = ObjectLeftCamera;
        ObjectScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
        Hit.transform.localScale = new Vector3(ObjectScaleData, ObjectScaleData, ObjectScaleData);
        Hit.transform.localPosition = new Vector3(0, 0, 0);
        Hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        ObjectOriginalScale = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        ObjectLeftRb = Hit.rigidbody;
        ObjectLeftT = Hit.transform;
        LeftGrabTwo();
        yield return new WaitForSeconds(0.5f);
        GrabbedObjL = true;
        ThrowCheck = true;
    }

    public void LeftGrabTwo()
    {
        int LayerIgnoreRaycast = LayerMask.NameToLayer("PlayerSanti");
        playerL.GetComponentInChildren<Transform>();
        cloneL = (GameObject)Instantiate(playerL, ObjectLeftHand.position, Quaternion.identity);
        cloneL.layer = LayerIgnoreRaycast;
        cloneL.transform.parent = ObjectLeftHand;
    }

    public IEnumerator RightGrab()
    {
        Hit.transform.position = ObjectRightCamera.position;
        Hit.rigidbody.isKinematic = true;
        Hit.transform.parent = ObjectRightCamera;
        ObjectScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
        Hit.transform.localScale = new Vector3(ObjectScaleData, ObjectScaleData, ObjectScaleData);
        Hit.transform.localPosition = new Vector3(0, 0, 0);
        Hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        ObjectOriginalScale = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        ObjectRightRb = Hit.rigidbody;
        ObjectRightT = Hit.transform;
        RightGrabTwo();
        yield return new WaitForSeconds(0.5f);
        GrabbedObjR = true;
        ThrowCheck = true;
    }

    public void RightGrabTwo()
    {
        playerR.GetComponentInChildren<Transform>();
        cloneR = (GameObject) Instantiate(playerR, ObjectRightHand.position, Quaternion.identity);
        cloneR.transform.parent = ObjectRightHand;
    }

    public IEnumerator LeftDrop()
    {
        ObjectLeftT.transform.localScale = new Vector3(ObjectOriginalScale, ObjectOriginalScale, ObjectOriginalScale);
        Vector3 camerDirection = PlayerCamera.transform.forward;
        ObjectLeftT.transform.parent = null;
        ObjectLeftRb.isKinematic = false;
        ObjectLeftRb.AddForce(camerDirection * ThrowForce);
        ThrowCheck = false;
        Destroy(cloneL);
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
        ThrowCheck = false;
        Destroy(cloneR);
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