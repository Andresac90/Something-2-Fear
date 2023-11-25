using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.ComponentModel;

public class Down : MonoBehaviourPun
{
    private Camera Camera;
    private CharacterController CharController;
    private GameObject playerCam;
    private float currentTime = 0f;
    [SerializeField]
    private float deadTime;
    private bool joseDown = false;
    private bool santiDown = false;
    private bool areDead = false;

    public PhotonView downingIndicator;

    public Animator playerAnimator;

    public bool isPlayerDowned;

    public void Start()
    {
        playerCam = transform.GetComponentInChildren<Camera>().gameObject;
        Camera = playerCam.GetComponent<Camera>();
        CharController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isPlayerDowned)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= deadTime)
            {
                Die();
            }
            else{
                downingIndicator.RPC("SyncEnableDowningIndicator", RpcTarget.All);
            }
        }
        else
        {
            downingIndicator.RPC("SyncDisableDowningIndicator", RpcTarget.All);
            currentTime = 0f;
        }
    }

    [PunRPC]
    public void playerDown(bool down)
    {
        if (name == "Santi(Clone)")
        {
            santiDown = down;
        }
        else if (name == "Jose(Clone)")
        {
            joseDown = down;
        }
    }

    [PunRPC]
    public void SyncDowned()
    {
        if (isPlayerDowned) return;

        Debug.Log("downing");
        // look for santiController or JoseMovement
        if (name == "Santi(Clone)")
        {
            photonView.RPC("playerDown", RpcTarget.All, true);
            GetComponent<SantiController>().enabled = false;
            photonView.RPC("UpdateDownedAnimationSanti", RpcTarget.All);
        }
        else if (name == "Jose(Clone)")
        {
            photonView.RPC("playerDown", RpcTarget.All, true);
            GetComponent<JoseMovement>().enabled = false;
            photonView.RPC("UpdateDownedAnimationJose", RpcTarget.All);
        }
        GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - 0.879f, transform.position.z);
        GetComponent<Transform>().rotation = Quaternion.Euler(Quaternion.identity.x - 90f, Quaternion.identity.y, Quaternion.identity.z);
        GetComponent<BoxCollider>().enabled = true;
        CharController.enabled = false;
        Camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x - 70f, Quaternion.identity.y, Quaternion.identity.z);
        GetComponentInChildren<PlayerLook>().enabled = false;
        isPlayerDowned = true;
    }

    [PunRPC]
    public void SyncRevive()
    {
        if (!isPlayerDowned) return;
        Debug.Log("reviving");
        if (name == "Santi(Clone)")
        {
            photonView.RPC("playerDown", RpcTarget.All, false);
            GetComponent<SantiController>().enabled = true;
            photonView.RPC("UpdateRevivedAnimationSanti", RpcTarget.All);
            //playerAnimator.ResetTrigger("SantiRevivedTrigger");
            
            

        }
        else if (name == "Jose(Clone)")
        {
            photonView.RPC("playerDown", RpcTarget.All, false);
            GetComponent<JoseMovement>().enabled = true;
            photonView.RPC("UpdateRevivedAnimationJose", RpcTarget.All);
            //playerAnimator.ResetTrigger("JoseRevivedTrigger");

        }
        GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 0.879f, transform.position.z);
        GetComponent<Transform>().rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
        CharController.enabled = true;
        GetComponent<BoxCollider>().enabled = false;
        Camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
        GetComponentInChildren<PlayerLook>().enabled = true;
        isPlayerDowned = false;
    }
    [PunRPC]
    public void AreDead()
    {
        areDead = true;
    }
    
    private void Die()
    {
        GameObject otherPlayer;
        if (name == "Santi(Clone)")
        {
            otherPlayer = GameObject.Find("Jose(Clone)");
            if (otherPlayer == null) return;
            joseDown = otherPlayer.GetComponent<Down>().isPlayerDowned;
        }
        else
        {
            otherPlayer = GameObject.Find("Santi(Clone)");
            if (otherPlayer == null) return;
            santiDown = otherPlayer.GetComponent<Down>().isPlayerDowned;
        }

        if(((currentTime >= deadTime) || (joseDown && santiDown)) && !areDead)
        {
            Debug.Log("eliminado");
            Cursor.lockState = CursorLockMode.None;
            // SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();

            // change ever
            // SceneManager.LoadScene("LoseScreen");
            PhotonNetwork.LoadLevel("LoseScreen");
            photonView.RPC("AreDead", RpcTarget.All);
            // gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void UpdateDownedAnimationJose()
    {
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger("IsHealing");
            playerAnimator.ResetTrigger("JoseRevivedTrigger");
            playerAnimator.ResetTrigger("IsStanding");
            playerAnimator.ResetTrigger("IsRightGrabbingTrigger");
            playerAnimator.ResetTrigger("IsLeftGrabbingTrigger");
            playerAnimator.ResetTrigger("IsBending");
            playerAnimator.ResetTrigger("IsStanding");
            playerAnimator.SetTrigger("JoseDownedTrigger");
        }
    }

    [PunRPC]
    void UpdateRevivedAnimationJose()
    {
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger("IsHealing");
            playerAnimator.ResetTrigger("JoseRevivedTrigger");
            playerAnimator.ResetTrigger("IsStanding");
            playerAnimator.ResetTrigger("IsRightGrabbingTrigger");
            playerAnimator.ResetTrigger("IsLeftGrabbingTrigger");
            playerAnimator.ResetTrigger("IsBending");
            playerAnimator.ResetTrigger("IsStanding");
            playerAnimator.SetTrigger("JoseDownedTrigger");
        }
    }

    [PunRPC]
    void UpdateDownedAnimationSanti()
    {
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger("SantiRevivedTrigger");
            playerAnimator.ResetTrigger("BeginPuzzleTrigger");
            playerAnimator.ResetTrigger("EndPuzzleTrigger");
            playerAnimator.ResetTrigger("SpecialIdleTrigger");
            playerAnimator.ResetTrigger("IsBengalThrowTrigger");
            playerAnimator.SetTrigger("SantiDownedTrigger");
        }
    }

    [PunRPC]
    void UpdateRevivedAnimationSanti()
    {
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger("SantiDownedTrigger");
            playerAnimator.ResetTrigger("BeginPuzzleTrigger");
            playerAnimator.ResetTrigger("EndPuzzleTrigger");
            playerAnimator.ResetTrigger("SpecialIdleTrigger");
            playerAnimator.ResetTrigger("IsBengalThrowTrigger");
            playerAnimator.SetTrigger("SantiRevivedTrigger");
        }
    }
}
