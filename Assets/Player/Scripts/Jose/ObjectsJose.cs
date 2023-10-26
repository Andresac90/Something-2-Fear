using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ObjectsJose : MonoBehaviour
{
    private InputMaster Controls;

    [SerializeField]
    private Transform ObjectRightCamera;
    [SerializeField]
    private Transform ObjectLeftCamera;
    [SerializeField]
    private Transform PlayerCamera;
    [SerializeField]
    private float rayline;
    [SerializeField]
    private float ThrowForce;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject ObjectLeftUI;
    [SerializeField]
    private GameObject ObjectRightUI;
    [SerializeField]
    private GameObject ThrowLeftUI;
    [SerializeField]
    private GameObject ThrowRightUI;
    [SerializeField]
    private GameObject Timer;

    //MasterKeys
    [SerializeField]
    private GameObject Key1UI;
    [SerializeField]
    private GameObject Key2UI;
    [SerializeField]
    private GameObject Key3UI;

    private TextMeshProUGUI textMeshProText;
    private RaycastHit hit;
    private float ObjectRScaleData;
    private float ObjectROriginalScale;
    private float ObjectLScaleData;
    private float ObjectLOriginalScale;
    private Rigidbody ObjectRightRb;
    private GameObject pascualita;
    private GameObject nurse;
    private Rigidbody ObjectLeftRb;
    private Transform ObjectRightT;
    private Transform ObjectLeftT;
    private bool HasObjectRight = false;
    private bool HasObjectLeft = false;
    private bool ThrowCheckR = false;
    private bool ThrowCheckL = false;
    private bool IsCrouched = false;
    private bool activated = false;
    private Cure activeStation = null;

    void Awake()
    {
        Controls = new InputMaster();
    }
    public void Start()
    {
        pascualita = GameObject.Find("Pascualita");
        nurse = GameObject.Find("nurse");
        AIControl aicontrolP = pascualita.GetComponent<AIControl>();
        NurseAI aicontrolN = nurse.GetComponent<NurseAI>();
        aicontrolP.joseActivation();
        aicontrolN.JoseActivation();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(PlayerCamera.position, PlayerCamera.TransformDirection(Vector3.forward), out hit, rayline);
        if (hit.transform != null)
        {
            StartCoroutine(Grab());
            
        }
        //UI Grab
        if (hit.transform != null && hit.transform.tag == "Object" && !HasObjectRight)
        {
            ObjectRightUI.SetActive(true);

        }
        else
        {
            ObjectRightUI.SetActive(false);
        }
        if (hit.transform != null && hit.transform.tag == "Object" && !HasObjectLeft)
        {
            ObjectLeftUI.SetActive(true);
        }
        else
        {
            ObjectLeftUI.SetActive(false);
        }

        //MasterKeys
        if (GameManager.Instance.Key1)
        {
            Key1UI.SetActive(true);
        }
        if (GameManager.Instance.Key2)
        {
            Key2UI.SetActive(true);
        }
        if (GameManager.Instance.Key3)
        {
            Key3UI.SetActive(true);
        }

        //Timer UI
        if (this.GetComponent<Injection>().isPlayerInjected)
        {
            Timer.SetActive(true);
            textMeshProText = Timer.GetComponent<TextMeshProUGUI>();
            textMeshProText.text = ((int)this.GetComponent<Injection>().downTime - (int)this.GetComponent<Injection>().currentTime).ToString();
        }
        else
        {
            Timer.SetActive(false);
            textMeshProText = Timer.GetComponent<TextMeshProUGUI>();
            textMeshProText.text = ((int)this.GetComponent<Injection>().currentTime).ToString();
        }

        //UI Throw
        if (HasObjectRight == true && ThrowCheckR)
        {
            ThrowRightUI.SetActive(true);
        }
        else
        {
            ThrowRightUI.SetActive(false);
        }
        if (HasObjectLeft == true && ThrowCheckL)
        {
            ThrowLeftUI.SetActive(true);
        }
        else
        {
            ThrowLeftUI.SetActive(false);
        }
        StartCoroutine(Throw());
        ThrowGrounded();

    }

    private void Activation()
    {
        if (hit.transform != null && hit.transform.tag == "Health")
        {
            Cure station = hit.transform.GetComponent<Cure>();
            bool isInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0.0f;

            if (station != null)
            {
                if (isInteractPressed)
                {
                    station.updateCure(true, this.gameObject);
                    activeStation = station;
                    activated = true;
                }
                else if (activeStation != null)
                {
                    station.updateCure(false, this.gameObject);
                    activeStation = null;
                    activated = false;
                }
            }
        }
        else if (activeStation != null && activated)
        {
            activeStation.updateCure(false, this.gameObject);
            activated = false;
        }
    }

    IEnumerator Grab()
    {
        
        if(hit.transform.tag == "Object")
        {
            
            bool IsRightPressed = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
            if(IsRightPressed && HasObjectRight == false)
            {
                // hit.transform.position = ObjectRightCamera.position;
                // hit.rigidbody.isKinematic = true;
                // hit.transform.parent = ObjectRightCamera;
                // ObjectRScaleData = hit.transform.GetComponent<ObjectsData>().ObjectScale;
                // hit.transform.localScale = new Vector3(ObjectRScaleData, ObjectRScaleData, ObjectRScaleData);
                // hit.transform.localPosition = new Vector3(0, 0, 0);
                // hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
                // ObjectROriginalScale = hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
                
                hit.transform.GetComponent<ObjectsData>().OnGrab(ObjectRightCamera);
                ObjectRightRb = hit.rigidbody;
                ObjectRightT = hit.transform;
                ObjectRightUI.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                HasObjectRight = true;
                ThrowCheckR = true;
                
            }

            bool IsLeftPressed = Controls.Player.LeftItem.ReadValue<float>() > 0.1f;
            if (IsLeftPressed && HasObjectLeft == false)
            {
                // hit.transform.position = ObjectRightCamera.position;
                // hit.rigidbody.isKinematic = true;
                // hit.transform.parent = ObjectLeftCamera;
                // ObjectLScaleData = hit.transform.GetComponent<ObjectsData>().ObjectScale;
                // hit.transform.localScale = new Vector3(ObjectLScaleData, ObjectLScaleData, ObjectLScaleData);
                // hit.transform.localPosition = new Vector3(0, 0, 0);
                // hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
                // ObjectLOriginalScale = hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
                
                hit.transform.GetComponent<ObjectsData>().OnGrab(ObjectLeftCamera);
                ObjectLeftRb = hit.rigidbody;
                ObjectLeftT = hit.transform;
                ObjectLeftUI.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                HasObjectLeft = true;
                ThrowCheckL = true;
            }

        }
        
    }

    IEnumerator Throw()
    {
        bool IsRightTPressed = Controls.Player.RightThrow.ReadValue<float>() > 0.1f;
        if(IsRightTPressed && HasObjectRight == true && ThrowCheckR)
        {
            // ObjectRightT.transform.localScale = new Vector3(ObjectROriginalScale, ObjectROriginalScale, ObjectROriginalScale);
            // Vector3 camerDirection = PlayerCamera.transform.forward;
            // ObjectRightT.transform.parent = null;
            // ObjectRightRb.isKinematic = false;
            // ObjectRightRb.AddForce(camerDirection * ThrowForce);
            Debug.Log("Throwing");
            ObjectRightRb.GetComponent<ObjectsData>().onThrow(PlayerCamera.transform.forward, ThrowForce);
            ThrowCheckR = false;
            yield return new WaitForSeconds(0.5f);
            HasObjectRight = false;
           
        }

        bool IsLeftPressed = Controls.Player.LeftThrow.ReadValue<float>() > 0.1f;
        if (IsLeftPressed && HasObjectLeft == true && ThrowCheckL)
        {
            // ObjectLeftT.transform.localScale = new Vector3(ObjectLOriginalScale, ObjectLOriginalScale, ObjectLOriginalScale);
            // Vector3 camerDirection = PlayerCamera.transform.forward;
            // ObjectLeftT.transform.parent = null;
            // ObjectLeftRb.isKinematic = false;
            // ObjectLeftRb.AddForce(camerDirection * ThrowForce);
            Debug.Log("Throwing");
            ObjectLeftRb.GetComponent<ObjectsData>().onThrow(PlayerCamera.transform.forward, ThrowForce);
            ThrowCheckL = false;
            yield return new WaitForSeconds(0.5f);
            HasObjectLeft = false;
        }

    }

    private void ThrowGrounded()
    {
        IsCrouched = Player.transform.GetComponent<JoseMovement>().IsCrouched;
        if (IsCrouched && HasObjectRight)
        {
            // ObjectRightT.transform.localScale = new Vector3(ObjectROriginalScale, ObjectROriginalScale, ObjectROriginalScale);
            // Vector3 camerDirection = PlayerCamera.transform.forward;
            // camerDirection += new Vector3(0, 1.2f, 0);
            // ObjectRightT.transform.parent = null;
            // ObjectRightRb.isKinematic = false;
            // ObjectRightRb.AddForce(camerDirection * 1);
            ObjectRightRb.GetComponent<ObjectsData>().OnRelease();
            HasObjectRight = false;
        }

        if (IsCrouched && HasObjectLeft)
        {
            // ObjectLeftT.transform.localScale = new Vector3(ObjectLOriginalScale, ObjectLOriginalScale, ObjectLOriginalScale);
            // Vector3 camerDirection = PlayerCamera.transform.forward;
            // camerDirection +=  new Vector3(0, 1.2f, 0);
            // ObjectLeftT.transform.parent = null;
            // ObjectLeftRb.isKinematic = false;
            // ObjectLeftRb.AddForce(camerDirection * 1);
            ObjectLeftRb.GetComponent<ObjectsData>().OnRelease();
            HasObjectLeft = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(PlayerCamera.position, PlayerCamera.TransformDirection(Vector3.forward) * rayline);
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
