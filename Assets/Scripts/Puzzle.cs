using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private GameObject puzzleCopy;
    private GameObject playerMove;
    private ObjectsSanti objectsSanti;

    [SerializeField]
    private GameObject puzzle;
    [SerializeField]
    private Door door;
    [SerializeField]
    private int comprobationsNeeded;

    public int comprobations;
    public string PasswordRef;

    void Start()
    {
        playerMove = GameObject.Find("Santi");
        objectsSanti = playerMove.GetComponent<ObjectsSanti>();
    }

    public void OpenPuzzle(bool puzzleCreated, bool puzzleActive, string objectName)
    {
        if(!puzzleActive && objectName == this.name && !puzzleCreated)
        {
            puzzleCopy = Instantiate(puzzle);
            PlayerMovement(false);
        }
        else if(!puzzleActive && puzzleCopy != null)
        {
            puzzleCopy.SetActive(true);
            PlayerMovement(false);
        }
    }

    public void ClosePuzzle(bool puzzleActive)
    {
        if(puzzleCopy != null)
        {
            PlayerMovement(true);
        }
    }

    public void PlayerMovement(bool puzzleActive)
    {
        if(!puzzleActive)
        {
            playerMove.GetComponent<SantiController>().enabled = false;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(puzzleActive)
        {
            playerMove.GetComponent<SantiController>().enabled = true;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            puzzleCopy.SetActive(false);
        }
    }

    public void Completed()
    {
        if(comprobations == comprobationsNeeded)
        {
            door.doorState = true;
            door.OpenDoor();
            Destroy(puzzleCopy.gameObject, 1f);
            PlayerMovement(true);
            objectsSanti.puzzleCreated = false;
            objectsSanti.puzzleActive = false;
            Destroy(this);
        }
    }
}
