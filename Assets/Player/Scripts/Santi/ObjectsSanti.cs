using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

// public class ObjectsSanti : MonoBehaviour
// {
//     private InputMaster Controls;

//     [SerializeField]
//     private Transform ObjectRightCamera;
//     [SerializeField]
//     private Transform ObjectLeftCamera;
//     [SerializeField]
//     private Transform PlayerCamera;
//     [SerializeField]
//     private float RayLine;

//     private RaycastHit Hit;
//     private float ObjectScaleData;
//     private float ObjectOriginalScale;
//     private Rigidbody ObjectRightRb;
//     private Rigidbody ObjectLeftRb;
//     private Transform ObjectRightT;
//     private Transform ObjectLeftT;
//     private bool GrabbedObjR = false;
//     private bool GrabbedObjL = false;

//     public void Awake()
//     {
//         Controls = new InputMaster();
//     }
//     public void Start()
//     {

//     }

//     // Update is called once per frame
//     public void Update()
//     {
//         Grab();
//         Drop();
//     }

//     public void Grab()
//     {
//         Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out Hit, RayLine);
//         if(Hit.transform != null && Hit.transform.tag == "Object")
//         {
//             //RightObject
//             bool IsRightObject = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
//             //LeftObject
//             bool IsLeftObject = Controls.Player.LeftItem.ReadValue<float>() > 0.1f;
//             if(GrabbedObjR == false && IsRightObject)
//             {
//                 RightGrab();
//             }
//             else if(GrabbedObjL == false && IsLeftObject)
//             {
//                 LeftGrab();
//             }
//         }
//     }

//     public void Drop()
//     {
//         //RightObject
//         bool IsRightObject = Controls.Player.RightThrow.ReadValue<float>() > 0.1f;
//         //LeftObject
//         bool IsLeftObject = Controls.Player.LeftThrow.ReadValue<float>() > 0.1f;
//         if(GrabbedObjR == true && IsRightObject)
//         {
//             RightGrab();
//         }
//         else if(GrabbedObjL == true && IsLeftObject)
//         {
//             LeftGrab();
//         }
//     }

//     public IEnumerator LeftGrab()
//     {
//         Hit.transform.position = ObjectLeftCamera.position;
//         Hit.rigidbody.isKinematic = true;
//         Hit.transform.parent = ObjectLeftCamera;
//         ObjectScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
//         Hit.transform.localScale = new Vector3(ObjectScaleData, ObjectScaleData, ObjectScaleData);
//         ObjectOriginalScale = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
//         ObjectLeftRb = Hit.rigidbody;
//         ObjectLeftT = Hit.transform;
//         yield return new WaitForSeconds(0.5f);
//         GrabbedObjL = true;
//     }
//     public IEnumerator RightGrab()
//     {
//         Hit.transform.position = ObjectRightCamera.position;
//         Hit.rigidbody.isKinematic = true;
//         Hit.transform.parent = ObjectRightCamera;
//         ObjectScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
//         Hit.transform.localScale = new Vector3(ObjectScaleData, ObjectScaleData, ObjectScaleData);
//         ObjectOriginalScale = Hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
//         ObjectRightRb = Hit.rigidbody;
//         ObjectRightT = Hit.transform;
//         yield return new WaitForSeconds(0.5f);
//         GrabbedObjR = true;
//     }

//     private void OnDrawGizmos()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawLine(PlayerCamera.position, PlayerCamera.forward * RayLine);
//     }

//     private void OnEnable()
//     {
//         Controls.Enable();
//     }

//     private void OnDisable()
//     {
//         Controls.Disable();
//     }
// }