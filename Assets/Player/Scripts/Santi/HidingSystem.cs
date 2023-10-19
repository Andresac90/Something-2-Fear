using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSystem : MonoBehaviour
{
    private InputMaster Controls;
    public GameObject hideText, stopHideText;
    public GameObject normalPlayer;
    private SantiController santicontroller;
    public Transform HidePosition;
    public Transform OutPosition;
    private GameObject enemy;
    private AIControl enemyAI;
    bool interactable, hiding;
    public float loseDistance;

    void Awake()
    {
        Controls = new InputMaster();
    }
    void Start()
    {
        enemy = GameObject.Find("Pascualita");
        enemyAI = enemy.GetComponent<AIControl>();
        interactable = false;
        hiding = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            hideText.SetActive(true);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(false);
            interactable = false;
        }
    }

    void Update()
    {
        bool IsClickPressed = Controls.Player.Interact.ReadValue<float>() > 0.1f;
        if (interactable)
        {
            if (IsClickPressed)
            {
                hideText.SetActive(false);
                normalPlayer.transform.position.Set(HidePosition.position.x,HidePosition.position.y,HidePosition.position.z);
                float distance = Vector3.Distance(enemy.transform.position, normalPlayer.transform.position);
                if(distance > loseDistance)
                {
                    if(enemyAI.playerInRange)
                    {
                        enemyAI.playerInRange = false;
                    }
                }
                stopHideText.SetActive(true);
                hiding = true;
                santicontroller = normalPlayer.GetComponent<SantiController>();
                santicontroller.isMovementActive = false;
                interactable = false;
            }
        }
        if (hiding)
        {
            if (IsClickPressed)
            {
                stopHideText.SetActive(false);
                santicontroller.isMovementActive = true;
                normalPlayer.transform.position.Set(OutPosition.position.x, OutPosition.position.y, OutPosition.position.z);
                hiding = false;
            }
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
