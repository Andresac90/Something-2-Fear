using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DowningIndicator : MonoBehaviour
{
    [SerializeField]
    private RawImage downingIndicator;

    private void Start()
    {
        downingIndicator.enabled = true;
    }

    [PunRPC]
    public void SyncEnableDowningIndicator()
    {
        downingIndicator.enabled = true;
    }

    [PunRPC]
    public void SyncDisableDowningIndicator()
    {
        downingIndicator.enabled = true;
    }


}
