using UnityEngine;

using Photon.Pun;

public class SpawnInjection : MonoBehaviour
{
    public GameObject injectionPrefab;

    public void Spawn()
    {
        GameObject spawn = GameObject.FindGameObjectsWithTag("InjectionSpawn")[0];
        PhotonNetwork.Instantiate(injectionPrefab.name, spawn.transform.position, Quaternion.identity);
        Debug.Log("Spawneando Injection");
    }
}
