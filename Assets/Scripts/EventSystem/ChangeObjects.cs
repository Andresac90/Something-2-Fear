using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjects : MonoBehaviourPun
{
    [SerializeField]
    private GameObject pascualita;
    [SerializeField] 
    private GameObject lights;

    [PunRPC]
    public void ActivatePascualita()
    {
        bool value = pascualita.activeInHierarchy;
        pascualita.SetActive(true);
    }

    [PunRPC]
    public void DeactivatePascualita()
    {
        bool value = pascualita.activeInHierarchy;
        pascualita.SetActive(false);
    }

    [PunRPC]
    public void ActivateLights()
    {
        bool value = lights.activeInHierarchy;
        lights.SetActive(true);
    }

    [PunRPC]
    public void DeactivateLights()
    {
        bool value = lights.activeInHierarchy;
        lights.SetActive(false);
    }
}
