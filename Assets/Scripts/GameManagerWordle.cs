using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ToppingType { RED, GREEN, ORANGE, BLUE, PURPLE};
public class GameManagerWordle : MonoBehaviour
{
    public string characterPool;
    private Dictionary<ToppingType, char> curDictionary;

    public TextMeshProUGUI promptText;
    string curPrompt;

    public static GameManagerWordle Instance;

    public StickController stick;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDictionary();
        Instance = this;
        GeneratePrompt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VerifyTanghulu()
    {
        string result = "";
        for(int i = 0; i < stick.fruits.Length; i++) { 
            if(curDictionary[stick.fruits[i].toppingType] == curPrompt[i]) {
                result += "✓";
                Debug.Log("right!");
            } else if (curPrompt.Contains(curDictionary[stick.fruits[i].toppingType].ToString())){
                result += "↔";
                Debug.Log("Exists but misplaced");
                }
            else {
                result += "x";
                Debug.Log("Wrong");
            }
        }
        Debug.Log(result);
        stick.ClearStick();
        GeneratePrompt();
    }

    private void GeneratePrompt()
    {
        curPrompt = "";
        for(int i = 0; i < 5; i++) {
            curPrompt += characterPool[Random.Range(0, characterPool.Length)];
        }
        promptText.text = "Make me a " + curPrompt;
    }

    void GenerateDictionary() {
        curDictionary = new Dictionary<ToppingType, char>();
        List<char> characters = new List<char>(characterPool.ToCharArray());

        for(int i = 0; i < characterPool.Length; i++) {
            int newIndex = Random.Range(0, characters.Count);
            curDictionary.Add((ToppingType)i, characters[newIndex]);
            characters.RemoveAt(newIndex);
        }
    }
}
