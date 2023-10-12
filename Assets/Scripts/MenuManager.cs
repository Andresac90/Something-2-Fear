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
