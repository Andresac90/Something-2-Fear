using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    private InputMaster Controls;
    [SerializeField]
    private string player;

    void Awake()
    {
        Controls = new InputMaster();
    }
    void Update()
    {
        if (Controls.Player.Movement.triggered)
        {
            Movement();
        }
    }

    private void Movement (){
        Vector2 move = Controls.Player.Movement.ReadValue<Vector2>();

        if (move.x < 0)
        {
            if (transform.localPosition.x == 500)
            {
                transform.position += new Vector3(-500, 0, 0);
                player = "";
            }
            if (transform.localPosition.x == 0)
            {
                transform.position += new Vector3(-500, 0, 0);
                player = "Jose";
            }
            
        }

        if (move.x > 0)
        {
            if (transform.localPosition.x == -500)
            {
                transform.position += new Vector3(500, 0, 0);
                player = "";
            }
            if (transform.localPosition.x == 0)
            {
                transform.position += new Vector3(500, 0, 0);
                player = "Santi";
            }
        }
    }
    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
