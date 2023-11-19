using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Note : MonoBehaviour
{
    private GameObject noteCopy;
    private GameObject playerMove;
    private ObjectsSanti objectsSanti;
    private TextMeshProUGUI textMesh;
    private RawImage img;

    [SerializeField]
    private GameObject note;
    [SerializeField]
    private string text;
    [SerializeField]
    private RawImage image;

    void Start()
    {
        
    }

    public void OpenNote(bool noteCreated)
    {
        playerMove = GameObject.Find("Santi(Clone)");
        objectsSanti = playerMove.GetComponent<ObjectsSanti>();
        GameObject child = note.transform.GetChild(0).gameObject;
        textMesh = child.GetComponentInChildren<TextMeshProUGUI>();
        GameObject sonChild = child.transform.GetChild(1).gameObject;
        img = sonChild.GetComponent<RawImage>();

        img = image;
        textMesh.text = text;
        noteCopy = Instantiate(note);
        PlayerMovement(noteCreated);
    }

    public void CloseNote(bool noteCreated)
    {
        PlayerMovement(noteCreated);
    }

    private void PlayerMovement(bool noteCreated)
    {
        if(!noteCreated)
        {
            playerMove.GetComponent<SantiController>().enabled = false;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(noteCreated)
        {
            playerMove.GetComponent<SantiController>().enabled = true;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(noteCopy);
        }
    }
}
