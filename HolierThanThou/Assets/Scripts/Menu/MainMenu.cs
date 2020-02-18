﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private InputField namingUser;
	[SerializeField] private Text namingText;
	[SerializeField] private Button StartButton;
	[SerializeField] private Button CustomizationButton;
	public void Start()
	{
		ResetNameText();
		AssignButtons();
	}

	public void AssignButtons()
	{
		SceneController controllerOfScenes = FindObjectOfType<SceneController>();
		StartButton.onClick.AddListener(controllerOfScenes.GoToScene);
		CustomizationButton.onClick.AddListener(delegate { controllerOfScenes.GoToScene(1); });
	}

	public void ResetNameText()
	{
        string currPlayerName = PlayerPrefs.GetString("PLAYER_INPUT_NAME", "Input Name");

        if (currPlayerName == "Player")
        {
            namingUser.text = "Input Name";
        }
        else
        {
            namingUser.text = currPlayerName;
        }
        
	}
	public void QuitOnClick()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif

		Application.Quit();
	}

	//Naming System: To send the customized name to the character
	//And load the saved name from the playerprefs on Awake
	public void UpdateCharacterName()
	{
		string playerName = namingUser.text.ToString();
        if (string.IsNullOrEmpty(playerName) || playerName == "Input Name" || string.IsNullOrWhiteSpace(playerName))
        {
            PlayerPrefs.SetString("PLAYER_INPUT_NAME", "Player");
        }
        else
        {
            namingText.text = playerName;
            PlayerPrefs.SetString("PLAYER_INPUT_NAME", playerName);
        }
	}


}
