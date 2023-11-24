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

public class ObjectsJose : MonoBehaviourPun
{
    private InputMaster Controls;

    [SerializeField]
    private Transform ObjectRightCamera;
    [SerializeField]
    private Transform ObjectLeftCamera;
    [SerializeField]
    private Transform objectRightHand;
    [SerializeField]
    private Transform objectLeftHand;
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
    private GameObject InteractUI;
    [SerializeField]
    private GameObject HealingUI;
    [SerializeField]
    private GameObject Timer;
    private GameObject LightBox;
    [SerializeField]
    private GameObject[] Buzzer;

    //MasterKeys
    [SerializeField]
    private GameObject Key1UI;
    [SerializeField]
    private GameObject Key2UI;
    [SerializeField]
    private GameObject Key3UI;

    //ObjectsNurse
    [SerializeField]
    private GameObject Object1UI;
    [SerializeField]
    private GameObject Object2UI;
    [SerializeField]
    private GameObject Object3UI;
    [SerializeField]
    private GameObject InjectionUI;
    //TutorialUI
    [SerializeField]
    private GameObject jumpUI;
    [SerializeField]
    private GameObject runUI;
    [SerializeField]
    private GameObject crouchUI;

    private TextMeshProUGUI textMeshProText;
    private RaycastHit hit;
    private int iBuzzer = 0;
    private Rigidbody ObjectRightRb;
    private GameObject pascualita;
    private GameObject nurse;
    private GameObject nina;
    private GameObject LDoor;
    private Rigidbody ObjectLeftRb;
    private bool HasObjectRight = false;
    private bool HasObjectLeft = false;
    private bool ThrowCheckR = false;
    private bool ThrowCheckL = false;
    private bool IsCrouched = false;
    private bool activated = false;
    private Cure activeStation = null;
    private Animator joseAnimator;
    private PhotonView PV;

    
    void Awake()
    {
        Controls = new InputMaster();
    }
    public void Start()
    {
        PV = GetComponent<PhotonView>();
        Buzzer = new GameObject[4];
        pascualita = GameObject.Find("Pascualita");
        nurse = GameObject.Find("nurse");
        nina = GameObject.Find("Nina");
        LightBox = GameObject.Find("LightBox");
        LDoor = GameObject.Find("L_DoorFinal");
        for(int i = 0; i < Buzzer.Length; i++) {
            Buzzer[i] = GameObject.Find("Buzzer"+i.ToString());
        }
        AIControl aicontrolP = pascualita.GetComponent<AIControl>();
        NurseAI aicontrolN = nurse.GetComponent<NurseAI>();
        NinaAI aicontrolNi = nina.GetComponent<NinaAI>();
        aicontrolP.joseActivation();
        aicontrolN.JoseActivation();
        aicontrolNi.JoseActivation();
        joseAnimator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {

        if (!PV.IsMine) return;
        
        Activation();
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

        //Interact UI
        if (hit.transform != null && hit.transform.tag == "Box")
        {
            InteractUI.SetActive(true);
        }
        else if (hit.transform != null && hit.transform.tag == "Health")
        {
            InteractUI.SetActive(true);
        }
        else if (hit.transform != null && hit.transform.tag == "Buzzer")
        {
            InteractUI.SetActive(true);
        }
        else
        {
            InteractUI.SetActive(false);
        }

        //MasterKeys UI
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

        //ObjectsNurseUI
        if (GameManager.Instance.Object1 && !GameManager.Instance.Injection && !GameManager.Instance.InjectionSpawn)
        {
            Object1UI.SetActive(true);
        }
        else
        {
            Object1UI.SetActive(false);
        }
        if (GameManager.Instance.Object2 && !GameManager.Instance.Injection && !GameManager.Instance.InjectionSpawn)
        {
            Object2UI.SetActive(true);
        }
        else
        {
            Object2UI.SetActive(false);
        }
        if (GameManager.Instance.Object3 && !GameManager.Instance.Injection && !GameManager.Instance.InjectionSpawn)
        {
            Object3UI.SetActive(true);
        }
        else
        {
            Object3UI.SetActive(false);
        }

        if (GameManager.Instance.Injection)
        {
            InjectionUI.SetActive(true);
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

        //UI Tutorial
        if (GameManager.Instance.tutorialJump)
        {
            jumpUI.SetActive(true);
        }
        else
        {
            jumpUI.SetActive(false);
        }

        if(GameManager.Instance.tutorialRun)
        {
            runUI.SetActive(true);
        }
        else
        {
            runUI.SetActive(false);
        }

        if(GameManager.Instance.tutorialCrouch)
        {
            crouchUI.SetActive(true);
        }
        else
        {
            crouchUI.SetActive(false);
        }

        StartCoroutine(Throw());
        ThrowGrounded();
    }

    private void Activation()
    {
        if (hit.transform != null && hit.transform.tag == "Box" && hit.transform.name == "LightBox" && !GameManager.Instance.audioH)
        {
            Electricity box = hit.transform.GetComponent<Electricity>();
            bool isInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0.0f;

            if (isInteractPressed)
            {
                LightBox.GetComponent<PhotonView>().RPC("Activation", RpcTarget.All, true);
            }
        }

        if (hit.transform != null && hit.transform.tag == "Box" && hit.transform.name == "L_Button")
        {
            bool isInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0.0f;

            if (isInteractPressed)
            {
                LDoor.GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, true);
                hit.transform.tag = "Untagged";
            }
        }

        if (hit.transform != null && hit.transform.tag == "Health")
        {
            Cure station = hit.transform.GetComponent<Cure>();
            bool isInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0.0f;

            if (station != null)
            {
                if (isInteractPressed)
                {
                    PV.RPC("UpdateHealingAnimation", RpcTarget.All, true);
                    HealingUI.SetActive(true);
                    station.updateCure(true, this.gameObject);
                    activeStation = station;
                    activated = true;
                }
                else if (activeStation != null)
                {
                    PV.RPC("UpdateHealingAnimation", RpcTarget.All, false);
                    HealingUI.SetActive(false);
                    station.updateCure(false, this.gameObject);
                    activeStation = null;
                    activated = false;
                }
            }
        }
        else if (activeStation != null && activated)
        {
            HealingUI.SetActive(false);
            activeStation.updateCure(false, this.gameObject);
            activated = false;
        }

        if (hit.transform != null && hit.transform.tag == "Buzzer" && !nina.GetComponent<NinaAI>().isWarping)
        {
            Buzzer buzzer = hit.transform.GetComponent<Buzzer>();
            bool isInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0.0f;

            if (isInteractPressed)
            {
                Buzzer[iBuzzer].GetComponent<PhotonView>().RPC("Activation", RpcTarget.All);
                Destroy(buzzer.GetComponent<Buzzer>());
                buzzer.transform.tag = "Untagged";
                iBuzzer++;
            }
        }
    }

    [PunRPC]
    void UpdateRightGrabbingAnimation(bool isRightGrabbing)
    {
        if (joseAnimator != null)
        {
            joseAnimator.SetBool("IsRightGrabbing", isRightGrabbing);
        }
    }

    [PunRPC]
    void UpdateLeftGrabbingAnimation(bool isLeftGrabbing)
    {
        if (joseAnimator != null)
        {
            joseAnimator.SetBool("IsLeftGrabbing", isLeftGrabbing);
        }
    }

    [PunRPC]
    void UpdateHealingAnimation(bool isHealing)
    {
        if (joseAnimator != null)
        {
            joseAnimator.SetBool("IsHealing", isHealing);
        }
    }

    IEnumerator Grab()
    {
        
        if(hit.transform.tag == "Object")
        {
            
            bool IsRightPressed = Controls.Player.RightItem.ReadValue<float>() > 0.1f;
            if(IsRightPressed && !HasObjectRight)
            {
                PV.RPC("UpdateRightGrabbingAnimation", RpcTarget.All, true);
                PV.RPC("UpdateRightGrabbingAnimation", RpcTarget.All, false);
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
                ObjectRightUI.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                HasObjectRight = true;
                ThrowCheckR = true;
                
            }

            bool IsLeftPressed = Controls.Player.LeftItem.ReadValue<float>() > 0.1f;
            if (IsLeftPressed && !HasObjectLeft)
            {
                photonView.RPC("UpdateLeftGrabbingAnimation", RpcTarget.All, true);
                photonView.RPC("UpdateLeftGrabbingAnimation", RpcTarget.All, false);
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
            yield return new WaitForSeconds(0.501f);
            HasObjectRight = false;
           
        }

        bool IsLeftPressed = Controls.Player.LeftThrow.ReadValue<float>() > 0.1f;
        if (IsLeftPressed && HasObjectLeft && ThrowCheckL)
        {
            
            // ObjectLeftT.transform.localScale = new Vector3(ObjectLOriginalScale, ObjectLOriginalScale, ObjectLOriginalScale);
            // Vector3 camerDirection = PlayerCamera.transform.forward;
            // ObjectLeftT.transform.parent = null;
            // ObjectLeftRb.isKinematic = false;
            // ObjectLeftRb.AddForce(camerDirection * ThrowForce);
            Debug.Log("Throwing");
            ObjectLeftRb.GetComponent<ObjectsData>().onThrow(PlayerCamera.transform.forward, ThrowForce);
            ThrowCheckL = false;
            yield return new WaitForSeconds(0.501f);
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
