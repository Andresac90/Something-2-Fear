using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.ComponentModel.Design;

public class Down : MonoBehaviourPun
{
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private CharacterController CharController;
    [SerializeField]
    public Animator playerAnimator;
    [SerializeField]
    private PlayerLook PlayerLook;

    private float timer = 0f;
    [SerializeField]
    private float downTime = 35f;
    public bool isPlayerDowned;

    void Update()
    {
        if(isPlayerDowned)
        {
            timer += Time.deltaTime;
            if (timer >= downTime)
            {
                timer = 0f;
                Die();
            }
        }
    }

    public void DownPlayer()
    {
        if (isPlayerDowned) return;
        
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.879f, transform.position.z);
        transform.rotation = Quaternion.Euler(Quaternion.identity.x - 90f, Quaternion.identity.y, Quaternion.identity.z);
        GetComponent<BoxCollider>().enabled = true;
        CharController.enabled = false;
        Camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x - 70f, Quaternion.identity.y, Quaternion.identity.z);
        PlayerLook.enabled = false;
        isPlayerDowned = true;
        photonView.RPC("SyncIsPlayerDowned", RpcTarget.All, isPlayerDowned);

        // call a function after two seconds that checks if the other player is downed
        // if the other player is downed, then call the function to lose the game

        StartCoroutine(CheckOtherPlayerDowned());
    }

    public void RevivePlayer()
    {
        if (!isPlayerDowned) return;

        transform.position = new Vector3(transform.position.x, transform.position.y + 0.879f, transform.position.z);
        transform.rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
        CharController.enabled = true;
        GetComponent<BoxCollider>().enabled = false;
        Camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
        PlayerLook.enabled = true;
        isPlayerDowned = false;
        photonView.RPC("SyncIsPlayerDowned", RpcTarget.All, isPlayerDowned);
    }

    [PunRPC]
    public void SyncIsPlayerDowned(bool downed)
    {
        isPlayerDowned = downed;
    }

    // [PunRPC]
    // public void playerDown(bool down)
    // {
    //     if (name == "Santi(Clone)")
    //     {
    //         santiDown = down;
    //     }
    //     else if (name == "Jose(Clone)")
    //     {
    //         joseDown = down;
    //     }
    // }

    // [PunRPC]
    // public void SyncDowned()
    // {
    //     if (isPlayerDowned) return;

    //     // look for santiController or JoseMovement
    //     if (name == "Santi(Clone)")
    //     {
    //         photonView.RPC("playerDown", RpcTarget.All, true);
    //         GetComponent<SantiController>().enabled = false;
    //     }
    //     else if (name == "Jose(Clone)")
    //     {
    //         photonView.RPC("playerDown", RpcTarget.All, true);
    //         GetComponent<JoseMovement>().enabled = false;
    //     }
    //     GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - 0.879f, transform.position.z);
    //     GetComponent<Transform>().rotation = Quaternion.Euler(Quaternion.identity.x - 90f, Quaternion.identity.y, Quaternion.identity.z);
    //     GetComponent<BoxCollider>().enabled = true;
    //     CharController.enabled = false;
    //     Camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x - 70f, Quaternion.identity.y, Quaternion.identity.z);
    //     GetComponentInChildren<PlayerLook>().enabled = false;
    //     isPlayerDowned = true;
    // }

    // [PunRPC]
    // public void SyncRevive()
    // {
    //     if (!isPlayerDowned) return;
    //     Debug.Log("reviving");
    //     if (name == "Santi(Clone)")
    //     {
    //         photonView.RPC("playerDown", RpcTarget.All, false);
    //         GetComponent<SantiController>().enabled = true;
    //         playerAnimator.ResetTrigger("SantiDownedTrigger");
    //         playerAnimator.SetTrigger("SantiRevivedTrigger");
    //         //playerAnimator.ResetTrigger("SantiRevivedTrigger");
            
            

    //     }
    //     else if (name == "Jose(Clone)")
    //     {
    //         photonView.RPC("playerDown", RpcTarget.All, false);
    //         GetComponent<JoseMovement>().enabled = true;
    //         playerAnimator.ResetTrigger("JoseDownedTrigger");
    //         playerAnimator.SetTrigger("JoseRevivedTrigger");
    //         //playerAnimator.ResetTrigger("JoseRevivedTrigger");

    //     }
    //     GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 0.879f, transform.position.z);
    //     GetComponent<Transform>().rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
    //     CharController.enabled = true;
    //     GetComponent<BoxCollider>().enabled = false;
    //     Camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
    //     GetComponentInChildren<PlayerLook>().enabled = true;
    //     isPlayerDowned = false;
    // }
    // [PunRPC]
    // public void AreDead()
    // {
    //     areDead = true;
    // }
    
    // private void Die()
    // {
    //     GameObject otherPlayer;
    //     if (name == "Santi(Clone)")
    //     {
    //         otherPlayer = GameObject.Find("Jose(Clone)");
    //         if (otherPlayer == null) return;
    //         joseDown = otherPlayer.GetComponent<Down>().isPlayerDowned;
    //     }
    //     else
    //     {
    //         otherPlayer = GameObject.Find("Santi(Clone)");
    //         if (otherPlayer == null) return;
    //         santiDown = otherPlayer.GetComponent<Down>().isPlayerDowned;
    //     }

    //     if(((currentTime >= deadTime) || (joseDown && santiDown)) && !areDead)
    //     {
    //         Debug.Log("eliminado");
    //         Cursor.lockState = CursorLockMode.None;
    //         // SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();

    //         // change ever
    //         // SceneManager.LoadScene("LoseScreen");
    //         PhotonNetwork.LoadLevel("LoseScreen");
    //         photonView.RPC("AreDead", RpcTarget.All);
    //         // gameObject.SetActive(false);
    //     }
    // }

    IEnumerator CheckOtherPlayerDowned()
    {

        yield return new WaitForSeconds(2f);

        GameObject otherPlayer;
        if (name == "Santi(Clone)")
        {
            otherPlayer = GameObject.Find("Jose(Clone)");
            if (otherPlayer == null) yield return null;
            if (otherPlayer.GetComponent<Down>().isPlayerDowned)
            {
                Die();
            }
        }
        else
        {
            otherPlayer = GameObject.Find("Santi(Clone)");
            if (otherPlayer == null) yield return null;
            if (otherPlayer.GetComponent<Down>().isPlayerDowned)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        PhotonNetwork.LoadLevel("LoseScreen");
    }
}
