﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
	public int currency { get; private set; } = 0;
	public string[] equippedItems { get; private set; }
	[SerializeField] private int defaultMoney = 0;

	private List<string>[] unlockedItems;
	private string defaultHat = "Hat 0";
	private string defaultBody = "Body 1Tennisball";
	private string toParse = "";
	private char separatorChar = ',';
	private char newLineChar = '.';

	public void Start()
	{
		LoadCurrency();
		LoadUnlockedItems();
		LoadEquippedItems();
		InitializeEquippedItems();
		NamePlayer();
	}

	public void OnDestroy()
	{
		if (unlockedItems == null || equippedItems == null)
		{
			return;
		}
		PlayerPrefs.SetInt("Currency", currency);
		SaveListOfArrays(unlockedItems, "UnlockedItems");
		SaveArray(equippedItems, "EquippedItems");
		PlayerPrefs.Save();
	}

	#region loading

	private void LoadCurrency()
	{
		currency = PlayerPrefs.GetInt("Currency", defaultMoney);
		CustomizationController customControl = FindObjectOfType<CustomizationController>();
		if (customControl)
		{
			customControl.UpdateCurrencyText();
		}
	}

	public void ResetAll()
	{
		PlayerPrefs.SetInt("Currency", defaultMoney);
		PlayerPrefs.SetString("EquippedItems", $"{defaultHat}{separatorChar}{defaultBody}{newLineChar}");
		PlayerPrefs.SetString("UnlockedItems", $"{defaultHat}{newLineChar}{defaultBody}{newLineChar}");
	}

	private void LoadUnlockedItems()
	{
		toParse = PlayerPrefs.GetString("UnlockedItems", $"{defaultHat}{newLineChar}{defaultBody}{newLineChar}");
		int numTypes = System.Enum.GetNames(typeof(ClothingOptions)).Length;
		char[] newLines = { newLineChar };
		char[] commas = { separatorChar };
		string[] categories = toParse.Split(newLines, System.StringSplitOptions.RemoveEmptyEntries);
		unlockedItems = new List<string>[System.Enum.GetNames(typeof(ClothingOptions)).Length];
		for (int i = 0; i < unlockedItems.Length; i++)
		{
			unlockedItems[i] = new List<string>();
		}
		for (int i = 0; i < categories.Length; i++)
		{
			string[] category = categories[i].Split(commas, System.StringSplitOptions.RemoveEmptyEntries);
			for (int j = 0; j < category.Length; j++)
			{
				unlockedItems[i].Add(category[j]);
			}
		}
	}

	private void LoadEquippedItems()
	{
		toParse = PlayerPrefs.GetString("EquippedItems", $"{defaultHat}{separatorChar}{defaultBody}{newLineChar}");
		char[] separators = { separatorChar, newLineChar };
		string[] nums = toParse.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
		equippedItems = new string[System.Enum.GetNames(typeof(ClothingOptions)).Length];
		for (int i = 0; i < nums.Length; i++)
		{
			equippedItems[i] = nums[i];
		}
	}

	private void InitializeEquippedItems()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "CustomizationMenu")
		{
			if (transform.GetComponent<MeshRenderer>())
			{
				transform.GetComponent<MeshRenderer>().enabled = false;
			}
			int i = 0;
			foreach (string item in equippedItems)
			{
				//find prefab of specific name
				GameObject option = Instantiate(
					(GameObject)Resources.Load($"Prefabs/Equipment/{item}"),
					transform.GetChild(i)
					);
				option.SetActive(true);
				i++;
			}
		}
		else
		{
			CustomizationController customizationController = FindObjectOfType<CustomizationController>();
			//Go backwards through the children so that the customizations menu always starts off on the first menu.
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				//find out 
				CustomizationSwitcher cSwitcher = transform.GetChild(i).GetComponent<CustomizationSwitcher>();
				GameObject[] options = cSwitcher.GetCustomizations();
				for (int j = 0; j < options.Length; j++)
				{
					if (options[j].name == equippedItems[i])
					{
						customizationController.SwitchPanelCustomization(i, j);
						break;
					}
				}
			}
		}
	}

	private void NamePlayer()
	{
		Competitor competitor = GetComponent<Competitor>();
		if (competitor)
		{
			competitor.Name = PlayerPrefs.GetString("PLAYER_INPUT_NAME", "Player");
		}
		ScoreManager sm = GameObject.FindObjectOfType<ScoreManager>();
		if (sm != null)
		{
			sm.UpdateScoreBoard();
		}
	}

	#endregion /loading

	#region saving

	private void SaveListOfArrays(List<string>[] arr, string name = "UnlockedItems")
	{
		//create string of comma separated values for all values of the unlockedItems array of lists.
		string saveData = "";
		for (int i = 0; i < arr.Length; i++)
		{
			List<string> someItems = arr[i];
			for (int j = 0; j < someItems.Count; j++)
			{
				saveData += someItems[j].ToString();
				if (j < someItems.Count - 1)
				{
					saveData += separatorChar;
				}
			}
			saveData += newLineChar;
		}
		PlayerPrefs.SetString(name, saveData);
	}
	private void SaveArray(string[] arr, string name = "EquippedItems")
	{
		string saveData = "";
		for (int i = 0; i < arr.Length; i++)
		{
			saveData += arr[i].ToString();
			if (i < arr.Length - 1)
			{
				saveData += separatorChar;
			}
			else
			{
				saveData += newLineChar;
			}
		}
		PlayerPrefs.SetString(name, saveData);
	}

	#endregion /saving

	#region currency

	/// <summary>Adds coins to the Player's currency.</summary>
	/// <param name="coins">The number of coins to add.</param>
	/// <returns></returns>
	public bool addCurrency(int coins)
	{
		if (currency + coins < CurrencySystem.max_currency)
		{
			currency += coins;
			PlayerPrefs.SetInt("Currency", currency);
			return true;
		}
		currency = CurrencySystem.max_currency;
		PlayerPrefs.SetInt("Currency", currency);
		return false;
	}

	/// <summary>Subtracts the given amount from the Player's currency.</summary>
	/// <param name="coins">The number of coins to be subtracted.</param>
	/// <returns>If the currency was successfully subtracted, returns true. Otherwise false.</returns>
	public bool subtractCurrency(int coins)
	{
		if (currency - coins >= 0)
		{
			currency -= coins;
			PlayerPrefs.SetInt("Currency", currency);
			return true;
		}
		return false;
	}
	#endregion /currency

	#region inventory
	//if item in inventory, returns true;
	//TODO extrapolate this to the player class.
	public bool CheckUnlockedItems(int panelIndex, string itemBeingChecked)
	{
		foreach (string itemName in unlockedItems[panelIndex])
		{
			if (itemName == itemBeingChecked)
			{
				return true;
			}
		}
		return false;
	}

	public bool AddUnlockedItem(int index, string item)
	{
		if (!unlockedItems[index].Contains(item))
		{
			unlockedItems[index].Add(item);
			return true;
		}
		return false;
	}

	//If item is equipped, returns true.
	public bool CheckEquippedItem(int type, string name)
	{
		if (equippedItems[type] == name)
		{
			return true;
		}
		return false;
	}

	public bool SetEquippedItem(int index, string item)
	{
		if (index < 0 || index > equippedItems.Length - 1)
		{
			return false;
		}
		equippedItems[index] = item;
		return true;
	}

	#endregion /inventory

}
