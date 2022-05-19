using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button contrinueButton = null;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNamekey = "PlayerName";

    private void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNamekey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNamekey);
        
        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }
    
    public void SetPlayerName(string name)
    {
        contrinueButton.interactable = !string.IsNullOrEmpty(name);
    }
    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNamekey, DisplayName);
    }

}
