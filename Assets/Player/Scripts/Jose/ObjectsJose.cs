using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ObjectsJose : MonoBehaviour
{
    private InputMaster Controls;

    [SerializeField]
    private Transform ObjectRight;
    [SerializeField]
    private Transform ObjectLeft;
    [SerializeField]
    private Camera PlayerCamera;

    public LayerMask PlayerMask;

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
