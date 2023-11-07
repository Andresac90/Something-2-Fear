// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.VisualScripting;

// public class Labyrinth : MonoBehaviour
// {
//     private string terminalCode;
//     private string doorCode;
//     private bool doorActive;
//     private GameObject Puzzle;
//     private Puzzle Comprobations;

//     public string minigameName;
//     public GameObject[] doorsList;
    
//     void Start()
//     {
//         Puzzle = GameObject.Find("Labyrinth MiniGame");
        

//         Comprobations = Puzzle.GetComponent<Puzzle>();
//         doorCode = doorsList[0].GetComponent<Door>().doorCode;
//         doorActive = doorsList[0].GetComponent<Door>().doorState;
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     public void AddNumber(string symbol)
//     {
//         terminalCode += symbol;

//         if (display.text.Length >= 4)
//         {
//             return;
//         }
//     }    

//     public void EraseDisplay()
//     {
//         display.text = "";
//     }

//     public void CheckPassword()
//     {
//         if (display.text.Equals(password))
//         {
//             display.color = Color.green;
//             display.text = "Granted";
//             Destroy(gameObject, 1.0f);
//             GameManager.Instance.numpadLevel += 1;
//             Comprobations.comprobations++;
//             Comprobations.Completed();
//         }
//         else
//         {
//             display.text = "Denied";
//         }
//     }    
// }
