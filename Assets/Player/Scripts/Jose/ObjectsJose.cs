using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using TMPro;

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
    private float rayline;
    [SerializeField]
    private float ThrowForce;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject ObjectLeftUI;
    [SerializeField]
    private GameObject ObjectRightUI;

    private RaycastHit hit;
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
        Physics.Raycast(PlayerCamera.position, PlayerCamera.TransformDirection(Vector3.forward), out hit, rayline);
        if (hit.transform != null)
        {
            StartCoroutine(Grab());
            
        }
        if (hit.transform != null && hit.transform.tag == "Object" && !HasObjectRight)
        {
            ObjectRightUI.SetActive(true);

        }
        if (hit.transform != null && hit.transform.tag == "Object" && !HasObjectLeft)
        {
            ObjectLeftUI.SetActive(true);
        }
        else
        {
            ObjectRightUI.SetActive(false);
            ObjectLeftUI.SetActive(false);
        }
        StartCoroutine(Throw());
        ThrowGrounded();

    }

    IEnumerator Grab()
    {
        
        if(hit.transform.tag == "Object")
        {
            
            bool IsRightPressed = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
            if(IsRightPressed && HasObjectRight == false)
            {
                hit.transform.position = ObjectRightCamera.position;
                hit.rigidbody.isKinematic = true;
                hit.transform.parent = ObjectRightCamera;
                ObjectRScaleData = hit.transform.GetComponent<ObjectsData>().ObjectScale;
                hit.transform.localScale = new Vector3(ObjectRScaleData, ObjectRScaleData, ObjectRScaleData);
                hit.transform.localPosition = new Vector3(0, 0, 0);
                hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
                ObjectROriginalScale = hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
                ObjectRightRb = hit.rigidbody;
                ObjectRightT = hit.transform;
                ObjectRightUI.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                HasObjectRight = true;
                ThrowCheckR = true;
                
            }

            bool IsLeftPressed = Controls.Player.LeftItem.ReadValue<float>() > 0.1f;
            if (IsLeftPressed && HasObjectLeft == false)
            {
                hit.transform.position = ObjectRightCamera.position;
                hit.rigidbody.isKinematic = true;
                hit.transform.parent = ObjectLeftCamera;
                ObjectLScaleData = hit.transform.GetComponent<ObjectsData>().ObjectScale;
                hit.transform.localScale = new Vector3(ObjectLScaleData, ObjectLScaleData, ObjectLScaleData);
                hit.transform.localPosition = new Vector3(0, 0, 0);
                hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
                ObjectLOriginalScale = hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
                ObjectLeftRb = hit.rigidbody;
                ObjectLeftT = hit.transform;
                ObjectLeftUI.SetActive(false);
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
        Gizmos.DrawLine(PlayerCamera.position, PlayerCamera.TransformDirection(Vector3.forward) * rayline);
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
