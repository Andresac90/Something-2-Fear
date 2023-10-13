using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewStopAI : MonoBehaviour
{
    [SerializeField] AIControl aiControlReference;

    public LayerMask enemyMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, GameObject.FindGameObjectWithTag("Enemy").transform.position, 100, enemyMask)); //Raycast from player to enemy
        {
            Debug.Log("Raycast from player");
            aiControlReference.isPatrol = false;
            aiControlReference.isSeen = true;
            //isSeen = true;

        }
    }
}
