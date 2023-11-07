using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableRandomizer : MonoBehaviour
{
    private void Awake()
    {
        List<Vector2> originalPositions = new List<Vector2>();

        foreach (Transform child in transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                originalPositions.Add(rectTransform.anchoredPosition);
            }
        }
        RandomizePositions(originalPositions);
    }

    private void RandomizePositions(List<Vector2> positions)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < positions.Count; i++)
        {
            indices.Add(i);
        }

        for (int i = 0; i < positions.Count; i++)
        {
            int randomIndex = Random.Range(i, positions.Count);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        int index = 0;
        foreach (Transform child in transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = positions[indices[index]];
                index++;
            }
        }
    }
}