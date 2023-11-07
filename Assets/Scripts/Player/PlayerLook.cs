using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Photon.Pun;

public class PlayerLook : MonoBehaviour
{
    private InputMaster Controls;
    private float LookX;
    private float LookY;

    [SerializeField]
    private float SensitivityX = 1f;
    [SerializeField]
    private float SensitivityY = 1f;

    [SerializeField]
    private bool InvertCamera;

    private float RotationY = 0f;

    private Transform Character;

    private Vector2 mouseLook;

    [SerializeField]
    private PhotonView PV;

    private MenuManager MenuManager;


    void Awake()
    {
        Controls = new InputMaster();
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        Character = transform.parent;

        GameObject canvas = GameObject.Find("/Canvas");
        MenuManager = canvas.GetComponent<MenuManager>();
    }

    void Update()
    {
        if(MenuManager.OptionsMenuActive) return;
        Look();
    }

    public void SetInvert(bool invert)
    {
        InvertCamera = invert;
    }

    void Look()
    {
        mouseLook = Controls.Player.Look.ReadValue<Vector2>();
        LookX = mouseLook.x * SensitivityX * Time.deltaTime;
        LookY = mouseLook.y * SensitivityY * Time.deltaTime;

        if(InvertCamera)
        {
            RotationY += LookY;
        }
        else
        {
            RotationY -= LookY;
        }

        RotationY = Mathf.Clamp(RotationY, -90f, 90f);

        Character.Rotate(Vector3.up * LookX);

        transform.localRotation = Quaternion.Euler(RotationY, 0, 0);
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }

    public void SetSens (float sens)
    {  
        SensitivityX *= sens;
        SensitivityY *= sens;
    }
}
