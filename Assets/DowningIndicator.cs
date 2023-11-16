using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DowningIndicator : MonoBehaviour
{
    [SerializeField]
    private RawImage downingIndicator;

    private Camera mainCamera;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        downingIndicator.enabled = false;

        // check if this is not the local player
        if (photonView.IsMine) return;
        
        mainCamera = Camera.main;
    }

    void Update ()
    {
        // check if this is the local player
        if (photonView.IsMine) return;

        // rotate the indicator to face the camera anf flip it
        downingIndicator.transform.LookAt(mainCamera.transform);
        downingIndicator.transform.Rotate(0, 180, 0);
    }

    [PunRPC]
    public void SyncEnableDowningIndicator()
    {
        downingIndicator.enabled = true;
    }

    [PunRPC]
    public void SyncDisableDowningIndicator()
    {
        downingIndicator.enabled = false;
    }


}
