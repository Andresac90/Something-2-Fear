using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompts : MonoBehaviour
{
    [SerializeField]
    private int promptType = 0;
    public void OnTriggerEnter(Collider collision)
    {
        if (promptType == 0)
        {
            GameManager.Instance.tutorialJump = true;
        }
        if (promptType == 1)
        {
            GameManager.Instance.tutorialRun = true;
        }
        if (promptType == 2)
        {
            GameManager.Instance.tutorialCrouch = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (promptType == 0)
        {
            GameManager.Instance.tutorialJump = false;
        }
        if (promptType == 1)
        {
            GameManager.Instance.tutorialRun = false;
        }
        if (promptType == 2)
        {
            GameManager.Instance.tutorialCrouch = false;
        }
    }
}
