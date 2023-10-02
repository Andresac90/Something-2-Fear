using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectsJose : MonoBehaviour
{
    private InputMaster Controls;

    [SerializeField]
    private Transform ObjectRight;
    [SerializeField]
    private Transform ObjectLeft;
    [SerializeField]
    private Transform PlayerCamera;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private float RayLine;

    private RaycastHit Hit;
    private float ObjectScaleData;

    public LayerMask PlayerMask;
    public float RadiousObject;
    public bool NearObject;
    

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
        Grab();
    }

    void Grab()
    {
        Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out Hit, RayLine);
        if(Hit.transform != null && Hit.transform.tag == "Object")
        {
            bool IsRightObject = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
            if(IsRightObject)
            {
                Hit.transform.position = ObjectRight.position;
                Hit.rigidbody.isKinematic = true;
                Hit.transform.parent = ObjectRight;
                ObjectScaleData = Hit.transform.GetComponent<ObjectsData>().ObjectScale;
                Hit.transform.localScale = new Vector3(ObjectScaleData, ObjectScaleData, ObjectScaleData);
            }
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
