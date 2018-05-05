using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour 
{
	private AudioManager _audioManager;
	private GameManager _gameManager;
	private BoardManager _boardManager;

	void Awake()
	{
		_audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
		_gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
		_boardManager = FindObjectOfType(typeof(BoardManager)) as BoardManager;
	}

	void OnMouseDown() 
	{
		_gameManager.ClearGameTexts();
		_boardManager.ClearBoard();
		_audioManager.Play("ReturnButton");
		SceneManager.LoadScene("Menu");
	}
}
