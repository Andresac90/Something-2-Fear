using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableRandomizer : MonoBehaviour
{
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject currentCable = transform.GetChild(i).gameObject;
            GameObject connectorCable = transform.GetChild(CableRandomizer.Range(0, transform.childCount)).gameObject;

            Vector2 newPosCurrentCable = connectorCable.transform.position;
            Vector2 newPosConnectorCable = currentCable.transform.position;

            currentCable.transform.position = newPosCurrentCable;
            connectorCable.transform.position = newPosConnectorCable;
        }
    }
}
