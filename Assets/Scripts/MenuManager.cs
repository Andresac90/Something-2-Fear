using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private InputMaster Controls;
    [SerializeField]
    private GameObject OptionsMenuUI;

    public bool OptionsMenuActive = false;

    void Awake()
    {
        Controls = new InputMaster();
    }

    void Update()
    {
        if(Controls.Player.Menu.triggered)
        {
            OptionsMenuActive = !OptionsMenuActive;
            OptionsMenuUI.SetActive(OptionsMenuActive);

            Cursor.visible = OptionsMenuActive;
            Cursor.lockState = OptionsMenuActive ? CursorLockMode.None : CursorLockMode.Locked;
        }
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
