using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private InputMaster Controls;

    void Awake()
    {
        Controls = new InputMaster();
    }

    void Start()
    {
        StartCoroutine(LoadMenuAfterCredits());
    }

    // Update is called once per frame
    void Update()
    {
        bool IsClickPressed = Controls.Player.Jump.ReadValue<float>() > 0.1f;
        if (IsClickPressed)
        {
            SceneManager.LoadScene("Menus");
        }
    }

    private IEnumerator LoadMenuAfterCredits()
    {
        yield return new WaitForSeconds(59.8f);
        SceneManager.LoadScene("Menus");
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
