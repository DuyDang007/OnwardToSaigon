using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public TMP_InputField levelCodeInput;
    public GameObject wrongCodeText;

    private Dictionary<string, string> codeDict = new Dictionary<string, string>()
    {
        {"", "phuoclong" },
        {"CDTN", "taynguyen" },
        {"HDNC", "huedanang" },
        {"TSHS", "truongsa" },
        {"3004", "saigon" }
    };
    // Start is called before the first frame update
    public void StartPress()
    {
        string inputCode = levelCodeInput.text.Trim().ToUpper();
        string levelName = "";
        

        if(!codeDict.TryGetValue(inputCode, out levelName))
        {
            wrongCodeText.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
