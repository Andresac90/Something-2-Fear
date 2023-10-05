using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    private float RandomNumber;
    [SerializeField]
    private Image AboveEye;
    [SerializeField]
    private Image BelowEye;

    private bool Check = true;
    private float Contador = 0;

    // Start is called before the first frame update
    void Start()
    {
        RandomNumber = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        Contador += Time.deltaTime;
        if (Contador >= RandomNumber && Check)
        {
            AboveEye.transform.localPosition += new Vector3(0, -7 * Time.deltaTime * 200, 0);
            BelowEye.transform.localPosition += new Vector3(0, 7 * Time.deltaTime * 200, 0);
            
        }
        if(Contador >= RandomNumber + 0.5f)
        {
            Check = false;
            AboveEye.transform.localPosition += new Vector3(0, 7 * Time.deltaTime * 200, 0);
            BelowEye.transform.localPosition += new Vector3(0, -7 * Time.deltaTime * 200, 0);
        }
        if (Contador >= RandomNumber + 1.0f)
        {
            AboveEye.transform.localPosition = new Vector3(0, 1100, 0);
            BelowEye.transform.localPosition = new Vector3(0, -1100, 0);
            RandomNumber = Random.Range(5, 10);
            Contador = 0;
            Check = true;
        }
    }
}
