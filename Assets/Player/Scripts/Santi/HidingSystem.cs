using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class HidingSystem : MonoBehaviour
{
    private InputMaster Controls;
    public GameObject hideText, stopHideText;
    private GameObject player;
    private SantiController santicontroller;
    private CharacterController characterController;

    [SerializeField]
    private float rayLine;
    private RaycastHit hit;
    private GameObject santicamera;
    public Transform HidePosition;
    public Transform OutPosition;
    private GameObject enemy;
    private AIControl enemyAI;
    public bool hiding;
    public float loseDistance;

    void Awake()
    {
        Controls = new InputMaster();
    }
    void Start()
    {
        enemy = GameObject.Find("Pascualita");
        enemyAI = enemy.GetComponent<AIControl>();
        player = GameObject.Find("Santi(Clone)");
        santicamera =  player.transform.GetChild(0).gameObject;
        hiding = false;
    }

    void Update()
    {
        Physics.Raycast(santicamera.GetComponent<Camera>().transform.position, santicamera.GetComponent<Camera>().transform.TransformDirection(Vector3.forward), out hit, rayLine);
        StartCoroutine(Hide());
        if (hit.transform != null && hit.transform.tag == ("Hide"))
        {
            hideText.SetActive(true);
            
        }
        else
        {
            hideText.SetActive(false);
        }
    }

    IEnumerator Hide()
    {
        bool IsClickPressed = Controls.Player.Interact.ReadValue<float>() > 0.1f;
        if (IsClickPressed && !hiding && hit.transform != null && hit.transform.tag == ("Hide"))
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<SantiController>().enabled = false;
            hideText.SetActive(false);
            stopHideText.SetActive(true);
            player.transform.localPosition = new Vector3(HidePosition.position.x, HidePosition.position.y, HidePosition.position.z);
            //Debug.Log("hide");
            float distance = Vector3.Distance(enemy.transform.position, player.transform.position);
            if (distance > loseDistance)
            {
                if (enemyAI.playerInRange)
                {
                    enemyAI.playerInRange = false;
                }
            }
            yield return new WaitForSeconds(1.5f);
            hiding = true;
        }
        else
        {
            if (IsClickPressed && hiding)
            {
                stopHideText.SetActive(false);
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<SantiController>().enabled = true;
                player.transform.localPosition = new Vector3(OutPosition.position.x, OutPosition.position.y, OutPosition.position.z);
                //Debug.Log("show");
                yield return new WaitForSeconds(1.5f);
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
