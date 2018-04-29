using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour 
{
	public GameObject GameManagerObject;
	public Slider GameModeSlider;
	public Dropdown InitiativeDropDown;
	public Slider DifficultySlider;

	private GameManager _gameManagerScript;

	void Start()
	{
		GameModeSlider.value = 0f;
		InitiativeDropDown.value = 0;
		DifficultySlider.value = 1f;

		_gameManagerScript = GameManagerObject.GetComponent<GameManager>();
	}

	public void PlayGame()
	{
		SetGameMode();
		SetDifficulty();
		SetInitiativeSelection();
		
		//SettingsMenuObject.SetActive(false);
		SceneManager.LoadScene("Main");

		_gameManagerScript.GameStarted = true;
	}

	private void SetGameMode()
	{
		if (GameModeSlider.value == 0)
			_gameManagerScript.PlayerTwo = GameManager.PlayerControl.AI;
		else
			_gameManagerScript.PlayerTwo = GameManager.PlayerControl.Human;
	}

	private void SetInitiativeSelection()
	{
		// if (InitiativeDropDown.value == 0)
		// 	_gameManagerScript.PlayerTwo = GameManager.PlayerControl.AI;
		// else if (InitiativeDropDown.value == 1)
		// 	_gameManagerScript.PlayerTwo = GameManager.PlayerControl.Human;
		// else
		// 	_gameManagerScript.PlayerTwo = GameManager.PlayerControl.Human;
	}

	private void SetDifficulty()
	{
		if (DifficultySlider.value == 0)
			_gameManagerScript.GameDifficultyChoice = GameManager.GameDifficulty.Easy;
		else
			_gameManagerScript.GameDifficultyChoice = GameManager.GameDifficulty.Hard;
	}	
}
