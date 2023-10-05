using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class ObjectsJose : MonoBehaviour
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
    [SerializeField]
    private GameObject Player;

    private RaycastHit Hit;
    private float ObjectRScaleData;
    private float ObjectROriginalScale;
    private float ObjectLScaleData;
    private float ObjectLOriginalScale;
    private Rigidbody ObjectRightRb;
    private Rigidbody ObjectLeftRb;
    private Transform ObjectRightT;
    private Transform ObjectLeftT;
    private bool HasObjectRight = false;
    private bool HasObjectLeft = false;
    private bool ThrowCheckR = false;
    private bool ThrowCheckL = false;
    private bool IsCrouched = false;

    void Awake()
    {
        Controls = new InputMaster();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Grab());
        StartCoroutine(Throw());
        ThrowGrounded();
    }

    IEnumerator Grab()
    {
        Physics.Raycast(PlayerCamera.position, PlayerCamera.TransformDirection(Vector3.forward), out Hit, RayLine);
        if(Hit.transform != null && Hit.transform.tag == "Object")
        {
            bool IsRightPressed = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
            if(IsRightPressed && HasObjectRight == false)
            {
                Hit.transform.position = ObjectRightCamera.position;
                Hit.rigidbody.isKinematic = true;
                Hit.transform.parent = ObjectRightCamera;
                ObjectRScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
                Hit.transform.localScale = new Vector3(ObjectRScaleData, ObjectRScaleData, ObjectRScaleData);
                Hit.transform.localPosition = new Vector3(0, 0, 0);
                Hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
                ObjectROriginalScale = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
                ObjectRightRb = Hit.rigidbody;
                ObjectRightT = Hit.transform;
                yield return new WaitForSeconds(0.5f);
                HasObjectRight = true;
                ThrowCheckR = true;
            }

            bool IsLeftPressed = Controls.Player.LeftItem.ReadValue<float>() > 0.1f;
            if (IsLeftPressed && HasObjectLeft == false)
            {
                Hit.transform.position = ObjectRightCamera.position;
                Hit.rigidbody.isKinematic = true;
                Hit.transform.parent = ObjectLeftCamera;
                ObjectLScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
                Hit.transform.localScale = new Vector3(ObjectLScaleData, ObjectLScaleData, ObjectLScaleData);
                Hit.transform.localPosition = new Vector3(0, 0, 0);
                Hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
                ObjectLOriginalScale = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
                ObjectLeftRb = Hit.rigidbody;
                ObjectLeftT = Hit.transform;
                yield return new WaitForSeconds(0.5f);
                HasObjectLeft = true;
                ThrowCheckL = true;
            }

        }
    }

    IEnumerator Throw()
    {
        bool IsRightTPressed = Controls.Player.RightThrow.ReadValue<float>() > 0.1f;
        if(IsRightTPressed && HasObjectRight == true && ThrowCheckR)
        {
            ObjectRightT.transform.localScale = new Vector3(ObjectROriginalScale, ObjectROriginalScale, ObjectROriginalScale);
            Vector3 camerDirection = PlayerCamera.transform.forward;
            ObjectRightT.transform.parent = null;
            ObjectRightRb.isKinematic = false;
            ObjectRightRb.AddForce(camerDirection * ThrowForce);
            ThrowCheckR = false;
            yield return new WaitForSeconds(0.5f);
            HasObjectRight = false;
           
        }

        bool IsLeftPressed = Controls.Player.LeftThrow.ReadValue<float>() > 0.1f;
        if (IsLeftPressed && HasObjectLeft == true && ThrowCheckL)
        {
            ObjectLeftT.transform.localScale = new Vector3(ObjectLOriginalScale, ObjectLOriginalScale, ObjectLOriginalScale);
            Vector3 camerDirection = PlayerCamera.transform.forward;
            ObjectLeftT.transform.parent = null;
            ObjectLeftRb.isKinematic = false;
            ObjectLeftRb.AddForce(camerDirection * ThrowForce);
            ThrowCheckL = false;
            yield return new WaitForSeconds(0.5f);
            HasObjectLeft = false;
        }

    }

    private void ThrowGrounded()
    {
        IsCrouched = Player.transform.GetComponent<JoseMovement>().IsCrouched;
        if (IsCrouched && HasObjectRight)
        {
            ObjectRightT.transform.localScale = new Vector3(ObjectROriginalScale, ObjectROriginalScale, ObjectROriginalScale);
            Vector3 camerDirection = PlayerCamera.transform.forward;
            camerDirection += new Vector3(0, 1.2f, 0);
            ObjectRightT.transform.parent = null;
            ObjectRightRb.isKinematic = false;
            ObjectRightRb.AddForce(camerDirection * 1);
            HasObjectRight = false;
        }

        if (IsCrouched && HasObjectLeft)
        {
            ObjectLeftT.transform.localScale = new Vector3(ObjectLOriginalScale, ObjectLOriginalScale, ObjectLOriginalScale);
            Vector3 camerDirection = PlayerCamera.transform.forward;
            camerDirection +=  new Vector3(0, 1.2f, 0);
            ObjectLeftT.transform.parent = null;
            ObjectLeftRb.isKinematic = false;
            ObjectLeftRb.AddForce(camerDirection * 1);
            HasObjectLeft = false;
        }
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
