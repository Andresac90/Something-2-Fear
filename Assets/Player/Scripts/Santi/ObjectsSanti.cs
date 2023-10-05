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
    private float ObjectScaleDataR;
    private float ObjectOriginalScaleR;
    private float ObjectScaleDataL;
    private float ObjectOriginalScaleL;
    private Rigidbody ObjectRightRb;
    private Rigidbody ObjectLeftRb;
    private Transform ObjectRightT;
    private Transform ObjectLeftT;
    private bool GrabObjR = false;
    private bool ObjectGrabbedR = true;
    private bool ThrowCheckR = true;
    private bool GrabObjL = false;
    private bool ObjectGrabbedL = true;
    private bool ThrowCheckL = true;

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
        if(Hit.transform != null && (Hit.transform.tag == "Object" || Hit.transform.tag == "Bengal"))
        {
            bool IsRightPressed = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
            bool IsLeftPressed = Controls.Player.LeftItem.ReadValue<float>() > 0.1f;
            if(IsRightPressed && !GrabObjR && ObjectGrabbedR && Hit.transform.tag == "Object")
            {
                StartCoroutine(RightGrab());
            }
            else if(IsLeftPressed && !GrabObjL && ObjectGrabbedL && Hit.transform.tag == "Bengal")
            {
                StartCoroutine(LeftGrab());
            }
        }
    }

    public void Drop()
    {
        bool IsRightPressed = Controls.Player.RightThrow.ReadValue<float>() > 0.1f;
        bool IsLeftPressed = Controls.Player.LeftThrow.ReadValue<float>() > 0.1f;
        if(IsRightPressed && GrabObjR && ThrowCheckR)
        {
            StartCoroutine(RightDrop());
        }
        else if(IsLeftPressed && GrabObjL && ThrowCheckL)
        {
            StartCoroutine(LeftDrop());
        }
    }

    public IEnumerator LeftGrab()
    {
        ObjectGrabbedL = false;
        Hit.transform.position = ObjectLeftCamera.position;
        Hit.rigidbody.isKinematic = true;
        Hit.transform.parent = ObjectLeftCamera;
        ObjectScaleDataL = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
        Hit.transform.localScale = new Vector3(ObjectScaleDataL, ObjectScaleDataL, ObjectScaleDataL);
        Hit.transform.localPosition = new Vector3(0, 0, 0);
        Hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        ObjectOriginalScaleL = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        ObjectLeftRb = Hit.rigidbody;
        ObjectLeftT = Hit.transform;
        LeftGrabTwo();
        yield return new WaitForSeconds(0.5f);
        GrabObjL = true;
        ObjectGrabbedL = true;
    }

    public void LeftGrabTwo()
    {   
        int LayerIgnoreRaycast = LayerMask.NameToLayer("PlayerSanti");
        // playerL.GetComponentInChildren<Transform>();
        GameObject child = playerL.transform.GetChild(0).gameObject;
        child.layer = LayerIgnoreRaycast;
        cloneL = (GameObject)Instantiate(child, ObjectLeftHand.position, Quaternion.identity);
        cloneL.transform.parent = ObjectLeftHand;
        LayerIgnoreRaycast = LayerMask.NameToLayer("PlayerJose");
        child.layer = LayerIgnoreRaycast;
    }

    public IEnumerator RightGrab()
    {
        ObjectGrabbedR = false;
        Hit.transform.position = ObjectRightCamera.position;
        Hit.rigidbody.isKinematic = true;
        Hit.transform.parent = ObjectRightCamera;
        ObjectScaleDataR = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
        Hit.transform.localScale = new Vector3(ObjectScaleDataR, ObjectScaleDataR, ObjectScaleDataR);
        Hit.transform.localPosition = new Vector3(0, 0, 0);
        Hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        ObjectOriginalScaleR = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        ObjectRightRb = Hit.rigidbody;
        ObjectRightT = Hit.transform;
        RightGrabTwo();
        yield return new WaitForSeconds(0.5f);
        GrabObjR = true;
        ObjectGrabbedR = true;
    }

    public void RightGrabTwo()
    {
        int LayerIgnoreRaycast = LayerMask.NameToLayer("PlayerSanti");
        // playerL.GetComponentInChildren<Transform>();
        GameObject child = playerR.transform.GetChild(0).gameObject;
        child.layer = LayerIgnoreRaycast;
        cloneR = (GameObject) Instantiate(child, ObjectRightHand.position, Quaternion.identity);
        cloneR.transform.parent = ObjectRightHand;
        LayerIgnoreRaycast = LayerMask.NameToLayer("PlayerJose");
        child.layer = LayerIgnoreRaycast;
    }

    public IEnumerator LeftDrop()
    {
        GameObject child = playerL.transform.GetChild(0).gameObject;
        child.layer = 0;
        ThrowCheckL = false;
        ObjectLeftT.transform.localScale = new Vector3(ObjectOriginalScaleL, ObjectOriginalScaleL, ObjectOriginalScaleL);
        Vector3 camerDirection = PlayerCamera.transform.forward;
        ObjectLeftT.transform.parent = null;
        ObjectLeftRb.isKinematic = false;
        ObjectLeftRb.AddForce(camerDirection * ThrowForce);
        Destroy(cloneL);
        yield return new WaitForSeconds(0.5f);
        GrabObjL = false;
        ThrowCheckL = true;
    }

    public IEnumerator RightDrop()
    {
        GameObject child = playerR.transform.GetChild(0).gameObject;
        child.layer = 0;
        ThrowCheckR = false;
        ObjectRightT.transform.localScale = new Vector3(ObjectOriginalScaleR, ObjectOriginalScaleR, ObjectOriginalScaleR);
        Vector3 camerDirection = PlayerCamera.transform.forward;
        ObjectRightT.transform.parent = null;
        ObjectRightRb.isKinematic = false;
        ObjectRightRb.AddForce(camerDirection * 0);
        Destroy(cloneR);
        yield return new WaitForSeconds(0.5f);
        GrabObjR = false;
        ThrowCheckR = true;
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