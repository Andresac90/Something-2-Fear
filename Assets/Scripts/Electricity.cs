using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Electricity : MonoBehaviour
{
    [SerializeField]
    private GameObject Lights;

    private float currentTime = 0f;
    private float lightsTime = 45f;

    // Start is called before the first frame update
    [PunRPC]
    public void Activation(bool state)
    {
        GameManager.Instance.Switch.Play();
        Lights.SetActive(state);
        currentTime = 0f;
    }

    public void Update()
    {
        checkTime();
    }

    public void checkTime()
    {
        if (Lights.activeSelf)
        {
            currentTime += Time.deltaTime;
        }

        if (currentTime > lightsTime)
        {
            this.GetComponent<PhotonView>().RPC("Activation", RpcTarget.All, false);
        }
    }
}
