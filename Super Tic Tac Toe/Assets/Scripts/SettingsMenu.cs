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
			_gameManagerScript.SecondPlayer = GameManager.PlayerControl.AI;
		else
			_gameManagerScript.SecondPlayer = GameManager.PlayerControl.Human;
	}

	private void SetInitiativeSelection()
	{
		// if (InitiativeDropDown.value == 0)
		// 	_gameManagerScript.CurrentPlayerController = GameManager.PlayerControl.AI;
		// else if (InitiativeDropDown.value == 1)
		// 	_gameManagerScript.CurrentPlayerController = GameManager.PlayerControl.Human;
		// else
		// {
		// 	var _random = UnityEngine.Random.Range(0f, 100f);

		// 	if (_random > 50)
		// 		_gameManagerScript.CurrentPlayerController = GameManager.PlayerControl.Human;
		// 	else
		// 		_gameManagerScript.CurrentPlayerController = GameManager.PlayerControl.AI;
		// }

		// if (GameModeSlider.value == 1)
		// {
		// 	_gameManagerScript.CurrentPlayerController = GameManager.PlayerControl.Human;
		// }			
	}

	private void SetDifficulty()
	{
		if (DifficultySlider.value == 0)
			_gameManagerScript.GameDifficultyChoice = GameManager.GameDifficulty.Easy;
		else
			_gameManagerScript.GameDifficultyChoice = GameManager.GameDifficulty.Hard;
	}	
}
