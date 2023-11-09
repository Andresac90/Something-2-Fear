using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ViewStopAI : MonoBehaviour
{
    public AIControl aiControlRef;
    private GameObject enemy;
    private bool isSeenRef;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject enemyRef;

    public LayerMask enemyMask;
    public LayerMask obstacleMask;

    private void Start()
    {
        radius = 15f;
        angle = 90f;

        enemy = GameObject.Find("Pascualita");
        enemyRef = GameObject.Find("Pascualita");
        aiControlRef = enemy.GetComponent<AIControl>();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true) //should be while pascualita is on scene
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, enemyMask);
        //Debug.Log(rangeChecks.Length);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    aiControlRef.GetComponent<PhotonView>().RPC("SyncSetIsSeen", RpcTarget.All, true, transform.name);
                    //aiControlRef.SyncSetIsSeen(true);
                }
                else
                {
                    aiControlRef.GetComponent<PhotonView>().RPC("SyncSetIsSeen", RpcTarget.All, false, transform.name);        
                }
            }
            else
            {
                aiControlRef.GetComponent<PhotonView>().RPC("SyncSetIsSeen", RpcTarget.All, false, transform.name);
            }
        }
        else if (aiControlRef)
        {
            aiControlRef.GetComponent<PhotonView>().RPC("SyncSetIsSeen", RpcTarget.All, false, transform.name);
        }
    }
}

