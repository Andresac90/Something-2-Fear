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


    private RaycastHit Hit;
    private float ObjectScaleData;
    private float ObjectOriginalScale;
    private Rigidbody ObjectRightRb;
    private Rigidbody ObjectLeftRb;
    private Transform ObjectRightT;
    private Transform ObjectLeftT;
    private bool HasObjectRight = false;
    private bool HasObjectLeft = false;

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
    }

    IEnumerator Grab()
    {
        Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out Hit, RayLine);
        if(Hit.transform != null && Hit.transform.tag == "Object")
        {
            bool IsRightPressed = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
            if(IsRightPressed && HasObjectRight == false)
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
                yield return new WaitForSeconds(0.5f);
                HasObjectRight = true;
            }
            
        }
    }

    IEnumerator Throw()
    {
        bool IsRightTPressed = Controls.Player.RightThrow.ReadValue<float>() > 0.1f;
        if(IsRightTPressed && HasObjectRight == true)
        {
            ObjectRightT.transform.localScale = new Vector3(ObjectOriginalScale, ObjectOriginalScale, ObjectOriginalScale);
            Vector3 camerDirection = PlayerCamera.transform.forward;
            ObjectRightT.transform.parent = null;
            ObjectRightRb.isKinematic = false;
            ObjectRightRb.AddForce(camerDirection * ThrowForce);
            yield return new WaitForSeconds(0.5f);
            HasObjectRight = false;
            Debug.Log("Hola");
           
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(PlayerCamera.position, PlayerCamera.forward * RayLine);
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
