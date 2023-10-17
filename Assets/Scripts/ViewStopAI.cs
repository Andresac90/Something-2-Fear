using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewStopAI : MonoBehaviour
{
    public AIControl aiControlRef;

    public float radius;
    [Range(0, 360)]
    public float angle;
    public bool timer;

    public GameObject enemyRef;

    public LayerMask enemyMask;
    public LayerMask obstacleMask;

    private void Start()
    {
        radius = 15f;
        angle = 90f;
        timer = true;
        enemyRef = GameObject.FindGameObjectWithTag("Enemy");
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
        
        if (rangeChecks.Length != 0)
        {   
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask) && aiControlRef.WaitTime != 0)        
                    aiControlRef.SetIsSeen(true);
                else
                    aiControlRef.SetIsSeen(false);
            }
            else
                aiControlRef.SetIsSeen(false);
        }
        else if (aiControlRef)
            aiControlRef.SetIsSeen(false);
    }
    private IEnumerator IsSeenTimer()
    {
        //timer = false;
        StopCoroutine(FOVRoutine());
        aiControlRef.SetIsSeen(true);
        yield return new WaitForSeconds(4f);
        aiControlRef.SetIsSeen(false);
        StartCoroutine(FOVRoutine());
        
    }
}

