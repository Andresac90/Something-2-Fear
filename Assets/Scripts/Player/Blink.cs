using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Blink : MonoBehaviourPun
{
    private float RandomNumber;
    [SerializeField]
    private Image AboveEye;
    [SerializeField]
    private Image BelowEye;

    private bool Check = true;
    private float Contador = 0;

    public bool IsBlinking = false;

    private PhotonView pascualitaPV;

    private string PlayerName;

    // Start is called before the first frame update
    void Start()
    {
        RandomNumber = Random.Range(5, 10);
        pascualitaPV = GameObject.Find("Pascualita").GetComponent<PhotonView>();

        PlayerName = transform.parent.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        Contador += Time.deltaTime;
        if (Contador >= RandomNumber && Check)
        {
            AboveEye.transform.localPosition += new Vector3(0, -7 * Time.deltaTime * 300, 0);
            BelowEye.transform.localPosition += new Vector3(0, 7 * Time.deltaTime * 300, 0);
            IsBlinking = true;

            pascualitaPV.RPC("BlinkRPC", RpcTarget.All, PlayerName, true);
            
        }
        if(Contador >= RandomNumber + 0.5f)
        {
            Check = false;
            AboveEye.transform.localPosition += new Vector3(0, 7 * Time.deltaTime * 300, 0);
            BelowEye.transform.localPosition += new Vector3(0, -7 * Time.deltaTime * 300, 0);
        }
        if (Contador >= RandomNumber + 1.0f)
        {
            IsBlinking = false;

            pascualitaPV.RPC("BlinkRPC", RpcTarget.All, PlayerName, false);
            AboveEye.transform.localPosition = new Vector3(0, 1000, 0);
            BelowEye.transform.localPosition = new Vector3(0, -1000, 0);
            RandomNumber = Random.Range(5, 10);
            Contador = 0;
            Check = true;
        }
    }
}
