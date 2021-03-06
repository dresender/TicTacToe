﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour 
{
	public GameObject GameMainMenu;
	public GameObject GameManagerObject;
	public GameObject AudioManagerObject;
	public Slider GameModeSlider;
	public Dropdown InitiativeDropDown;
	public Slider DifficultySlider;
	public Button InstructionsButton;

	private GameManager _gameManagerScript;
	private GameObject _gameManagerObject;
	private AudioManager _audioManager;
	private float _random;

	void Start()
	{
		GameModeSlider.value = 0f;
		InitiativeDropDown.value = 0;
		DifficultySlider.value = 1f;

		_gameManagerScript = FindObjectOfType(typeof(GameManager)) as GameManager;
		_audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;

		GameModeSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
		DifficultySlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
	}

	public void PlayGame()
	{
		SetGameMode();
		SetDifficulty();
		SetInitiativeSelection();

		SceneManager.LoadScene("Main");

		_gameManagerScript.GameOverConfirmed = false;

		if (InitiativeDropDown.value == 1)
			_gameManagerScript.FirstMoveAI();
		else if (InitiativeDropDown.value == 2 && _random < 50f)
			_gameManagerScript.FirstMoveAI();

		this.gameObject.SetActive(false);
		GameMainMenu.SetActive(true);
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
		if (InitiativeDropDown.value == 0)
		{
			//If Player chose Human plays first, set the CurrentPlayerController to the First Player
			_gameManagerScript.CurrentPlayerController = _gameManagerScript.FirstPlayer;
			//And then put the current Turn to the PlayerOne (Cross Pawn)
			_gameManagerScript.Turn = GameManager.Player.PlayerOne;
			//Setting the first player variable
			_gameManagerScript.WhoPlaysFirst = _gameManagerScript.FirstPlayer;
		}
		else if (InitiativeDropDown.value == 1)
		{
			//If Player chose CPU plays first, set the CurrentPlayerController to the Second Player
			_gameManagerScript.CurrentPlayerController = _gameManagerScript.SecondPlayer;
			//And then put the current Turn to the PlayerTwo (Circle Pawn)
			_gameManagerScript.Turn = GameManager.Player.PlayerTwo;
			//Setting the first player variable
			_gameManagerScript.WhoPlaysFirst = _gameManagerScript.SecondPlayer;
		}
		else
		{
			_random = UnityEngine.Random.Range(0f, 100f);

			if (_random > 50)
			{
				_gameManagerScript.CurrentPlayerController = _gameManagerScript.FirstPlayer;
				_gameManagerScript.Turn = GameManager.Player.PlayerOne;
			}
			else
			{
				_gameManagerScript.CurrentPlayerController = _gameManagerScript.SecondPlayer;
				_gameManagerScript.Turn = GameManager.Player.PlayerTwo;
			}
		}			
	}

	private void SetDifficulty()
	{
		if (DifficultySlider.value == 0)
			_gameManagerScript.GameDifficultyChoice = GameManager.GameDifficulty.Easy;
		else
			_gameManagerScript.GameDifficultyChoice = GameManager.GameDifficulty.Hard;
	}	

	public void ValueChangeCheck()
    {
		_audioManager.Play("MenuButtonSound");
    }

	public void PlayStartMatchSound()
	{
		_audioManager.Play("StartMatchButton");
	}

	public void PlayReturnButtonSound()
	{
		_audioManager.Play("ReturnButton");
	}

	public void PlayInitialMenuSound()
    {
		_audioManager.Play("InitialMenuButtons");
    }
}
