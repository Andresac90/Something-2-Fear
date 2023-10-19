using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Numpad : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI display;

    private string password;

    private GameObject Puzzle;
    private Puzzle Comprobations;

    public string minigameName;
    // Start is called before the first frame update
    void Start()
    {
        Puzzle = GameObject.Find("Numpad MiniGame" + GameManager.Instance.numpadLevel);
        GameManager.Instance.numpadLevel += 1;

        Comprobations = Puzzle.GetComponent<Puzzle>();
        password = Comprobations.PasswordRef;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNumber(string number)
    {
        if (display.text.Length >= 4)
        {
            return;
        }

        display.text += number;
    }    

    public void EraseDisplay()
    {
        display.text = "";
    }

    public void CheckPassword()
    {
        if (display.text.Equals(password))
        {
            display.color = Color.green;
            display.text = "Granted";
            Destroy(gameObject, 1.0f);
            Comprobations.comprobations++;
            Comprobations.Completed();
        }
        else
        {
            display.text = "Denied";
        }
    }    
}
