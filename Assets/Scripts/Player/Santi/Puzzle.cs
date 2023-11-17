using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Puzzle : MonoBehaviourPun
{
    private GameObject puzzleCopy;
    [SerializeField]
    private GameObject playerMove;
    private ObjectsSanti objectsSanti;

    public GameObject puzzle;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private int comprobationsNeeded;

    public int comprobations;
    public string PasswordRef;

    public void OpenPuzzle(bool puzzleCreated, bool puzzleActive, string objectName)
    {
        playerMove = GameObject.Find("Santi(Clone)");
        GameManager.Instance.puzzle = true;
        objectsSanti = playerMove.GetComponent<ObjectsSanti>();
        
        if(!puzzleActive && objectName == this.name && !puzzleCreated)
        {
            if (objectsSanti.objectNameString == "Key" && puzzle.name == "LockPick")
            {
                puzzleCopy = Instantiate(puzzle);
                PlayerMovement(false);
            }
            else if (puzzle.name != "LockPick")
            {
                puzzleCopy = Instantiate(puzzle);
                PlayerMovement(false);
            }
        }
    }

    public void ClosePuzzle(bool puzzleActive)
    {

        GameManager.Instance.puzzle = false;
        if(puzzleCopy != null)
        {
            comprobations = 0;
            objectsSanti.puzzleCreated = false;
            objectsSanti.puzzleActive = false;
            PlayerMovement(true);
            Destroy(puzzleCopy.gameObject, 1f);
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
        if(puzzleCopy.name == "Labyrinth MiniGame" && comprobations == comprobationsNeeded)
        {
            PlayerMovement(true);
            objectsSanti.puzzleCreated = false;
            objectsSanti.puzzleActive = false;
            Destroy(puzzleCopy.gameObject, 1f);
            Destroy(this);
        }
        else if (comprobations == comprobationsNeeded)
        {
            door.GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, true);
            GameManager.Instance.Door.Play();
            door.GetComponent<Door>().doorState = true;
            door.GetComponent<Door>().OpenDoor();
            PlayerMovement(true);
            objectsSanti.puzzleCreated = false;
            objectsSanti.puzzleActive = false;
            Destroy(puzzleCopy.gameObject, 1f);
            Destroy(this);
            if (objectsSanti.objectNameString == "Key" && puzzle.name == "LockPick")
            {
                StartCoroutine(objectsSanti.RightDrop());
                objectsSanti.objectNameString = "";
                GameObject santi = GameObject.Find("Santi(Clone)"); 
                int keylevel = santi.GetComponent<ObjectsSanti>().keylevel;

                GameObject.Find("SmallKey_Item" + keylevel).GetComponent<PhotonView>().RPC("DestroyKeyOnline", RpcTarget.All, "SmallKey_Item" + keylevel);
                santi.GetComponent<PhotonView>().RPC("SyncKeyLevel", RpcTarget.All, keylevel + 1);
            }

        }
    }
}